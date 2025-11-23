using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class spellcorrection : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_DocumetType_DocumetTypeId",
                table: "Farmers");

            migrationBuilder.RenameColumn(
                name: "DocumetTypeId",
                table: "Farmers",
                newName: "DocumentTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Farmers_DocumetTypeId",
                table: "Farmers",
                newName: "IX_Farmers_DocumentTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_DocumetType_DocumentTypeId",
                table: "Farmers",
                column: "DocumentTypeId",
                principalTable: "DocumetType",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_DocumetType_DocumentTypeId",
                table: "Farmers");

            migrationBuilder.RenameColumn(
                name: "DocumentTypeId",
                table: "Farmers",
                newName: "DocumetTypeId");

            migrationBuilder.RenameIndex(
                name: "IX_Farmers_DocumentTypeId",
                table: "Farmers",
                newName: "IX_Farmers_DocumetTypeId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_DocumetType_DocumetTypeId",
                table: "Farmers",
                column: "DocumetTypeId",
                principalTable: "DocumetType",
                principalColumn: "Id");
        }
    }
}
