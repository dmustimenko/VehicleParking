using Microsoft.EntityFrameworkCore;
using VehicleParking.Application.Interfaces.Repositories;
using VehicleParking.Api.Configuration.Constants;
using VehicleParking.Infrastructure.Persistence.Databases;
using VehicleParking.Infrastructure.Persistence.Repositories;

namespace VehicleParking.Api.DependencyInjection;

public static class InfrastructureServicesDi
{
    public static IServiceCollection AddInfrastructureServices(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<ParkingDbContext>(options => options.UseNpgsql(
            configuration.GetConnectionString(DatabaseConstants.PostgresConnectionStringName),
            db => db.EnableRetryOnFailure(5)
        ));

        services.AddScoped<IParkingRepository, ParkingRepository>();

        return services;
    }
}
