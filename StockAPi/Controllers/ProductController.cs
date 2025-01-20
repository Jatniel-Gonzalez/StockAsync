using Microsoft.AspNetCore.Mvc;
using StockAPi.Models;
using StockAPi.Services;
using StockDto;

namespace StockAPi.Controllers
{
    // Definición de la clase ProductController, que gestiona las operaciones relacionadas con los productos
    // y las categorías en el inventario.
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : Controller
    {
        // Inyección del servicio de productos, encargado de manejar la lógica de negocio
        private readonly ProductService _productService;

        // Constructor que recibe el servicio de productos
        public ProductController(ProductService productService)
        {
            _productService = productService;
        }

        // Método para obtener una lista de productos de tipo "entrada" (stocks de productos)
        [HttpGet("inputs")]
        public async Task<IActionResult> GetProductInputs()
        {
            try
            {
                // Llamada al servicio para obtener los productos
                var inputs = await _productService.GetProductAsync();
                return Ok(inputs);  // Retorna 200 OK si la operación es exitosa
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Obtener un producto por su ID
        [HttpGet("inputs/{id}")]
        public async Task<IActionResult> GetProductById(int id)
        {
            try
            {
                // Llamada al servicio para obtener un producto por ID
                var product = await _productService.GetProductByAsync(id);
                if (product == null)
                {
                    return NotFound(new { Message = $"Producto con ID {id} no encontrado." }); // 404 Not Found si no se encuentra
                }

                return Ok(product); // 200 OK si el producto se encuentra
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Obtener una lista de productos de tipo "venta"
        [HttpGet("sales")]
        public async Task<IActionResult> GetProductSales()
        {
            try
            {
                // Llamada al servicio para obtener las ventas de productos
                var sales = await _productService.GetSalesAsync();
                return Ok(sales);  // 200 OK si la operación es exitosa
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Obtener una lista de categorías de productos
        [HttpGet("category")]
        public async Task<IActionResult> GetCategory()
        {
            try
            {
                // Llamada al servicio para obtener las categorías
                var category = await _productService.GetCategoryAsync();
                return Ok(category); // 200 OK si la operación es exitosa
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Registrar una venta de producto
        [HttpPost("sale")]
        public async Task<IActionResult> RegisterProductSale([FromBody] ProductSale_DTO productSaleDto)
        {
            try
            {
                // Llamada al servicio para registrar la venta
                int result = await _productService.SavedSaleAsync(productSaleDto);
                return result switch
                {
                    1 => Ok(new { Message = "Venta registrada exitosamente" }), // 200 OK si la venta fue exitosa
                    0 => NotFound(new { Message = "Producto no encontrado" }), // 404 Not Found si no se encuentra el producto
                    -1 => Conflict(new { Message = "Stock insuficiente" }), // 409 Conflict si no hay suficiente stock
                    _ => StatusCode(500, new { Message = "Error interno del servidor" }) // 500 Internal Server Error en caso de error
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Registrar un producto en el inventario (stock)
        [HttpPost("stock")]
        public async Task<IActionResult> CreateProductStock([FromBody] ProductInput_DTO productInputDto)
        {
            try
            {
                // Llamada al servicio para registrar un nuevo producto en el inventario
                int result = await _productService.SavedProductAsync(productInputDto);

                return result switch
                {
                    1 => Ok(new { Message = "Producto registrado en el inventario exitosamente" }), // 200 OK si la operación es exitosa
                    _ => StatusCode(500, new { Message = "Error interno del servidor" }) // 500 Internal Server Error en caso de error
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Crear una nueva categoría (solo si la categoría no existe previamente)
        [HttpPost("category")]
        public async Task<IActionResult> CreateCategory([FromBody] CategoryInput_DTO categoryInput_DTO)
        {
            try
            {
                // Llamada al servicio para registrar la categoría
                var category = await _productService.SavedCategoryAsync(categoryInput_DTO);
                return CreatedAtAction(nameof(CreateCategory), new { Message = "Categoría registrada exitosamente", Category = category }); // 201 Created si la categoría es creada
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Actualizar los datos de un producto existente
        [HttpPut("{productId}")]
        public async Task<IActionResult> UpdateProduct(int productId, [FromBody] ProductInput_DTO productDto)
        {
            try
            {
                // Llamada al servicio para actualizar un producto
                int result = await _productService.UpdateProductAsync(productId, productDto);

                return result switch
                {
                    1 => NoContent(), // 204 No Content si la operación es exitosa
                    0 => NotFound(new { Message = "Producto no encontrado" }), // 404 Not Found si el producto no existe
                    _ => StatusCode(500, new { Message = "Error interno del servidor" }) // 500 Internal Server Error en caso de error
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }

        // Eliminar un producto del inventario
        [HttpDelete("{productId}")]
        public async Task<IActionResult> DeleteProduct(int productId)
        {
            try
            {
                // Llamada al servicio para eliminar un producto
                int result = await _productService.DeleteProductAsync(productId);

                return result switch
                {
                    1 => NoContent(), // 204 No Content si la operación es exitosa
                    0 => NotFound(new { Message = "Producto no encontrado" }), // 404 Not Found si el producto no existe
                    _ => StatusCode(500, new { Message = "Error interno del servidor" }) // 500 Internal Server Error en caso de error
                };
            }
            catch (Exception ex)
            {
                return BadRequest(new { Message = ex.Message }); // 400 Bad Request si ocurre una excepción
            }
        }
    }
}
