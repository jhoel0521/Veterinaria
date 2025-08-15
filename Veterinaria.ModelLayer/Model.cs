using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.Database;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Implementa el patrón Active Record como en Laravel Eloquent
    /// </summary>
    public abstract class Model<T> where T : Model<T>, new()
    {
        protected virtual string Table { get; set; } = "";
        protected virtual string PrimaryKey { get; set; } = "id";
        protected virtual string[] Fillable { get; set; } = Array.Empty<string>();
        protected virtual string[] Hidden { get; set; } = Array.Empty<string>();
        protected virtual bool Timestamps { get; set; } = true;
        protected virtual bool SoftDeletes { get; set; } = false;

        private Dictionary<string, object?> _attributes = new Dictionary<string, object?>();
        private Dictionary<string, object?> _original = new Dictionary<string, object?>();
        private bool _exists = false;

        public Model()
        {
            // Si no se especifica tabla, usar el nombre de la clase en minúscula + 's'
            if (string.IsNullOrEmpty(Table))
            {
                Table = typeof(T).Name.ToLower() + "s";
            }
        }

        /// <summary>
        /// Llena el modelo con atributos
        /// </summary>
        public T Fill(Dictionary<string, object?> attributes)
        {
            // Determinar si es hidratación desde BD o asignación manual
            bool isHydrating = attributes.ContainsKey(PrimaryKey);

            foreach (var attr in attributes)
            {
                if (isHydrating || Fillable.Contains(attr.Key))
                {
                    _attributes[attr.Key] = attr.Value;
                    SetProperty(attr.Key, attr.Value);
                }
            }

            if (isHydrating)
            {
                _exists = true;
                // Guardar estado original para detectar cambios
                _original = new Dictionary<string, object?>(_attributes);
            }

            // Manejar timestamps para nuevos registros
            if (Timestamps && !_exists)
            {
                _attributes["created_at"] = _attributes.GetValueOrDefault("created_at", DateTime.Now);
                _attributes["updated_at"] = _attributes.GetValueOrDefault("updated_at", DateTime.Now);
            }

            return (T)this;
        }

        /// <summary>
        /// Guarda el modelo en la base de datos
        /// </summary>
        public T Save()
        {
            var db = Database.GetInstance();

            if (_exists)
            {
                return PerformUpdate(db);
            }

            return PerformInsert(db);
        }

        /// <summary>
        /// Realiza INSERT en la base de datos
        /// </summary>
        private T PerformInsert(Database db)
        {
            var attributes = GetAttributesForSave();

            if (Timestamps)
            {
                attributes["created_at"] = DateTime.Now;
                attributes["updated_at"] = DateTime.Now;
            }

            var columns = string.Join(", ", attributes.Keys);
            var placeholders = string.Join(", ", attributes.Keys.Select(k => $"@{k}"));

            var sql = $"INSERT INTO {Table} ({columns}) VALUES ({placeholders}); SELECT SCOPE_IDENTITY();";

            var parameters = new Dictionary<string, object>();
            foreach (var attr in attributes)
            {
                parameters[$"@{attr.Key}"] = attr.Value ?? DBNull.Value;
            }

            using var command = db.Query(sql, parameters);
            var newId = command.ExecuteScalar();

            _attributes[PrimaryKey] = Convert.ToInt32(newId);
            _exists = true;

            return (T)this;
        }

        /// <summary>
        /// Realiza UPDATE en la base de datos
        /// </summary>
        private T PerformUpdate(Database db)
        {
            var attributes = GetAttributesForSave();

            if (Timestamps)
            {
                attributes["updated_at"] = DateTime.Now;
            }

            var updates = attributes.Keys.Select(k => $"{k} = @{k}");
            var sql = $"UPDATE {Table} SET {string.Join(", ", updates)} WHERE {PrimaryKey} = @{PrimaryKey}Id";

            var parameters = new Dictionary<string, object>();
            foreach (var attr in attributes)
            {
                parameters[$"@{attr.Key}"] = attr.Value ?? DBNull.Value;
            }
            var key = GetKey();
            if (key == null)
                throw new InvalidOperationException("No se puede actualizar un registro sin clave primaria.");
            parameters[$"@{PrimaryKey}Id"] = key;

            using var command = db.Query(sql, parameters);
            command.ExecuteNonQuery();

            return (T)this;
        }

        /// <summary>
        /// Elimina el registro
        /// </summary>
        public bool Delete()
        {
            if (SoftDeletes)
            {
                return PerformSoftDelete();
            }
            return PerformForceDelete();
        }

        private bool PerformForceDelete()
        {
            var db = Database.GetInstance();
            var sql = $"DELETE FROM {Table} WHERE {PrimaryKey} = @id";
            var parameters = new Dictionary<string, object> { { "@id", GetKey()! } };

            using var command = db.Query(sql, parameters);
            command.ExecuteNonQuery();

            _exists = false;
            return true;
        }

        private bool PerformSoftDelete()
        {
            _attributes["deleted_at"] = DateTime.Now;
            Save();
            return true;
        }

        /// <summary>
        /// Obtiene la clave primaria del modelo
        /// </summary>
        public object? GetKey()
        {
            // Primero intentar obtener de los atributos
            var keyFromAttributes = _attributes.GetValueOrDefault(PrimaryKey);
            if (keyFromAttributes != null)
                return keyFromAttributes;

            // Si no está en atributos, intentar obtener de la propiedad
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(this);
                if (idValue != null && !idValue.Equals(0))
                {
                    // Actualizar los atributos con el valor encontrado
                    _attributes[PrimaryKey] = idValue;
                    return idValue;
                }
            }

            return null;
        }

        /// <summary>
        /// Obtiene los atributos para guardar
        /// </summary>
        private Dictionary<string, object?> GetAttributesForSave()
        {
            var attributes = new Dictionary<string, object?>();

            // Solo incluir campos fillable
            foreach (var field in Fillable)
            {
                if (_attributes.ContainsKey(field))
                {
                    attributes[field] = _attributes[field];
                }
            }

            return attributes;
        }

        public static QueryBuilder Query()
        {
            var instance = new T();
            return Database.GetInstance().Table(instance.Table, typeof(T));
        }

        public static QueryBuilder Where(string column, object operatorOrValue, object? value = null)
        {
            // verificaremos 2 si operatorOrValue es un SqlOperator o un valor
            if (operatorOrValue is string opStr)
            {
                var sqlOp = SqlOperatorExtensions.ToStringSql(opStr);
                if (sqlOp.HasValue)
                    return Query().Where(column, sqlOp.Value, value);
                if (value == null)
                    return Query().Where(column, SqlOperator.Equal, operatorOrValue);

            }
            else if (operatorOrValue is SqlOperator sqlOp)
            {
                return Query().Where(column, sqlOp, value);
            }
            else if (value == null)
            {
                return Query().Where(column, SqlOperator.Equal, operatorOrValue);
            }
            throw new ArgumentException("Parámetros inválidos para Where");
        }

        public static T? Find(object id)
        {
            var instance = new T();
            var result = Query().Where(instance.PrimaryKey, id).First();
            
            // Si encontramos un resultado, marcarlo como existente
            if (result is T model)
            {
                model.MarkAsExisting();
                return model;
            }
            
            return result as T;
        }

        public static List<T> All()
        {
            return Query().Get().Cast<T>().ToList();
        }

        public static List<T> WhereIn(string column, IEnumerable<object> values)
        {
            return Query().WhereIn(column, values).Get().Cast<T>().ToList();
        }

        public static int Count()
        {
            return Query().Count();
        }

        public static QueryBuilder OrderBy(string column, string direction = "ASC")
        {
            return Query().OrderBy(column, direction);
        }

        public static QueryBuilder Limit(int limit)
        {
            return Query().Limit(limit);
        }

        // Métodos auxiliares
        private void SetProperty(string name, object? value)
        {
            // Intentar primero con el nombre exacto
            var property = typeof(T).GetProperty(name,
                System.Reflection.BindingFlags.Public |
                System.Reflection.BindingFlags.Instance |
                System.Reflection.BindingFlags.IgnoreCase);

            // Si no se encuentra, intentar convertir de snake_case a PascalCase
            if (property == null)
            {
                string pascalCaseName = ConvertToPascalCase(name);
                property = typeof(T).GetProperty(pascalCaseName,
                    System.Reflection.BindingFlags.Public |
                    System.Reflection.BindingFlags.Instance);
            }

            if (property != null && property.CanWrite)
            {
                if (value != DBNull.Value && value != null)
                {
                    // Convertir el tipo si es necesario
                    var convertedValue = ConvertValueToPropertyType(value, property.PropertyType);
                    property.SetValue(this, convertedValue);
                }
                else if (IsNullableType(property.PropertyType))
                {
                    property.SetValue(this, null);
                }
            }
        }

        /// <summary>
        /// Convierte un nombre de snake_case a PascalCase
        /// Ejemplo: cliente_id -> ClienteId
        /// </summary>
        private string ConvertToPascalCase(string snakeCase)
        {
            if (string.IsNullOrEmpty(snakeCase))
                return snakeCase;

            var parts = snakeCase.Split('_');
            for (int i = 0; i < parts.Length; i++)
            {
                if (!string.IsNullOrEmpty(parts[i]))
                {
                    parts[i] = char.ToUpperInvariant(parts[i][0]) + parts[i].Substring(1).ToLowerInvariant();
                }
            }
            return string.Join("", parts);
        }

        /// <summary>
        /// Convierte un valor al tipo de la propiedad
        /// </summary>
        private object? ConvertValueToPropertyType(object value, Type targetType)
        {
            if (value == null || value == DBNull.Value)
                return null;

            // Si el tipo ya es correcto, devolverlo tal como está
            if (targetType.IsAssignableFrom(value.GetType()))
                return value;

            // Manejar tipos nullable
            Type underlyingType = Nullable.GetUnderlyingType(targetType) ?? targetType;

            // Conversiones específicas
            if (underlyingType == typeof(int) && value is long longValue)
                return (int)longValue;
            
            if (underlyingType == typeof(decimal) && (value is double || value is float))
                return Convert.ToDecimal(value);

            if (underlyingType == typeof(DateTime) && value is string dateString)
                return DateTime.Parse(dateString);

            // Conversión genérica
            try
            {
                return Convert.ChangeType(value, underlyingType);
            }
            catch
            {
                return value;
            }
        }

        /// <summary>
        /// Verifica si un tipo es nullable
        /// </summary>
        private bool IsNullableType(Type type)
        {
            return !type.IsValueType || Nullable.GetUnderlyingType(type) != null;
        }

        public string GetTable()
        {
            return Table;
        }

        public Dictionary<string, object?> GetAttributes()
        {
            return _attributes;
        }

        public string[] GetFillable()
        {
            return Fillable;
        }

        /// <summary>
        /// Marca el modelo como existente en la base de datos
        /// </summary>
        public T MarkAsExisting()
        {
            _exists = true;
            // Asegurarse de que el ID esté en los atributos
            var idProperty = typeof(T).GetProperty("Id");
            if (idProperty != null)
            {
                var idValue = idProperty.GetValue(this);
                if (idValue != null)
                {
                    _attributes[PrimaryKey] = idValue;
                }
            }
            return (T)this;
        }

        /// <summary>
        /// Verifica si el modelo existe en la base de datos
        /// </summary>
        public bool Exists()
        {
            return _exists;
        }
    }
}