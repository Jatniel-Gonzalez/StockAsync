using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using StockDashboard.Services.Interfaces; // Se importan las interfaces de servicios.
using StockDashboard.Services; // Se importan las implementaciones de los servicios.
using CurrieTechnologies.Razor.SweetAlert2; // Se importa la librería SweetAlert2 para alertas personalizadas.

namespace StockDashboard
{
    public class Program
    {
        // Método principal que arranca la aplicación Blazor WebAssembly.
        public static async Task Main(string[] args)
        {
            // Crear el host de la aplicación Blazor.
            var builder = WebAssemblyHostBuilder.CreateDefault(args);

            // Agregar el componente raíz de la aplicación y asignar el elemento HTML con el id "app".
            builder.RootComponents.Add<App>("#app");

            // Agregar el componente HeadOutlet para manejar el contenido del <head> de manera dinámica.
            builder.RootComponents.Add<HeadOutlet>("head::after");

            // Registrar un servicio HttpClient con la URL base de la API.
            builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("http://localhost:5109") });

            // Agregar el servicio SweetAlert2 para mostrar alertas.
            builder.Services.AddSweetAlert2();

            // Registrar las implementaciones de los servicios de productos con sus interfaces respectivas.
            builder.Services.AddScoped<IProductSales, ProductSale>(); // Servicio de ventas de productos.
            builder.Services.AddScoped<IProductInput, ProductInput>(); // Servicio de entrada de productos.
            builder.Services.AddScoped<IProductCategory, ProductCategory>(); // Servicio de categorías de productos.

            // Construir y ejecutar la aplicación Blazor.
            await builder.Build().RunAsync();
        }
    }
}
