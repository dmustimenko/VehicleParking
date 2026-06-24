namespace VehicleParking.Domain.Exceptions;

public sealed class VehicleAlreadyParkedException(string vehicleReg)
    : Exception($"Vehicle '{vehicleReg}' is already parked.");
