using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Categoria - Representa la tabla 'categoria' en la base de datos
    /// Categorías de productos con indicador si requieren diagnóstico
    /// </summary>
    public class Categoria : Model<Categoria>
    {
        protected override string Table { get; set; } = "categoria";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "nombre", "requiere_diagnostico"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public bool RequiereDiagnostico { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Categoria() : base() { }

        public Categoria(string nombre, bool requiereDiagnostico = false)
        {
            Nombre = nombre;
            RequiereDiagnostico = requiereDiagnostico;
        }

        // Métodos de conveniencia
        public static List<Categoria> CategoriasQueRequierenDiagnostico()
        {
            return Where("requiere_diagnostico", true)
                   .OrderBy("nombre")
                   .Get().Cast<Categoria>().ToList();
        }

        public static List<Categoria> CategoriasLibres()
        {
            return Where("requiere_diagnostico", false)
                   .OrderBy("nombre")
                   .Get().Cast<Categoria>().ToList();
        }

        public static Categoria? BuscarPorNombre(string nombre)
        {
            return Where("nombre", nombre).Get().Cast<Categoria>().FirstOrDefault();
        }

        // Relación con Productos
        public List<Producto> GetProductos()
        {
            return Producto.Where("categoria_id", Id).Get().Cast<Producto>().ToList();
        }

        /// <summary>
        /// Obtiene productos de esta categoría
        /// </summary>
        public List<Producto> GetProductosActivos()
        {
            return GetProductos();
        }

        /// <summary>
        /// Cuenta la cantidad de productos en esta categoría
        /// </summary>
        public int ContarProductos()
        {
            return GetProductos().Count;
        }

        public override string ToString()
        {
            return $"Categoria[{Id}]: {Nombre} - {(RequiereDiagnostico ? "Requiere diagnóstico" : "Libre")}";
        }
    }
}
