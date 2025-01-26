using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamazSchedulerApp.API.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Locations",
                columns: table => new
                {
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false),
                    Timezone = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Prayers",
                columns: table => new
                {
                    Fajr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Dhuhr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Asr = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Maghrib = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Isha = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                });

            migrationBuilder.CreateTable(
                name: "Preferences",
                columns: table => new
                {
                    PreferenceId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Method = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    SilentMode = table.Column<bool>(type: "bit", nullable: false),
                    Latitude = table.Column<double>(type: "float", nullable: false),
                    Longitude = table.Column<double>(type: "float", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Preferences", x => x.PreferenceId);
                });

            migrationBuilder.CreateTable(
                name: "Users",
                columns: table => new
                {
                    UserId = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                    Username = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Email = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    PasswordHash = table.Column<string>(type: "nvarchar(max)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.UserId);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Locations");

            migrationBuilder.DropTable(
                name: "Prayers");

            migrationBuilder.DropTable(
                name: "Preferences");

            migrationBuilder.DropTable(
                name: "Users");
        }
    }
}
