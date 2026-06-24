namespace VehicleParking.Application.Models.Responses;

public sealed record ParkingResponse(
    string VehicleReg,
    int SpaceNumber,
    DateTime TimeIn
);