using System;
using System.Collections.Generic;
using Microsoft.Data.SqlClient;
using System.Data;

namespace Veterinaria.DataLayer.Database
{
    /// <summary>
    /// Clase Database - Equivalente a DB.php
    /// Patrón Singleton para manejar conexiones a la base de datos
    /// Factory para crear QueryBuilder instances
    /// </summary>
    public class Database
    {
        private static Database? _instance = null;
        private static readonly object _lock = new object();
        private SqlConnection? _connection;
        private readonly string _connectionString;

        private Database(string connectionString)
        {
            _connectionString = connectionString;
            Connect();
        }

        /// <summary>
        /// Obtiene la instancia singleton de Database
        /// </summary>
        public static Database GetInstance(string? connectionString = null)
        {
            if (_instance == null)
            {
                lock (_lock)
                {
                    if (_instance == null)
                    {
                        if (string.IsNullOrEmpty(connectionString))
                        {
                            // Connection string por defecto para SQL Server local
                            connectionString = "server=.\\SQLEXPRESS;database=SistemaVeterinario;integrated security=true;TrustServerCertificate=true;";
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
                Console.WriteLine($"[DEBUG] Conexión establecida exitosamente");
            }
            catch (Exception ex)
            {
                throw new DatabaseException($"Error de conexión: {ex.Message}");
            }
        }

        /// <summary>
        /// Ejecuta una consulta SQL con parámetros
        /// Equivalente al método query() de PHP
        /// </summary>
        public SqlCommand Query(string sql, Dictionary<string, object>? parameters = null)
        {
            // Debug: Mostrar SQL y parámetros como en PHP
            Console.WriteLine($"[DEBUG] SQL: {sql}");
            if (parameters != null && parameters.Count > 0)
            {
                Console.WriteLine($"[DEBUG] Params: {string.Join(", ", parameters)}");
            }

            try
            {
                // Reabrir conexión si está cerrada
                if (_connection?.State != ConnectionState.Open)
                {
                    Connect();
                }

                var command = new SqlCommand(sql, _connection);

                // Agregar parámetros si existen
                if (parameters != null)
                {
                    foreach (var param in parameters)
                    {
                        command.Parameters.AddWithValue(param.Key, param.Value ?? DBNull.Value);
                    }
                }

                return command;
            }
            catch (Exception ex)
            {
                throw new QueryException($"Error en la consulta: {ex.Message}", sql);
            }
        }

        /// <summary>
        /// Crea un QueryBuilder para la tabla especificada
        /// Equivalente al método table() de PHP
        /// </summary>
        public QueryBuilder.QueryBuilder Table(string table, Type? modelType = null)
        {
            return new QueryBuilder.QueryBuilder(this, table, modelType);
        }

        public SqlConnection? GetConnection()
        {
            return _connection;
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}