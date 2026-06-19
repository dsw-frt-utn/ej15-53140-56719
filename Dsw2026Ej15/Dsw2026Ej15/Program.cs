
using Dsw2026Ej15.Data.Services;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Domain.Exceptions;

namespace Dsw2026Ej15
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen();
            builder.Services.AddSingleton<IPersistence, PersistenceInMemory>();

            var app = builder.Build();
            app.UseMiddleware<ExceptionMiddlewares>();
            

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health-check");

            app.Run();


        }
    }
}
