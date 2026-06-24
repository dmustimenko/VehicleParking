using VehicleParking.Domain.Enums;

namespace VehicleParking.Domain.Services.Interfaces;

public interface IParkingChargeService
{
    decimal Calculate(VehicleTypeEnum vehicleType, DateTime timeIn, DateTime timeOut);
}