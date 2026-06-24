namespace VehicleParking.Domain.Exceptions;

public sealed class ParkingFullException()
    : Exception("No parking spaces are currently available.");
