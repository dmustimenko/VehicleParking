using VehicleParking.Domain.Enums;

namespace VehicleParking.Domain.Entities;

public sealed class ParkingSession(
    string vehicleReg,
    VehicleTypeEnum vehicleType,
    int spaceNumber,
    DateTime timeIn)
{
    public long Id { get; init; }
    public string VehicleReg { get; init; } = vehicleReg;
    public VehicleTypeEnum VehicleType { get; init; } = vehicleType;
    public int SpaceNumber { get; init; } = spaceNumber;
    public DateTime TimeIn { get; init; } = timeIn;

    public DateTime? TimeOut { get; private set; }

    public decimal? Charge { get; private set; }

    public void Complete(DateTime timeOut, decimal charge)
    {
        TimeOut = timeOut;
        Charge = charge;
    }
}
