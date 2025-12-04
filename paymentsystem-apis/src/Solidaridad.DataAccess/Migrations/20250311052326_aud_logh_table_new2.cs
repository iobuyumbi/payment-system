using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class aud_logh_table_new2 : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.Sql(@"
                DO $$
                BEGIN
                    IF NOT EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'AuditLog') THEN
                        CREATE TABLE ""AuditLog"" (
                            ""Id"" character varying(36) NOT NULL,
                            ""EntityType"" character varying(100) NOT NULL,
                            ""EntityId"" character varying(36) NOT NULL,
                            ""OldValue"" text NULL,
                            ""NewValue"" text NULL,
                            ""ChangedOn"" timestamp without time zone NOT NULL,
                            ""ChangedBy"" character varying(36) NOT NULL,
                            ""ChangeType"" character varying(50) NOT NULL,
                            ""Field"" text NOT NULL,
                            CONSTRAINT ""PK_AuditLog"" PRIMARY KEY (""Id"")
                        );
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
                    IF EXISTS (SELECT 1 FROM information_schema.tables WHERE table_name = 'AuditLog') THEN
                        DROP TABLE ""AuditLog"";
                    END IF;
                END $$;
            ");
        }
    }
}
