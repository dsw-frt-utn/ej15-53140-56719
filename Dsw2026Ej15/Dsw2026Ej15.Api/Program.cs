
using Dsw2026Ej15.Data.Services;
using Dsw2026Ej15.Domain.Interfaces;
using Dsw2026Ej15.Api.Middlewares;
using Dsw2026Ej15.Domain.Exceptions;
using Dsw2026Ej15.Data;
using Microsoft.EntityFrameworkCore;
using Dsw2026Ej15.Api.Configurations;
using Dsw2026Ej15.Data.Extensions;

namespace Dsw2026Ej15
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddAplicationPersistence(builder.Configuration);

            builder.Services.AddControllers();
            builder.Services.AddHealthChecks();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseMiddleware<ExceptionMiddlewares>();
            app.UseAuthorization();
            app.MapControllers();
            app.MapHealthChecks("/health-check");

            using var scope = app.Services.CreateScope();
            var services = scope.ServiceProvider;
            var context = services.GetRequiredService<Dsw2026Ej15DbContext>();
            context.SeedworkSpecialities(@"specialities.json");

            app.Run();
        }
    }
}
