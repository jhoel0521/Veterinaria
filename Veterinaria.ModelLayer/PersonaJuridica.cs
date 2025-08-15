using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo PersonaJuridica - Representa la tabla 'persona_juridica' en la base de datos
    /// Extiende la información de Persona para personas jurídicas (empresas)
    /// </summary>
    public class PersonaJuridica : Model<PersonaJuridica>
    {
        protected override string Table { get; set; } = "persona_juridica";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "razon_social", "nit", "encargado_nombre"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string RazonSocial { get; set; } = string.Empty;
        public string? Nit { get; set; }
        public string? EncargadoNombre { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PersonaJuridica() : base() { }

        public PersonaJuridica(string razonSocial, string? nit = null, string? encargadoNombre = null)
        {
            RazonSocial = razonSocial;
            Nit = nit;
            EncargadoNombre = encargadoNombre;
        }

        // Métodos de conveniencia
        public static PersonaJuridica? BuscarPorNit(string nit)
        {
            return Where("nit", nit).Get().Cast<PersonaJuridica>().FirstOrDefault();
        }

        public static List<PersonaJuridica> BuscarPorRazonSocial(string razonSocial)
        {
            return Where("razon_social", SqlOperator.Like, $"%{razonSocial}%")
                   .Get().Cast<PersonaJuridica>().ToList();
        }

        // Relación con Persona base
        public Persona? GetPersona()
        {
            return Persona.Find(Id);
        }

        /// <summary>
        /// Crea una PersonaJuridica con su Persona base asociada
        /// </summary>
        public static (bool Success, string Message, PersonaJuridica? PersonaJuridica) CrearCompleta(
            string razonSocial, 
            string? nit = null,
            string? encargadoNombre = null,
            string? email = null, 
            string? telefono = null, 
            string? direccion = null)
        {
            try
            {
                // Primero crear la Persona base
                var persona = new Persona("Juridica", email, telefono, direccion);
                
                var personaAttributes = new Dictionary<string, object?>
                {
                    { "tipo", "Juridica" },
                    { "email", email },
                    { "telefono", telefono },
                    { "direccion", direccion }
                };
                
                persona.Fill(personaAttributes).Save();
                var personaId = persona.Id;

                // Luego crear la PersonaJuridica con el mismo ID
                var personaJuridica = new PersonaJuridica(razonSocial, nit, encargadoNombre)
                {
                    Id = personaId
                };

                var juridicaAttributes = new Dictionary<string, object?>
                {
                    { "razon_social", razonSocial },
                    { "nit", nit },
                    { "encargado_nombre", encargadoNombre }
                };

                personaJuridica.Fill(juridicaAttributes).Save();

                return (true, "Persona jurídica creada exitosamente", personaJuridica);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear persona jurídica: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza una PersonaJuridica con su Persona base asociada
        /// </summary>
        public (bool Success, string Message) ActualizarCompleta(
            string razonSocial, 
            string? nit = null,
            string? encargadoNombre = null,
            string? email = null, 
            string? telefono = null, 
            string? direccion = null)
        {
            try
            {
                // Actualizar PersonaJuridica
                var juridicaAttributes = new Dictionary<string, object?>
                {
                    { "razon_social", razonSocial },
                    { "nit", nit },
                    { "encargado_nombre", encargadoNombre }
                };
                
                this.Fill(juridicaAttributes).Save();

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

                return (true, "Persona jurídica actualizada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar persona jurídica: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"PersonaJuridica[{Id}]: {RazonSocial} - NIT: {Nit ?? "N/A"}";
        }
    }
}