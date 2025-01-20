using Microsoft.EntityFrameworkCore;
using StockAPi.Models;
using StockAPi.Services;

namespace StockAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Creación de la aplicación web
            var builder = WebApplication.CreateBuilder(args);

            // Configuración de servicios en el contenedor DI (Inyección de Dependencias)
            builder.Services.AddControllers();  // Añade soporte para controladores de API

            // Configuración de Swagger para documentar y probar la API de forma interactiva
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();  // Genera la documentación Swagger automáticamente

            // Configuración de la base de datos con Entity Framework
            builder.Services.AddDbContext<StockControlDbContext>(options =>
            {
                // Conexión a la base de datos SQL Server utilizando la cadena de conexión desde el archivo de configuración
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionSQl"));
            });

            // Inyección de dependencias para el servicio de productos (ProductService)
            builder.Services.AddScoped<ProductService>();  // Scoped significa que se crea una nueva instancia por cada solicitud HTTP

            // Configuración de CORS (Cross-Origin Resource Sharing)
            builder.Services.AddCors(opciones => {
                opciones.AddPolicy("politicasCors", app =>
                {
                    // Permite todas las orígenes, encabezados y métodos (útil durante el desarrollo, pero se debe restringir para producción)
                    app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Construcción y configuración de la aplicación web
            var app = builder.Build();

            // Configuración del pipeline de solicitudes HTTP

            app.UseCors("politicasCors");  // Habilita CORS usando la política configurada
            app.UseAuthorization();  // Habilita la autorización (aunque no se usa explícitamente aquí, puedes agregarla más tarde)
            app.MapControllers();  // Mapea las rutas de los controladores de API

            // Ejecuta la aplicación
            app.Run();  // Inicia el servidor web
        }
    }
}
