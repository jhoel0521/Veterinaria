using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo DetalleHistorico - Representa la tabla 'detalle_historico' en la base de datos
    /// Eventos específicos en el histórico clínico de un animal
    /// </summary>
    public class DetalleHistorico : Model<DetalleHistorico>
    {
        protected override string Table { get; set; } = "detalle_historico";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "tipo", "historico_id", "referencia", "observaciones", "tratamiento", "fecha_evento"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string Tipo { get; set; } = string.Empty; // Diagnóstico, Tratamiento, Control, Vacunación, Cirugía
        public int HistoricoId { get; set; }
        public string? Referencia { get; set; }
        public string? Observaciones { get; set; }
        public string? Tratamiento { get; set; }
        public DateTime FechaEvento { get; set; } = DateTime.Today;
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public DetalleHistorico() : base() { }

        public DetalleHistorico(string tipo, int historicoId, string? referencia = null, 
                               string? observaciones = null, string? tratamiento = null, 
                               DateTime? fechaEvento = null)
        {
            Tipo = tipo;
            HistoricoId = historicoId;
            Referencia = referencia;
            Observaciones = observaciones;
            Tratamiento = tratamiento;
            FechaEvento = fechaEvento ?? DateTime.Today;
        }

        // Métodos de conveniencia
        public static List<DetalleHistorico> PorHistorico(int historicoId)
        {
            return Where("historico_id", historicoId)
                   .OrderBy("fecha_evento", "DESC")
                   .Get().Cast<DetalleHistorico>().ToList();
        }

        public static List<DetalleHistorico> PorTipo(string tipo)
        {
            return Where("tipo", tipo)
                   .OrderBy("fecha_evento", "DESC")
                   .Get().Cast<DetalleHistorico>().ToList();
        }

        public static List<DetalleHistorico> PorFecha(DateTime fecha)
        {
            return Where("fecha_evento", fecha.Date)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<DetalleHistorico>().ToList();
        }

        public static List<DetalleHistorico> PorRangoFecha(DateTime fechaDesde, DateTime fechaHasta)
        {
            return Where("fecha_evento", SqlOperator.GreaterThanOrEqual, fechaDesde.Date)
                   .Where("fecha_evento", SqlOperator.LessThanOrEqual, fechaHasta.Date)
                   .OrderBy("fecha_evento", "DESC")
                   .Get().Cast<DetalleHistorico>().ToList();
        }

        // Relación con Historico
        public Historico? GetHistorico()
        {
            return Historico.Find(HistoricoId);
        }

        // Relación con Diagnostico (si existe)
        public Diagnostico? GetDiagnostico()
        {
            return Diagnostico.Where("ref_detalle_historico_id", Id).Get().Cast<Diagnostico>().FirstOrDefault();
        }

        /// <summary>
        /// Obtiene los tipos de eventos disponibles
        /// </summary>
        public static List<string> GetTiposDisponibles()
        {
            return new List<string> { "Diagnóstico", "Tratamiento", "Control", "Vacunación", "Cirugía" };
        }

        /// <summary>
        /// Verifica si el tipo es válido
        /// </summary>
        public static bool EsTipoValido(string tipo)
        {
            return GetTiposDisponibles().Contains(tipo);
        }

        /// <summary>
        /// Verifica si este detalle tiene un diagnóstico asociado
        /// </summary>
        public bool TieneDiagnostico()
        {
            return GetDiagnostico() != null;
        }

        /// <summary>
        /// Obtiene información del animal asociado
        /// </summary>
        public Animal? GetAnimal()
        {
            var historico = GetHistorico();
            return historico?.GetAnimal();
        }

        /// <summary>
        /// Obtiene el nombre del animal
        /// </summary>
        public string GetNombreAnimal()
        {
            var animal = GetAnimal();
            return animal?.Nombre ?? "Animal no encontrado";
        }

        /// <summary>
        /// Genera un resumen del detalle
        /// </summary>
        public string GenerarResumen()
        {
            var resumen = $"{Tipo} - {FechaEvento:dd/MM/yyyy}";
            
            if (!string.IsNullOrEmpty(Referencia))
                resumen += $" (Ref: {Referencia})";
                
            if (!string.IsNullOrEmpty(Observaciones))
                resumen += $" - {Observaciones}";
                
            return resumen;
        }

        /// <summary>
        /// Verifica si es un evento reciente (últimos 30 días)
        /// </summary>
        public bool EsReciente()
        {
            return (DateTime.Today - FechaEvento.Date).TotalDays <= 30;
        }

        public override string ToString()
        {
            return $"DetalleHistorico[{Id}]: {GenerarResumen()} - Animal: {GetNombreAnimal()}";
        }
    }
}