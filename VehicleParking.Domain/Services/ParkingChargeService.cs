using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Services.Interfaces;
using VehicleParking.Domain.Settings;

namespace VehicleParking.Domain.Services;

public class ParkingChargeService(ChargeSettings chargeSettings)
    : IParkingChargeService
{
    public decimal Calculate(VehicleTypeEnum vehicleType, DateTime timeIn, DateTime timeOut)
    {
        int sessionPeriodInMinutes = Math.Max(0, (int)Math.Ceiling((timeOut - timeIn).TotalMinutes));
        int additionalChargeCount = sessionPeriodInMinutes / chargeSettings.AdditionalChargeIntervalMinutes;
        decimal vehicleTypeRate = chargeSettings.ChargeRatePerMinute[vehicleType];
        
        return sessionPeriodInMinutes * vehicleTypeRate
            + additionalChargeCount * chargeSettings.AdditionalCharge;
    }
}