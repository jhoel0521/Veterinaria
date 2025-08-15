using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo DetalleFactura - Representa la tabla 'detalle_factura' en la base de datos
    /// Items individuales de una factura con c�lculo autom�tico de subtotales via triggers
    /// </summary>
    public class DetalleFactura : Model<DetalleFactura>
    {
        protected override string Table { get; set; } = "detalle_factura";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "factura_id", "producto_id", "cantidad", "precio_unitario", 
            "subtotal", "diagnostico_verificado"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades p�blicas
        public int Id { get; set; }
        public int FacturaId { get; set; }
        public int ProductoId { get; set; }
        public int Cantidad { get; set; }
        public decimal PrecioUnitario { get; set; }
        public decimal Subtotal { get; set; } = 0m; // Calculado por trigger
        public bool DiagnosticoVerificado { get; set; } = false;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DetalleFactura() : base() { }

        public DetalleFactura(int facturaId, int productoId, int cantidad, decimal precioUnitario)
        {
            FacturaId = facturaId;
            ProductoId = productoId;
            Cantidad = cantidad;
            PrecioUnitario = precioUnitario;
            // Subtotal ser� calculado por el trigger
        }

        // M�todos de conveniencia
        public static List<DetalleFactura> PorFactura(int facturaId)
        {
            return Where("factura_id", facturaId)
                   .OrderBy("id")
                   .Get().Cast<DetalleFactura>().ToList();
        }

        public static List<DetalleFactura> PorProducto(int productoId)
        {
            return Where("producto_id", productoId)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<DetalleFactura>().ToList();
        }

        public static List<DetalleFactura> SinVerificarDiagnostico()
        {
            return Where("diagnostico_verificado", false)
                   .Get().Cast<DetalleFactura>().ToList();
        }

        public static List<DetalleFactura> PorFecha(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fechaInicio.AddDays(1);
            
            return Where("created_at", SqlOperator.GreaterThanOrEqual, fechaInicio)
                   .Where("created_at", SqlOperator.LessThan, fechaFin)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<DetalleFactura>().ToList();
        }

        // Relaciones
        public Factura? GetFactura()
        {
            return Factura.Find(FacturaId);
        }

        public Producto? GetProducto()
        {
            return Producto.Find(ProductoId);
        }

        /// <summary>
        /// Obtiene informaci�n del producto
        /// </summary>
        public string GetNombreProducto()
        {
            var producto = GetProducto();
            return producto?.Nombre ?? $"Producto {ProductoId}";
        }

        /// <summary>
        /// Verifica si el producto requiere diagn�stico
        /// </summary>
        public bool ProductoRequiereDiagnostico()
        {
            var producto = GetProducto();
            return producto?.RequiereDiagnostico ?? false;
        }

        /// <summary>
        /// Verifica diagn�stico para un producto cr�tico
        /// </summary>
        public (bool Success, string Message) VerificarDiagnostico()
        {
            try
            {
                if (!ProductoRequiereDiagnostico())
                {
                    return (false, "Este producto no requiere verificaci�n de diagn�stico");
                }

                if (DiagnosticoVerificado)
                {
                    return (false, "El diagn�stico ya est� verificado");
                }

                var attributes = new Dictionary<string, object?>
                {
                    { "diagnostico_verificado", true }
                };

                this.Fill(attributes).Save();

                return (true, "Diagn�stico verificado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al verificar diagn�stico: {ex.Message}");
            }
        }

        /// <summary>
        /// Calcula el subtotal manualmente (aunque el trigger lo haga autom�ticamente)
        /// </summary>
        public decimal CalcularSubtotal()
        {
            return Cantidad * PrecioUnitario;
        }

        /// <summary>
        /// Obtiene estad�sticas del detalle para reportes
        /// </summary>
        public (string NombreProducto, string Categoria, bool RequiereDiagnostico, bool Verificado) GetEstadisticas()
        {
            var producto = GetProducto();
            // Obtener categoría usando el modelo base
            var categoria = producto != null ? Categoria.Find(producto.CategoriaId) : null;
            
            return (
                producto?.Nombre ?? "Producto no encontrado",
                categoria?.Nombre ?? "Sin categor�a",
                producto?.RequiereDiagnostico ?? false,
                DiagnosticoVerificado
            );
        }

        /// <summary>
        /// Obtiene informaci�n de la factura asociada
        /// </summary>
        public string GetInfoFactura()
        {
            var factura = GetFactura();
            return factura?.RefFactura ?? $"Factura {FacturaId}";
        }

        /// <summary>
        /// Calcula descuento si aplica
        /// </summary>
        public decimal CalcularDescuento(decimal porcentajeDescuento)
        {
            return Subtotal * (porcentajeDescuento / 100);
        }

        /// <summary>
        /// Aplica descuento al subtotal
        /// </summary>
        public (bool Success, string Message) AplicarDescuento(decimal porcentajeDescuento)
        {
            try
            {
                if (porcentajeDescuento < 0 || porcentajeDescuento > 100)
                {
                    return (false, "El porcentaje de descuento debe estar entre 0 y 100");
                }

                var descuento = CalcularDescuento(porcentajeDescuento);
                var nuevoSubtotal = Subtotal - descuento;

                var attributes = new Dictionary<string, object?>
                {
                    { "subtotal", nuevoSubtotal }
                };

                this.Fill(attributes).Save();

                return (true, $"Descuento del {porcentajeDescuento}% aplicado. Descuento: ${descuento:F2}");
            }
            catch (Exception ex)
            {
                return (false, $"Error al aplicar descuento: {ex.Message}");
            }
        }

        public override string ToString()
        {
            return $"DetalleFactura[{Id}]: {GetNombreProducto()} - Qty: {Cantidad} - ${PrecioUnitario:F2} - Subtotal: ${Subtotal:F2} - Factura: {GetInfoFactura()}";
        }
    }
}