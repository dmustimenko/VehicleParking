using Microsoft.Extensions.Options;
using VehicleParking.Api.Configuration;
using VehicleParking.Api.Validators;
using VehicleParking.Domain.Settings;

namespace VehicleParking.Api.DependencyInjection;

public static class ChargeOptionsDi
{
    public static IServiceCollection ConfigureChargeOptions(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSingleton<IValidateOptions<ChargeOptions>, ChargeOptionsValidator>();

        services.AddOptions<ChargeOptions>()
            .Bind(configuration.GetSection(ChargeOptions.SectionName))
            .ValidateOnStart();

        services.AddSingleton<ChargeSettings>(serviceProvider =>
        {
            ChargeOptions chargeOptions = serviceProvider
                .GetRequiredService<IOptions<ChargeOptions>>()
                .Value;

            return new ChargeSettings(
                chargeOptions.AdditionalChargeIntervalMinutes,
                chargeOptions.AdditionalCharge,
                chargeOptions.ChargeRatePerMinute
            );
        });

        return services;
    }
}
