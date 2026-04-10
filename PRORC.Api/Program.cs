using PRORC.Application.DependencyInjection;
using PRORC.Infrastructure.DependencyInjection;
using PRORC.Persistence.DependencyInjection;
using System.Text.Json;

namespace PRORC.Api
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();
            builder.Services.AddApplication();
            builder.Services.AddInfrastructure(builder.Configuration);
            builder.Services.AddPersistence(builder.Configuration);
            builder.Services.AddCors(options =>
            {
                options.AddPolicy("AllowAll", policy =>
                {
                    policy.AllowAnyOrigin()
                          .AllowAnyHeader()
                          .AllowAnyMethod();
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            // Middleware global de manejo de errores (Evita repetir try-catch en los Controllers)
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (Exception ex)
                {
                    // Obtenemos un logger desde el contenedor de servicios
                    var logger = context.RequestServices.GetRequiredService<ILogger<Program>>();

                    // Registramos el error
                    logger.LogError(ex, "Unhandled exception in API. Path: {Path}", context.Request.Path);

                    // Definimos el status code según el tipo de error
                    context.Response.ContentType = "application/json";
                    context.Response.StatusCode = ex switch
                    {
                        KeyNotFoundException => StatusCodes.Status404NotFound,
                        ArgumentException => StatusCodes.Status400BadRequest,
                        InvalidOperationException => StatusCodes.Status400BadRequest,
                        _ => StatusCodes.Status500InternalServerError
                    };

                    var response = new
                    {
                        statusCode = context.Response.StatusCode,
                        message = ex.Message,
                        path = context.Request.Path.Value
                    };

                    var json = JsonSerializer.Serialize(response);

                    await context.Response.WriteAsync(json);
                }
            });


            app.UseHttpsRedirection();
            app.UseCors("AllowAll");
            app.MapControllers();
            app.Run();
        }
    }
}