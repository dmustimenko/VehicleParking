using VehicleParking.Domain.Entities;
using VehicleParking.Domain.Enums;

namespace VehicleParking.Application.Interfaces.Repositories;

public interface IParkingRepository
{
    Task<ParkingSession?> FindActiveParkingSessionAsync(string vehicleReg);
    Task<ParkingCapacity> GetParkingCapacityAsync();

    Task<ParkingSession?> TryParkVehicleAsync(string vehicleReg, VehicleTypeEnum vehicleType, DateTime timeIn);
    Task CompleteParkingSessionAsync(ParkingSession session);
}
