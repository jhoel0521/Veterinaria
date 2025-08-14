using System;
using System.Collections.Generic;
using System.Linq;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Producto - Representa la tabla 'Productos' en la base de datos
    /// </summary>
    public class Producto : Model<Producto>
    {
        protected override string Table { get; set; } = "Productos";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "codigo", "nombre", "descripcion", "precio", "stock", 
            "stock_minimo", "categoria", "proveedor", "activo"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string Codigo { get; set; } = string.Empty;
        public string Nombre { get; set; } = string.Empty;
        public string? Descripcion { get; set; }
        public decimal Precio { get; set; }
        public int Stock { get; set; }
        public int StockMinimo { get; set; }
        public string? Categoria { get; set; }
        public string? Proveedor { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Activo { get; set; } = true;

        public Producto() : base() { }

        // Métodos de conveniencia
        public static List<Producto> ProductosConStockBajo()
        {
            return Query()
                .WhereRaw("stock <= stock_minimo")
                .Where("activo", true)
                .Get()
                .Cast<Producto>()
                .ToList();
        }

        public static List<Producto> PorCategoria(string categoria)
        {
            return Where("categoria", categoria)
                   .Where("activo", true)
                   .OrderBy("nombre")
                   .Get()
                   .Cast<Producto>()
                   .ToList();
        }

        public bool TieneStock(int cantidad = 1)
        {
            return Stock >= cantidad;
        }

        public void ReducirStock(int cantidad)
        {
            if (TieneStock(cantidad))
            {
                Stock -= cantidad;
                Save();
            }
            else
            {
                throw new InvalidOperationException($"Stock insuficiente. Disponible: {Stock}, Requerido: {cantidad}");
            }
        }

        public override string ToString()
        {
            return $"Producto[{Id}]: {Nombre} - Stock: {Stock} - Precio: ${Precio}";
        }
    }
}