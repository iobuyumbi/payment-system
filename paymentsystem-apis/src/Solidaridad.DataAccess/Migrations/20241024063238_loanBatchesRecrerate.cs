using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchesRecrerate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Table already exists from migration 20240729062055_loanBatches
            // This migration should not recreate it - the table structure is already correct
            // Only create if it doesn't exist (handles edge cases)
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'LoanBatches') THEN
                        CREATE TABLE ""LoanBatches"" (
                            ""Id"" uuid NOT NULL,
                            ""Name"" text,
                            ""InitiatedBy"" uuid NOT NULL,
                            ""InitiationDate"" timestamp with time zone NOT NULL,
                            ""ClosedBy"" uuid NOT NULL,
                            ""ClosingDate"" timestamp with time zone,
                            ""ProjectId"" uuid NOT NULL,
                            ""StatusId"" integer NOT NULL,
                            CONSTRAINT ""PK_LoanBatches"" PRIMARY KEY (""Id"")
                        );
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<int>(
                name: "Tenure",
                table: "LoanBatches",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);

            migrationBuilder.AlterColumn<int>(
                name: "GracePeriod",
                table: "LoanBatches",
                type: "integer",
                nullable: false,
                defaultValue: 0,
                oldClrType: typeof(int),
                oldType: "integer",
                oldNullable: true);
        }
    }
}
