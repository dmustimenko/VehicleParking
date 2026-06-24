using VehicleParking.Domain.Enums;

namespace VehicleParking.Domain.Settings;

public sealed record ChargeSettings(
    int AdditionalChargeIntervalMinutes,
    decimal AdditionalCharge,
    IReadOnlyDictionary<VehicleTypeEnum, decimal> ChargeRatePerMinute
);