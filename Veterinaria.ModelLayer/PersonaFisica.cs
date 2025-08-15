using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo PersonaFisica - Representa la tabla 'persona_fisica' en la base de datos
    /// Extiende la información de Persona para personas físicas
    /// </summary>
    public class PersonaFisica : Model<PersonaFisica>
    {
        protected override string Table { get; set; } = "persona_fisica";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "ci", "nombre", "apellido"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string? Ci { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PersonaFisica() : base() { }

        public PersonaFisica(string nombre, string apellido, string? ci = null)
        {
            Nombre = nombre;
            Apellido = apellido;
            Ci = ci;
        }

        // Métodos de conveniencia
        public static PersonaFisica? BuscarPorCi(string ci)
        {
            return Where("ci", ci).Get().Cast<PersonaFisica>().FirstOrDefault();
        }

        public static List<PersonaFisica> BuscarPorNombre(string nombre)
        {
            return Where("nombre", SqlOperator.Like, $"%{nombre}%")
                   .Get().Cast<PersonaFisica>().ToList();
        }

        public static List<PersonaFisica> BuscarPorApellido(string apellido)
        {
            return Where("apellido", SqlOperator.Like, $"%{apellido}%")
                   .Get().Cast<PersonaFisica>().ToList();
        }

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        // Relación con Persona base
        public Persona? GetPersona()
        {
            return Persona.Find(Id);
        }

        /// <summary>
        /// Crea una PersonaFisica con su Persona base asociada
        /// </summary>
        public static (bool Success, string Message, PersonaFisica? PersonaFisica) CrearCompleta(
            string nombre, 
            string apellido, 
            string? ci = null,
            string? email = null, 
            string? telefono = null, 
            string? direccion = null)
        {
            try
            {
                // Primero crear la Persona base
                var persona = new Persona("Fisica", email, telefono, direccion);
                
                var personaAttributes = new Dictionary<string, object?>
                {
                    { "tipo", "Fisica" },
                    { "email", email },
                    { "telefono", telefono },
                    { "direccion", direccion }
                };
                
                persona.Fill(personaAttributes).Save();
                var personaId = persona.Id;

                // Luego crear la PersonaFisica con el mismo ID
                var personaFisica = new PersonaFisica(nombre, apellido, ci)
                {
                    Id = personaId
                };

                var fisicaAttributes = new Dictionary<string, object?>
                {
                    { "ci", ci },
                    { "nombre", nombre },
                    { "apellido", apellido }
                };

                personaFisica.Fill(fisicaAttributes).Save();

                return (true, "Persona física creada exitosamente", personaFisica);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear persona física: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza una PersonaFisica con su Persona base asociada
        /// </summary>
        public (bool Success, string Message) ActualizarCompleta(
            string nombre, 
            string apellido, 
            string? ci = null,
            string? email = null, 
            string? telefono = null, 
            string? direccion = null)
        {
            try
            {
                // Actualizar PersonaFisica
                var fisicaAttributes = new Dictionary<string, object?>
                {
                    { "ci", ci },
                    { "nombre", nombre },
                    { "apellido", apellido }
                };
                
                this.Fill(fisicaAttributes).Save();

                // Actualizar Persona base
                var persona = GetPersona();
                if (persona != null)
                {
                    var personaAttributes = new Dictionary<string, object?>
                    {
                        { "email", email },
                        { "telefono", telefono },
                        { "direccion", direccion }
                    };
                    
                    persona.Fill(personaAttributes).Save();
                }

                return (true, "Persona física actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar persona física: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"PersonaFisica[{Id}]: {NombreCompleto()} - CI: {Ci ?? "N/A"}";
        }
    }
}