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
        public static List<Mascota> GetAll(int? limit = null)
        {
            try
            {
                var query = Mascota.Where("activo", true).OrderBy("nombre");
                
                if (limit.HasValue)
                {
                    query = query.Limit(limit.Value);
                }

                return query.Get().Cast<Mascota>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener mascotas: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por texto (nombre, especie, raza) y por dueño
        /// </summary>
        public static List<Mascota> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }

                var mascotas = new List<Mascota>();
                var searchTerm = $"%{searchText.Trim()}%";

                // Buscar por nombre
                var mascotasPorNombre = Mascota.Where("nombre", SqlOperator.Like, searchTerm)
                                             .Where("activo", true)
                                             .Get().Cast<Mascota>().ToList();
                mascotas.AddRange(mascotasPorNombre);

                // Buscar por especie
                var mascotasPorEspecie = Mascota.Where("especie", SqlOperator.Like, searchTerm)
                                               .Where("activo", true)
                                               .Get().Cast<Mascota>().ToList();
                mascotas.AddRange(mascotasPorEspecie);

                // Buscar por raza
                var mascotasPorRaza = Mascota.Where("raza", SqlOperator.Like, searchTerm)
                                            .Where("activo", true)
                                            .Get().Cast<Mascota>().ToList();
                mascotas.AddRange(mascotasPorRaza);

                // Buscar por nombre del dueño (cliente)
                var clientes = Cliente.Where("nombre", SqlOperator.Like, searchTerm)
                                    .Where("activo", true)
                                    .Get().Cast<Cliente>().ToList();

                // También buscar por apellido del cliente
                var clientesPorApellido = Cliente.Where("apellido", SqlOperator.Like, searchTerm)
                                               .Where("activo", true)
                                               .Get().Cast<Cliente>().ToList();
                
                // Combinar ambas listas de clientes
                var todosLosClientes = clientes.Concat(clientesPorApellido)
                                             .GroupBy(c => c.Id)
                                             .Select(g => g.First())
                                             .ToList();

                foreach (var cliente in todosLosClientes)
                {
                    var mascotasDelCliente = Mascota.Where("cliente_id", cliente.Id)
                                                   .Where("activo", true)
                                                   .Get().Cast<Mascota>().ToList();
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
        public static List<Mascota> SearchByEspecie(string especie)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(especie))
                    return GetAll();

                return Mascota.PorEspecie(especie.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por especie: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por raza
        /// </summary>
        public static List<Mascota> SearchByRaza(string raza)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(raza))
                    return GetAll();

                return Mascota.PorRaza(raza.Trim());
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por raza: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca mascotas por cliente/dueño
        /// </summary>
        public static List<Mascota> SearchByCliente(int clienteId)
        {
            try
            {
                return Mascota.Where("cliente_id", clienteId)
                             .Where("activo", true)
                             .Get().Cast<Mascota>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar mascotas por cliente: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene una mascota por ID
        /// </summary>
        public static Mascota? GetById(int id)
        {
            try
            {
                return Mascota.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener mascota con ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea una nueva mascota
        /// </summary>
        public static (bool Success, string Message, Mascota? Mascota) Create(
            int clienteId,
            string nombre, 
            string especie, 
            string? raza = null, 
            int? edad = null,
            decimal? peso = null,
            string? color = null,
            string? observaciones = null)
        {
            try
            {
                // Validaciones
                if (clienteId <= 0)
                    return (false, "El cliente es requerido", null);

                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(especie))
                    return (false, "La especie es requerida", null);

                // Verificar que el cliente existe
                var cliente = Cliente.Find(clienteId);
                if (cliente == null)
                    return (false, "El cliente seleccionado no existe", null);

                // Validar edad
                if (edad.HasValue && edad < 0)
                    return (false, "La edad no puede ser negativa", null);

                // Validar peso
                if (peso.HasValue && peso <= 0)
                    return (false, "El peso debe ser mayor que cero", null);

                // Crear mascota
                var mascota = new Mascota
                {
                    ClienteId = clienteId,
                    Nombre = nombre.Trim(),
                    Especie = especie.Trim(),
                    Raza = string.IsNullOrWhiteSpace(raza) ? null : raza.Trim(),
                    Edad = edad,
                    Peso = peso,
                    Color = string.IsNullOrWhiteSpace(color) ? null : color.Trim(),
                    Observaciones = string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim(),
                    Activo = true
                };

                // Llenar con atributos fillable
                var attributes = new Dictionary<string, object?>
                {
                    { "cliente_id", mascota.ClienteId },
                    { "nombre", mascota.Nombre },
                    { "especie", mascota.Especie },
                    { "raza", mascota.Raza },
                    { "edad", mascota.Edad },
                    { "peso", mascota.Peso },
                    { "color", mascota.Color },
                    { "observaciones", mascota.Observaciones },
                    { "activo", mascota.Activo }
                };

                mascota.Fill(attributes).Save();

                return (true, "Mascota creada exitosamente", mascota);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear mascota: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza una mascota existente
        /// </summary>
        public static (bool Success, string Message, Mascota? Mascota) Update(
            int id,
            int clienteId,
            string nombre, 
            string especie, 
            string? raza = null, 
            int? edad = null,
            decimal? peso = null,
            string? color = null,
            string? observaciones = null)
        {
            try
            {
                var mascota = GetById(id);
                if (mascota == null)
                    return (false, "Mascota no encontrada", null);

                // Validaciones
                if (clienteId <= 0)
                    return (false, "El cliente es requerido", null);

                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(especie))
                    return (false, "La especie es requerida", null);

                // Verificar que el cliente existe
                var cliente = Cliente.Find(clienteId);
                if (cliente == null)
                    return (false, "El cliente seleccionado no existe", null);

                // Validar edad
                if (edad.HasValue && edad < 0)
                    return (false, "La edad no puede ser negativa", null);

                // Validar peso
                if (peso.HasValue && peso <= 0)
                    return (false, "El peso debe ser mayor que cero", null);

                // Actualizar atributos
                var attributes = new Dictionary<string, object?>
                {
                    { "cliente_id", clienteId },
                    { "nombre", nombre.Trim() },
                    { "especie", especie.Trim() },
                    { "raza", string.IsNullOrWhiteSpace(raza) ? null : raza.Trim() },
                    { "edad", edad },
                    { "peso", peso },
                    { "color", string.IsNullOrWhiteSpace(color) ? null : color.Trim() },
                    { "observaciones", string.IsNullOrWhiteSpace(observaciones) ? null : observaciones.Trim() }
                };

                mascota.Fill(attributes).Save();

                return (true, "Mascota actualizada exitosamente", mascota);
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
                return Mascota.Where("activo", true)
                             .Get()
                             .Cast<Mascota>()
                             .Select(m => m.Especie)
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

                return Mascota.Where("especie", especie)
                             .Where("activo", true)
                             .Get()
                             .Cast<Mascota>()
                             .Select(m => m.Raza)
                             .Where(r => !string.IsNullOrEmpty(r))
                             .Distinct()
                             .OrderBy(r => r)
                             .ToList()!;
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
                var total = Mascota.Count();
                var activas = Mascota.Where("activo", true).Count();
                var inactivas = total - activas;
                
                var mascotasActivas = GetAll();
                var cachorros = mascotasActivas.Count(m => m.EsCachorro());
                var adultos = mascotasActivas.Count(m => m.EsAdulto());

                return (total, activas, inactivas, cachorros, adultos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas: {ex.Message}", ex);
            }
        }
    }
}