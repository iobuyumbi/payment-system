using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class doctype : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<Guid>(
                name: "DocumetTypeId",
                table: "Farmers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateTable(
                name: "DocumetType",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    TypeName = table.Column<string>(type: "character varying(20)", maxLength: 20, nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_DocumetType", x => x.Id);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_DocumetTypeId",
                table: "Farmers",
                column: "DocumetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_DocumetType_DocumetTypeId",
                table: "Farmers",
                column: "DocumetTypeId",
                principalTable: "DocumetType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_DocumetType_DocumetTypeId",
                table: "Farmers");

            migrationBuilder.DropTable(
                name: "DocumetType");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_DocumetTypeId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "DocumetTypeId",
                table: "Farmers");
        }
    }
}
