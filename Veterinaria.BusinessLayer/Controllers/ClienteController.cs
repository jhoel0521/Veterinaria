using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.ModelLayer;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.BusinessLayer.Controllers
{
    /// <summary>
    /// Controlador para gestión de Clientes
    /// Maneja la lógica de negocio para operaciones CRUD
    /// </summary>
    public static class ClienteController
    {
        /// <summary>
        /// Obtiene todos los clientes activos con paginación opcional
        /// </summary>
        public static List<Cliente> GetAll(int? limit = null)
        {
            try
            {
                var query = Cliente.Where("activo", true).OrderBy("nombre");
                
                if (limit.HasValue)
                {
                    query = query.Limit(limit.Value);
                }

                return query.Get().Cast<Cliente>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener clientes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca clientes por texto (nombre, apellido, email)
        /// </summary>
        public static List<Cliente> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }

                // Buscar en nombre, apellido o email
                var query = Cliente.Where("activo", true);

                // Usar WhereRaw para búsqueda más compleja
                var searchSql = "(nombre LIKE @search OR apellido LIKE @search OR email LIKE @search)";
                var parameters = new Dictionary<string, object>
                {
                    { "@search", $"%{searchText}%" }
                };

                return query.WhereRaw(searchSql, parameters)
                           .OrderBy("nombre")
                           .Get().Cast<Cliente>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar clientes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un cliente por ID
        /// </summary>
        public static Cliente? GetById(int id)
        {
            try
            {
                return Cliente.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente con ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        public static (bool Success, string Message, Cliente? Cliente) Create(
            string nombre, 
            string apellido, 
            string? telefono = null, 
            string? email = null, 
            string? direccion = null)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(apellido))
                    return (false, "El apellido es requerido", null);

                if (!string.IsNullOrWhiteSpace(email))
                {
                    // Validar formato de email (básico)
                    if (!IsValidEmail(email))
                        return (false, "El formato del email no es válido", null);

                    // Verificar que el email no esté en uso
                    var existingClient = Cliente.BuscarPorEmail(email);
                    if (existingClient != null)
                        return (false, "Ya existe un cliente con ese email", null);
                }

                // Crear cliente
                var cliente = new Cliente
                {
                    Nombre = nombre.Trim(),
                    Apellido = apellido.Trim(),
                    Telefono = string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim(),
                    Email = string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLower(),
                    Direccion = string.IsNullOrWhiteSpace(direccion) ? null : direccion.Trim(),
                    Activo = true
                };

                // Llenar con atributos fillable
                var attributes = new Dictionary<string, object?>
                {
                    { "nombre", cliente.Nombre },
                    { "apellido", cliente.Apellido },
                    { "telefono", cliente.Telefono },
                    { "email", cliente.Email },
                    { "direccion", cliente.Direccion },
                    { "activo", cliente.Activo }
                };

                cliente.Fill(attributes).Save();

                return (true, "Cliente creado exitosamente", cliente);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear cliente: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza un cliente existente
        /// </summary>
        public static (bool Success, string Message, Cliente? Cliente) Update(
            int id,
            string nombre, 
            string apellido, 
            string? telefono = null, 
            string? email = null, 
            string? direccion = null)
        {
            try
            {
                var cliente = GetById(id);
                if (cliente == null)
                    return (false, "Cliente no encontrado", null);

                // Validaciones
                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (string.IsNullOrWhiteSpace(apellido))
                    return (false, "El apellido es requerido", null);

                if (!string.IsNullOrWhiteSpace(email))
                {
                    if (!IsValidEmail(email))
                        return (false, "El formato del email no es válido", null);

                    // Verificar que el email no esté en uso por otro cliente
                    var existingClient = Cliente.BuscarPorEmail(email);
                    if (existingClient != null && existingClient.Id != id)
                        return (false, "Ya existe otro cliente con ese email", null);
                }

                // Actualizar atributos
                var attributes = new Dictionary<string, object?>
                {
                    { "nombre", nombre.Trim() },
                    { "apellido", apellido.Trim() },
                    { "telefono", string.IsNullOrWhiteSpace(telefono) ? null : telefono.Trim() },
                    { "email", string.IsNullOrWhiteSpace(email) ? null : email.Trim().ToLower() },
                    { "direccion", string.IsNullOrWhiteSpace(direccion) ? null : direccion.Trim() }
                };

                cliente.Fill(attributes).Save();

                return (true, "Cliente actualizado exitosamente", cliente);
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar cliente: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Elimina un cliente (soft delete)
        /// </summary>
        public static (bool Success, string Message) Delete(int id)
        {
            try
            {
                var cliente = GetById(id);
                if (cliente == null)
                    return (false, "Cliente no encontrado");

                // Verificar si tiene mascotas o ventas asociadas
                var mascotas = cliente.GetMascotas();
                var ventas = cliente.GetVentas();

                if (mascotas.Any() || ventas.Any())
                {
                    // Soft delete - marcar como inactivo
                    var attributes = new Dictionary<string, object?>
                    {
                        { "activo", false }
                    };
                    cliente.Fill(attributes).Save();
                    
                    return (true, "Cliente desactivado exitosamente (tiene registros asociados)");
                }
                else
                {
                    // Hard delete si no tiene registros asociados
                    cliente.Delete();
                    return (true, "Cliente eliminado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Reactiva un cliente desactivado
        /// </summary>
        public static (bool Success, string Message) Reactivate(int id)
        {
            try
            {
                var cliente = GetById(id);
                if (cliente == null)
                    return (false, "Cliente no encontrado");

                var attributes = new Dictionary<string, object?>
                {
                    { "activo", true }
                };
                cliente.Fill(attributes).Save();

                return (true, "Cliente reactivado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al reactivar cliente: {ex.Message}");
            }
        }

        /// <summary>
        /// Validación básica de formato de email
        /// </summary>
        private static bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Obtiene estadísticas básicas de clientes
        /// </summary>
        public static (int Total, int Activos, int Inactivos) GetStats()
        {
            try
            {
                var total = Cliente.Count();
                var activos = Cliente.Where("activo", true).Count();
                var inactivos = total - activos;

                return (total, activos, inactivos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas: {ex.Message}", ex);
            }
        }
    }
}