using Microsoft.Extensions.Configuration;

namespace Veterinaria.App.Configuration
{
    /// <summary>
    /// Clase de configuración estática que maneja la lectura de configuraciones
    /// Similar al patrón usado en ASP.NET Core MVC
    /// </summary>
    public static class AppConfiguration
    {
        private static IConfiguration? _configuration;

        /// <summary>
        /// Inicializa la configuración leyendo el archivo appsettings.json
        /// </summary>
        public static void Initialize()
        {
            var builder = new ConfigurationBuilder()
                .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
                .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

            _configuration = builder.Build();
        }

        /// <summary>
        /// Obtiene el connection string por nombre
        /// </summary>
        /// <param name="name">Nombre del connection string (por defecto: "DefaultConnection")</param>
        /// <returns>Connection string configurado</returns>
        public static string GetConnectionString(string name = "DefaultConnection")
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuración no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var connectionString = _configuration.GetConnectionString(name);
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"No se encontró el connection string '{name}' en la configuración.");
            }

            return connectionString;
        }

        /// <summary>
        /// Obtiene un valor de configuración por clave
        /// </summary>
        /// <param name="key">Clave de configuración</param>
        /// <returns>Valor de configuración</returns>
        public static string? GetValue(string key)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuración no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            return _configuration[key];
        }

        /// <summary>
        /// Obtiene un valor de configuración tipado
        /// </summary>
        /// <typeparam name="T">Tipo del valor</typeparam>
        /// <param name="key">Clave de configuración</param>
        /// <param name="defaultValue">Valor por defecto si no se encuentra</param>
        /// <returns>Valor tipado de configuración</returns>
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuración no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var value = _configuration[key];
            if (value == null)
            {
                return defaultValue;
            }

            // Conversión manual básica para tipos comunes
            try
            {
                if (typeof(T) == typeof(string))
                    return (T)(object)value;
                if (typeof(T) == typeof(int))
                    return (T)(object)int.Parse(value);
                if (typeof(T) == typeof(bool))
                    return (T)(object)bool.Parse(value);
                if (typeof(T) == typeof(double))
                    return (T)(object)double.Parse(value);

                // Para otros tipos, intentar conversión directa
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Bind una sección de configuración a un objeto (versión simplificada)
        /// </summary>
        /// <typeparam name="T">Tipo del objeto</typeparam>
        /// <param name="sectionKey">Clave de la sección</param>
        /// <returns>Objeto configurado</returns>
        public static T GetSection<T>(string sectionKey) where T : new()
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuración no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var section = new T();
            var configSection = _configuration.GetSection(sectionKey);
            
            // Binding manual básico usando reflexión
            var properties = typeof(T).GetProperties();
            foreach (var property in properties)
            {
                var value = configSection[property.Name];
                if (value != null && property.CanWrite)
                {
                    try
                    {
                        var convertedValue = Convert.ChangeType(value, property.PropertyType);
                        property.SetValue(section, convertedValue);
                    }
                    catch
                    {
                        // Ignorar errores de conversión
                    }
                }
            }

            return section;
        }

        /// <summary>
        /// Verifica si la configuración está inicializada
        /// </summary>
        public static bool IsInitialized => _configuration != null;
    }
}