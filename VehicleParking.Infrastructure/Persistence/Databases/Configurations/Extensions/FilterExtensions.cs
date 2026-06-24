using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using VehicleParking.Domain.Entities;

namespace VehicleParking.Infrastructure.Persistence.Databases.Configurations.Extensions;

public static class FilterExtensions
{
    public static IndexBuilder<TEntity> HasEmptyTimeOutFilter<TEntity>(this IndexBuilder<TEntity> indexBuilder)
    {
        return indexBuilder
            .HasFilter($"\"{nameof(ParkingSession.TimeOut)}\" IS NULL");
    }
}