using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.Entities;

namespace Veterinaria.DataLayer.Entities
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
            return Where("fecha_venta", QueryBuilder.SqlOperator.GreaterThanOrEqual, fechaBuscar)
                   .Where("fecha_venta", QueryBuilder.SqlOperator.LessThan, fechaBuscar.AddDays(1))
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

        public static List<Venta> VentasEntreFechas(DateTime fechaInicio, DateTime fechaFin)
        {
            return Where("fecha_venta", QueryBuilder.SqlOperator.GreaterThanOrEqual, fechaInicio)
                   .Where("fecha_venta", QueryBuilder.SqlOperator.LessThanOrEqual, fechaFin)
                   .OrderBy("fecha_venta", "DESC")
                   .Get().Cast<Venta>().ToList();
        }

        public static List<Venta> VentasPorEstado(string estado)
        {
            return Where("estado", estado)
                   .OrderBy("fecha_venta", "DESC")
                   .Get().Cast<Venta>().ToList();
        }

        public static decimal TotalVentasDelDia(DateTime? fecha = null)
        {
            var ventas = VentasDelDia(fecha);
            return ventas.Sum(v => v.Total);
        }

        public static decimal TotalVentasDelMes(int año, int mes)
        {
            var fechaInicio = new DateTime(año, mes, 1);
            var fechaFin = fechaInicio.AddMonths(1).AddDays(-1);
            var ventas = VentasEntreFechas(fechaInicio, fechaFin);
            return ventas.Sum(v => v.Total);
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

        public List<DetalleVenta> DetallesVenta()
        {
            return DetalleVenta.Where("venta_id", Id)
                              .OrderBy("id")
                              .Get().Cast<DetalleVenta>().ToList();
        }

        // Métodos de negocio
        public void CalcularTotales()
        {
            var detalles = DetallesVenta();
            Subtotal = detalles.Sum(d => d.Subtotal);
            Impuesto = Subtotal * 0.19m; // 19% de IVA
            Total = Subtotal + Impuesto;
        }

        public int CantidadProductos()
        {
            var detalles = DetallesVenta();
            return detalles.Sum(d => d.Cantidad);
        }

        public bool PuedeSerCancelada()
        {
            return Estado == "COMPLETADA" && (DateTime.Now - FechaVenta).TotalHours <= 24;
        }

        public void Cancelar()
        {
            if (PuedeSerCancelada())
            {
                Estado = "CANCELADA";
                Save();
            }
        }

        public override string ToString()
        {
            return $"Venta[{Id}]: Cliente {ClienteId} - ${Total:F2} - {FechaVenta:dd/MM/yyyy} - {Estado}";
        }
    }
}
