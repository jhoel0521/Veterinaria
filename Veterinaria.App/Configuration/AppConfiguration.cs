using Microsoft.Extensions.Configuration;

namespace Veterinaria.App.Configuration
{
    /// <summary>
    /// Clase de configuraci�n est�tica que maneja la lectura de configuraciones
    /// Similar al patr�n usado en ASP.NET Core MVC
    /// </summary>
    public static class AppConfiguration
    {
        private static IConfiguration? _configuration;

        /// <summary>
        /// Inicializa la configuraci�n leyendo el archivo appsettings.json
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
                throw new InvalidOperationException("La configuraci�n no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var connectionString = _configuration.GetConnectionString(name);
            
            if (string.IsNullOrEmpty(connectionString))
            {
                throw new InvalidOperationException($"No se encontr� el connection string '{name}' en la configuraci�n.");
            }

            return connectionString;
        }

        /// <summary>
        /// Obtiene un valor de configuraci�n por clave
        /// </summary>
        /// <param name="key">Clave de configuraci�n</param>
        /// <returns>Valor de configuraci�n</returns>
        public static string? GetValue(string key)
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuraci�n no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            return _configuration[key];
        }

        /// <summary>
        /// Obtiene un valor de configuraci�n tipado
        /// </summary>
        /// <typeparam name="T">Tipo del valor</typeparam>
        /// <param name="key">Clave de configuraci�n</param>
        /// <param name="defaultValue">Valor por defecto si no se encuentra</param>
        /// <returns>Valor tipado de configuraci�n</returns>
        public static T GetValue<T>(string key, T defaultValue = default(T))
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuraci�n no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var value = _configuration[key];
            if (value == null)
            {
                return defaultValue;
            }

            // Conversi�n manual b�sica para tipos comunes
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

                // Para otros tipos, intentar conversi�n directa
                return (T)Convert.ChangeType(value, typeof(T));
            }
            catch
            {
                return defaultValue;
            }
        }

        /// <summary>
        /// Bind una secci�n de configuraci�n a un objeto (versi�n simplificada)
        /// </summary>
        /// <typeparam name="T">Tipo del objeto</typeparam>
        /// <param name="sectionKey">Clave de la secci�n</param>
        /// <returns>Objeto configurado</returns>
        public static T GetSection<T>(string sectionKey) where T : new()
        {
            if (_configuration == null)
            {
                throw new InvalidOperationException("La configuraci�n no ha sido inicializada. Llame a AppConfiguration.Initialize() primero.");
            }

            var section = new T();
            var configSection = _configuration.GetSection(sectionKey);
            
            // Binding manual b�sico usando reflexi�n
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
                        // Ignorar errores de conversi�n
                    }
                }
            }

            return section;
        }

        /// <summary>
        /// Verifica si la configuraci�n est� inicializada
        /// </summary>
        public static bool IsInitialized => _configuration != null;
    }
}