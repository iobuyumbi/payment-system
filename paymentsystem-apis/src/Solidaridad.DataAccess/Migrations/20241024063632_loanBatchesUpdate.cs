using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class loanBatchesUpdate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // Add columns only if they don't exist (handles partial migration states)
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'CalculationTimeframe') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""CalculationTimeframe"" text;
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'Effectivedate') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""Effectivedate"" timestamp with time zone NOT NULL DEFAULT '0001-01-01 00:00:00+00';
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'GracePeriod') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""GracePeriod"" integer NOT NULL DEFAULT 0;
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'ProcessingFee') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""ProcessingFee"" numeric NOT NULL DEFAULT 0;
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'RateType') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""RateType"" text;
                    END IF;
                    
                    IF NOT EXISTS (SELECT 1 FROM information_schema.columns WHERE table_name = 'LoanBatches' AND column_name = 'Tenure') THEN
                        ALTER TABLE ""LoanBatches"" ADD ""Tenure"" integer NOT NULL DEFAULT 0;
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {

        }
    }
}
