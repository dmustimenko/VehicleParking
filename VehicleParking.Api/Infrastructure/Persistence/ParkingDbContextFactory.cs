using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using VehicleParking.Api.Configuration;
using VehicleParking.Api.Configuration.Constants;
using VehicleParking.Infrastructure.Persistence.Databases;

namespace VehicleParking.Api.Infrastructure.Persistence;

public sealed class ParkingDbContextFactory
    : IDesignTimeDbContextFactory<ParkingDbContext>
{
    public ParkingDbContext CreateDbContext(string[] args)
    {
        string environmentName = Environment.GetEnvironmentVariable(ConfigurationConstants.EnvironmentKey)
            ?? Environments.Production;

        IConfiguration configuration = new ConfigurationBuilder()
            .AddAppConfiguration(environmentName)
            .Build();

        DbContextOptions<ParkingDbContext> options = new DbContextOptionsBuilder<ParkingDbContext>()
            .UseNpgsql(configuration.GetConnectionString(DatabaseConstants.PostgresConnectionStringName))
            .Options;

        return new ParkingDbContext(options);
    }
}
