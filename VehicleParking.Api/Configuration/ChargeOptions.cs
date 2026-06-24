using VehicleParking.Domain.Enums;

namespace VehicleParking.Api.Configuration;

public class ChargeOptions
{
    public const string SectionName = "Charge";

    public int AdditionalChargeIntervalMinutes { get; init; }
    public decimal AdditionalCharge { get; init; }
    public Dictionary<VehicleTypeEnum, decimal> ChargeRatePerMinute { get; init; } = new();
}