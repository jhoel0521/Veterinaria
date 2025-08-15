using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Persona - Representa la tabla 'persona' en la base de datos
    /// Clase base para PersonaFisica y PersonaJuridica
    /// </summary>
    public class Persona : Model<Persona>
    {
        protected override string Table { get; set; } = "persona";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "tipo", "email", "direccion", "telefono"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades p�blicas
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // 'Fisica' o 'Juridica'
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public string? Telefono { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Persona() : base() { }

        public Persona(string tipo, string? email = null, string? telefono = null, string? direccion = null)
        {
            Tipo = tipo;
            Email = email;
            Telefono = telefono;
            Direccion = direccion;
        }

        // M�todos de conveniencia
        public static List<Persona> PersonasFisicas()
        {
            return Where("tipo", "Fisica").Get().Cast<Persona>().ToList();
        }

        public static List<Persona> PersonasJuridicas()
        {
            return Where("tipo", "Juridica").Get().Cast<Persona>().ToList();
        }

        public bool EsFisica()
        {
            return Tipo.Equals("Fisica", StringComparison.OrdinalIgnoreCase);
        }

        public bool EsJuridica()
        {
            return Tipo.Equals("Juridica", StringComparison.OrdinalIgnoreCase);
        }

        // Relaci�n con PersonaFisica
        public PersonaFisica? GetPersonaFisica()
        {
            if (!EsFisica()) return null;
            return PersonaFisica.Find(Id);
        }

        // Relaci�n con PersonaJuridica
        public PersonaJuridica? GetPersonaJuridica()
        {
            if (!EsJuridica()) return null;
            return PersonaJuridica.Find(Id);
        }

        // Relaci�n con Animales
        public List<Animal> GetAnimales()
        {
            return Animal.Where("persona_id", Id).Get().Cast<Animal>().ToList();
        }

        // Relaci�n con Facturas
        public List<Factura> GetFacturas()
        {
            return Factura.Where("persona_id", Id).Get().Cast<Factura>().ToList();
        }

        /// <summary>
        /// Obtiene el nombre completo de la persona (f�sico o jur�dico)
        /// </summary>
        public string GetNombreCompleto()
        {
            if (EsFisica())
            {
                var personaFisica = GetPersonaFisica();
                return personaFisica != null ? $"{personaFisica.Nombre} {personaFisica.Apellido}" : "Persona F�sica";
            }
            else if (EsJuridica())
            {
                var personaJuridica = GetPersonaJuridica();
                return personaJuridica?.RazonSocial ?? "Persona Jur�dica";
            }
            return "Persona Desconocida";
        }

        public override string ToString()
        {
            return $"Persona[{Id}]: {Tipo} - {GetNombreCompleto()} - {Email}";
        }
    }
}