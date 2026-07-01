using VehicleParking.Application.Interfaces.Repositories;
using VehicleParking.Domain.Entities;
using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Values;

namespace VehicleParking.Tests.Repositories;

public sealed class FakeParkingRepository(int totalSpaces)
    : IParkingRepository
{
    private readonly List<ParkingSession> _sessions = [];

    public Task<ParkingSession?> FindActiveParkingSessionAsync(string vehicleReg)
    {
        return Task.FromResult(_sessions.FirstOrDefault(session =>
            session.TimeOut is null
            && session.VehicleReg == vehicleReg
        ));
    }

    public Task<ParkingCapacity> GetParkingCapacityAsync()
    {
        int occupied = _sessions.Count(session =>
            session.TimeOut is null
        );
        
        return Task.FromResult(new ParkingCapacity(
            totalSpaces - occupied,
            occupied
        ));
    }

    public Task<ParkingSession?> TryParkVehicleAsync(string vehicleReg, VehicleTypeEnum vehicleType, DateTime timeIn)
    {
        HashSet<int> occupied = _sessions
            .Where(session => session.TimeOut is null)
            .Select(session => session.SpaceNumber)
            .ToHashSet();

        for (var number = 1; number <= totalSpaces; number++)
        {
            if (occupied.Contains(number))
            {
                continue;
            }
            
            ParkingSession session = new(
                vehicleReg,
                vehicleType,
                number,
                timeIn
            );
            
            _sessions.Add(session);
            
            return Task.FromResult<ParkingSession?>(session);
        }

        return Task.FromResult<ParkingSession?>(null);
    }

    public Task CompleteParkingSessionAsync(ParkingSession session)
        => Task.CompletedTask;
}
