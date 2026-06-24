namespace VehicleParking.Application.Models.Responses;

public sealed record ParkingExitResponse(
    string VehicleReg,
    double VehicleCharge,
    DateTime TimeIn,
    DateTime TimeOut
);