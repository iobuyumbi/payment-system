using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanStatement : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "IsPaid",
                table: "LoanRepaymentSchedule",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.CreateTable(
                name: "LoanStatement",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    StatementDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: false),
                    LastPaymentDate = table.Column<DateTime>(type: "timestamp with time zone", nullable: true),
                    CurrentPrincipal = table.Column<decimal>(type: "numeric", nullable: false),
                    PendingPayments = table.Column<decimal>(type: "numeric", nullable: false),
                    InterestAccrued = table.Column<decimal>(type: "numeric", nullable: false),
                    FarmerId = table.Column<Guid>(type: "uuid", nullable: false),
                    SystemId = table.Column<string>(type: "text", nullable: true),
                    ApplicationId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsDeleted = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_LoanStatement", x => x.Id);
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "LoanStatement");

            migrationBuilder.DropColumn(
                name: "IsPaid",
                table: "LoanRepaymentSchedule");
        }
    }
}
