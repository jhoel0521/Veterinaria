using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.ModelLayer;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.BusinessLayer.Controllers
{
    /// <summary>
    /// Controlador para gesti�n de Clientes
    /// Maneja la l�gica de negocio para operaciones CRUD
    /// </summary>
    public static class ClienteController
    {
        /// <summary>
        /// Obtiene todos los clientes activos con paginación opcional
        /// </summary>
        public static List<PersonaFisica> GetAll(int? limit = null)
        {
            try
            {
                var query = PersonaFisica.OrderBy("nombre");
                
                if (limit.HasValue)
                {
                    query = query.Limit(limit.Value);
                }

                return query.Get().Cast<PersonaFisica>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener clientes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca clientes por texto (nombre, apellido, email)
        /// </summary>
        public static List<PersonaFisica> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }

                var clientes = new List<PersonaFisica>();
                var searchTerm = $"%{searchText.Trim()}%";

                // Buscar por nombre
                var clientesPorNombre = PersonaFisica.Where("nombre", SqlOperator.Like, searchTerm)
                                             .Get().Cast<PersonaFisica>().ToList();
                clientes.AddRange(clientesPorNombre);

                // Buscar por apellido
                var clientesPorApellido = PersonaFisica.Where("apellido", SqlOperator.Like, searchTerm)
                                               .Get().Cast<PersonaFisica>().ToList();
                clientes.AddRange(clientesPorApellido);

                // Buscar por email a través de la relación con Persona
                var personasConEmail = Persona.Where("email", SqlOperator.Like, searchTerm)
                                            .Get().Cast<Persona>().ToList();
                foreach (var persona in personasConEmail)
                {
                    var personaFisica = PersonaFisica.Find(persona.Id);
                    if (personaFisica != null)
                    {
                        clientes.Add(personaFisica);
                    }
                }

                // Eliminar duplicados y ordenar
                return clientes
                    .GroupBy(c => c.Id)
                    .Select(g => g.First())
                    .OrderBy(c => c.Nombre)
                    .ThenBy(c => c.Apellido)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar clientes: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un cliente por ID
        /// </summary>
        public static PersonaFisica? GetById(int id)
        {
            try
            {
                return PersonaFisica.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener cliente con ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo cliente
        /// </summary>
        public static (bool Success, string Message, PersonaFisica? Cliente) Create(
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
                    // Validar formato de email (b�sico)
                    if (!IsValidEmail(email))
                        return (false, "El formato del email no es v�lido", null);

                    // Verificar que el email no esté en uso
                    var existingClient = Persona.Where("email", email).First();
                    if (existingClient != null)
                        return (false, "Ya existe un cliente con ese email", null);
                }

                // Crear cliente
                var cliente = new PersonaFisica(nombre.Trim(), apellido.Trim())
                {
                    Id = 0 // Se asignará después de crear la persona base
                };

                // Crear primero la persona base
                var persona = new Persona("Fisica", email?.Trim().ToLower(), direccion?.Trim(), telefono?.Trim());
                
                // Llenar con atributos fillable de persona
                var personaAttributes = new Dictionary<string, object?>
                {
                    { "tipo", "Fisica" },
                    { "email", persona.Email },
                    { "direccion", persona.Direccion },
                    { "telefono", persona.Telefono }
                };

                persona.Fill(personaAttributes).Save();
                
                // Asignar el ID de la persona al cliente
                cliente.Id = persona.Id;

                // Llenar con atributos fillable de persona física
                var clienteAttributes = new Dictionary<string, object?>
                {
                    { "nombre", cliente.Nombre },
                    { "apellido", cliente.Apellido }
                };

                cliente.Fill(clienteAttributes).Save();

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
        public static (bool Success, string Message, PersonaFisica? Cliente) Update(
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
                        return (false, "El formato del email no es v�lido", null);

                    // Verificar que el email no esté en uso por otro cliente
                    var existingClient = Persona.Where("email", email).First() as Persona;
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
                var mascotasCount = Animal.Where("persona_id", cliente.Id).Count();
                var ventasCount = Factura.Where("persona_id", cliente.Id).Count();

                if (mascotasCount > 0 || ventasCount > 0)
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
        /// Validaci�n b�sica de formato de email
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
        /// Obtiene estad�sticas b�sicas de clientes
        /// </summary>
        public static (int Total, int Activos, int Inactivos) GetStats()
        {
            try
            {
                var total = Persona.Count();
                var activos = Persona.Where("activo", true).Count();
                var inactivos = total - activos;

                return (total, activos, inactivos);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estad�sticas: {ex.Message}", ex);
            }
        }
    }
}