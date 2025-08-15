using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Diagnostico - Representa la tabla 'diagnostico' en la base de datos
    /// Diagnósticos veterinarios asociados a facturas y personal
    /// </summary>
    public class Diagnostico : Model<Diagnostico>
    {
        protected override string Table { get; set; } = "diagnostico";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "descripcion", "factura_id", "precio", "personal_id", "ref_detalle_historico_id"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string? Descripcion { get; set; }
        public int FacturaId { get; set; }
        public decimal Precio { get; set; } = 0m;
        public int PersonalId { get; set; }
        public int RefDetalleHistoricoId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Diagnostico() : base() { }

        public Diagnostico(string descripcion, int facturaId, decimal precio, int personalId, int refDetalleHistoricoId)
        {
            Descripcion = descripcion;
            FacturaId = facturaId;
            Precio = precio;
            PersonalId = personalId;
            RefDetalleHistoricoId = refDetalleHistoricoId;
        }

        // Métodos de conveniencia
        public static List<Diagnostico> PorPersonal(int personalId)
        {
            return Where("personal_id", personalId)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<Diagnostico>().ToList();
        }

        public static List<Diagnostico> PorFactura(int facturaId)
        {
            return Where("factura_id", facturaId)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<Diagnostico>().ToList();
        }

        public static List<Diagnostico> PorFecha(DateTime fecha)
        {
            var fechaInicio = fecha.Date;
            var fechaFin = fechaInicio.AddDays(1);
            
            return Where("created_at", SqlOperator.GreaterThanOrEqual, fechaInicio)
                   .Where("created_at", SqlOperator.LessThan, fechaFin)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<Diagnostico>().ToList();
        }

        public static Diagnostico? PorDetalleHistorico(int detalleHistoricoId)
        {
            return Where("ref_detalle_historico_id", detalleHistoricoId)
                   .Get().Cast<Diagnostico>().FirstOrDefault();
        }

        // Relaciones
        public Factura? GetFactura()
        {
            return Factura.Find(FacturaId);
        }

        public Personal? GetPersonal()
        {
            return Personal.Find(PersonalId);
        }

        public DetalleHistorico? GetDetalleHistorico()
        {
            return DetalleHistorico.Find(RefDetalleHistoricoId);
        }

        /// <summary>
        /// Obtiene el nombre del veterinario
        /// </summary>
        public string GetNombreVeterinario()
        {
            var personal = GetPersonal();
            return personal?.NombreCompleto() ?? "Veterinario no encontrado";
        }

        /// <summary>
        /// Obtiene información del animal diagnosticado
        /// </summary>
        public Animal? GetAnimal()
        {
            var detalleHistorico = GetDetalleHistorico();
            return detalleHistorico?.GetAnimal();
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
        /// Obtiene información de la persona (cliente)
        /// </summary>
        public Persona? GetCliente()
        {
            var factura = GetFactura();
            return factura?.GetPersona();
        }

        /// <summary>
        /// Obtiene el nombre del cliente
        /// </summary>
        public string GetNombreCliente()
        {
            var cliente = GetCliente();
            return cliente?.GetNombreCompleto() ?? "Cliente no encontrado";
        }

        /// <summary>
        /// Crea un diagnóstico completo con factura
        /// </summary>
        public static (bool Success, string Message, Diagnostico? Diagnostico) CrearCompleto(
            string descripcion,
            decimal precio,
            int personalId,
            int refDetalleHistoricoId,
            int personaId)
        {
            try
            {
                // Crear factura para el diagnóstico
                var refFactura = Factura.GenerarReferenciaFactura();
                var factura = new Factura(personaId, refFactura);
                
                var facturaAttributes = new Dictionary<string, object?>
                {
                    { "ref_factura", refFactura },
                    { "fecha", DateTime.Now },
                    { "persona_id", personaId },
                    { "total", precio }
                };

                factura.Fill(facturaAttributes).Save();

                // Crear diagnóstico
                var diagnostico = new Diagnostico(descripcion, factura.Id, precio, personalId, refDetalleHistoricoId);
                var diagnosticoAttributes = new Dictionary<string, object?>
                {
                    { "descripcion", descripcion },
                    { "factura_id", factura.Id },
                    { "precio", precio },
                    { "personal_id", personalId },
                    { "ref_detalle_historico_id", refDetalleHistoricoId }
                };

                diagnostico.Fill(diagnosticoAttributes).Save();

                return (true, "Diagnóstico creado exitosamente", diagnostico);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear diagnóstico: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Genera un resumen del diagnóstico
        /// </summary>
        public string GenerarResumen()
        {
            var resumen = $"Diagnóstico para {GetNombreAnimal()}";
            
            if (!string.IsNullOrEmpty(Descripcion))
                resumen += $" - {Descripcion}";
                
            resumen += $" - ${Precio:F2}";
            resumen += $" - Dr. {GetNombreVeterinario()}";
            
            return resumen;
        }

        /// <summary>
        /// Verifica si el diagnóstico es reciente (últimos 7 días)
        /// </summary>
        public bool EsReciente()
        {
            return (DateTime.Now - CreatedAt).TotalDays <= 7;
        }

        /// <summary>
        /// Obtiene estadísticas del diagnóstico
        /// </summary>
        public (string Animal, string Cliente, string Veterinario, decimal Precio, DateTime Fecha) GetEstadisticas()
        {
            return (
                GetNombreAnimal(),
                GetNombreCliente(), 
                GetNombreVeterinario(),
                Precio,
                CreatedAt
            );
        }

        public override string ToString()
        {
            return $"Diagnostico[{Id}]: {GenerarResumen()} - {CreatedAt:dd/MM/yyyy}";
        }
    }
}