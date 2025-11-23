using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class FKM : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "AdminLevel4",
                table: "Addresses",
                newName: "CountryId");

            migrationBuilder.RenameColumn(
                name: "AdminLevel3",
                table: "Addresses",
                newName: "AdminLevel4Id");

            migrationBuilder.RenameColumn(
                name: "AdminLevel2",
                table: "Addresses",
                newName: "AdminLevel3Id");

            migrationBuilder.RenameColumn(
                name: "AdminLevel1",
                table: "Addresses",
                newName: "AdminLevel2Id");

            migrationBuilder.RenameColumn(
                name: "AdminLevel0",
                table: "Addresses",
                newName: "AdminLevel1Id");

            migrationBuilder.AddColumn<Guid>(
                name: "AddressId",
                table: "Projects",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Projects_AddressId",
                table: "Projects",
                column: "AddressId");

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

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdminLevel1Id",
                table: "Addresses",
                column: "AdminLevel1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdminLevel2Id",
                table: "Addresses",
                column: "AdminLevel2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdminLevel3Id",
                table: "Addresses",
                column: "AdminLevel3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_AdminLevel4Id",
                table: "Addresses",
                column: "AdminLevel4Id");

            migrationBuilder.CreateIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AdminLevel1_AdminLevel1Id",
                table: "Addresses",
                column: "AdminLevel1Id",
                principalTable: "AdminLevel1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AdminLevel2_AdminLevel2Id",
                table: "Addresses",
                column: "AdminLevel2Id",
                principalTable: "AdminLevel2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AdminLevel3_AdminLevel3Id",
                table: "Addresses",
                column: "AdminLevel3Id",
                principalTable: "AdminLevel3",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_AdminLevel4_AdminLevel4Id",
                table: "Addresses",
                column: "AdminLevel4Id",
                principalTable: "AdminLevel4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

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

            migrationBuilder.AddForeignKey(
                name: "FK_Projects_Addresses_AddressId",
                table: "Projects",
                column: "AddressId",
                principalTable: "Addresses",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AdminLevel1_AdminLevel1Id",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AdminLevel2_AdminLevel2Id",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AdminLevel3_AdminLevel3Id",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_AdminLevel4_AdminLevel4Id",
                table: "Addresses");

            migrationBuilder.DropForeignKey(
                name: "FK_Addresses_Countries_CountryId",
                table: "Addresses");

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

            migrationBuilder.DropForeignKey(
                name: "FK_Projects_Addresses_AddressId",
                table: "Projects");

            migrationBuilder.DropIndex(
                name: "IX_Projects_AddressId",
                table: "Projects");

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

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AdminLevel1Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AdminLevel2Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AdminLevel3Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_AdminLevel4Id",
                table: "Addresses");

            migrationBuilder.DropIndex(
                name: "IX_Addresses_CountryId",
                table: "Addresses");

            migrationBuilder.DropColumn(
                name: "AddressId",
                table: "Projects");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "Addresses",
                newName: "AdminLevel4");

            migrationBuilder.RenameColumn(
                name: "AdminLevel4Id",
                table: "Addresses",
                newName: "AdminLevel3");

            migrationBuilder.RenameColumn(
                name: "AdminLevel3Id",
                table: "Addresses",
                newName: "AdminLevel2");

            migrationBuilder.RenameColumn(
                name: "AdminLevel2Id",
                table: "Addresses",
                newName: "AdminLevel1");

            migrationBuilder.RenameColumn(
                name: "AdminLevel1Id",
                table: "Addresses",
                newName: "AdminLevel0");
        }
    }
}
