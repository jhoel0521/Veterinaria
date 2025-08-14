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
    /// NOTA: Es reutilizable, no tiene configuración hardcodeada
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
        /// Configura e inicializa la instancia singleton de Database
        /// Debe ser llamado desde la aplicación que consume la librería
        /// </summary>
        /// <param name="connectionString">Connection string obligatorio</param>
        /// <exception cref="ArgumentNullException">Si connectionString es null o vacío</exception>
        /// <exception cref="InvalidOperationException">Si ya existe una instancia configurada</exception>
        public static void Configure(string connectionString)
        {
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new ArgumentNullException(nameof(connectionString), "El connection string no puede ser null o vacío.");
            }

            if (_instance != null)
            {
                throw new InvalidOperationException("Database ya ha sido configurado. No se puede reconfigurar.");
            }

            lock (_lock)
            {
                if (_instance == null)
                {
                    _instance = new Database(connectionString);
                }
            }
        }

        /// <summary>
        /// Obtiene la instancia singleton de Database
        /// Debe haberse llamado Configure() previamente
        /// </summary>
        /// <exception cref="InvalidOperationException">Si no se ha configurado la base de datos</exception>
        public static Database GetInstance()
        {
            if (_instance == null)
            {
                throw new InvalidOperationException(
                    "Database no ha sido configurado. Debe llamar Database.Configure(connectionString) " +
                    "antes de usar GetInstance(). Esto debe hacerse en la aplicación principal, " +
                    "no en la librería DataLayer.");
            }
            return _instance;
        }

        /// <summary>
        /// Método obsoleto mantenido por compatibilidad
        /// Use Configure() + GetInstance() en su lugar
        /// </summary>
        [Obsolete("Use Database.Configure() en la aplicación principal y Database.GetInstance() para obtener la instancia. Este método será removido en futuras versiones.")]
        public static Database GetInstance(string? connectionString)
        {
            if (_instance == null && !string.IsNullOrEmpty(connectionString))
            {
                Configure(connectionString);
            }
            return GetInstance();
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

        /// <summary>
        /// Reinicia la instancia singleton (útil para testing)
        /// </summary>
        internal static void Reset()
        {
            lock (_lock)
            {
                _instance?.Dispose();
                _instance = null;
            }
        }

        public void Dispose()
        {
            _connection?.Close();
            _connection?.Dispose();
        }
    }
}