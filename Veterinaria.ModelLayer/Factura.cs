using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Factura - Representa la tabla 'factura' en la base de datos
    /// Sistema de facturacion con triggers automáticos para cálculo de totales
    /// </summary>
    public class Factura : Model<Factura>
    {
        protected override string Table { get; set; } = "factura";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "ref_factura", "fecha", "persona_id", "total"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades publicas
        public int Id { get; set; }
        public string RefFactura { get; set; } = string.Empty;
        public DateTime Fecha { get; set; } = DateTime.Now;
        public int PersonaId { get; set; }
        public decimal Total { get; set; } = 0m; // Calculado automáticamente por triggers
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Factura() : base() { }

        public Factura(int personaId, string? refFactura = null)
        {
            PersonaId = personaId;
            RefFactura = refFactura ?? GenerarReferenciaFactura();
            Fecha = DateTime.Now;
        }

        // Metodos de conveniencia
        public static List<Factura> PorPersona(int personaId)
        {
            return Where("persona_id", personaId)
                   .OrderBy("fecha", "DESC")
                   .Get().Cast<Factura>().ToList();
        }

        public static List<Factura> PorFecha(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fechaInicio.AddDays(1);
            
            return Where("fecha", SqlOperator.GreaterThanOrEqual, fechaInicio)
                   .Where("fecha", SqlOperator.LessThan, fechaFin)
                   .OrderBy("fecha", "DESC")
                   .Get().Cast<Factura>().ToList();
        }

        public static List<Factura> PorRangoFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return Where("fecha", SqlOperator.GreaterThanOrEqual, fechaDesde.Date)
                   .Where("fecha", SqlOperator.LessThanOrEqual, fechaHasta.Date.AddDays(1).AddTicks(-1))
                   .OrderBy("fecha", "DESC")
                   .Get().Cast<Factura>().ToList();
        }

        public static Factura? BuscarPorReferencia(string refFactura)
        {
            return Where("ref_factura", refFactura).Get().Cast<Factura>().FirstOrDefault();
        }

        // Relación con Persona
        public Persona? GetPersona()
        {
            return Persona.Find(PersonaId);
        }

        // Relación con DetalleFactura
        public List<DetalleFactura> GetDetalles()
        {
            return DetalleFactura.Where("factura_id", Id)
                                 .OrderBy("id")
                                 .Get().Cast<DetalleFactura>().ToList();
        }

        // Relación con Diagnóstico
        public List<Diagnostico> GetDiagnosticos()
        {
            return Diagnostico.Where("factura_id", Id).Get().Cast<Diagnostico>().ToList();
        }

        /// <summary>
        /// Genera una referencia única para la factura
        /// </summary>
        public static string GenerarReferenciaFactura()
        {
            var fecha = DateTime.Now.ToString("yyyyMMdd");
            var random = new Random().Next(1000, 9999);
            return $"FAC-{fecha}-{random}";
        }

        /// <summary>
        /// Agrega un producto a la factura
        /// El trigger calculará automáticamente el subtotal y total
        /// </summary>
        public (bool Success, string Message) AgregarProducto(int productoId, int cantidad, decimal? precioUnitario = null)
        {
            try
            {
                var producto = Producto.Find(productoId);
                if (producto == null)
                    return (false, "Producto no encontrado");

                var precio = precioUnitario ?? producto.Precio;

                // Verificar si el producto requiere diagnóstico
                if (producto.RequiereDiagnostico)
                {
                    // Aquí se podría agregar lógica para verificar diagnóstico
                    // Por ahora, permitimos la venta con advertencia
                }

                var detalle = new DetalleFactura(Id, productoId, cantidad, precio);
                var attributes = new Dictionary<string, object?>
                {
                    { "factura_id", Id },
                    { "producto_id", productoId },
                    { "cantidad", cantidad },
                    { "precio_unitario", precio },
                    { "subtotal", cantidad * precio }, 
                    { "diagnostico_verificado", !producto.RequiereDiagnostico }
                };

                detalle.Fill(attributes).Save();

                return (true, "Producto agregado a la factura");
            }
            catch (Exception ex)
            {
                return (false, $"Error al agregar producto: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene el nombre del cliente
        /// </summary>
        public string GetNombreCliente()
        {
            var persona = GetPersona();
            return persona?.GetNombreCompleto() ?? "Cliente no encontrado";
        }

        /// <summary>
        /// Calcula estadísticas de la factura
        /// </summary>
        public (int CantidadItems, decimal SubtotalCalculado, bool TieneProductosCriticos) GetEstadisticas()
        {
            var detalles = GetDetalles();
            var cantidadItems = detalles.Sum(d => d.Cantidad);
            var subtotal = detalles.Sum(d => d.Subtotal);
            var tieneProductosCriticos = detalles.Any(d => !d.DiagnosticoVerificado);

            return (cantidadItems, subtotal, tieneProductosCriticos);
        }

        /// <summary>
        /// Verifica si la factura está completa y válida
        /// </summary>
        public bool EstaCompleta()
        {
            var detalles = GetDetalles();
            return detalles.Any() && detalles.All(d => d.DiagnosticoVerificado || !RequiereVerificacion(d));
        }

        private bool RequiereVerificacion(DetalleFactura detalle)
        {
            var producto = detalle.GetProducto();
            return producto?.RequiereDiagnostico ?? false;
        }

        public override string ToString()
        {
            return $"Factura[{Id}]: {RefFactura} - {Fecha:dd/MM/yyyy} - ${Total:F2} - Cliente: {GetNombreCliente()}";
        }
    }
}