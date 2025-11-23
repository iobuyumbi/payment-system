using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class updateAddress : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel1_AdminLevel1Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel2_AdminLevel2Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel3_AdminLevel3Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel4_AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel1Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel2Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel3Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel1Id",
                table: "Farmers",
                column: "AdminLevel1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel2Id",
                table: "Farmers",
                column: "AdminLevel2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel3Id",
                table: "Farmers",
                column: "AdminLevel3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers",
                column: "AdminLevel4Id");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel1_AdminLevel1Id",
                table: "Farmers",
                column: "AdminLevel1Id",
                principalTable: "AdminLevel1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel2_AdminLevel2Id",
                table: "Farmers",
                column: "AdminLevel2Id",
                principalTable: "AdminLevel2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel3_AdminLevel3Id",
                table: "Farmers",
                column: "AdminLevel3Id",
                principalTable: "AdminLevel3",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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
