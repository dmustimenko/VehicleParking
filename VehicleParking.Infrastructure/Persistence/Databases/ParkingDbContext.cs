using Microsoft.EntityFrameworkCore;
using VehicleParking.Domain.Entities;

namespace VehicleParking.Infrastructure.Persistence.Databases;

public class ParkingDbContext(DbContextOptions<ParkingDbContext> options)
    : DbContext(options)
{
    public DbSet<ParkingSession> ParkingSessions => Set<ParkingSession>();
    public DbSet<ParkingSpace> ParkingSpaces => Set<ParkingSpace>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(
            typeof(ParkingDbContext).Assembly
        );
    }
}
