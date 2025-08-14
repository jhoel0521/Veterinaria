using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Mascota - Representa la tabla 'Mascotas' en la base de datos
    /// </summary>
    public class Mascota : Model<Mascota>
    {
        protected override string Table { get; set; } = "Mascotas";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "cliente_id", "nombre", "especie", "raza", "edad", "peso", 
            "color", "observaciones", "activo"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Especie { get; set; } = string.Empty;
        public string? Raza { get; set; }
        public int? Edad { get; set; }
        public decimal? Peso { get; set; }
        public string? Color { get; set; }
        public string? Observaciones { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Activo { get; set; } = true;

        public Mascota() : base() { }

        public Mascota(string nombre, string especie, int clienteId)
        {
            Nombre = nombre;
            Especie = especie;
            ClienteId = clienteId;
        }

        // Métodos de conveniencia
        public static List<Mascota> MascotasActivas()
        {
            return Where("activo", true).Get().Cast<Mascota>().ToList();
        }

        public static List<Mascota> PorEspecie(string especie)
        {
            return Where("especie", especie)
                   .Where("activo", true)
                   .Get().Cast<Mascota>().ToList();
        }

        public static List<Mascota> PorRaza(string raza)
        {
            return Where("raza", raza)
                   .Where("activo", true)
                   .Get().Cast<Mascota>().ToList();
        }

        public static List<Mascota> BuscarPorNombre(string nombre)
        {
            return Where("nombre", SqlOperator.Like, $"%{nombre}%")
                   .Where("activo", true)
                   .Get().Cast<Mascota>().ToList();
        }

        // Relación con Cliente (belongsTo)
        public Cliente? GetCliente()
        {
            return Cliente.Find(ClienteId);
        }

        public string DescripcionCompleta()
        {
            return $"{Nombre} - {Especie}" + (string.IsNullOrEmpty(Raza) ? "" : $" ({Raza})");
        }

        public bool EsCachorro()
        {
            return Edad.HasValue && Edad <= 1;
        }

        public bool EsAdulto()
        {
            return Edad.HasValue && Edad > 1;
        }

        public override string ToString()
        {
            return $"Mascota[{Id}]: {DescripcionCompleta()} - Cliente: {ClienteId}";
        }
    }
}