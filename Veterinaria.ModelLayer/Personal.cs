using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Personal - Representa la tabla 'personal' en la base de datos
    /// Representa a los empleados del sistema veterinario
    /// </summary>
    public class Personal : Model<Personal>
    {
        protected override string Table { get; set; } = "personal";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "fecha_contratacion", "nombre", "apellido", "email", "usuario", 
            "contrasena", "telefono", "direccion"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public DateTime? FechaContratacion { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Personal() : base() { }

        public Personal(string nombre, string apellido, string email, string usuario, string contrasena)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Usuario = usuario;
            Contrasena = contrasena;
        }

        // Métodos de conveniencia
        public static Personal? BuscarPorUsuario(string usuario)
        {
            return Where("usuario", usuario).Get().Cast<Personal>().FirstOrDefault();
        }

        public static Personal? BuscarPorEmail(string email)
        {
            return Where("email", email).Get().Cast<Personal>().FirstOrDefault();
        }

        public static List<Personal> BuscarPorNombre(string nombre)
        {
            return Where("nombre", SqlOperator.Like, $"%{nombre}%")
                   .Get().Cast<Personal>().ToList();
        }

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        // Relación con PersonalRol
        public List<PersonalRol> GetRoles()
        {
            return PersonalRol.Where("personal_id", Id).Get().Cast<PersonalRol>().ToList();
        }

        /// <summary>
        /// Obtiene la lista de roles como strings
        /// </summary>
        public List<string> GetRolesString()
        {
            return GetRoles().Select(r => r.Rol).ToList();
        }

        /// <summary>
        /// Verifica si el personal tiene un rol específico
        /// </summary>
        public bool TieneRol(string rol)
        {
            return GetRoles().Any(r => r.Rol.Equals(rol, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Verifica si es administrador
        /// </summary>
        public bool EsAdministrador()
        {
            return TieneRol("Administrador");
        }

        /// <summary>
        /// Verifica si es veterinario
        /// </summary>
        public bool EsVeterinario()
        {
            return TieneRol("Veterinario");
        }

        /// <summary>
        /// Verifica las credenciales de login
        /// </summary>
        public static Personal? ValidarCredenciales(string usuario, string contrasena)
        {
            var personal = BuscarPorUsuario(usuario);
            if (personal != null && personal.Contrasena == contrasena)
            {
                return personal;
            }
            return null;
        }

        /// <summary>
        /// Asigna un rol al personal
        /// </summary>
        public (bool Success, string Message) AsignarRol(string rol)
        {
            try
            {
                // Verificar que el rol sea válido
                var rolesValidos = new[] { "Veterinario", "Auxiliar", "Recepcionista", "Administrador" };
                if (!rolesValidos.Contains(rol))
                {
                    return (false, "Rol no válido");
                }

                // Verificar si ya tiene el rol
                if (TieneRol(rol))
                {
                    return (false, "El personal ya tiene este rol");
                }

                var personalRol = new PersonalRol(Id, rol);
                var attributes = new Dictionary<string, object?>
                {
                    { "personal_id", Id },
                    { "rol", rol }
                };
                
                personalRol.Fill(attributes).Save();

                return (true, "Rol asignado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al asignar rol: {ex.Message}");
            }
        }

        // Relación con Diagnósticos
        public List<Diagnostico> GetDiagnosticos()
        {
            return Diagnostico.Where("personal_id", Id).Get().Cast<Diagnostico>().ToList();
        }

        public override string ToString()
        {
            return $"Personal[{Id}]: {NombreCompleto()} - {Usuario} - Roles: {string.Join(", ", GetRolesString())}";
        }
    }
}