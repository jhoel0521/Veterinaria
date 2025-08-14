using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.Entities;

namespace Veterinaria.DataLayer.Entities
{
    /// <summary>
    /// Modelo DetalleVenta - Representa la tabla 'DetalleVenta' en la base de datos
    /// </summary>
    public class DetalleVenta : Model<DetalleVenta>
    {
        protected override string Table { get; set; } = "DetalleVenta";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "venta_id", "producto_id", "cantidad", "precio_unitario", "subtotal"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public int VentaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DetalleVenta() : base() { }

        public DetalleVenta(int ventaId, int productoId, int cantidad, decimal precioUnitario)
        {
            VentaId = ventaId;
            ProductoId = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            CalcularSubtotal();
        }

        // Métodos de conveniencia
        public static List<DetalleVenta> PorVenta(int ventaId)
        {
            return Where("venta_id", ventaId)
                   .OrderBy("id")
                   .Get().Cast<DetalleVenta>().ToList();
        }

        public static List<DetalleVenta> PorProducto(int productoId)
        {
            return Where("producto_id", productoId)
                   .OrderBy("id", "DESC")
                   .Get().Cast<DetalleVenta>().ToList();
        }

        public static int CantidadVendidaPorProducto(int productoId, DateTime? desde = null, DateTime? hasta = null)
        {
            var query = Where("producto_id", productoId);
            
            if (desde.HasValue)
            {
                query = query.Where("created_at", QueryBuilder.SqlOperator.GreaterThanOrEqual, desde.Value);
            }
            
            if (hasta.HasValue)
            {
                query = query.Where("created_at", QueryBuilder.SqlOperator.LessThanOrEqual, hasta.Value);
            }

            var detalles = query.Get().Cast<DetalleVenta>().ToList();
            return detalles.Sum(d => d.Cantidad);
        }

        public static decimal MontoVendidoPorProducto(int productoId, DateTime? desde = null, DateTime? hasta = null)
        {
            var query = Where("producto_id", productoId);
            
            if (desde.HasValue)
            {
                query = query.Where("created_at", QueryBuilder.SqlOperator.GreaterThanOrEqual, desde.Value);
            }
            
            if (hasta.HasValue)
            {
                query = query.Where("created_at", QueryBuilder.SqlOperator.LessThanOrEqual, hasta.Value);
            }

            var detalles = query.Get().Cast<DetalleVenta>().ToList();
            return detalles.Sum(d => d.Subtotal);
        }

        // Relaciones
        public Venta? GetVenta()
        {
            return Venta.Find(VentaId);
        }

        public Producto? GetProducto()
        {
            return Producto.Find(ProductoId);
        }

        // Métodos de negocio
        public void CalcularSubtotal()
        {
            Subtotal = Cantidad * PrecioUnitario;
        }

        public decimal CalcularDescuento(decimal porcentajeDescuento)
        {
            return Subtotal * (porcentajeDescuento / 100);
        }

        public void AplicarDescuento(decimal porcentajeDescuento)
        {
            var descuento = CalcularDescuento(porcentajeDescuento);
            Subtotal -= descuento;
        }

        public bool ValidarStock()
        {
            var producto = GetProducto();
            return producto?.Stock >= Cantidad;
        }

        // Validación antes de guardar
        public new DetalleVenta Save()
        {
            // Recalcular subtotal antes de guardar
            CalcularSubtotal();
            
            // Validar que el producto exista
            if (GetProducto() == null)
            {
                throw new InvalidOperationException($"El producto con ID {ProductoId} no existe");
            }

            // Validar stock disponible (solo para nuevos registros)
            if (Id == 0 && !ValidarStock())
            {
                throw new InvalidOperationException($"Stock insuficiente para el producto {ProductoId}. Stock disponible: {GetProducto()?.Stock}, Cantidad solicitada: {Cantidad}");
            }

            return base.Save();
        }

        public string GetDescripcionProducto()
        {
            var producto = GetProducto();
            return producto?.Nombre ?? $"Producto {ProductoId}";
        }

        public override string ToString()
        {
            return $"DetalleVenta[{Id}]: {GetDescripcionProducto()} - Qty: {Cantidad} - ${PrecioUnitario:F2} - Subtotal: ${Subtotal:F2}";
        }
    }
}
