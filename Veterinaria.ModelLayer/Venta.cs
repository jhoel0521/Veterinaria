using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Venta - Representa la tabla 'Ventas' en la base de datos
    /// </summary>
    public class Venta : Model<Venta>
    {
        protected override string Table { get; set; } = "Ventas";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "cliente_id", "mascota_id", "fecha_venta", "subtotal", "impuesto", 
            "total", "metodo_pago", "notas", "estado"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public int ClienteId { get; set; }
        public int? MascotaId { get; set; }
        public DateTime FechaVenta { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Impuesto { get; set; }
        public decimal Total { get; set; }
        public string MetodoPago { get; set; } = string.Empty;
        public string? Notas { get; set; }
        public string Estado { get; set; } = "COMPLETADA";
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Venta() : base() 
        {
            FechaVenta = DateTime.Now;
        }

        public Venta(int clienteId, decimal total)
        {
            ClienteId = clienteId;
            Total = total;
            FechaVenta = DateTime.Now;
            Estado = "COMPLETADA";
        }

        // Métodos de conveniencia
        public static List<Venta> VentasDelDia(DateTime? fecha = null)
        {
            var fechaBuscar = fecha ?? DateTime.Today;
            return Where("fecha_venta", SqlOperator.GreaterThanOrEqual, fechaBuscar)
                   .Where("fecha_venta", SqlOperator.LessThan, fechaBuscar.AddDays(1))
                   .Get().Cast<Venta>().ToList();
        }

        public static List<Venta> VentasPorCliente(int clienteId)
        {
            return Where("cliente_id", clienteId)
                   .OrderBy("fecha_venta", "DESC")
                   .Get().Cast<Venta>().ToList();
        }

        public static List<Venta> VentasPorMascota(int mascotaId)
        {
            return Where("mascota_id", mascotaId)
                   .OrderBy("fecha_venta", "DESC")
                   .Get().Cast<Venta>().ToList();
        }

        // Relaciones
        public Cliente? GetCliente()
        {
            return Cliente.Find(ClienteId);
        }

        public Mascota? GetMascota()
        {
            return MascotaId.HasValue ? Mascota.Find(MascotaId.Value) : null;
        }

        public List<DetalleVenta> GetDetalles()
        {
            return DetalleVenta.Where("venta_id", Id).Get().Cast<DetalleVenta>().ToList();
        }

        // Métodos de negocio
        public decimal CalcularTotal()
        {
            var detalles = GetDetalles();
            Subtotal = detalles.Sum(d => d.Subtotal);
            Impuesto = Subtotal * 0.18m; // 18% de impuesto (ajustar según el país)
            Total = Subtotal + Impuesto;
            return Total;
        }

        public void AgregarDetalle(int productoId, int cantidad, decimal? precioUnitario = null)
        {
            var producto = Producto.Find(productoId);
            if (producto == null)
                throw new ArgumentException($"El producto con ID {productoId} no existe");

            var precio = precioUnitario ?? producto.Precio;
            var detalle = new DetalleVenta(Id, productoId, cantidad, precio);
            detalle.Save();

            // Reducir stock
            producto.ReducirStock(cantidad);
        }

        public bool EstaCompleta()
        {
            return Estado.ToUpper() == "COMPLETADA";
        }

        public bool EstaPendiente()
        {
            return Estado.ToUpper() == "PENDIENTE";
        }

        public void Completar()
        {
            Estado = "COMPLETADA";
            Save();
        }

        public void Cancelar()
        {
            Estado = "CANCELADA";
            Save();
        }

        public override string ToString()
        {
            return $"Venta[{Id}]: Cliente {ClienteId} - {FechaVenta:dd/MM/yyyy} - ${Total:F2} - {Estado}";
        }
    }
}