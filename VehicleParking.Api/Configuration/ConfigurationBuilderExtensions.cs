using VehicleParking.Api.Configuration.Constants;

namespace VehicleParking.Api.Configuration;

public static class ConfigurationBuilderExtensions
{
    public static IConfigurationBuilder AddAppConfiguration(this IConfigurationBuilder builder, string environmentName)
    {
        return builder
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile($"{ConfigurationConstants.AppSettingsFileName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddJsonFile($"{ConfigurationConstants.AppSettingsFileName}.{environmentName}.json",
                optional: true,
                reloadOnChange: true
            )
            .AddEnvironmentVariables();
    }
}
