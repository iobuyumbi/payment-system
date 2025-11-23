using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class importcolumns : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "PaymentRequestFacilitation",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "PaymentRequestFacilitation",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "PaymentRequestDeductible",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Remarks",
                table: "Farmers",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<int>(
                name: "StatusId",
                table: "Farmers",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "PaymentRequestFacilitation");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "PaymentRequestFacilitation");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "PaymentRequestDeductible");

            migrationBuilder.DropColumn(
                name: "Remarks",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "StatusId",
                table: "Farmers");
        }
    }
}
