using Microsoft.EntityFrameworkCore;
using StockAPi.Models;
using StockDto;

namespace StockAPi.Services
{
    public class ProductService
    {
        // Contexto de la base de datos
        private readonly StockControlDbContext _stockControlDbContext;

        // Constructor para inyectar el contexto de la base de datos
        public ProductService(StockControlDbContext stockControlDbContext)
        {
            _stockControlDbContext = stockControlDbContext;
        }

        // Método para obtener todos los productos con sus categorías asociadas
        public async Task<List<ProductInput_DTO>> GetProductAsync()
        {
            try
            {
                // Realiza un JOIN entre las tablas Products y Categories
                return await _stockControlDbContext.Products
                    .Join(_stockControlDbContext.Categories,
                          p => p.CategoryId,
                          c => c.CategoryId,
                          (p, c) => new { p, c }) // Proyección de los datos combinados
                    .Select(pc => new ProductInput_DTO
                    {
                        Product_Id = pc.p.ProductId,
                        CategoryId = pc.p.CategoryId,
                        Name_Product = pc.p.Name,
                        Name_Category = pc.c.Name,
                        Description = pc.p.Description,
                        OperationDate = pc.p.CreatedAt,
                        Stock = pc.p.Stock,
                        Price = pc.p.Price
                    })
                    .OrderByDescending(dto => dto.Product_Id) // Ordena los resultados por Product_Id de manera descendente
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Lanza una excepción si ocurre algún error durante la consulta
                throw new ApplicationException($"Error al obtener los productos: {ex.Message}", ex);
            }
        }

        // Método para obtener un producto específico por su ID
        public async Task<ProductInput_DTO> GetProductByAsync(int id)
        {
            try
            {
                // Realiza un JOIN entre Products y Categories, luego filtra por ProductId
                return await _stockControlDbContext.Products
                    .Join(_stockControlDbContext.Categories,
                          p => p.CategoryId,
                          c => c.CategoryId,
                          (p, c) => new { p, c })
                    .Where(pc => pc.p.ProductId == id) // Filtra por el ID del producto
                    .Select(pc => new ProductInput_DTO
                    {
                        Product_Id = pc.p.ProductId,
                        Name_Product = pc.p.Name,
                        Name_Category = pc.c.Name,
                        Description = pc.p.Description,
                        OperationDate = pc.p.CreatedAt,
                        Stock = pc.p.Stock,
                        Price = pc.p.Price
                    })
                    .FirstOrDefaultAsync(); // Retorna el primer resultado o null si no se encuentra
            }
            catch (Exception ex)
            {
                // Lanza una excepción si ocurre un error en la consulta
                throw new ApplicationException($"Error al obtener el producto con ID {id}: {ex.Message}", ex);
            }
        }

        // Método para obtener todas las ventas de productos
        public async Task<List<ProductSale_DTO>> GetSalesAsync()
        {
            try
            {
                // Realiza un JOIN entre Sales, Products y Categories
                return await _stockControlDbContext.Sales
                    .Join(_stockControlDbContext.Products,
                          s => s.ProductId,
                          p => p.ProductId,
                          (s, p) => new { s, p })
                    .Join(_stockControlDbContext.Categories,
                          sp => sp.p.CategoryId,
                          c => c.CategoryId,
                          (sp, c) => new ProductSale_DTO
                          {
                              Product_Id = sp.p.ProductId,
                              Name_Product = sp.p.Name,
                              CustomerName = sp.s.CustomerName,
                              SaleDate = sp.s.SaleDate,
                              Quantity = sp.s.Quantity,
                              TotalPrice = sp.s.TotalPrice
                          })
                    .OrderByDescending(dto => dto.SaleDate) // Ordena las ventas por fecha descendente
                    .ToListAsync();
            }
            catch (Exception ex)
            {
                // Lanza una excepción si ocurre un error durante la consulta
                throw new ApplicationException($"Error al obtener las ventas: {ex.Message}", ex);
            }
        }

        // Método para obtener todas las categorías disponibles
        public async Task<List<CategoryInput_DTO>> GetCategoryAsync()
        {
            try
            {
                // Consulta todas las categorías y las proyecta en el DTO CategoryInput_DTO
                var categories = await _stockControlDbContext.Categories
                    .Select(c => new CategoryInput_DTO
                    {
                        CategoryId = c.CategoryId,
                        Name_Category = c.Name,
                        Description = c.Description,
                        OperationDate = c.CreatedAt
                    })
                    .ToListAsync();

                return categories;
            }
            catch (Exception ex)
            {
                // Lanza una excepción si ocurre un error durante la consulta
                Console.WriteLine($"Error al obtener las categorías: {ex.Message}");
                throw new ApplicationException("Hubo un error al intentar obtener las categorías.", ex);
            }
        }
        // Método asincrónico que guarda el historial de un producto (entradas o salidas)
        public async Task SavedProductHistoryAsync(int ProductId, int quantity, string type, string? remarks = null)
        {
            try
            {
                // Se crea una nueva instancia del historial del producto con los valores proporcionados
                var productHistory = new ProductHistory
                {
                    ProductId = ProductId, // ID del producto
                    Quantity = quantity, // Cantidad afectada
                    Type = type, // Tipo de movimiento ("Entrada" o "Salida")
                    Remarks = remarks, // Comentarios adicionales (opcional)
                    OperationDate = DateTime.Now, // Fecha y hora actual de la operación
                };

                // Se agrega el historial al contexto de la base de datos de forma asincrónica
                await _stockControlDbContext.AddAsync(productHistory);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                // Si ocurre un error, se lanza una excepción con el mensaje personalizado
                throw new ApplicationException("Error al registrar el historial del producto.", ex);
            }
        }

        // Método asincrónico para guardar una venta de un producto
        public async Task<int> SavedSaleAsync(ProductSale_DTO productSaleDto)
        {
            try
            {
                // Se busca el producto correspondiente al nombre proporcionado en el DTO de venta
                var product = await _stockControlDbContext.Products
                    .FirstOrDefaultAsync(p => p.Name == productSaleDto.Name_Product);

                // Si no se encuentra el producto, se retorna 0 (producto no encontrado)
                if (product == null)
                {
                    return 0;
                }

                // Si no hay suficiente stock para la venta, se retorna -1 (stock insuficiente)
                if (product.Stock < productSaleDto.Quantity)
                {
                    return -1;
                }

                // Se decrementa el stock del producto según la cantidad vendida
                product.Stock -= productSaleDto.Quantity;

                // Se crea una nueva venta con los detalles proporcionados
                var sale = new Sale
                {
                    ProductId = product.ProductId, // ID del producto
                    Quantity = productSaleDto.Quantity, // Cantidad vendida
                    TotalPrice = product.Price * productSaleDto.Quantity, // Cálculo del precio total
                    CustomerName = productSaleDto.CustomerName, // Nombre del cliente
                    Remarks = "Venta registrada" // Comentarios adicionales sobre la venta
                };

                // Se guarda el historial del producto (salida de inventario)
                await SavedProductHistoryAsync(product.ProductId, productSaleDto.Quantity, "Salida", "Producto vendido");

                // Se agrega la venta al contexto de la base de datos
                await _stockControlDbContext.AddAsync(sale);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();

                // Se retorna 1 para indicar que la venta fue registrada correctamente
                return 1;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, se lanza una excepción con el mensaje personalizado
                throw new ApplicationException("Error al registrar la venta.", ex);
            }
        }

        // Método asincrónico para guardar un producto en la base de datos
        public async Task<int> SavedProductAsync(ProductInput_DTO productInputDto)
        {
            try
            {
                // Se busca la categoría correspondiente al ID proporcionado en el DTO del producto
                var category = await _stockControlDbContext.Categories
                    .FirstOrDefaultAsync(c => c.CategoryId == productInputDto.CategoryId);

                // Si no se encuentra la categoría, se retorna 0 (categoría no encontrada)
                if (category.CategoryId == 0)
                {
                    return 0; // Categoría no encontrada
                }

                // Se crea una nueva instancia del producto con los valores proporcionados
                var product = new Product
                {
                    Name = productInputDto.Name_Product, // Nombre del producto
                    Description = productInputDto.Description, // Descripción del producto
                    CategoryId = category.CategoryId, // ID de la categoría del producto
                    Stock = productInputDto.Stock, // Cantidad de stock
                    Price = productInputDto.Price, // Precio del producto
                    CreatedAt = DateTime.Now, // Fecha de creación del producto
                };

                // Se agrega el producto al contexto de la base de datos
                await _stockControlDbContext.AddAsync(product);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();

                // Se guarda el historial del producto (entrada al inventario)
                await SavedProductHistoryAsync(product.ProductId, productInputDto.Stock, "Entrada", "Producto registrado");

                // Se retorna 1 para indicar que el producto fue registrado correctamente
                return 1; // Éxito
            }
            catch
            {
                // Si ocurre cualquier error, se retorna -2 (error general)
                return -2; // Error general
            }
        }

        // Método asincrónico para guardar una nueva categoría
        public async Task<Category> SavedCategoryAsync(CategoryInput_DTO categoryInput_DTO)
        {
            try
            {
                // Verifica si ya existe una categoría con el mismo nombre
                var existingCategory = await _stockControlDbContext.Categories
                    .FirstOrDefaultAsync(c => c.Name == categoryInput_DTO.Name_Category);

                // Si la categoría ya existe, la retorna
                if (existingCategory != null)
                    return existingCategory;

                // Si la categoría no existe, crea una nueva categoría
                var category = new Category
                {
                    Name = categoryInput_DTO.Name_Category, // Asigna el nombre de la categoría
                    Description = categoryInput_DTO.Description, // Asigna la descripción de la categoría
                    UpdatedAt = DateTime.Now // Asigna la fecha de actualización actual
                };

                // Se agrega la nueva categoría al contexto de la base de datos de manera asincrónica
                await _stockControlDbContext.AddAsync(category);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();

                // Se retorna la categoría recién creada
                return category;
            }
            catch (Exception ex)
            {
                // Si ocurre un error, lanza una excepción personalizada con el mensaje del error
                throw new ApplicationException($"Error al crear la categoría: {ex.Message}", ex);
            }
        }

        // Método asincrónico para actualizar un producto
        public async Task<int> UpdateProductAsync(int productId, ProductInput_DTO productDto)
        {
            try
            {
                // Se busca el producto en la base de datos por su ID
                var product = await _stockControlDbContext.Products.FindAsync(productId);

                // Si el producto no se encuentra, retorna 0
                if (product == null)
                {
                    return 0; // Producto no encontrado
                }

                // Si el producto existe, actualiza sus campos con los datos proporcionados
                product.Name = productDto.Name_Product; // Actualiza el nombre del producto
                product.Description = productDto.Description; // Actualiza la descripción del producto
                product.Stock = productDto.Stock; // Actualiza el stock del producto
                product.Price = productDto.Price; // Actualiza el precio del producto
                product.UpdatedAt = DateTime.Now; // Asigna la fecha de actualización

                // Se actualiza el producto en el contexto de la base de datos
                _stockControlDbContext.Products.Update(product);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();

                // Retorna 1 para indicar que la actualización fue exitosa
                return 1; // Éxito
            }
            catch
            {
                // Si ocurre cualquier error, retorna -2
                return -2; // Error general
            }
        }

        // Método asincrónico para eliminar un producto
        public async Task<int> DeleteProductAsync(int productId)
        {
            try
            {
                // Se busca el producto en la base de datos por su ID
                var product = await _stockControlDbContext.Products.FindAsync(productId);

                // Si el producto no se encuentra, retorna 0
                if (product == null)
                {
                    return 0; // Producto no encontrado
                }

                // Si el producto existe, lo elimina del contexto de la base de datos
                _stockControlDbContext.Products.Remove(product);

                // Se guardan los cambios en la base de datos
                await _stockControlDbContext.SaveChangesAsync();

                // Retorna 1 para indicar que la eliminación fue exitosa
                return 1; // Éxito
            }
            catch
            {
                // Si ocurre cualquier error, retorna -2
                return -2; // Error general
            }
        }

    }
}
