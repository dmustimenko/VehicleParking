using VehicleParking.Application.Interfaces.Repositories;
using VehicleParking.Application.Interfaces.Services;
using VehicleParking.Application.Models.Requests;
using VehicleParking.Application.Models.Responses;
using VehicleParking.Domain.Entities;
using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Exceptions;
using VehicleParking.Domain.Services.Interfaces;
using VehicleParking.Domain.Helpers;
using VehicleParking.Domain.Values;

namespace VehicleParking.Application.Services;

public class ParkingService(
    IParkingRepository parkingRepository,
    IParkingChargeService parkingChargeService,
    TimeProvider timeProvider
    ): IParkingService
{
    public async Task<ParkingResponse> ParkVehicleAsync(ParkingRequest parkingRequest)
    {
        if (!Enum.IsDefined(typeof(VehicleTypeEnum), parkingRequest.VehicleType))
        {
            throw new InvalidVehicleTypeException(parkingRequest.VehicleType);
        }
        
        string vehicleReg = VerifyAndNormalizeVehicleReg(parkingRequest.VehicleReg);

        ParkingSession? activeParkingSession = await parkingRepository.FindActiveParkingSessionAsync(vehicleReg);
        if (activeParkingSession is not null)
        {
            throw new VehicleAlreadyParkedException(vehicleReg);
        }

        ParkingSession? parkingSession = await parkingRepository.TryParkVehicleAsync(
            vehicleReg,
            (VehicleTypeEnum)parkingRequest.VehicleType,
            GetUtcNow()
        );

        if (parkingSession is null)
        {
            throw new ParkingFullException();
        }

        return new ParkingResponse(
            parkingSession.VehicleReg,
            parkingSession.SpaceNumber,
            parkingSession.TimeIn
        );
    }

    public async Task<ParkingExitResponse> ExitVehicleAsync(ParkingExitRequest parkingExitRequest)
    {
        string vehicleReg = VerifyAndNormalizeVehicleReg(parkingExitRequest.VehicleReg);

        ParkingSession? parkingSession = await parkingRepository.FindActiveParkingSessionAsync(vehicleReg);
        if (parkingSession is null)
        {
            throw new VehicleNotFoundException(vehicleReg);
        }

        DateTime timeOut = GetUtcNow();
        decimal charge = parkingChargeService.Calculate(
            parkingSession.VehicleType,
            parkingSession.TimeIn,
            timeOut
        );

        parkingSession.Complete(timeOut, charge);
        await parkingRepository.CompleteParkingSessionAsync(parkingSession);

        return new ParkingExitResponse(
            parkingSession.VehicleReg,
            (double)charge,
            parkingSession.TimeIn,
            timeOut
        );
    }

    public async Task<ParkingCapacityResponse> GetParkingCapacityAsync()
    {
        ParkingCapacity capacity = await parkingRepository.GetParkingCapacityAsync();

        return new ParkingCapacityResponse(
            capacity.Available,
            capacity.Occupied
        );
    }

    private static string VerifyAndNormalizeVehicleReg(string vehicleReg)
    {
        if (string.IsNullOrWhiteSpace(vehicleReg) || vehicleReg.Length > 20)
        {
            throw new InvalidVehicleRegException(vehicleReg);
        }

        return VehicleRegHelpers.Normalize(vehicleReg);
    }
    
    private DateTime GetUtcNow()
    {
        return timeProvider.GetUtcNow()
            .UtcDateTime;
    }
}
