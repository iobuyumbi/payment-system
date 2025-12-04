using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class roolepermission : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            // PostgreSQL requires explicit USING clause for string to uuid conversion
            // Since this is a fresh database with no data, convert empty strings to a default UUID
            migrationBuilder.Sql(@"
                ALTER TABLE ""RolePermission"" 
                ALTER COLUMN ""RoleId"" TYPE uuid USING 
                    CASE 
                        WHEN ""RoleId"" = '' OR ""RoleId"" IS NULL THEN '00000000-0000-0000-0000-000000000000'::uuid
                        ELSE (""RoleId"")::uuid
                    END;
            ");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            // Convert uuid back to string
            migrationBuilder.Sql(@"
                ALTER TABLE ""RolePermission"" 
                ALTER COLUMN ""RoleId"" TYPE character varying(36) USING ""RoleId""::text;
            ");
        }
    }
}
