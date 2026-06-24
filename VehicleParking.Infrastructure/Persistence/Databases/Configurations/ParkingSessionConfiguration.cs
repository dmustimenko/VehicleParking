using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleParking.Domain.Entities;
using VehicleParking.Infrastructure.Persistence.Databases.Configurations.Extensions;

namespace VehicleParking.Infrastructure.Persistence.Databases.Configurations;

public sealed class ParkingSessionConfiguration : IEntityTypeConfiguration<ParkingSession>
{
    private const string ActiveSpaceIndex = "ix_parking_sessions_active_space";
    public const string ActiveVehicleRegIndex = "ix_parking_sessions_active_vehicle_reg";

    public void Configure(EntityTypeBuilder<ParkingSession> builder)
    {
        builder.ToTable("parking_sessions");

        builder.HasKey(session => session.Id);
        builder.Property(session => session.Id)
            .ValueGeneratedOnAdd();

        builder.Property(session => session.VehicleReg)
            .IsRequired()
            .HasMaxLength(20);
        
        builder.Property(session => session.VehicleType)
            .IsRequired();
        
        builder.Property(session => session.SpaceNumber)
            .IsRequired();
        
        builder.Property(session => session.TimeIn)
            .IsRequired();
        
        builder.Property(session => session.Charge)
            .HasPrecision(10, 2);

        builder.HasOne<ParkingSpace>()
            .WithMany()
            .HasForeignKey(session => session.SpaceNumber)
            .HasPrincipalKey(space => space.Number);

        builder.HasIndex(session => session.SpaceNumber)
            .IsUnique()
            .HasDatabaseName(ActiveSpaceIndex)
            .HasEmptyTimeOutFilter();

        builder.HasIndex(session => session.VehicleReg)
            .IsUnique()
            .HasDatabaseName(ActiveVehicleRegIndex)
            .HasEmptyTimeOutFilter();
    }
}
