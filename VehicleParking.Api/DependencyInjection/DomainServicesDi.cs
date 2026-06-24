using VehicleParking.Domain.Services;
using VehicleParking.Domain.Services.Interfaces;

namespace VehicleParking.Api.DependencyInjection;

public static class DomainServicesDi
{
    public static IServiceCollection AddDomainServices(this IServiceCollection services)
    {
        services.AddSingleton<IParkingChargeService, ParkingChargeService>();

        return services;
    }
}