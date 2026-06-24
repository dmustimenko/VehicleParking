namespace VehicleParking.Application.Models.Requests;

public sealed record ParkingRequest(
    string VehicleReg,
    int VehicleType
);