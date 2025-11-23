using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class Location : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            //migrationBuilder.DropForeignKey(
            //    name: "FK_Farmers_Projects_ProjectId",
            //    table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatches_Countries_CountryId",
                table: "LoanBatches");

            migrationBuilder.DropIndex(
                name: "IX_LoanBatches_CountryId",
                table: "LoanBatches");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_ProjectId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "CostituencyId",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "ClosedBy",
                table: "LoanBatches");

            migrationBuilder.DropColumn(
                name: "ClosingDate",
                table: "LoanBatches");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "LoanBatches");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Counties");

            migrationBuilder.RenameColumn(
                name: "Name",
                table: "Wards",
                newName: "WardName");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Wards",
                newName: "WardCode");

            migrationBuilder.RenameColumn(
                name: "Code",
                table: "Counties",
                newName: "CountyCode");

            migrationBuilder.AddColumn<string>(
                name: "SubCountyCode",
                table: "Wards",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CategoryId",
                table: "LoanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<float>(
                name: "Cost",
                table: "LoanItems",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyName",
                table: "Countries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencyPrefix",
                table: "Countries",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "CurrencySuffix",
                table: "Countries",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Counties",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Counties",
                type: "text",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "SubCounties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubCountyName = table.Column<string>(type: "text", nullable: true),
                    SubCountyCode = table.Column<string>(type: "text", nullable: true),
                    CountyCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCounties", x => x.Id);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VillageName = table.Column<string>(type: "text", nullable: true),
                    VillageCode = table.Column<string>(type: "text", nullable: true),
                    WardCode = table.Column<string>(type: "text", nullable: true),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "SubCounties");

            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropColumn(
                name: "SubCountyCode",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "CategoryId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "CurrencyName",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CurrencyPrefix",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CurrencySuffix",
                table: "Countries");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Counties");

            migrationBuilder.RenameColumn(
                name: "WardName",
                table: "Wards",
                newName: "Name");

            migrationBuilder.RenameColumn(
                name: "WardCode",
                table: "Wards",
                newName: "Code");

            migrationBuilder.RenameColumn(
                name: "CountyCode",
                table: "Counties",
                newName: "Code");

            migrationBuilder.AddColumn<Guid>(
                name: "CostituencyId",
                table: "Wards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ClosedBy",
                table: "LoanBatches",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<DateTime>(
                name: "ClosingDate",
                table: "LoanBatches",
                type: "timestamp with time zone",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "LoanBatches",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Counties",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Counties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatches_CountryId",
                table: "LoanBatches",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ProjectId",
                table: "Farmers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_Projects_ProjectId",
                table: "Farmers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatches_Countries_CountryId",
                table: "LoanBatches",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
