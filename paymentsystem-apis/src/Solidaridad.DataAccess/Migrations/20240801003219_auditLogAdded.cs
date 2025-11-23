using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class auditLogAdded : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropPrimaryKey(
                name: "PK_\"LogEntry\"",
                table: "\"LogEntry\"");

            migrationBuilder.RenameTable(
                name: "\"LogEntry\"",
                newName: "LogEntry");

            migrationBuilder.AddPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry",
                column: "Id");

            //migrationBuilder.CreateTable(
            //    name: "AuditLog",
            //    columns: table => new
            //    {
            //        Id = table.Column<int>(type: "integer", nullable: false)
            //            .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn),
            //        TableName = table.Column<string>(type: "text", nullable: false),
            //        ColumnName = table.Column<string>(type: "text", nullable: false),
            //        OldValue = table.Column<string>(type: "text", nullable: true),
            //        NewValue = table.Column<string>(type: "text", nullable: true),
            //        DateTime = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
            //        Username = table.Column<string>(type: "text", nullable: false)
            //    },
            //    constraints: table =>
            //    {
            //        table.PrimaryKey("PK_AuditLog", x => x.Id);
            //    });

            migrationBuilder.CreateIndex(
                name: "IX_LoanBatches_CountryId",
                table: "LoanBatches",
                column: "CountryId");

            migrationBuilder.AddForeignKey(
                name: "FK_LoanBatches_Countries_CountryId",
                table: "LoanBatches",
                column: "CountryId",
                principalTable: "Countries",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LoanBatches_Countries_CountryId",
                table: "LoanBatches");

            migrationBuilder.DropTable(
                name: "AuditLog");

            migrationBuilder.DropIndex(
                name: "IX_LoanBatches_CountryId",
                table: "LoanBatches");

            migrationBuilder.DropPrimaryKey(
                name: "PK_LogEntry",
                table: "LogEntry");

            migrationBuilder.RenameTable(
                name: "LogEntry",
                newName: "\"LogEntry\"");

            migrationBuilder.AddPrimaryKey(
                name: "PK_\"LogEntry\"",
                table: "\"LogEntry\"",
                column: "Id");
        }
    }
}
