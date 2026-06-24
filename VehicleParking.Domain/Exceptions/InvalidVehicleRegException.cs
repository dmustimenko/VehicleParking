namespace VehicleParking.Domain.Exceptions;

public sealed class InvalidVehicleRegException(string? vehicleReg)
    : Exception($"Invalid vehicle reg '{vehicleReg}' was provided.");
