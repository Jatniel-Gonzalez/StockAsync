using StockAPi.Models;
using System.ComponentModel.DataAnnotations;

public partial class Category
{
    // Identificador único para la categoría
    public int CategoryId { get; set; }

    // Nombre de la categoría, requerido. Si falta, se mostrará el mensaje de error especificado
    [Required(ErrorMessage = "Debes rellenar el campo nombre")]
    public string Name { get; set; } = null!;

    // Descripción opcional de la categoría
    public string? Description { get; set; }

    // Fecha de creación de la categoría
    public DateTime? CreatedAt { get; set; }

    // Fecha de última actualización de la categoría
    public DateTime? UpdatedAt { get; set; }

    // Relación uno a muchos con los productos que pertenecen a esta categoría
    public virtual ICollection<Product> Products { get; set; } = new List<Product>();
}
