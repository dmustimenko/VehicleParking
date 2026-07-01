namespace VehicleParking.Domain.Values;

public sealed record ParkingCapacity(
    int Available,
    int Occupied
);
