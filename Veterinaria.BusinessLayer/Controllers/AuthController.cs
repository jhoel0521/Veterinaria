using System;
using System.Threading.Tasks;
using Veterinaria.ModelLayer;

namespace Veterinaria.BusinessLayer.Controllers
{
    /// <summary>
    /// Controlador de autenticación - Maneja login, logout y validación de usuarios
    /// </summary>
    public class AuthController
    {
        private static Personal? _usuarioActual;

        /// <summary>
        /// Usuario actualmente autenticado
        /// </summary>
        public static Personal? UsuarioActual => _usuarioActual;

        /// <summary>
        /// Verifica si hay un usuario autenticado
        /// </summary>
        public static bool EstaAutenticado => _usuarioActual != null;

        /// <summary>
        /// Intenta autenticar un usuario con sus credenciales
        /// </summary>
        /// <param name="usuario">Nombre de usuario o email</param>
        /// <param name="contrasena">Contraseña del usuario</param>
        /// <returns>Resultado de la autenticación</returns>
        public static async Task<AuthResult> LoginAsync(string usuario, string contrasena)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrWhiteSpace(usuario))
                    return new AuthResult { Exitoso = false, Mensaje = "El usuario es requerido" };

                if (string.IsNullOrWhiteSpace(contrasena))
                    return new AuthResult { Exitoso = false, Mensaje = "La contraseña es requerida" };

                // Buscar usuario por nombre de usuario o email
                var usuarioEncontrado = await Task.Run(() =>
                    Personal.ValidarLogin(usuario, contrasena));

                if (usuarioEncontrado == null)
                {
                    return new AuthResult
                    {
                        Exitoso = false,
                        Mensaje = "Usuario o contraseña incorrectos"
                    };
                }

                // Login exitoso
                _usuarioActual = usuarioEncontrado;

                return new AuthResult
                {
                    Exitoso = true,
                    Mensaje = "Login exitoso",
                    Usuario = usuarioEncontrado
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Exitoso = false,
                    Mensaje = $"Error durante la autenticación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Versión síncrona del login
        /// </summary>
        /// <param name="usuario">Nombre de usuario o email</param>
        /// <param name="contrasena">Contraseña del usuario</param>
        /// <returns>Resultado de la autenticación</returns>
        public static AuthResult Login(string usuario, string contrasena)
        {
            try
            {
                // Validar parámetros
                if (string.IsNullOrWhiteSpace(usuario))
                    return new AuthResult { Exitoso = false, Mensaje = "El usuario es requerido" };

                if (string.IsNullOrWhiteSpace(contrasena))
                    return new AuthResult { Exitoso = false, Mensaje = "La contraseña es requerida" };

                // Buscar usuario por nombre de usuario o email
                var usuarioEncontrado = Personal.ValidarLogin(usuario, contrasena);

                if (usuarioEncontrado == null)
                {
                    return new AuthResult
                    {
                        Exitoso = false,
                        Mensaje = "Usuario o contraseña incorrectos"
                    };
                }

                // Personal no tiene propiedad Activo en la base de datos actual
                // Comentamos esta validación por ahora
                // if (!usuarioEncontrado.Activo)
                // {
                //     return new AuthResult 
                //     { 
                //         Exitoso = false, 
                //         Mensaje = "El usuario está inactivo. Contacte al administrador." 
                //     };
                // }

                // Login exitoso
                _usuarioActual = usuarioEncontrado;

                return new AuthResult
                {
                    Exitoso = true,
                    Mensaje = "Login exitoso",
                    Usuario = usuarioEncontrado
                };
            }
            catch (Exception ex)
            {
                return new AuthResult
                {
                    Exitoso = false,
                    Mensaje = $"Error durante la autenticación: {ex.Message}"
                };
            }
        }

        /// <summary>
        /// Cierra la sesión del usuario actual
        /// </summary>
        public static void Logout()
        {
            _usuarioActual = null;
        }

        /// <summary>
        /// Verifica si el usuario actual tiene un rol específico
        /// </summary>
        /// <param name="rol">Rol a verificar</param>
        /// <returns>True si el usuario tiene el rol</returns>
        public static bool TieneRol(string rol)
        {
            if (!EstaAutenticado) return false;

            // Aquí puedes implementar lógica de roles más compleja
            // Por ahora, verificamos si el usuario es admin
            return _usuarioActual?.Usuario?.ToLower() == "admin";
        }

        /// <summary>
        /// Obtiene información del usuario actual
        /// </summary>
        /// <returns>Información del usuario o null si no está autenticado</returns>
        public static UserInfo? GetUserInfo()
        {
            if (!EstaAutenticado) return null;

            return new UserInfo
            {
                Id = _usuarioActual!.Id,
                Nombre = _usuarioActual.Nombre,
                Apellido = _usuarioActual.Apellido,
                Email = _usuarioActual.Email,
                Usuario = _usuarioActual.Usuario,
                NombreCompleto = $"{_usuarioActual.Nombre} {_usuarioActual.Apellido}"
            };
        }
    }

    /// <summary>
    /// Resultado de una operación de autenticación
    /// </summary>
    public class AuthResult
    {
        public bool Exitoso { get; set; }
        public string Mensaje { get; set; } = string.Empty;
        public Personal? Usuario { get; set; }
    }

    /// <summary>
    /// Información básica del usuario para la UI
    /// </summary>
    public class UserInfo
    {
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string NombreCompleto { get; set; } = string.Empty;
    }
}
