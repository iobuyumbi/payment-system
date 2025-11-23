using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class farmerUpdated : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel4_AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "CooperativeId",
                table: "Farmers");

            migrationBuilder.AddColumn<short>(
                name: "BirthMonth",
                table: "Farmers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<short>(
                name: "BirthYear",
                table: "Farmers",
                type: "smallint",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Village",
                table: "Farmers",
                type: "text",
                nullable: true);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "BirthMonth",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "BirthYear",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "Village",
                table: "Farmers");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminLevel4Id",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "CooperativeId",
                table: "Farmers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers",
                column: "AdminLevel4Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel4_AdminLevel4Id",
                table: "Farmers",
                column: "AdminLevel4Id",
                principalTable: "AdminLevel4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
