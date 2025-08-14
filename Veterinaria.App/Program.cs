using Veterinaria.App.Configuration;
using Veterinaria.DataLayer.Database;

namespace Veterinaria.App
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            try
            {
                // To customize application configuration such as set high DPI settings or default font,
                // see https://aka.ms/applicationconfiguration.
                ApplicationConfiguration.Initialize();

                // Inicializar configuración (similar a Program.cs en MVC)
                AppConfiguration.Initialize();

                // Configurar la base de datos con el connection string del archivo de configuración
                var connectionString = AppConfiguration.GetConnectionString();
                Database.Configure(connectionString);

                Console.WriteLine("[INFO] Aplicación iniciada correctamente");
                Console.WriteLine($"[INFO] Connection String configurado: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

                // Iniciar con el formulario de Login
                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error fatal al inicializar la aplicación:\n\n{ex.Message}\n\nDetalles:\n{ex}", 
                    "Error de Inicialización", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                
                Console.WriteLine($"[ERROR] Error fatal: {ex}");
            }
        }
    }
}