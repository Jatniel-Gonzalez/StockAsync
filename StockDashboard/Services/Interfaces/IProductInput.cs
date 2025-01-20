using StockDto;

namespace StockDashboard.Services.Interfaces
{
    // Interfaz para el servicio de gestión de productos
    public interface IProductInput
    {
        // Método para obtener la lista de productos.
        Task<List<ProductInput_DTO>> GetAsync();

        // Método para obtener un producto específico por su ID.
        Task<ProductInput_DTO> GetByAsync(int id);

        // Método para guardar un nuevo producto.
        Task<int> SavedAsync(ProductInput_DTO input);

        // Método para actualizar un producto existente.
        Task<int> UpdateAsync(int productId, ProductInput_DTO input);

        // Método para eliminar un producto por su ID.
        Task<int> DeleteAsync(int productId);
    }
}
