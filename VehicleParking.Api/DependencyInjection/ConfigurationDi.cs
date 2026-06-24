using VehicleParking.Api.Configuration;

namespace VehicleParking.Api.DependencyInjection;

public static class ConfigurationDi
{
    public static IHostApplicationBuilder AddHostConfiguration(this IHostApplicationBuilder builder)
    {
        builder.Configuration.AddAppConfiguration(builder.Environment.EnvironmentName);

        return builder;
    }
}
