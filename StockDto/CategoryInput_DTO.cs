    using System.ComponentModel.DataAnnotations;

    namespace StockDto
    {
    public class CategoryInput_DTO
    {
        // Campo obligatorio para la identificación de la categoría (requiere un valor)
        [Required]
        public int CategoryId { get; set; }  // ID de Categoría (para actualizar o eliminar)

        // Campo obligatorio para el nombre de la categoría
        public string Name_Category { get; set; } = null!;

        // Campo obligatorio para la descripción de la categoría
        public string Description { get; set; } = null!;

        // Fecha de la operación relacionada con la categoría (puede ser nula)
        public DateTime? OperationDate { get; set; }
    }
}
