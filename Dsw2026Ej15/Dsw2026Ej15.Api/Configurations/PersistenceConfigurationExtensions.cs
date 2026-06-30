using Dsw2026Ej15.Data;
using Dsw2026Ej15.Data.Services;
using Dsw2026Ej15.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Dsw2026Ej15.Api.Configurations;

public static class PersistenceConfigurationExtensions
{
    public static IServiceCollection AddAplicationPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<Dsw2026Ej15DbContext>(options =>
            options.UseSqlServer(connectionString));
        services.AddScoped<IPersistence, PersistenceEF>();
        return services;
    }
}
