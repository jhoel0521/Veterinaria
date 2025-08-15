using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.ModelLayer;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.BusinessLayer.Controllers
{
    /// <summary>
    /// Controlador para gestión de Mascotas
    /// Maneja la lógica de negocio para operaciones CRUD
    /// </summary>
    public static class MascotaController
    {
        /// <summary>
        /// Obtiene todas las mascotas activas con paginación opcional
        /// </summary>
        public static List<Animal> GetAll(int? limit = null)
        {
            try
            {
                var query = Animal.Where("activo", true).OrderBy("nombre");
                
                if (limit.HasValue)
                {
                    query = query.Limit(limit.Value);
                }

                return query.Get().Cast<Animal>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener mascotas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por texto (nombre, especie, raza) y por dueño
        /// </summary>
        public static List<Animal> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }

                var mascotas = new List<Animal>();
                var searchTerm = $"%{searchText.Trim()}%";

                // Buscar por nombre
                var mascotasPorNombre = Animal.Where("nombre", SqlOperator.Like, searchTerm)
                                             .Get().Cast<Animal>().ToList();
                mascotas.AddRange(mascotasPorNombre);

                // Buscar por tipo
                var mascotasPorEspecie = Animal.Where("tipo", SqlOperator.Like, searchTerm)
                                               .Get().Cast<Animal>().ToList();
                mascotas.AddRange(mascotasPorEspecie);

                // Buscar por nombre del dueño (cliente)
                var clientes = PersonaFisica.Where("nombre", SqlOperator.Like, searchTerm)
                                    .Get().Cast<PersonaFisica>().ToList();

                // También buscar por apellido del cliente (usando PersonaFisica)
                var clientesPorApellido = PersonaFisica.Where("apellido", SqlOperator.Like, searchTerm)
                                               .Get().Cast<PersonaFisica>().ToList();
                
                // Combinar ambas listas de clientes
                var todosLosClientes = clientes.Concat(clientesPorApellido)
                                             .GroupBy(c => c.Id)
                                             .Select(g => g.First())
                                             .ToList();

                foreach (var cliente in todosLosClientes)
                {
                    var mascotasDelCliente = Animal.Where("persona_id", cliente.Id)
                                                   .Get().Cast<Animal>().ToList();
                    mascotas.AddRange(mascotasDelCliente);
                }

                // Eliminar duplicados y ordenar
                return mascotas
                    .GroupBy(m => m.Id)
                    .Select(g => g.First())
                    .OrderBy(m => m.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por especie
        /// </summary>
        public static List<Animal> SearchByEspecie(string especie)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(especie))
                    return GetAll();

                return Animal.PorTipo(especie.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por especie: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por raza
        /// </summary>
        public static List<Animal> SearchByRaza(string raza)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(raza))
                    return GetAll();

                return Animal.PorTipo(raza.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por raza: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por cliente/dueño
        /// </summary>
        public static List<Animal> SearchByCliente(int clienteId)
        {
            try
            {
                return Animal.Where("persona_id", clienteId)
                             .Get().Cast<Animal>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por cliente: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene una mascota por ID
        /// </summary>
        public static Animal? GetById(int id)
        {
            try
            {
                return Animal.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener mascota con ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva mascota
        /// </summary>
        public static (bool Success, string Message, Animal? Animal) Create(
            int personaId,
            string nombre, 
            string tipo, 
            DateTime? fechaNacimiento = null, 
            string? observaciones = null)
        {
            try
            {
                // Validaciones
                if (personaId <= 0)
                    return (false, "La persona es requerida", null);

                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(tipo))
                    return (false, "El tipo es requerido", null);

                // Verificar que la persona existe
                var persona = Persona.Find(personaId);
                if (persona == null)
                    return (false, "La persona seleccionada no existe", null);

                // Crear animal
                var animal = new Animal
                {
                    PersonaId = personaId,
                    Nombre = nombre.Trim(),
                    Tipo = tipo.Trim(),
                    FechaNacimiento = fechaNacimiento
                };

                // Llenar con atributos fillable
                var attributes = new Dictionary<string, object?>
                {
                    { "persona_id", animal.PersonaId },
                    { "nombre", animal.Nombre },
                    { "tipo", animal.Tipo },
                    { "fecha_nacimiento", animal.FechaNacimiento }
                };

                animal.Fill(attributes).Save();

                return (true, "Animal creado exitosamente", animal);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear mascota: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza una mascota existente
        /// </summary>
        public static (bool Success, string Message, Animal? Animal) Update(
            int id,
            int personaId,
            string nombre, 
            string tipo, 
            DateTime? fechaNacimiento = null,
            string? observaciones = null)
        {
            try
            {
                var animal = GetById(id);
                if (animal == null)
                    return (false, "Animal no encontrado", null);

                // Validaciones
                if (personaId <= 0)
                    return (false, "La persona es requerida", null);

                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(tipo))
                    return (false, "El tipo es requerido", null);

                // Verificar que la persona existe
                var persona = Persona.Find(personaId);
                if (persona == null)
                    return (false, "La persona seleccionada no existe", null);

                // Actualizar atributos
                var attributes = new Dictionary<string, object?>
                {
                    { "persona_id", personaId },
                    { "nombre", nombre.Trim() },
                    { "tipo", tipo.Trim() },
                    { "fecha_nacimiento", fechaNacimiento }
                };

                animal.Fill(attributes).Save();

                return (true, "Animal actualizado exitosamente", animal);
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar mascota: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Elimina una mascota (soft delete)
        /// </summary>
        public static (bool Success, string Message) Delete(int id)
        {
            try
            {
                var mascota = GetById(id);
                if (mascota == null)
                    return (false, "Mascota no encontrada");

                // Soft delete - marcar como inactiva
                var attributes = new Dictionary<string, object?>
                {
                    { "activo", false }
                };
                mascota.Fill(attributes).Save();

                return (true, "Mascota desactivada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar mascota: {ex.Message}");
            }
        }

        /// <summary>
        /// Reactiva una mascota desactivada
        /// </summary>
        public static (bool Success, string Message) Reactivate(int id)
        {
            try
            {
                var mascota = GetById(id);
                if (mascota == null)
                    return (false, "Mascota no encontrada");

                var attributes = new Dictionary<string, object?>
                {
                    { "activo", true }
                };
                mascota.Fill(attributes).Save();

                return (true, "Mascota reactivada exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al reactivar mascota: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todas las especies disponibles
        /// </summary>
        public static List<string> GetEspecies()
        {
            try
            {
                return Animal.All()
                             .Select(m => m.Tipo)
                             .Where(e => !string.IsNullOrEmpty(e))
                             .Distinct()
                             .OrderBy(e => e)
                             .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener especies: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todas las razas disponibles para una especie específica
        /// </summary>
        public static List<string> GetRazasByEspecie(string especie)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(especie))
                    return new List<string>();

                return Animal.Where("tipo", SqlOperator.Like, $"%{especie}%")
                             .Get()
                             .Cast<Animal>()
                             .Select(m => m.Tipo)
                             .Where(r => !string.IsNullOrEmpty(r))
                             .Distinct()
                             .OrderBy(r => r)
                             .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener razas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene estadísticas básicas de mascotas
        /// </summary>
        public static (int Total, int Activas, int Inactivas, int Cachorros, int Adultos) GetStats()
        {
            try
            {
                var total = Animal.Count();
                var activas = Animal.Count();
                var inactivas = 0; // No hay campo activo en Animal
                
                var mascotasActivas = GetAll();
                var cachorros = mascotasActivas.Count(m => m.FechaNacimiento.HasValue && 
                    m.FechaNacimiento.Value > DateTime.Now.AddYears(-1));
                var adultos = mascotasActivas.Count(m => m.FechaNacimiento.HasValue && 
                    m.FechaNacimiento.Value <= DateTime.Now.AddYears(-1));

                return (total, activas, inactivas, cachorros, adultos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas: {ex.Message}", ex);
            }
        }
    }
}