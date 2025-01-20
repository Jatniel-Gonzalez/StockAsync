using StockDto;

namespace StockDashboard.Services.Interfaces
{
    // Interfaz para el servicio de gestión de categorías de productos
    public interface IProductCategory
    {
        // Método para obtener una lista de categorías de productos.
        Task<List<CategoryInput_DTO>> GetAsync();

        // Método para guardar una nueva categoría de producto.
        Task<CategoryInput_DTO> SavedAsync(CategoryInput_DTO input);
    }
}
