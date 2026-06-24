using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Settings;

namespace VehicleParking.Tests.Data;

public static class ChargeSettingsFactory
{
    public static ChargeSettings Default() => new(
        AdditionalChargeIntervalMinutes: 5,
        AdditionalCharge: 1.00m,
        ChargeRatePerMinute: new Dictionary<VehicleTypeEnum, decimal>
        {
            [VehicleTypeEnum.Small] = 0.10m,
            [VehicleTypeEnum.Medium] = 0.20m,
            [VehicleTypeEnum.Large] = 0.40m
        }
    );
}
