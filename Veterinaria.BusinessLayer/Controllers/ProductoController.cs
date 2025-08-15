using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.ModelLayer;
using Veterinaria.DataLayer.QueryBuilder;

namespace Veterinaria.BusinessLayer.Controllers
{
    /// <summary>
    /// Controlador para gestión de Productos
    /// Maneja la lógica de negocio para operaciones CRUD
    /// </summary>
    public static class ProductoController
    {
        /// <summary>
        /// Obtiene todos los productos con paginación opcional
        /// </summary>
        public static List<Producto> GetAll(int? limit = null)
        {
            try
            {
                var query = Producto.OrderBy("nombre");
                
                if (limit.HasValue)
                {
                    query = query.Limit(limit.Value);
                }

                return query.Get().Cast<Producto>().ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener productos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca productos por texto (nombre)
        /// </summary>
        public static List<Producto> Search(string searchText)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(searchText))
                {
                    return GetAll();
                }

                var searchTerm = $"%{searchText.Trim()}%";

                // Buscar por nombre
                var productosPorNombre = Producto.Where("nombre", SqlOperator.Like, searchTerm)
                                               .Get().Cast<Producto>().ToList();

                // Eliminar duplicados y ordenar
                return productosPorNombre
                    .GroupBy(p => p.Id)
                    .Select(g => g.First())
                    .OrderBy(p => p.Nombre)
                    .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar productos: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca productos por categoría
        /// </summary>
        public static List<Producto> SearchByCategoria(string categoria)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(categoria))
                    return GetAll();

                // Buscar categoría por nombre
                var categoriaObj = Categoria.Where("nombre", SqlOperator.Like, $"%{categoria.Trim()}%")
                                           .First() as Categoria;
                
                if (categoriaObj != null)
                {
                    // Buscar productos por categoria_id
                    return Producto.Where("categoria_id", categoriaObj.Id)
                                  .Get().Cast<Producto>().ToList();
                }

                return new List<Producto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar productos por categoría: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene productos con stock bajo
        /// </summary>
        public static List<Producto> GetProductosConStockBajo()
        {
            try
            {
                // Como no hay campo de stock, devolver lista vacía por ahora
                return new List<Producto>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener productos con stock bajo: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene un producto por ID
        /// </summary>
        public static Producto? GetById(int id)
        {
            try
            {
                return Producto.Find(id);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener producto con ID {id}: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Busca un producto por código
        /// </summary>
        public static Producto? GetByCodigo(string codigo)
        {
            try
            {
                if (string.IsNullOrWhiteSpace(codigo))
                    return null;

                return Producto.Where("codigo", codigo.Trim())
                              .Where("activo", true)
                              .Get()
                              .Cast<Producto>()
                              .FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al buscar producto por código: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Crea un nuevo producto
        /// </summary>
        public static (bool Success, string Message, Producto? Producto) Create(
            string nombre,
            decimal precio = 0,
            bool requiereDiagnostico = true,
            int categoriaId = 1)
        {
            try
            {
                // Validaciones
                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (precio < 0)
                    return (false, "El precio no puede ser negativo", null);

                if (categoriaId <= 0)
                    return (false, "La categoría es requerida", null);

                // Crear producto
                var producto = new Producto
                {
                    Nombre = nombre.Trim(),
                    Precio = precio,
                    RequiereDiagnostico = requiereDiagnostico,
                    CategoriaId = categoriaId
                };

                // Llenar con atributos fillable
                var attributes = new Dictionary<string, object?>
                {
                    { "nombre", producto.Nombre },
                    { "precio", producto.Precio },
                    { "requiere_diagnostico", producto.RequiereDiagnostico },
                    { "categoria_id", producto.CategoriaId }
                };

                producto.Fill(attributes).Save();

                return (true, "Producto creado exitosamente", producto);
            }
            catch (Exception ex)
            {
                return (false, $"Error al crear producto: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza un producto existente
        /// </summary>
        public static (bool Success, string Message, Producto? Producto) Update(
            int id,
            string codigo,
            string nombre,
            string? descripcion = null,
            decimal precio = 0,
            int stock = 0,
            int stockMinimo = 0,
            string? categoria = null,
            string? proveedor = null)
        {
            try
            {
                var producto = GetById(id);
                if (producto == null)
                    return (false, "Producto no encontrado", null);

                // Validaciones
                if (string.IsNullOrWhiteSpace(codigo))
                    return (false, "El código es requerido", null);

                if (string.IsNullOrWhiteSpace(nombre))
                    return (false, "El nombre es requerido", null);

                if (precio < 0)
                    return (false, "El precio no puede ser negativo", null);

                if (stock < 0)
                    return (false, "El stock no puede ser negativo", null);

                if (stockMinimo < 0)
                    return (false, "El stock mínimo no puede ser negativo", null);

                // Verificar que el código no esté en uso por otro producto
                var existingProduct = GetByCodigo(codigo);
                if (existingProduct != null && existingProduct.Id != id)
                    return (false, "Ya existe otro producto con ese código", null);

                // Actualizar atributos
                var attributes = new Dictionary<string, object?>
                {
                    { "codigo", codigo.Trim().ToUpper() },
                    { "nombre", nombre.Trim() },
                    { "descripcion", string.IsNullOrWhiteSpace(descripcion) ? null : descripcion.Trim() },
                    { "precio", precio },
                    { "stock", stock },
                    { "stock_minimo", stockMinimo },
                    { "categoria", string.IsNullOrWhiteSpace(categoria) ? null : categoria.Trim() },
                    { "proveedor", string.IsNullOrWhiteSpace(proveedor) ? null : proveedor.Trim() }
                };

                producto.Fill(attributes).Save();

                return (true, "Producto actualizado exitosamente", producto);
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar producto: {ex.Message}", null);
            }
        }

        /// <summary>
        /// Actualiza solo el stock de un producto
        /// </summary>
        public static (bool Success, string Message) UpdateStock(int id, int nuevoStock)
        {
            try
            {
                var producto = GetById(id);
                if (producto == null)
                    return (false, "Producto no encontrado");

                if (nuevoStock < 0)
                    return (false, "El stock no puede ser negativo");

                var attributes = new Dictionary<string, object?>
                {
                    { "stock", nuevoStock }
                };

                producto.Fill(attributes).Save();

                return (true, "Stock actualizado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al actualizar stock: {ex.Message}");
            }
        }

        /// <summary>
        /// Elimina un producto (soft delete)
        /// </summary>
        public static (bool Success, string Message) Delete(int id)
        {
            try
            {
                var producto = GetById(id);
                if (producto == null)
                    return (false, "Producto no encontrado");

                // Verificar si el producto tiene ventas asociadas
                var tieneVentas = DetalleFactura.Where("producto_id", id).Count() > 0;

                if (tieneVentas)
                {
                    // Soft delete - marcar como inactivo
                    var attributes = new Dictionary<string, object?>
                    {
                        { "activo", false }
                    };
                    producto.Fill(attributes).Save();
                    
                    return (true, "Producto desactivado exitosamente (tiene ventas asociadas)");
                }
                else
                {
                    // Hard delete si no tiene ventas asociadas
                    producto.Delete();
                    return (true, "Producto eliminado exitosamente");
                }
            }
            catch (Exception ex)
            {
                return (false, $"Error al eliminar producto: {ex.Message}");
            }
        }

        /// <summary>
        /// Reactiva un producto desactivado
        /// </summary>
        public static (bool Success, string Message) Reactivate(int id)
        {
            try
            {
                var producto = GetById(id);
                if (producto == null)
                    return (false, "Producto no encontrado");

                var attributes = new Dictionary<string, object?>
                {
                    { "activo", true }
                };
                producto.Fill(attributes).Save();

                return (true, "Producto reactivado exitosamente");
            }
            catch (Exception ex)
            {
                return (false, $"Error al reactivar producto: {ex.Message}");
            }
        }

        /// <summary>
        /// Obtiene todas las categorías disponibles
        /// </summary>
        public static List<string> GetCategorias()
        {
            try
            {
                return Categoria.All()
                               .Select(c => c.Nombre)
                               .Where(c => !string.IsNullOrEmpty(c))
                               .Distinct()
                               .OrderBy(c => c)
                               .ToList();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener categorías: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene todos los proveedores disponibles
        /// </summary>
        public static List<string> GetProveedores()
        {
            try
            {
                // Como no hay campo proveedor en el modelo actual, devolver lista vacía
                return new List<string>();
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener proveedores: {ex.Message}", ex);
            }
        }

        /// <summary>
        /// Obtiene estadísticas básicas de productos
        /// </summary>
        public static (int Total, int Activos, int Inactivos, int ConStockBajo, decimal ValorInventario) GetStats()
        {
            try
            {
                var total = Producto.Count();
                var activos = total; // Como no hay campo activo, todos son activos
                var inactivos = 0;
                
                var productosActivos = GetAll();
                var conStockBajo = 0; // Como no hay stock, no hay productos con stock bajo
                var valorInventario = productosActivos.Sum(p => p.Precio); // Solo precio, sin stock

                return (total, activos, inactivos, conStockBajo, valorInventario);
            }
            catch (Exception ex)
            {
                throw new Exception($"Error al obtener estadísticas: {ex.Message}", ex);
            }
        }
    }
}