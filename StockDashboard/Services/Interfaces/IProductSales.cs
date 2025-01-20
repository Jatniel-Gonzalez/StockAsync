using StockDto;

namespace StockDashboard.Services.Interfaces
{
    // Interfaz para el servicio de gestión de ventas de productos
    public interface IProductSales
    {
        // Método para obtener la lista de ventas de productos.
        Task<List<ProductSale_DTO>> GetAsync();

        // Método para guardar una nueva venta de producto.
        Task<int> SavedAsync(ProductSale_DTO input);
    }
}
