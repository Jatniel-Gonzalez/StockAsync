using System.ComponentModel.DataAnnotations;

namespace StockDto
{
    public class ProductSale_DTO
    {
        // Campo obligatorio para la identificación del producto vendido
        [Required]
        public int Product_Id { get; set; }

        // Campo obligatorio para el nombre del producto vendido
        [Required]
        public string Name_Product { get; set; } = null!;

        // Campo obligatorio para la fecha en que se realizó la venta
        [Required]
        public DateTime? SaleDate { get; set; }

        // Campo obligatorio para la cantidad de productos vendidos, con validación para evitar cantidades negativas
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "No se permite cantidad negativa o valor inferior a uno.")]
        public int Quantity { get; set; }

        // Campo obligatorio para el nombre del cliente que realizó la compra
        [Required]
        public string CustomerName { get; set; } = null!;

        // Campo obligatorio para el precio total de la venta
        [Required]
        public decimal TotalPrice { get; set; }
    }
}
