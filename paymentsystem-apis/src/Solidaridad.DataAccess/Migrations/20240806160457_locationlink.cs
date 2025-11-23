using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class locationlink : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "SubCountyCode",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "WardCode",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "CountyCode",
                table: "SubCounties");

            migrationBuilder.DropColumn(
                name: "CountryCode",
                table: "Counties");

            migrationBuilder.AlterColumn<string>(
                name: "WardName",
                table: "Wards",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "WardCode",
                table: "Wards",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "SubCountyId",
                table: "Wards",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "VillageName",
                table: "Villages",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VillageCode",
                table: "Villages",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "WardId",
                table: "Villages",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "SubCountyName",
                table: "SubCounties",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubCountyCode",
                table: "SubCounties",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubCounties",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountyId",
                table: "SubCounties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySuffix",
                table: "Countries",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyPrefix",
                table: "Countries",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                table: "Countries",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Counties",
                type: "boolean",
                nullable: false,
                defaultValue: false,
                oldClrType: typeof(bool),
                oldType: "boolean",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountyName",
                table: "Counties",
                type: "character varying(126)",
                maxLength: 126,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CountyCode",
                table: "Counties",
                type: "character varying(5)",
                maxLength: 5,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "text",
                oldNullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "CountryId",
                table: "Counties",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.CreateIndex(
                name: "IX_Wards_SubCountyId",
                table: "Wards",
                column: "SubCountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_WardId",
                table: "Villages",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCounties_CountyId",
                table: "SubCounties",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Counties_CountryId",
                table: "Counties",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Counties_Countries_CountryId",
                table: "Counties",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_SubCounties_Counties_CountyId",
                table: "SubCounties",
                column: "CountyId",
                principalTable: "Counties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Villages_Wards_WardId",
                table: "Villages",
                column: "WardId",
                principalTable: "Wards",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Wards_SubCounties_SubCountyId",
                table: "Wards",
                column: "SubCountyId",
                principalTable: "SubCounties",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Counties_Countries_CountryId",
                table: "Counties");

            migrationBuilder.DropForeignKey(
                name: "FK_SubCounties_Counties_CountyId",
                table: "SubCounties");

            migrationBuilder.DropForeignKey(
                name: "FK_Villages_Wards_WardId",
                table: "Villages");

            migrationBuilder.DropForeignKey(
                name: "FK_Wards_SubCounties_SubCountyId",
                table: "Wards");

            migrationBuilder.DropIndex(
                name: "IX_Wards_SubCountyId",
                table: "Wards");

            migrationBuilder.DropIndex(
                name: "IX_Villages_WardId",
                table: "Villages");

            migrationBuilder.DropIndex(
                name: "IX_SubCounties_CountyId",
                table: "SubCounties");

            migrationBuilder.DropIndex(
                name: "IX_Counties_CountryId",
                table: "Counties");

            migrationBuilder.DropColumn(
                name: "SubCountyId",
                table: "Wards");

            migrationBuilder.DropColumn(
                name: "WardId",
                table: "Villages");

            migrationBuilder.DropColumn(
                name: "CountyId",
                table: "SubCounties");

            migrationBuilder.DropColumn(
                name: "CountryId",
                table: "Counties");

            migrationBuilder.AlterColumn<string>(
                name: "WardName",
                table: "Wards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126);

            migrationBuilder.AlterColumn<string>(
                name: "WardCode",
                table: "Wards",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "SubCountyCode",
                table: "Wards",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "VillageName",
                table: "Villages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126);

            migrationBuilder.AlterColumn<string>(
                name: "VillageCode",
                table: "Villages",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "WardCode",
                table: "Villages",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "SubCountyName",
                table: "SubCounties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126);

            migrationBuilder.AlterColumn<string>(
                name: "SubCountyCode",
                table: "SubCounties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "SubCounties",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AddColumn<string>(
                name: "CountyCode",
                table: "SubCounties",
                type: "text",
                nullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencySuffix",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyPrefix",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<string>(
                name: "CurrencyName",
                table: "Countries",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AlterColumn<bool>(
                name: "IsActive",
                table: "Counties",
                type: "boolean",
                nullable: true,
                oldClrType: typeof(bool),
                oldType: "boolean");

            migrationBuilder.AlterColumn<string>(
                name: "CountyName",
                table: "Counties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(126)",
                oldMaxLength: 126);

            migrationBuilder.AlterColumn<string>(
                name: "CountyCode",
                table: "Counties",
                type: "text",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(5)",
                oldMaxLength: 5);

            migrationBuilder.AddColumn<string>(
                name: "CountryCode",
                table: "Counties",
                type: "text",
                nullable: true);
        }
    }
}
