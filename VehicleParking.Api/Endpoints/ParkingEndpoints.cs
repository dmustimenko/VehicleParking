using VehicleParking.Application.Interfaces.Services;
using VehicleParking.Application.Models.Requests;
using VehicleParking.Application.Models.Responses;

namespace VehicleParking.Api.Endpoints;

public static class ParkingEndpoints
{
    public static IEndpointRouteBuilder MapParkingEndpoints(this IEndpointRouteBuilder app)
    {
        RouteGroupBuilder group = app.MapGroup("/parking")
            .WithTags("Parking");
        
        group.MapGet("/", HandleParkingCapacityAsync)
            .WithTags("Capacity");

        group.MapPost("/", HandleParkingAsync)
            .WithTags("Park");
        
        group.MapPost("/exit", HandleParkingExitAsync)
            .WithTags("Exit");
        
        return app;
    }

    private static async Task<ParkingCapacityResponse> HandleParkingCapacityAsync(
        IParkingService parkingService)
    {
        return await parkingService.GetParkingCapacityAsync();
    }
    
    private static async Task<ParkingResponse> HandleParkingAsync(
        ParkingRequest request,
        IParkingService parkingService)
    {
        return await parkingService.ParkVehicleAsync(request);
    }
    
    private static async Task<ParkingExitResponse> HandleParkingExitAsync(
        ParkingExitRequest request,
        IParkingService parkingService)
    {
        return await parkingService.ExitVehicleAsync(request);
    }
}