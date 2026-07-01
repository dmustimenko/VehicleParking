using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Services;
using VehicleParking.Tests.Data;

namespace VehicleParking.Tests;

public class ParkingChargeServiceTests
{
    private static readonly DateTime TimeIn = new(2026, 6, 25, 12, 0, 0, DateTimeKind.Utc);
    
    private readonly ParkingChargeService _parkingChargeServiceWithDefaultChargeSettings
        = new(ChargeSettingsFactory.Default());

    [Theory]
    [InlineData(VehicleTypeEnum.Small, 4, 0.40)]
    [InlineData(VehicleTypeEnum.Small, 5, 1.50)]
    [InlineData(VehicleTypeEnum.Small, 9, 1.90)]
    [InlineData(VehicleTypeEnum.Medium, 10, 4.00)]
    [InlineData(VehicleTypeEnum.Large, 7, 3.80)]
    public void CalculateCharge_PerVehicleTypeMinuteRate_DefaultChargeSettings(
        VehicleTypeEnum vehicleType,
        int minutes,
        double expected)
    {
        DateTime timeIn = TimeIn;
        DateTime timeOut = timeIn.AddMinutes(minutes);

        decimal charge = _parkingChargeServiceWithDefaultChargeSettings.Calculate(
            vehicleType,
            timeIn,
            timeOut
        );

        Assert.Equal((decimal)expected, charge);
    }
    
    [Fact]
    public void CalculateCharge_TimeOutLessTimeIn_DefaultChargeSettings()
    {
        DateTime timeIn = TimeIn;
        DateTime timeOut = timeIn.AddMinutes(-10);

        decimal charge = _parkingChargeServiceWithDefaultChargeSettings.Calculate(VehicleTypeEnum.Small, timeIn, timeOut);

        Assert.Equal(0m, charge);
    }

    [Fact]
    public void CalculateCharge_CeilingPartialMinutes_DefaultChargeSettings()
    {
        DateTime timeIn = TimeIn;
        
        // 210 seconds = 3.5 minutes - charged as 4 minutes
        DateTime timeOut = timeIn.AddSeconds(210);

        decimal charge = _parkingChargeServiceWithDefaultChargeSettings.Calculate(VehicleTypeEnum.Small, timeIn, timeOut);

        Assert.Equal(0.40m, charge);
    }

    [Fact]
    public void Calculate_ZeroDuration_ReturnsZero_DefaultChargeSettings()
    {
        DateTime time = TimeIn;

        decimal charge = _parkingChargeServiceWithDefaultChargeSettings.Calculate(VehicleTypeEnum.Large, time, time);

        Assert.Equal(0m, charge);
    }
}
