using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Data;
using Veterinaria.DataLayer.Database;

namespace Veterinaria.DataLayer.QueryBuilder
{
    public class QueryBuilder
    {
        private readonly Database.Database _db;
        private readonly string _table;
        private Type? _modelType;
        
        private string _selectClause = "*";
        private List<string> _whereConditions = new List<string>();
        private Dictionary<string, object> _parameters = new Dictionary<string, object>();
        private string _orderBy = string.Empty;
        private int? _limit = null;
        private List<string> _eagerLoads = new List<string>();
        private int _parameterCount = 0;

        public QueryBuilder(Database.Database db, string table, Type? modelType = null)
        {
            _db = db;
            _table = table;
            _modelType = modelType;
        }

        public QueryBuilder Select(params string[] columns)
        {
            _selectClause = columns.Length > 0 ? string.Join(", ", columns) : "*";
            return this;
        }

        public QueryBuilder Where(string column, SqlOperator operatorOrValue, object? value = null)
        {
            string operatorStr;
            object actualValue;

            if (value == null)
            {
                operatorStr = operatorOrValue.ToSqlString();
                throw new ArgumentException("Debe proporcionar un valor cuando usa un operador específico");
            }
            else
            {
                operatorStr = operatorOrValue.ToSqlString();
                actualValue = value;
            }

            var paramName = $"@param{_parameterCount++}";
            _whereConditions.Add($"{column} {operatorStr} {paramName}");
            _parameters[paramName] = actualValue;

            return this;
        }

        /// <summary>
        /// WHERE con valor simple (igualdad por defecto)
        /// Ejemplo: Where("usuario", "admin")
        /// </summary>
        public QueryBuilder Where(string column, object value)
        {
            var paramName = $"@param{_parameterCount++}";
            _whereConditions.Add($"{column} = {paramName}");
            _parameters[paramName] = value;

            return this;
        }

        public QueryBuilder WhereRaw(string sql, Dictionary<string, object>? bindings = null)
        {
            _whereConditions.Add(sql);
            
            if (bindings != null)
            {
                foreach (var binding in bindings)
                {
                    _parameters[binding.Key] = binding.Value;
                }
            }

            return this;
        }

        public QueryBuilder WhereIn(string column, IEnumerable<object> values)
        {
            var valuesList = values.ToList();
            if (!valuesList.Any()) return this;

            var paramNames = new List<string>();
            foreach (var value in valuesList)
            {
                var paramName = $"@param{_parameterCount++}";
                paramNames.Add(paramName);
                _parameters[paramName] = value;
            }

            _whereConditions.Add($"{column} IN ({string.Join(", ", paramNames)})");
            return this;
        }

        public QueryBuilder WhereBetween(string column, object[] values)
        {
            if (values.Length < 2) return this;

            var param1 = $"@param{_parameterCount++}";
            var param2 = $"@param{_parameterCount++}";

            _whereConditions.Add($"{column} BETWEEN {param1} AND {param2}");
            _parameters[param1] = values[0];
            _parameters[param2] = values.Length > 1 ? values[1] : values[0];

            return this;
        }

        public QueryBuilder OrderBy(string column, string direction = "ASC")
        {
            _orderBy = $"ORDER BY {column} {direction}";
            return this;
        }

        public QueryBuilder Limit(int limit)
        {
            _limit = limit;
            return this;
        }

        public QueryBuilder With(params string[] relationships)
        {
            _eagerLoads.AddRange(relationships);
            return this;
        }

        public int Count()
        {
            var originalSelect = _selectClause;
            _selectClause = "COUNT(*) as count";
            
            var results = Get();
            _selectClause = originalSelect;

            if (results.Any())
            {
                var firstResult = results.First();
                if (firstResult is Dictionary<string, object> dict && dict.ContainsKey("count"))
                {
                    return Convert.ToInt32(dict["count"]);
                }
            }

            return 0;
        }

        public List<object> Get()
        {
            var sql = BuildSelectSql();
            
            using var command = _db.Query(sql, _parameters);
            using var reader = command.ExecuteReader();
            
            var results = new List<object>();
            
            if (_modelType != null && !IsAggregateQuery(sql))
            {
                while (reader.Read())
                {
                    var modelInstance = CreateModelInstance(reader);
                    results.Add(modelInstance);
                }

                if (_eagerLoads.Any())
                {
                    LoadRelations(results);
                }
            }
            else
            {
                while (reader.Read())
                {
                    var row = new Dictionary<string, object>();
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        row[reader.GetName(i)] = reader.GetValue(i);
                    }
                    results.Add(row);
                }
            }

            return results;
        }

        public object? First()
        {
            var results = Limit(1).Get();
            return results.FirstOrDefault();
        }

        private string BuildSelectSql()
        {
            var sql = $"SELECT {_selectClause} FROM {_table}";

            if (_whereConditions.Any())
            {
                sql += " WHERE " + string.Join(" AND ", _whereConditions);
            }

            if (!string.IsNullOrEmpty(_orderBy))
            {
                sql += $" {_orderBy}";
            }

            if (_limit.HasValue)
            {
                sql = sql.Replace($"SELECT {_selectClause}", $"SELECT TOP {_limit.Value} {_selectClause}");
            }

            return sql;
        }

        private bool IsValidOperator(string operatorStr)
        {
            var validOperators = new[]
            {
                "=", "<", ">", "<=", ">=", "<>", "!=", "LIKE", "NOT LIKE",
                "BETWEEN", "IN", "NOT IN"
            };

            return validOperators.Contains(operatorStr.ToUpper());
        }

        private bool IsAggregateQuery(string sql)
        {
            return sql.Contains("COUNT(") || sql.Contains("AVG(") || 
                   sql.Contains("SUM(") || sql.Contains("MIN(") || sql.Contains("MAX(");
        }

        private object CreateModelInstance(SqlDataReader reader)
        {
            var instance = Activator.CreateInstance(_modelType!);
            var properties = _modelType!.GetProperties();

            for (int i = 0; i < reader.FieldCount; i++)
            {
                var columnName = reader.GetName(i);
                var value = reader.GetValue(i);

                // Intentar primero con el nombre exacto
                var property = properties.FirstOrDefault(p => 
                    p.Name.Equals(columnName, StringComparison.OrdinalIgnoreCase));

                // Si no se encuentra, intentar convertir de snake_case a PascalCase
                if (property == null)
                {
                    string pascalCaseName = ConvertToPascalCase(columnName);
                    property = properties.FirstOrDefault(p => 
                        p.Name.Equals(pascalCaseName, StringComparison.OrdinalIgnoreCase));
                }

                if (property != null && property.CanWrite)
                {
                    if (value != DBNull.Value && value != null)
                    {
                        // Convertir el tipo si es necesario
                        var convertedValue = ConvertValueToPropertyType(value, property.PropertyType);
                        property.SetValue(instance, convertedValue);
                    }
                    else if (IsNullableType(property.PropertyType))
                    {
                        property.SetValue(instance, null);
                    }
                }
            }

            return instance!;
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

        private void LoadRelations(List<object> results)
        {
            // TODO: Implementar eager loading
        }

        public Dictionary<string, object> GetBindings()
        {
            return _parameters;
        }
    }
}
