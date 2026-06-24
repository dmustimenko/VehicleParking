namespace VehicleParking.Domain.Entities;

public sealed record ParkingCapacity(
    int Available,
    int Occupied
);