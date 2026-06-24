namespace VehicleParking.Domain.Exceptions;

public sealed class InvalidVehicleTypeException(int vehicleType)
    : Exception($"Invalid vehicle type '{vehicleType}' was provided.");