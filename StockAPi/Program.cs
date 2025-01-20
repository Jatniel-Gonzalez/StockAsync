using Microsoft.EntityFrameworkCore;
using StockAPi.Models;
using StockAPi.Services;

namespace StockAPi
{
    public class Program
    {
        public static void Main(string[] args)
        {
            // Creaci�n de la aplicaci�n web
            var builder = WebApplication.CreateBuilder(args);

            // Configuraci�n de servicios en el contenedor DI (Inyecci�n de Dependencias)
            builder.Services.AddControllers();  // A�ade soporte para controladores de API

            // Configuraci�n de Swagger para documentar y probar la API de forma interactiva
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();  // Genera la documentaci�n Swagger autom�ticamente

            // Configuraci�n de la base de datos con Entity Framework
            builder.Services.AddDbContext<StockControlDbContext>(options =>
            {
                // Conexi�n a la base de datos SQL Server utilizando la cadena de conexi�n desde el archivo de configuraci�n
                options.UseSqlServer(builder.Configuration.GetConnectionString("connectionSQl"));
            });

            // Inyecci�n de dependencias para el servicio de productos (ProductService)
            builder.Services.AddScoped<ProductService>();  // Scoped significa que se crea una nueva instancia por cada solicitud HTTP

            // Configuraci�n de CORS (Cross-Origin Resource Sharing)
            builder.Services.AddCors(opciones => {
                opciones.AddPolicy("politicasCors", app =>
                {
                    // Permite todas las or�genes, encabezados y m�todos (�til durante el desarrollo, pero se debe restringir para producci�n)
                    app.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
                });
            });

            // Construcci�n y configuraci�n de la aplicaci�n web
            var app = builder.Build();

            // Configuraci�n del pipeline de solicitudes HTTP

            app.UseCors("politicasCors");  // Habilita CORS usando la pol�tica configurada
            app.UseAuthorization();  // Habilita la autorizaci�n (aunque no se usa expl�citamente aqu�, puedes agregarla m�s tarde)
            app.MapControllers();  // Mapea las rutas de los controladores de API

            // Ejecuta la aplicaci�n
            app.Run();  // Inicia el servidor web
        }
    }
}
