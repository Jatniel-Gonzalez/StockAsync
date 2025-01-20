using System.ComponentModel.DataAnnotations;

namespace StockDto
{
    public class ProductInput_DTO
    {
        // Campo obligatorio para la identificación del producto
        [Required]
        public int Product_Id { get; set; }

        // Campo obligatorio para la categoría del producto, con mensaje de error personalizado si no se selecciona
        [Required(ErrorMessage = "Selecciona una categoría")]
        public int CategoryId { get; set; }

        // Campo obligatorio para el nombre del producto
        [Required]
        public string? Name_Product { get; set; } = null;

        // Campo opcional para el nombre de la categoría del producto (puede ser nulo)
        public string? Name_Category { get; set; } = null;

        // Campo opcional para la descripción del producto (puede ser nulo)
        public string? Description { get; set; }

        // Campo obligatorio para la fecha de la operación relacionada con el producto
        [Required]
        public DateTime? OperationDate { get; set; }

        // Campo obligatorio para el stock del producto, con validación para evitar valores negativos
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El stock no puede ser negativo.")]
        public int Stock { get; set; }

        // Campo obligatorio para el precio del producto, con validación para asegurar que el precio sea mayor que 0
        [Required]
        [Range(1, int.MaxValue, ErrorMessage = "El precio debe ser mayor que 0.")]
        public decimal Price { get; set; }
    }
}
