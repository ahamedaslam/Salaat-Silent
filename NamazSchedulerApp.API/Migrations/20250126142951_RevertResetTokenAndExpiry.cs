using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace NamazSchedulerApp.API.Migrations
{
    /// <inheritdoc />
    public partial class RevertResetTokenAndExpiry : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "ResetToken",
                table: "Users",
                type: "nvarchar(max)",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<DateTime>(
                name: "TokenExpiry",
                table: "Users",
                type: "datetime2",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ResetToken",
                table: "Users");

            migrationBuilder.DropColumn(
                name: "TokenExpiry",
                table: "Users");
        }
    }
}
