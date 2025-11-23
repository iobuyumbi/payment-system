using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class UserOtpUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Otp",
                table: "UserOtp");

            migrationBuilder.AddColumn<int>(
                name: "AttemptCount",
                table: "UserOtp",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<DateTime>(
                name: "CreatedAt",
                table: "UserOtp",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "OtpHash",
                table: "UserOtp",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "AttemptCount",
                table: "UserOtp");

            migrationBuilder.DropColumn(
                name: "CreatedAt",
                table: "UserOtp");

            migrationBuilder.DropColumn(
                name: "OtpHash",
                table: "UserOtp");

            migrationBuilder.AddColumn<string>(
                name: "Otp",
                table: "UserOtp",
                type: "character varying(12)",
                maxLength: 12,
                nullable: true);
        }
    }
}
