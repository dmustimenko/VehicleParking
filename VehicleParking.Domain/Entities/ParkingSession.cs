using VehicleParking.Domain.Enums;

namespace VehicleParking.Domain.Entities;

public sealed record ParkingSession(
    string VehicleReg,
    VehicleTypeEnum VehicleType,
    int SpaceNumber,
    DateTime TimeIn)
{
    public long Id { get; init; }

    public DateTime? TimeOut { get; private set; }

    public decimal? Charge { get; private set; }

    public void Complete(DateTime timeOut, decimal charge)
    {
        TimeOut = timeOut;
        Charge = charge;
    }
}
