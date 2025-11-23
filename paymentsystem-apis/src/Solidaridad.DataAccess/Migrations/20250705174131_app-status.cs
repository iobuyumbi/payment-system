using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class appstatus : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_ApplicationStatusLogs_StatusId",
                table: "ApplicationStatusLogs",
                column: "StatusId");

            migrationBuilder.AddForeignKey(
                name: "FK_ApplicationStatusLogs_ApplicationStatus_StatusId",
                table: "ApplicationStatusLogs",
                column: "StatusId",
                principalTable: "ApplicationStatus",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_ApplicationStatusLogs_ApplicationStatus_StatusId",
                table: "ApplicationStatusLogs");

            migrationBuilder.DropIndex(
                name: "IX_ApplicationStatusLogs_StatusId",
                table: "ApplicationStatusLogs");
        }
    }
}
