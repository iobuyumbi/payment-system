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
            // First, convert text columns to uuid (nullable) before renaming
            // PostgreSQL requires explicit USING clause for text to uuid conversion
            // Since this is a fresh database with no data, we can set all to NULL
            migrationBuilder.Sql(@"
                ALTER TABLE ""Addresses"" 
                ALTER COLUMN ""AdminLevel4"" TYPE uuid USING NULL;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Addresses"" 
                ALTER COLUMN ""AdminLevel3"" TYPE uuid USING NULL;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Addresses"" 
                ALTER COLUMN ""AdminLevel2"" TYPE uuid USING NULL;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Addresses"" 
                ALTER COLUMN ""AdminLevel1"" TYPE uuid USING NULL;
            ");

            migrationBuilder.Sql(@"
                ALTER TABLE ""Addresses"" 
                ALTER COLUMN ""AdminLevel0"" TYPE uuid USING NULL;
            ");

            // Now rename the columns
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

            // Rename columns back first
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

            // Convert uuid columns back to text
            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel4",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel3",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel2",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel1",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel0",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);
        }
    }
}
