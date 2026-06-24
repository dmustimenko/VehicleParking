using VehicleParking.Application.Interfaces.Services;
using VehicleParking.Application.Services;

namespace VehicleParking.Api.DependencyInjection;

public static class ApplicationServicesDi
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services)
    {
        services.AddSingleton(TimeProvider.System);

        services.AddScoped<IParkingService, ParkingService>();

        return services;
    }
}