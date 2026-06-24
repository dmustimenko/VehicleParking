using Scalar.AspNetCore;
using VehicleParking.Api.DependencyInjection;
using VehicleParking.Api.DependencyInjection.Migration;
using VehicleParking.Api.Endpoints;

WebApplicationBuilder builder = WebApplication.CreateBuilder(args);
IServiceCollection services = builder.Services;

builder.AddHostConfiguration();

services.AddOpenApi();

services.ConfigureHttpJsonOptions(options =>
    options.SerializerOptions.PropertyNamingPolicy = null
);

services.ConfigureChargeOptions(builder.Configuration);

services.AddErrorHandling();

services.AddDomainServices();
services.AddApplicationServices();
services.AddInfrastructureServices(builder.Configuration);

WebApplication app = builder.Build();

if (app.Environment.IsDevelopment())
{
    await app.ApplyMigrationsAsync();

    app.MapOpenApi();
    
    app.MapScalarApiReference();
    app.MapGet("/", () =>
        Results.Redirect("/scalar/v1")
    );
}

app.UseExceptionHandler();

app.MapParkingEndpoints();

await app.RunAsync();