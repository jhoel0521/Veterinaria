using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Animal - Representa la tabla 'animal' en la base de datos
    /// Representa las mascotas de los clientes
    /// </summary>
    public class Animal : Model<Animal>
    {
        protected override string Table { get; set; } = "animal";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "tipo", "nombre", "fecha_nacimiento", "persona_id"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string? Tipo { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public DateTime? FechaNacimiento { get; set; }
        public int PersonaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Animal() : base() { }

        public Animal(string nombre, string? tipo = null, DateTime? fechaNacimiento = null, int personaId = 0)
        {
            Nombre = nombre;
            Tipo = tipo;
            FechaNacimiento = fechaNacimiento;
            PersonaId = personaId;
        }

        // Métodos de conveniencia
        public static List<Animal> BuscarPorNombre(string nombre)
        {
            return Where("nombre", SqlOperator.Like, $"%{nombre}%")
                   .Get().Cast<Animal>().ToList();
        }

        public static List<Animal> PorTipo(string tipo)
        {
            return Where("tipo", SqlOperator.Like, $"%{tipo}%")
                   .Get().Cast<Animal>().ToList();
        }

        public static List<Animal> PorPersona(int personaId)
        {
            return Where("persona_id", personaId)
                   .OrderBy("nombre")
                   .Get().Cast<Animal>().ToList();
        }

        // Relación con Persona (dueño)
        public Persona? GetPersona()
        {
            return Persona.Find(PersonaId);
        }

        // Relación con Histórico
        public List<Historico> GetHistoricos()
        {
            return Historico.Where("animal_id", Id).Get().Cast<Historico>().ToList();
        }

        /// <summary>
        /// Calcula la edad del animal en años
        /// </summary>
        public int? GetEdadEnAnios()
        {
            if (!FechaNacimiento.HasValue) return null;
            
            var hoy = DateTime.Today;
            var edad = hoy.Year - FechaNacimiento.Value.Year;
            
            if (FechaNacimiento.Value.Date > hoy.AddYears(-edad))
                edad--;
                
            return Math.Max(0, edad);
        }

        /// <summary>
        /// Verifica si el animal es cachorro (menos de 1 año)
        /// </summary>
        public bool EsCachorro()
        {
            var edad = GetEdadEnAnios();
            return edad.HasValue && edad < 1;
        }

        /// <summary>
        /// Verifica si el animal es adulto (1 año o más)
        /// </summary>
        public bool EsAdulto()
        {
            var edad = GetEdadEnAnios();
            return edad.HasValue && edad >= 1;
        }

        /// <summary>
        /// Obtiene el nombre del dueño
        /// </summary>
        public string GetNombreDueno()
        {
            var persona = GetPersona();
            return persona?.GetNombreCompleto() ?? "Dueño no encontrado";
        }

        /// <summary>
        /// Obtiene información detallada del animal
        /// </summary>
        public string GetDescripcionCompleta()
        {
            var descripcion = $"{Nombre}";
            
            if (!string.IsNullOrEmpty(Tipo))
                descripcion += $" - {Tipo}";
                
            var edad = GetEdadEnAnios();
            if (edad.HasValue)
            {
                descripcion += $" - {edad} año{(edad != 1 ? "s" : "")}";
                if (EsCachorro())
                    descripcion += " (Cachorro)";
            }
            
            return descripcion;
        }

        public override string ToString()
        {
            return $"Animal[{Id}]: {GetDescripcionCompleta()} - Dueño: {GetNombreDueno()}";
        }
    }
}