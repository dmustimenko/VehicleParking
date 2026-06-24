namespace VehicleParking.Application.Models.Responses;

public sealed record ParkingCapacityResponse(
    int AvailableSpaces,
    int OccupiedSpaces
);