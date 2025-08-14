using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Linq;
using System.Reflection;
using Veterinaria.DataLayer.Database;
using Veterinaria.DataLayer.Entities;

namespace Veterinaria.DataLayer.QueryBuilder
{
    public class QueryBuilder
    {
        private readonly Database.Database _db;
        private readonly string _table;
        private Type? _modelType;
        private List<string> _selectColumns = new List<string>();
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
            _selectColumns.AddRange(columns);
            return this;
        }

        public QueryBuilder Where(string column, string operatorSymbol, object? value = null)
        {
            if (value == null)
            {
                value = operatorSymbol;
                operatorSymbol = "=";
            }

            var paramName = $"@param{_parameterCount++}";
            _whereConditions.Add($"{column} {operatorSymbol} {paramName}");
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
            var paramNames = new List<string>();
            
            for (int i = 0; i < valuesList.Count; i++)
            {
                var paramName = $"@param{_parameterCount++}";
                paramNames.Add(paramName);
                _parameters[paramName] = valuesList[i];
            }

            _whereConditions.Add($"{column} IN ({string.Join(", ", paramNames)})");
            return this;
        }

        public QueryBuilder WhereBetween(string column, object value1, object value2)
        {
            var param1 = $"@param{_parameterCount++}";
            var param2 = $"@param{_parameterCount++}";
            
            _whereConditions.Add($"{column} BETWEEN {param1} AND {param2}");
            _parameters[param1] = value1;
            _parameters[param2] = value2;
            return this;
        }

        public QueryBuilder OrderBy(string column, string direction = "ASC")
        {
            _orderBy = $"ORDER BY {column} {direction.ToUpper()}";
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

        public List<T> Get<T>() where T : Model, new()
        {
            var sql = BuildSelectSql();
            var results = new List<T>();

            using (var command = _db.Query(sql, _parameters))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var model = new T();
                        var attributes = new Dictionary<string, object>();

                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            var fieldValue = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            attributes[fieldName] = fieldValue;
                        }

                        model.Fill(attributes, true); // true indica que viene de la DB
                        results.Add(model);
                    }
                }
            }

            // Cargar relaciones eagerly loaded
            if (_eagerLoads.Any())
            {
                foreach (var model in results)
                {
                    LoadRelations(model);
                }
            }

            return results;
        }

        public List<object> Get()
        {
            if (_modelType != null)
            {
                var method = typeof(QueryBuilder).GetMethod("Get", Type.EmptyTypes);
                var genericMethod = method?.MakeGenericMethod(_modelType);
                return (List<object>)(genericMethod?.Invoke(this, null) ?? new List<object>());
            }

            var sql = BuildSelectSql();
            var results = new List<object>();

            using (var command = _db.Query(sql, _parameters))
            {
                using (var reader = command.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        var row = new Dictionary<string, object>();
                        for (int i = 0; i < reader.FieldCount; i++)
                        {
                            var fieldName = reader.GetName(i);
                            var fieldValue = reader.IsDBNull(i) ? null : reader.GetValue(i);
                            row[fieldName] = fieldValue;
                        }
                        results.Add(row);
                    }
                }
            }

            return results;
        }

        public T? First<T>() where T : Model, new()
        {
            return Limit(1).Get<T>().FirstOrDefault();
        }

        public object? First()
        {
            return Limit(1).Get().FirstOrDefault();
        }

        public int Count()
        {
            var originalSelect = _selectColumns.ToList();
            _selectColumns.Clear();
            _selectColumns.Add("COUNT(*) as count");

            var sql = BuildSelectSql();
            
            using (var command = _db.Query(sql, _parameters))
            {
                var result = command.ExecuteScalar();
                
                // Restaurar select original
                _selectColumns = originalSelect;
                
                return Convert.ToInt32(result);
            }
        }

        private string BuildSelectSql()
        {
            var selectClause = _selectColumns.Any() ? string.Join(", ", _selectColumns) : "*";
            var sql = $"SELECT {selectClause} FROM {_table}";

            if (_whereConditions.Any())
            {
                sql += " WHERE " + string.Join(" AND ", _whereConditions);
            }

            if (!string.IsNullOrEmpty(_orderBy))
            {
                sql += " " + _orderBy;
            }

            if (_limit.HasValue)
            {
                // En SQL Server usamos TOP en lugar de LIMIT
                sql = sql.Replace($"SELECT {selectClause}", $"SELECT TOP {_limit.Value} {selectClause}");
            }

            return sql;
        }

        private void LoadRelations<T>(T model) where T : Model
        {
            foreach (var relation in _eagerLoads)
            {
                var method = typeof(T).GetMethod(relation, BindingFlags.Public | BindingFlags.Instance);
                if (method != null)
                {
                    var relationResult = method.Invoke(model, null);
                    model.SetRelation(relation, relationResult);
                }
            }
        }

        public Dictionary<string, object> GetBindings()
        {
            return _parameters;
        }
    }
}
