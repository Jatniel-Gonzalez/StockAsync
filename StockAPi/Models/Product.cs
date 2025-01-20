using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockAPi.Models;

public partial class Product
{
    // Identificador único del producto
    public int ProductId { get; set; }

    // Identificador de la categoría a la que pertenece el producto
    public int CategoryId { get; set; }

    // Nombre del producto, requerido. Si falta, se muestra un mensaje de error
    [Required(ErrorMessage = "Debe escribir el campo {0}")]
    public string Name { get; set; } = null!;

    // Descripción opcional del producto
    public string? Description { get; set; }

    // Cantidad de producto disponible en inventario, requerida
    [Required(ErrorMessage = "Debe especificar cantidad de {0}")]
    public int Stock { get; set; }

    // Precio del producto, requerido
    [Required(ErrorMessage = "coloque el {0}")]
    public decimal Price { get; set; }

    // Fecha de creación del producto
    public DateTime? CreatedAt { get; set; }

    // Fecha de última actualización del producto
    public DateTime? UpdatedAt { get; set; } = null!;

    // Relación con la categoría a la que pertenece este producto
    public virtual Category Category { get; set; } = null!;

    // Relación uno a muchos con el historial de operaciones del producto
    public virtual ICollection<ProductHistory> ProductHistories { get; set; } = new List<ProductHistory>();

    // Relación uno a muchos con las ventas realizadas de este producto
    public virtual ICollection<Sale> Sales { get; set; } = new List<Sale>();
}
