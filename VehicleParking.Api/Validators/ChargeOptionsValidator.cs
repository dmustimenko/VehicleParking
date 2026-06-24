using Microsoft.Extensions.Options;
using VehicleParking.Api.Configuration;
using VehicleParking.Domain.Enums;

namespace VehicleParking.Api.Validators;

public sealed class ChargeOptionsValidator
    : IValidateOptions<ChargeOptions>
{
    public ValidateOptionsResult Validate(string? name, ChargeOptions chargeOptions)
    {
        var errors = new List<string>();

        if (chargeOptions.AdditionalChargeIntervalMinutes <= 0)
        {
            errors.Add("Additional charge interval should be > 0.");
        }

        if (chargeOptions.AdditionalCharge < 0)
        {
            errors.Add("Additional charge should not be < 0.");
        }
        
        VehicleTypeEnum[] missingRates = Enum.GetValues<VehicleTypeEnum>()
            .Where(o => !chargeOptions.ChargeRatePerMinute.ContainsKey(o))
            .ToArray();
        
        if (missingRates.Length > 0)
        {
            errors.Add($"There are no rates for the following vehicle types: {string.Join(", ", missingRates)}");
        }

        VehicleTypeEnum[] nonPositiveRates = chargeOptions.ChargeRatePerMinute
            .Where(o => o.Value <= 0)
            .Select(o => o.Key)
            .ToArray();
        
        if (nonPositiveRates.Length > 0)
        {
            errors.Add($"Charge rate should be > 0 for the following vehicle types: {string.Join(", ", nonPositiveRates)}");
        }
        
        VehicleTypeEnum[] undefinedRates = chargeOptions.ChargeRatePerMinute.Keys
            .Where(o => !Enum.IsDefined(o))
            .ToArray();
        
        if (undefinedRates.Length > 0)
        {
            errors.Add($"Undefined charge rates: {string.Join(", ", undefinedRates)}");
        }

        return errors.Count > 0
            ? ValidateOptionsResult.Fail(errors)
            : ValidateOptionsResult.Success;
    }
}