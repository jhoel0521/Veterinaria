using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Producto - Representa la tabla 'producto' en la base de datos
    /// Productos disponibles en el inventario de la veterinaria
    /// </summary>
    public class Producto : Model<Producto>
    {
        protected override string Table { get; set; } = "producto";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "nombre", "precio", "requiere_diagnostico", "categoria_id"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public decimal Precio { get; set; }
        public bool RequiereDiagnostico { get; set; } = true;
        public int CategoriaId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Producto() : base() { }

        /// <summary>
        /// Buscar productos por nombre
        /// </summary>
        /// <param name="nombre">Nombre del producto a buscar</param>
        /// <returns>Lista de productos que coinciden con el nombre</returns>
        public static List<Producto> BuscarPorNombre(string nombre)
        {
            return Where("nombre", SqlOperator.Like, $"%{nombre}%")
                   .Get().Cast<Producto>().ToList();
        }

        /// <summary>
        /// Buscar productos por categoría
        /// </summary>
        /// <param name="categoriaId">ID de la categoría</param>
        /// <returns>Lista de productos de la categoría</returns>
        public static List<Producto> BuscarPorCategoria(int categoriaId)
        {
            return Where("categoria_id", categoriaId)
                   .Get().Cast<Producto>().ToList();
        }

        /// <summary>
        /// Obtener productos que requieren diagnóstico
        /// </summary>
        /// <returns>Lista de productos que requieren diagnóstico</returns>
        public static List<Producto> RequierenDiagnostico()
        {
            return Where("requiere_diagnostico", 1)
                   .Get().Cast<Producto>().ToList();
        }

        /// <summary>
        /// Obtener productos por rango de precio
        /// </summary>
        /// <param name="precioMin">Precio mínimo</param>
        /// <param name="precioMax">Precio máximo</param>
        /// <returns>Lista de productos en el rango de precio</returns>
        public static List<Producto> BuscarPorRangoPrecio(decimal precioMin, decimal precioMax)
        {
            return Where("precio", SqlOperator.GreaterThanOrEqual, precioMin)
                   .Where("precio", SqlOperator.LessThanOrEqual, precioMax)
                   .Get().Cast<Producto>().ToList();
        }

        /// <summary>
        /// Validar que el producto tenga los datos mínimos requeridos
        /// </summary>
        /// <returns>Verdadero si es válido, falso en caso contrario</returns>
        public bool EsValido()
        {
            if (string.IsNullOrWhiteSpace(Nombre))
                return false;

            if (Precio <= 0)
                return false;

            if (CategoriaId <= 0)
                return false;

            return true;
        }

        /// <summary>
        /// Formatear precio para mostrar
        /// </summary>
        /// <returns>Precio formateado como string</returns>
        public string PrecioFormateado()
        {
            return Precio.ToString("C2");
        }

        /// <summary>
        /// Obtener descripción completa del producto
        /// </summary>
        /// <returns>String con nombre, precio y categoría</returns>
        public string DescripcionCompleta()
        {
            return $"{Nombre} - {PrecioFormateado()} (Categoría ID: {CategoriaId})";
        }

        public override string ToString()
        {
            return $"Producto: {Nombre} - {PrecioFormateado()}";
        }
    }
}
