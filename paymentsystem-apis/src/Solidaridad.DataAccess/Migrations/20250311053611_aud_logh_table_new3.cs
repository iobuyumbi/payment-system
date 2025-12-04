using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class aud_logh_table_new3 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = current_schema()
                          AND lower(table_name) = 'auditlog'
                          AND lower(column_name) = 'changetype'
                    ) THEN
                        ALTER TABLE ""AuditLog"" ADD ""ChangeType"" character varying(50) NOT NULL DEFAULT '';
                    END IF;
                END $$;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF EXISTS (
                        SELECT 1
                        FROM information_schema.columns
                        WHERE table_schema = current_schema()
                          AND lower(table_name) = 'auditlog'
                          AND lower(column_name) = 'changetype'
                    ) THEN
                        ALTER TABLE ""AuditLog"" DROP COLUMN ""ChangeType"";
                    END IF;
                END $$;
            ");
        }
    }
}
