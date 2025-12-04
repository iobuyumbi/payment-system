using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanAppUpd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.DropTable(
                name: "Seedling");

            migrationBuilder.DropColumn(
                name: "Cost",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "Unit",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "AdminLevel1",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel2",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel3",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "AdminLevel4",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "BeneficiaryId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "CooperativeId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FacilitationCost",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FarmSize",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FirstPagePath",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "FullName",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "GrandTotal",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "Mobile",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "NationalId",
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
                name: "SolidaridadId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "ThirdPagePath",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "TotalSeedlingCost",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "TotalSeedlings",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "TransportCost",
                table: "LoanApplications");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "LoanApplications",
                newName: "FarmerId");

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "LoanItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoanItems",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LoanApplicationId",
                table: "LoanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "MasterLoanItemId",
                table: "LoanItems",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<decimal>(
                name: "Quantity",
                table: "LoanItems",
                type: "numeric",
                nullable: false,
                defaultValue: 0m);

            migrationBuilder.AddColumn<int>(
                name: "UnitId",
                table: "LoanItems",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<decimal>(
                name: "UnitPrice",
                table: "LoanItems",
                type: "numeric",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessPhoneNo",
                table: "LoanApplications",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessNationalId",
                table: "LoanApplications",
                type: "character varying(30)",
                maxLength: 30,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessFullName",
                table: "LoanApplications",
                type: "character varying(126)",
                maxLength: 126,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WinessRelation",
                table: "LoanApplications",
                type: "character varying(50)",
                maxLength: 50,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanBatchId",
                table: "LoanApplications",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "EnumeratorFullName",
                table: "LoanApplications",
                type: "character varying(126)",
                maxLength: 126,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.CreateTable(
                name: "MasterLoanItems",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    ItemName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    Description = table.Column<string>(type: "character varying(500)", maxLength: 500, nullable: true),
                    Unit = table.Column<string>(type: "text", nullable: true),
                    CategoryId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_MasterLoanItems", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_LoanItems_LoanApplicationId",
                table: "LoanItems",
                column: "LoanApplicationId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanItems_MasterLoanItemId",
                table: "LoanItems",
                column: "MasterLoanItemId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanItems_UnitId",
                table: "LoanItems",
                column: "UnitId");

            migrationBuilder.CreateIndex(
                name: "IX_LoanApplications_FarmerId",
                table: "LoanApplications",
                column: "FarmerId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanApplications_Farmers_FarmerId",
                table: "LoanApplications",
                column: "FarmerId",
                principalTable: "Farmers",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_MasterLoanItems_LoanItemId",
                table: "LoanBatchItem",
                column: "LoanItemId",
                principalTable: "MasterLoanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanItems_ItemUnit_UnitId",
                table: "LoanItems",
                column: "UnitId",
                principalTable: "ItemUnit",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanItems_LoanApplications_LoanApplicationId",
                table: "LoanItems",
                column: "LoanApplicationId",
                principalTable: "LoanApplications",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_LoanItems_MasterLoanItems_MasterLoanItemId",
                table: "LoanItems",
                column: "MasterLoanItemId",
                principalTable: "MasterLoanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanApplications_Farmers_FarmerId",
                table: "LoanApplications");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatchItem_MasterLoanItems_LoanItemId",
                table: "LoanBatchItem");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanItems_ItemUnit_UnitId",
                table: "LoanItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanItems_LoanApplications_LoanApplicationId",
                table: "LoanItems");

            migrationBuilder.DropForeignKey(
                name: "FK_LoanItems_MasterLoanItems_MasterLoanItemId",
                table: "LoanItems");

            migrationBuilder.DropTable(
                name: "MasterLoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanItems_LoanApplicationId",
                table: "LoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanItems_MasterLoanItemId",
                table: "LoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanItems_UnitId",
                table: "LoanItems");

            migrationBuilder.DropIndex(
                name: "IX_LoanApplications_FarmerId",
                table: "LoanApplications");

            migrationBuilder.DropColumn(
                name: "LoanApplicationId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "MasterLoanItemId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "Quantity",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "UnitId",
                table: "LoanItems");

            migrationBuilder.DropColumn(
                name: "UnitPrice",
                table: "LoanItems");

            migrationBuilder.RenameColumn(
                name: "FarmerId",
                table: "LoanApplications",
                newName: "CountryId");

            migrationBuilder.AlterColumn<string>(
                name: "ItemName",
                table: "LoanItems",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "LoanItems",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<float>(
                name: "Cost",
                table: "LoanItems",
                type: "real",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Unit",
                table: "LoanItems",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessPhoneNo",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(20)",
                oldMaxLength: 20,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessNationalId",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(30)",
                oldMaxLength: 30,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WitnessFullName",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126,
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WinessRelation",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(50)",
                oldMaxLength: 50,
                oldNullable: true);

            migrationBuilder.AlterColumn<Guid>(
                name: "LoanBatchId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "EnumeratorFullName",
                table: "LoanApplications",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126,
                oldNullable: true);

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel1",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel2",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel3",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel4",
                table: "LoanApplications",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "BeneficiaryId",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CooperativeId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "FacilitationCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<string>(
                name: "FarmSize",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FirstPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "FullName",
                table: "LoanApplications",
                type: "text",
                nullable: true);

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
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "NationalId",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "QRSeedling",
                table: "LoanApplications",
                type: "boolean",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "QSFarmer",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "SecondPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SolidaridadId",
                table: "LoanApplications",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "ThirdPagePath",
                table: "LoanApplications",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<float>(
                name: "TotalSeedlingCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.AddColumn<int>(
                name: "TotalSeedlings",
                table: "LoanApplications",
                type: "integer",
                nullable: false,
                defaultValue: 0);

            migrationBuilder.AddColumn<float>(
                name: "TransportCost",
                table: "LoanApplications",
                type: "real",
                nullable: false,
                defaultValue: 0f);

            migrationBuilder.CreateTable(
                name: "Seedling",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    LoanApplicationId = table.Column<Guid>(type: "uuid", nullable: true),
                    Name = table.Column<string>(type: "text", nullable: true),
                    PricePerSeedling = table.Column<float>(type: "real", nullable: false),
                    Quantity = table.Column<int>(type: "integer", nullable: false)
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
                name: "IX_Seedling_LoanApplicationId",
                table: "Seedling",
                column: "LoanApplicationId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatchItem_LoanItems_LoanItemId",
                table: "LoanBatchItem",
                column: "LoanItemId",
                principalTable: "LoanItems",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
