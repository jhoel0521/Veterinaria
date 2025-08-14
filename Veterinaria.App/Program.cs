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

                // Inicializar configuraci�n (similar a Program.cs en MVC)
                AppConfiguration.Initialize();

                // Configurar la base de datos con el connection string del archivo de configuraci�n
                var connectionString = AppConfiguration.GetConnectionString();
                Database.Configure(connectionString);

                Console.WriteLine("[INFO] Aplicaci�n iniciada correctamente");
                Console.WriteLine($"[INFO] Connection String configurado: {connectionString.Substring(0, Math.Min(50, connectionString.Length))}...");

                // Iniciar con el formulario de Login
                Application.Run(new Login());
            }
            catch (Exception ex)
            {
                MessageBox.Show(
                    $"Error fatal al inicializar la aplicaci�n:\n\n{ex.Message}\n\nDetalles:\n{ex}", 
                    "Error de Inicializaci�n", 
                    MessageBoxButtons.OK, 
                    MessageBoxIcon.Error);
                
                Console.WriteLine($"[ERROR] Error fatal: {ex}");
            }
        }
    }
}