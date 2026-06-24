using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleParking.Domain.Entities;
using VehicleParking.Infrastructure.Constants;

namespace VehicleParking.Infrastructure.Persistence.Databases.Configurations;

public sealed class ParkingSpaceConfiguration : IEntityTypeConfiguration<ParkingSpace>
{
    public void Configure(EntityTypeBuilder<ParkingSpace> builder)
    {
        builder.ToTable("parking_spaces");

        builder.HasKey(space => space.Number);
        
        builder.Property(space => space.Number)
            .ValueGeneratedNever();

        builder.HasData(Enumerable
            .Range(1, DatabaseConstants.InitialTotalParkingSpaces)
            .Select(number => new ParkingSpace
            {
                Number = number
            })
        );
    }
}
