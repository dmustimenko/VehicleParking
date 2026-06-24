namespace VehicleParking.Domain.Exceptions;

public sealed class VehicleNotFoundException(string vehicleReg)
    : Exception($"No active parking session was found for vehicle '{vehicleReg}'.");
