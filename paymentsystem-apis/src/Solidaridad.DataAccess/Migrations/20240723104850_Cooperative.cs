using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Cooperative : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_Addresses_AddressId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_Farmers_ApplicantId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_AddressId",
                table: "LoanApplications");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_ApplicantId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ApplicantId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "InterestRate",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "LoanAmount",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Phone",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "LoanTerm",
                table: "LoanApplications",
                newName: "TotalSeedlings");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "FarmSize",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Cooperative",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "DateOfWitness",
                table: "LoanApplications",
                type: "timestamp with time zone",
                nullable: false,
                defaultValue: new DateTime(1, 1, 1, 0, 0, 0, 0, DateTimeKind.Unspecified));

            migrationBuilder.AddColumn<string>(
                name: "EnumeratorFullName",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "FacilitationCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "FirstPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "GrandTotal",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "Mobile",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<bool>(
                name: "QRSeedling",
                table: "LoanApplications",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<string>(
                name: "QSFarmer",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "SecondPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "ThirdPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<float>(
                name: "TotalSeedlingCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<float>(
                name: "TransportCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "WinessRelation",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessFullName",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessNationalId",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "WitnessPhoneNo",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.CreateTable(
                name: "Cooperative",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", maxLength: 36, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Cooperative", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Cooperative_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Seedling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "text", nullable: true),
                    Quantity = table.Column<int>(type: "integer", nullable: false),
                    PricePerSeedling = table.Column<float>(type: "real", nullable: false),
                    LoanApplicationId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Seedling", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Seedling_LoanApplications_LoanApplicationId",
                        column: x => x.LoanApplicationId,
                        principalTable: "LoanApplications",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateIndex(
                name: "IX_Cooperative_CountryId",
                table: "Cooperative",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Seedling_LoanApplicationId",
                table: "Seedling",
                column: "LoanApplicationId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Cooperative");

            migrationBuilder.DropTable(
                name: "Seedling");

            migrationBuilder.DropColumn(
                name: "DateOfWitness",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "EnumeratorFullName",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FacilitationCost",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FirstPagePath",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "QRSeedling",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "QSFarmer",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "SecondPagePath",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ThirdPagePath",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "TotalSeedlingCost",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "TransportCost",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "WinessRelation",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "WitnessFullName",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "WitnessNationalId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "WitnessPhoneNo",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "TotalSeedlings",
                table: "LoanApplications",
                newName: "LoanTerm");

            migrationBuilder.AlterColumn<string>(
                name: "NationalId",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Location",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FullName",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "FarmSize",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AlterColumn<string>(
                name: "Cooperative",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "ApplicantId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<double>(
                name: "InterestRate",
                table: "LoanApplications",
                type: "double precision",
                nullable: false,
                defaultValue: 0.0);

            migrationBuilder.AddColumn<decimal>(
                name: "LoanAmount",
                table: "LoanApplications",
                type: "numeric",
                maxLength: 255,
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<string>(
                name: "Phone",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_AddressId",
                table: "LoanApplications",
                column: "AddressId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_ApplicantId",
                table: "LoanApplications",
                column: "ApplicantId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_Addresses_AddressId",
                table: "LoanApplications",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_Farmers_ApplicantId",
                table: "LoanApplications",
                column: "ApplicantId",
                principalTable: "Farmers",
                principalColumn: "Id");
        }
    }
}
