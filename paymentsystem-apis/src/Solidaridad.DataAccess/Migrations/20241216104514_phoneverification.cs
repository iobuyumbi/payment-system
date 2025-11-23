using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class phoneverification : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Farmers");

            migrationBuilder.AddColumn<bool>(
                name: "IsFarmerVerified",
                table: "Farmers",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "IsFarmerVerified",
                table: "Farmers");

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Farmers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }
    }
}
