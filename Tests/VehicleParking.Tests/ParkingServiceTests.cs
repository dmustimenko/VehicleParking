using Microsoft.Extensions.Time.Testing;
using VehicleParking.Application.Models.Requests;
using VehicleParking.Application.Models.Responses;
using VehicleParking.Application.Services;
using VehicleParking.Domain.Enums;
using VehicleParking.Domain.Exceptions;
using VehicleParking.Domain.Services;
using VehicleParking.Tests.Data;
using VehicleParking.Tests.Repositories;

namespace VehicleParking.Tests;

public class ParkingServiceTests
{
    private static readonly DateTimeOffset Now = new(2026, 6, 25, 12, 0, 0, TimeSpan.Zero);

    [Fact]
    public async Task ParkVehicle_AllocateFirstAvailableSpace()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 3),
            new FakeTimeProvider(Now)
        );

        ParkingResponse response = await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "DW4WX41",
            (int)VehicleTypeEnum.Small
        ));

        Assert.Equal("DW4WX41", response.VehicleReg);
        Assert.Equal(1, response.SpaceNumber);
        Assert.Equal(Now.UtcDateTime, response.TimeIn);
    }

    [Fact]
    public async Task ParkVehicle_NormalizesRegistration()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 3),
            new FakeTimeProvider(Now)
        );

        ParkingResponse response = await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "  dw4wX41  ",
            (int)VehicleTypeEnum.Small
        ));

        Assert.Equal("DW4WX41", response.VehicleReg);
    }

    [Fact]
    public async Task ParkVehicle_AlreadyParked_Throws()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 3),
            new FakeTimeProvider(Now)
        );
        
        await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "DW4WX41",
            (int)VehicleTypeEnum.Small
        ));

        await Assert.ThrowsAsync<VehicleAlreadyParkedException>(() =>
            parkingService.ParkVehicleAsync(new ParkingRequest(
                VehicleReg: "dw4wX41",
                (int)VehicleTypeEnum.Medium
            ))
        );
    }

    [Fact]
    public async Task ParkVehicle_FullParking_Throws()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 1),
            new FakeTimeProvider(Now)
        );
        
        await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "DW4WX41",
            (int)VehicleTypeEnum.Small
        ));

        await Assert.ThrowsAsync<ParkingFullException>(() =>
            parkingService.ParkVehicleAsync(new ParkingRequest(
                VehicleReg: "PK4AB42",
                (int)VehicleTypeEnum.Small
            ))
        );
    }

    [Fact]
    public async Task ExitVehicle_NotParked_Throws()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 3),
            new FakeTimeProvider(Now)
        );

        await Assert.ThrowsAsync<VehicleNotFoundException>(() =>
            parkingService.ExitVehicleAsync(new ParkingExitRequest(
                VehicleReg: "DW4WX41"
            ))
        );
    }

    [Fact]
    public async Task ExitVehicle_ComputeCharge()
    {
        var timeProvider = new FakeTimeProvider(Now);
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 3),
            timeProvider
        );
        
        await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "DW4WX41",
            (int)VehicleTypeEnum.Medium
        ));

        timeProvider.Advance(TimeSpan.FromMinutes(10));
        
        ParkingExitResponse response = await parkingService.ExitVehicleAsync(new ParkingExitRequest(
            "DW4WX41"
        ));

        Assert.Equal(4.00, response.VehicleCharge);
        Assert.Equal("DW4WX41", response.VehicleReg);
        Assert.Equal(Now.UtcDateTime, response.TimeIn);
        Assert.Equal(Now.UtcDateTime.AddMinutes(10), response.TimeOut);
    }

    [Fact]
    public async Task GetParkingCapacity_VerifyCapacity()
    {
        ParkingService parkingService = CreateService(
            new FakeParkingRepository(totalSpaces: 5),
            new FakeTimeProvider(Now)
        );
        
        await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "DW4WX41",
            (int)VehicleTypeEnum.Small
        ));
        
        await parkingService.ParkVehicleAsync(new ParkingRequest(
            VehicleReg: "PK4WA42",
            (int)VehicleTypeEnum.Large
        ));

        ParkingCapacityResponse capacity = await parkingService.GetParkingCapacityAsync();

        Assert.Equal(3, capacity.AvailableSpaces);
        Assert.Equal(2, capacity.OccupiedSpaces);
    }
    
    private static ParkingService CreateService(FakeParkingRepository repository, FakeTimeProvider timeProvider)
    {
        return new ParkingService(
            repository,
            new ParkingChargeService(ChargeSettingsFactory.Default()),
            timeProvider
        );
    }
}
