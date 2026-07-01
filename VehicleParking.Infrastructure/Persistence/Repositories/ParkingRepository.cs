using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using Npgsql;
using VehicleParking.Application.Interfaces.Repositories;
using VehicleParking.Domain.Entities;
using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Exceptions;
using VehicleParking.Domain.Values;
using VehicleParking.Infrastructure.Constants;
using VehicleParking.Infrastructure.Persistence.Databases;
using VehicleParking.Infrastructure.Persistence.Databases.Configurations;

namespace VehicleParking.Infrastructure.Persistence.Repositories;

public class ParkingRepository(ParkingDbContext dbContext)
    : IParkingRepository
{
    public async Task<ParkingSession?> FindActiveParkingSessionAsync(string vehicleReg)
    {
        return await dbContext.ParkingSessions.FirstOrDefaultAsync(session =>
            session.VehicleReg == vehicleReg
            && session.TimeOut == null
        );
    }

    public async Task<ParkingCapacity> GetParkingCapacityAsync()
    {
        int totalParkingSpaces = await dbContext.ParkingSpaces.CountAsync();
        int occupiedParkingSpaces = await dbContext.ParkingSessions.CountAsync(session =>
            session.TimeOut == null
        );

        return new ParkingCapacity(
            totalParkingSpaces - occupiedParkingSpaces,
            occupiedParkingSpaces
        );
    }

    public async Task<ParkingSession?> TryParkVehicleAsync(string vehicleReg, VehicleTypeEnum vehicleType, DateTime timeIn)
    {
        IExecutionStrategy strategy = dbContext.Database.CreateExecutionStrategy();

        return await strategy.ExecuteAsync(async () =>
        {
            await using IDbContextTransaction transaction = await dbContext.Database.BeginTransactionAsync();

            int? parkingSpaceNumber = await dbContext.Database
                .SqlQueryRaw<int?>(DbScripts.AllocateParkingSpaceSql)
                .FirstOrDefaultAsync();

            if (parkingSpaceNumber is null)
            {
                return null;
            }

            ParkingSession parkingSession = new(
                vehicleReg,
                vehicleType,
                parkingSpaceNumber.Value,
                timeIn
            );
            
            dbContext.ParkingSessions.Add(parkingSession);

            try
            {
                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException e) when (e.InnerException is PostgresException
            {
                SqlState: PostgresErrorCodes.UniqueViolation,
                ConstraintName: ParkingSessionConfiguration.ActiveVehicleRegIndex
            })
            {
                throw new VehicleAlreadyParkedException(vehicleReg);
            }

            await transaction.CommitAsync();

            return parkingSession;
        });
    }

    public async Task CompleteParkingSessionAsync(ParkingSession session)
    {
        int updatedSessions = await dbContext.ParkingSessions
            .Where(o => o.Id == session.Id && o.TimeOut == null)
            .ExecuteUpdateAsync(o => o
                .SetProperty(x => x.TimeOut, session.TimeOut)
                .SetProperty(x => x.Charge, session.Charge)
            );

        if (updatedSessions == 0)
        {
            throw new VehicleNotFoundException(session.VehicleReg);
        }
    }
}
