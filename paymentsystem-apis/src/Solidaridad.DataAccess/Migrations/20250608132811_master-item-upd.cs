using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class masteritemupd : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_MasterLoanItems_CategoryId",
                table: "MasterLoanItems",
                column: "CategoryId");

            migrationBuilder.AddForeignKey(
                name: "FK_MasterLoanItems_ItemCategories_CategoryId",
                table: "MasterLoanItems",
                column: "CategoryId",
                principalTable: "ItemCategories",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_MasterLoanItems_ItemCategories_CategoryId",
                table: "MasterLoanItems");

            migrationBuilder.DropIndex(
                name: "IX_MasterLoanItems_CategoryId",
                table: "MasterLoanItems");
        }
    }
}
