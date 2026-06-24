using VehicleParking.Application.Models.Requests;
using VehicleParking.Application.Models.Responses;

namespace VehicleParking.Application.Interfaces.Services;

public interface IParkingService
{
    Task<ParkingResponse> ParkVehicleAsync(ParkingRequest parkingRequest);
    Task<ParkingExitResponse> ExitVehicleAsync(ParkingExitRequest parkingExitRequest);
    Task<ParkingCapacityResponse> GetParkingCapacityAsync();
}