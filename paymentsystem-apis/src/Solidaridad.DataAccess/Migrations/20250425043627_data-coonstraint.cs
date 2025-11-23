using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class datacoonstraint : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_Projects_ProjectId",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_ProjectId",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "ProjectId",
                table: "Farmers");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_SystemId",
                table: "Farmers",
                column: "SystemId",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"SystemId\" IS NOT NULL AND \"SystemId\" <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Farmers_SystemId",
                table: "Farmers");

            migrationBuilder.AddColumn<Guid>(
                name: "ProjectId",
                table: "Farmers",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ProjectId",
                table: "Farmers",
                column: "ProjectId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_Projects_ProjectId",
                table: "Farmers",
                column: "ProjectId",
                principalTable: "Projects",
                principalColumn: "Id");
        }
    }
}
