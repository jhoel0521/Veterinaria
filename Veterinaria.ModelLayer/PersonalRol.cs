using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo PersonalRol - Representa la tabla 'personal_rol' en la base de datos
    /// Gestiona los roles asignados al personal
    /// </summary>
    public class PersonalRol : Model<PersonalRol>
    {
        protected override string Table { get; set; } = "personal_rol";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "personal_id", "rol"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public int PersonalId { get; set; }
        public string Rol { get; set; } = string.Empty; // Veterinario, Auxiliar, Recepcionista, Administrador
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public PersonalRol() : base() { }

        public PersonalRol(int personalId, string rol)
        {
            PersonalId = personalId;
            Rol = rol;
        }

        // Métodos de conveniencia
        public static List<PersonalRol> PorPersonal(int personalId)
        {
            return Where("personal_id", personalId)
                   .OrderBy("rol")
                   .Get().Cast<PersonalRol>().ToList();
        }

        public static List<PersonalRol> PorRol(string rol)
        {
            return Where("rol", rol)
                   .Get().Cast<PersonalRol>().ToList();
        }

        // Relación con Personal
        public Personal? GetPersonal()
        {
            return Personal.Find(PersonalId);
        }

        /// <summary>
        /// Obtiene todos los roles disponibles
        /// </summary>
        public static List<string> GetRolesDisponibles()
        {
            return new List<string> { "Veterinario", "Auxiliar", "Recepcionista", "Administrador" };
        }

        /// <summary>
        /// Verifica si un rol es válido
        /// </summary>
        public static bool EsRolValido(string rol)
        {
            return GetRolesDisponibles().Contains(rol);
        }

        public override string ToString()
        {
            return $"PersonalRol[{Id}]: Personal {PersonalId} - {Rol}";
        }
    }
}