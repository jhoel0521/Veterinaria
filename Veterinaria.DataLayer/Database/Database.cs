using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Configuration;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.DataLayer.Database
{
    public class Database
    {
        private static Database _instance = null;
        private static readonly object _lock = new object();
        private SqlConnection _connection;
        private readonly string _connectionString;

        private Database(string connectionString)
        {
            _connectionString = connectionString;
            Connect();
        }

        public static Database GetInstance(string connectionString = null)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            // Intentar obtener de app.config o variables de entorno
                            connectionString = ConfigurationManager.ConnectionStrings["DefaultConnection"]?.ConnectionString
                                ?? Environment.GetEnvironmentVariable("DB_CONNECTION_STRING");
                        }

                        if (string.IsNullOrEmpty(connectionString))
                        {
                            throw new DatabaseException("Configuración de base de datos inválida. No se encontró connection string.");
                        }

                        _instance = new Database(connectionString);
                    }
                }
            }
            return _instance;
        }

        private void Connect()
        {
            try
            {
                _connection = new SqlConnection(_connectionString);
                _connection.Open();
            }
            catch (SqlException ex)
            {
                throw new DatabaseException($"Error de conexión: {ex.Message}", ex);
            }
        }

        public SqlCommand Query(string sql, Dictionary<string, object>? parameters = null)
        {
            try
            {
                // Log para debugging
                System.Diagnostics.Debug.WriteLine($"SQL: {sql}");
                if (parameters != null)
                {
                    System.Diagnostics.Debug.WriteLine($"Parameters: {string.Join(", ", parameters)}");
                }

                // Asegurar que la conexión esté abierta
                if (_connection.State != System.Data.ConnectionState.Open)
                {
                    Connect();
                }

                var command = new SqlCommand(sql, _connection);

                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                return command;
            }
            catch (SqlException ex)
            {
                throw new QueryException($"Error en la consulta: {ex.Message}", sql, ex);
            }
        }

        public QueryBuilder.QueryBuilder Table(string table, Type? modelType = null)
        {
            return new QueryBuilder.QueryBuilder(this, table, modelType);
        }

        public SqlConnection GetConnection()
        {
            if (_connection.State != System.Data.ConnectionState.Open)
            {
                Connect();
            }
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}
