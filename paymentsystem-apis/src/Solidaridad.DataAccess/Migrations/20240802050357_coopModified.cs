using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class coopModified : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Rename table from "Cooperative" to "Cooperatives" to match DbSet name
            migrationBuilder.RenameTable(
                name: "Cooperative",
                newName: "Cooperatives");

            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cooperatives",
                type: "character varying(500)",
                maxLength: 500,
                nullable: true,
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Description",
                table: "Cooperatives",
                type: "character varying(500)",
                maxLength: 500,
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "character varying(500)",
                oldMaxLength: 500,
                oldNullable: true);

            // Rename table back from "Cooperatives" to "Cooperative"
            migrationBuilder.RenameTable(
                name: "Cooperatives",
                newName: "Cooperative");
        }
    }
}
