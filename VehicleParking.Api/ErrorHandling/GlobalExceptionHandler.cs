using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using VehicleParking.Domain.Exceptions;

namespace VehicleParking.Api.ErrorHandling;

public sealed class GlobalExceptionHandler(
    IProblemDetailsService problemDetailsService,
    ILogger<GlobalExceptionHandler> logger
) : IExceptionHandler
{
    public async ValueTask<bool> TryHandleAsync(
        HttpContext httpContext,
        Exception exception,
        CancellationToken cancellationToken)
    {
        int statusCode = exception switch
        {
            VehicleNotFoundException => StatusCodes.Status404NotFound,
            VehicleAlreadyParkedException or ParkingFullException => StatusCodes.Status409Conflict,
            InvalidVehicleTypeException or InvalidVehicleRegException => StatusCodes.Status400BadRequest,
            _ => StatusCodes.Status500InternalServerError
        };

        if (IsServerErrorStatus(statusCode))
        {
            logger.LogError(
                exception,
                "Unhandled exception while processing {Method} {Path}",
                httpContext.Request.Method, httpContext.Request.Path
            );
        }

        httpContext.Response.StatusCode = statusCode;
        return await problemDetailsService.TryWriteAsync(new ProblemDetailsContext
        {
            HttpContext = httpContext,
            Exception = exception,
            ProblemDetails = new ProblemDetails
            {
                Status = statusCode,
                Title = IsServerErrorStatus(statusCode)
                    ? "An unexpected error occurred."
                    : exception.Message
            }
        });
    }

    private static bool IsServerErrorStatus(int statusCode)
    {
        return statusCode == StatusCodes.Status500InternalServerError;
    }
}
