using System;
using System.Collections.Generic;

namespace StockAPi.Models;

public partial class Sale
{
    // Identificador único de la venta
    public int SaleId { get; set; }

    // Identificador del producto vendido
    public int ProductId { get; set; }

    // Cantidad de productos vendidos
    public int Quantity { get; set; }

    // Precio total de la venta (generalmente cantidad * precio)
    public decimal TotalPrice { get; set; }

    // Fecha en que se realizó la venta
    public DateTime? SaleDate { get; set; }

    // Nombre del cliente que realizó la compra (opcional)
    public string? CustomerName { get; set; }

    // Comentarios adicionales sobre la venta (opcional)
    public string? Remarks { get; set; }

    // Relación con el producto que fue vendido
    public virtual Product Product { get; set; } = null!;
}
