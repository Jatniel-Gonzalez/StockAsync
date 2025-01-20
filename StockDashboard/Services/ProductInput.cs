using StockDashboard.Services.Interfaces;
using StockDto;
using System.Net.Http.Json;
using System.Text.Json;

namespace StockDashboard.Services
{
    public class ProductInput : IProductInput
    {
        private readonly HttpClient _httpClient; // Instancia de HttpClient para hacer solicitudes HTTP.

        // Constructor que inyecta la dependencia de HttpClient.
        public ProductInput(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener una lista de productos.
        public async Task<List<ProductInput_DTO>> GetAsync()
        {
            try
            {
                // Realiza una solicitud GET para obtener los productos de la API.
                var result = await _httpClient.GetFromJsonAsync<List<ProductInput_DTO>>("api/Product/inputs");

                if (result == null)
                {
                    throw new Exception("No se encontraron productos en la respuesta.");
                }
                return result; // Devuelve la lista de productos obtenida.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Lanza la excepción para manejarla en otro nivel.
            }
        }

        // Método para obtener un producto por su ID.
        public async Task<ProductInput_DTO> GetByAsync(int id)
        {
            try
            {
                // Realiza una solicitud GET para obtener un producto específico de la API.
                var result = await _httpClient.GetFromJsonAsync<ProductInput_DTO>($"api/Product/inputs/{id}");

                if (result == null)
                {
                    throw new Exception("No se encontraron productos en la respuesta.");
                }
                return result; // Devuelve el producto obtenido.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Lanza la excepción para manejarla en otro nivel.
            }
        }

        // Método para guardar un nuevo producto.
        public async Task<int> SavedAsync(ProductInput_DTO input)
        {
            try
            {
                // Realiza una solicitud POST para guardar el producto en la API.
                var result = await _httpClient.PostAsJsonAsync("/api/Product/stock", input);
                if (result.IsSuccessStatusCode)
                    return 1; // Devuelve 1 si la solicitud fue exitosa.
                else
                    return 0; // Devuelve 0 si hubo un error.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                return 0; // Devuelve 0 en caso de error.
            }
        }

        // Método para actualizar un producto existente.
        public async Task<int> UpdateAsync(int productId, ProductInput_DTO input)
        {
            try
            {
                // Convierte el producto a formato JSON.
                string json = JsonSerializer.Serialize(input);
                Console.WriteLine("JSON Enviado: " + json);

                // Realiza una solicitud PUT para actualizar el producto.
                var result = await _httpClient.PutAsJsonAsync($"api/Product/{productId}", input);
                if (result.IsSuccessStatusCode)
                    return 1; // Devuelve 1 si la actualización fue exitosa.
                else
                    return 0; // Devuelve 0 si hubo un error.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Lanza la excepción para manejarla en otro nivel.
            }
        }

        // Método para eliminar un producto.
        public async Task<int> DeleteAsync(int productId)
        {
            try
            {
                // Realiza una solicitud DELETE para eliminar el producto.
                var result = await _httpClient.DeleteAsync($"api/Product/{productId}");
                if (result.IsSuccessStatusCode)
                    return 1; // Devuelve 1 si la eliminación fue exitosa.
                else
                    return 0; // Devuelve 0 si hubo un error.
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Lanza la excepción para manejarla en otro nivel.
            }
        }
    }
}
