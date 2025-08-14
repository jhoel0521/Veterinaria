using System;
using Veterinaria.DataLayer.Entities;

namespace Veterinaria.DataLayer.Entities
{
    /// <summary>
    /// Modelo Persona - Equivalente a un modelo Eloquent en Laravel
    /// Representa la tabla 'Persona' en la base de datos
    /// </summary>
    public class Persona : Model<Persona>
    {
        // Configuración del modelo - Equivalente a las propiedades protected de PHP
        protected override string Table { get; set; } = "Persona";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "nombre", "apellido", "email", "usuario", "contrasena", 
            "telefono", "direccion", "activo"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas que mapean a las columnas de la base de datos
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Usuario { get; set; } = string.Empty;
        public string Contrasena { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Direccion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Activo { get; set; } = true;

        // Constructors
        public Persona() : base() { }

        public Persona(string nombre, string apellido, string email, string usuario, string contrasena)
        {
            Nombre = nombre;
            Apellido = apellido;
            Email = email;
            Usuario = usuario;
            Contrasena = contrasena;
        }

        // Métodos de conveniencia específicos para Persona
        public static Persona? ValidarLogin(string usuario, string contrasena)
        {
            var user = Where("usuario", usuario)
                   .Where("contrasena", contrasena)
                   .Where("activo", 1)
                   .Get()
                   .FirstOrDefault();
            return user as Persona;
        }

        public static List<Persona> UsuariosActivos()
        {
            return Where("activo", true).Get().Cast<Persona>().ToList();
        }

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        // Override ToString para debugging
        public override string ToString()
        {
            return $"Persona[{Id}]: {NombreCompleto()} ({Usuario})";
        }
    }
}
