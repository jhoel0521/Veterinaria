using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.ModelLayer
{
    /// <summary>
    /// Modelo Historico - Representa la tabla 'historico' en la base de datos
    /// Histórico clínico de los animales
    /// </summary>
    public class Historico : Model<Historico>
    {
        protected override string Table { get; set; } = "historico";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "notas_importantes", "animal_id"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string? NotasImportantes { get; set; }
        public int AnimalId { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }

        public Historico() : base() { }

        public Historico(int animalId, string? notasImportantes = null)
        {
            AnimalId = animalId;
            NotasImportantes = notasImportantes;
        }

        // Métodos de conveniencia
        public static List<Historico> PorAnimal(int animalId)
        {
            return Where("animal_id", animalId)
                   .OrderBy("created_at", "DESC")
                   .Get().Cast<Historico>().ToList();
        }

        // Relación con Animal
        public Animal? GetAnimal()
        {
            return Animal.Find(AnimalId);
        }

        // Relación con DetalleHistorico
        public List<DetalleHistorico> GetDetalles()
        {
            return DetalleHistorico.Where("historico_id", Id)
                                   .OrderBy("fecha_evento", "DESC")
                                   .Get().Cast<DetalleHistorico>().ToList();
        }

        /// <summary>
        /// Agrega un detalle al histórico
        /// </summary>
        public (bool Success, string Message, DetalleHistorico? Detalle) AgregarDetalle(
            string tipo, 
            string? referencia = null, 
            string? observaciones = null, 
            string? tratamiento = null,
            DateTime? fechaEvento = null)
        {
            try
            {
                var detalle = new DetalleHistorico(tipo, Id, referencia, observaciones, tratamiento, fechaEvento);
                var attributes = new Dictionary<string, object?>
                {
                    { "tipo", tipo },
                    { "historico_id", Id },
                    { "referencia", referencia },
                    { "observaciones", observaciones },
                    { "tratamiento", tratamiento },
                    { "fecha_evento", fechaEvento ?? DateTime.Today }
                };

                detalle.Fill(attributes).Save();

                return (true, "Detalle agregado al histórico", detalle);
            }
            catch (Exception ex)
            {
                return (false, $"Error al agregar detalle: {ex.Message}", null);
            }
        }

        public override string ToString()
        {
            var animal = GetAnimal();
            return $"Historico[{Id}]: Animal {animal?.Nombre ?? AnimalId.ToString()} - {GetDetalles().Count} registros";
        }
    }
}