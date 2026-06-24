using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace VehicleParking.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Initial : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "parking_spaces",
                columns: table => new
                {
                    Number = table.Column<int>(type: "integer", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parking_spaces", x => x.Number);
                });

            migrationBuilder.CreateTable(
                name: "parking_sessions",
                columns: table => new
                {
                    Id = table.Column<long>(type: "bigint", nullable: false)
                        .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
                    VehicleReg = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    VehicleType = table.Column<int>(type: "integer", nullable: false),
                    SpaceNumber = table.Column<int>(type: "integer", nullable: false),
                    TimeIn = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    TimeOut = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    Charge = table.Column<decimal>(type: "numeric(10,2)", precision: 10, scale: 2, nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_parking_sessions", x => x.Id);
                    table.ForeignKey(
                        name: "FK_parking_sessions_parking_spaces_SpaceNumber",
                        column: x => x.SpaceNumber,
                        principalTable: "parking_spaces",
                        principalColumn: "Number",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.InsertData(
                table: "parking_spaces",
                column: "Number",
                values: new object[]
                {
                    1,
                    2,
                    3,
                    4,
                    5,
                    6,
                    7,
                    8,
                    9,
                    10,
                    11,
                    12,
                    13,
                    14,
                    15,
                    16,
                    17,
                    18,
                    19,
                    20,
                    21,
                    22,
                    23,
                    24,
                    25,
                    26,
                    27,
                    28,
                    29,
                    30,
                    31,
                    32,
                    33,
                    34,
                    35,
                    36,
                    37,
                    38,
                    39,
                    40,
                    41,
                    42,
                    43,
                    44,
                    45,
                    46,
                    47,
                    48,
                    49,
                    50
                });

            migrationBuilder.CreateIndex(
                name: "ix_parking_sessions_active_space",
                table: "parking_sessions",
                column: "SpaceNumber",
                unique: true,
                filter: "\"TimeOut\" IS NULL");

            migrationBuilder.CreateIndex(
                name: "ix_parking_sessions_active_vehicle_reg",
                table: "parking_sessions",
                column: "VehicleReg",
                unique: true,
                filter: "\"TimeOut\" IS NULL");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "parking_sessions");

            migrationBuilder.DropTable(
                name: "parking_spaces");
        }
    }
}
