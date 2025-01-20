using StockDashboard.Services.Interfaces;
using StockDto;
using System.Net.Http.Json;

namespace StockDashboard.Services
{
    public class ProductCategory : IProductCategory
    {
        private readonly HttpClient _httpClient; // Instancia de HttpClient para hacer solicitudes HTTP.

        // Constructor que inyecta la dependencia de HttpClient.
        public ProductCategory(HttpClient httpClient)
        {
            _httpClient = httpClient;
        }

        // Método para obtener la lista de categorías desde la API.
        public async Task<List<CategoryInput_DTO>> GetAsync()
        {
            try
            {
                // Realiza una solicitud GET para obtener las categorías de la API.
                var result = await _httpClient.GetFromJsonAsync<List<CategoryInput_DTO>>("api/Product/category");

                // Si no se reciben categorías, lanza una excepción.
                if (result == null)
                {
                    throw new Exception("No se encontraron Categorias en la respuesta.");
                }
                return result; // Devuelve la lista de categorías obtenida.
            }
            catch (Exception ex)
            {
                // Captura cualquier excepción y la muestra por consola.
                Console.WriteLine($"Error: {ex.Message}");
                throw; // Vuelve a lanzar la excepción para que la maneje el código que llama al método.
            }
        }

        // Método para guardar una nueva categoría o devolver la existente si ya existe.
        public async Task<CategoryInput_DTO> SavedAsync(CategoryInput_DTO input)
        {
            try
            {
                // Realiza una solicitud POST para guardar la nueva categoría.
                var result = await _httpClient.PostAsJsonAsync("/api/Product/category", input);

                if (result.IsSuccessStatusCode)
                {
                    // Si la categoría ya existe, la respuesta contendrá la categoría existente.
                    var category = await result.Content.ReadFromJsonAsync<CategoryInput_DTO>();
                    Console.WriteLine($"Categoría: {input.Name_Category} ya existe.");
                    return category; // Devuelve la categoría existente.
                }
                else
                {
                    // Si ocurre un error al insertar la categoría, se muestra un mensaje.
                    Console.WriteLine($"Error: en la comprobación o inserción de la categoría");
                    return null; // Devuelve null en caso de error.
                }
            }
            catch (Exception ex)
            {
                // Captura y muestra cualquier excepción que ocurra durante el proceso de guardado.
                Console.WriteLine($"Error al guardar la categoría: {ex.Message}");
                throw; // Vuelve a lanzar la excepción para manejarla en otro nivel si es necesario.
            }
        }
    }
}
