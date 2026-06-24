using Microsoft.EntityFrameworkCore.Migrations;
using VehicleParking.Infrastructure.Constants;

#nullable disable

namespace VehicleParking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddAllocateParkingSpaceFunction : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DbScripts.AllocateParkingSpaceFuncCreate);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(DbScripts.AllocateParkingSpaceFuncDrop);
        }
    }
}
