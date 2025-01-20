using StockDashboard.Services.Interfaces;
using StockDto;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockDashboard.Services
{
    public class ProductSale : IProductSales
    {
        private readonly HttpClient _httpClient;

        public ProductSale (HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        public  async Task<List<ProductSale_DTO>> GetAsync()
        {
            try
            {
                var result = await _httpClient.GetFromJsonAsync<List<ProductSale_DTO>>("api/Product/sales");

                if (result == null)
                {
                    throw new Exception("No se encontraron Salida en la respuesta.");
                }
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }

        public async  Task<int> SavedAsync(ProductSale_DTO input)
        {
            try
            {
                var result = await _httpClient.PostAsJsonAsync("/api/Product/sale", input);
                if (result.IsSuccessStatusCode)
                    return 1;
                else
                    return 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw;
            }
        }
    }
}
