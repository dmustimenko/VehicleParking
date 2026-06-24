using VehicleParking.Api.ErrorHandling;

namespace VehicleParking.Api.DependencyInjection;

public static class ErrorHandlingDi
{
    public static IServiceCollection AddErrorHandling(this IServiceCollection services)
    {
        services.AddProblemDetails();
        services.AddExceptionHandler<GlobalExceptionHandler>();

        return services;
    }
}
