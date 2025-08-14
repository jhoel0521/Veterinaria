using System;
using System.Collections.Generic;
using System.Linq;
using Veterinaria.DataLayer.Entities;

namespace Veterinaria.DataLayer.Entities
{
    /// <summary>
    /// Modelo Cliente - Representa la tabla 'Clientes' en la base de datos
    /// </summary>
    public class Cliente : Model<Cliente>
    {
        protected override string Table { get; set; } = "Clientes";
        protected override string PrimaryKey { get; set; } = "id";
        protected override string[] Fillable { get; set; } = 
        {
            "nombre", "apellido", "telefono", "email", "direccion", "activo"
        };
        protected override bool Timestamps { get; set; } = true;

        // Propiedades públicas
        public int Id { get; set; }
        public string Nombre { get; set; } = string.Empty;
        public string Apellido { get; set; } = string.Empty;
        public string? Telefono { get; set; }
        public string? Email { get; set; }
        public string? Direccion { get; set; }
        public DateTime CreatedAt { get; set; }
        public DateTime UpdatedAt { get; set; }
        public bool Activo { get; set; } = true;

        public Cliente() : base() { }

        public Cliente(string nombre, string apellido, string? telefono = null, string? email = null)
        {
            Nombre = nombre;
            Apellido = apellido;
            Telefono = telefono;
            Email = email;
        }

        // Métodos de conveniencia
        public static List<Cliente> ClientesActivos()
        {
            return Where("activo", true).Get().Cast<Cliente>().ToList();
        }

        public static Cliente? BuscarPorEmail(string email)
        {
            return Where("email", email).Get().Cast<Cliente>().FirstOrDefault();
        }

        public static List<Cliente> BuscarPorNombre(string nombre)
        {
            return Where("nombre", QueryBuilder.SqlOperator.Like, $"%{nombre}%")
                   .Get().Cast<Cliente>().ToList();
        }

        public string NombreCompleto()
        {
            return $"{Nombre} {Apellido}";
        }

        // Relación con Mascotas
        public List<Mascota> GetMascotas()
        {
            return Mascota.Where("cliente_id", Id).Get().Cast<Mascota>().ToList();
        }

        // Relación con Ventas
        public List<Venta> GetVentas()
        {
            return Venta.Where("cliente_id", Id).Get().Cast<Venta>().ToList();
        }

        public override string ToString()
        {
            return $"Cliente[{Id}]: {NombreCompleto()} - {Email}";
        }
    }
}
