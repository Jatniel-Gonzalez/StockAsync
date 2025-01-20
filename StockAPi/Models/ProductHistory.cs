using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace StockAPi.Models;

public partial class ProductHistory
{
    // Identificador único del historial
    public int HistoryId { get; set; }

    // Identificador del producto asociado a este historial
    public int ProductId { get; set; }

    // Cantidad que fue movida (entrada o salida de stock)
    public int Quantity { get; set; }

    // Tipo de movimiento (por ejemplo, "Entrada" o "Salida")
    public string Type { get; set; } = null!;

    // Fecha de la operación de movimiento del producto
    [Required]
    public DateTime OperationDate { get; set; }

    // Comentarios adicionales sobre la operación (opcional)
    public string? Remarks { get; set; }

    // Relación con el producto que tiene este historial de movimiento
    public virtual Product Product { get; set; } = null!;
}

