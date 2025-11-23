using System;
using Microsoft.EntityFrameworkCore.Migrations;
using Npgsql.EntityFrameworkCore.PostgreSQL.Metadata;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class aud_logh : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ColumnName",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "TableName",
                table: "AuditLog");

             migrationBuilder.DropColumn(
                name: "Id",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Username",
                table: "AuditLog");

            migrationBuilder.RenameColumn(
                name: "DateTime",
                table: "AuditLog",
                newName: "ChangedOn");


            migrationBuilder.AddColumn<string>(
                name: "ChangeType",
                table: "AuditLog",
                type: "character varying(50)",
                maxLength: 50,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<Guid>(
                name: "ChangedBy",
                table: "AuditLog",
                type: "uuid",
                maxLength: 100,
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "EntityId",
                table: "AuditLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<string>(
                name: "EntityType",
                table: "AuditLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Field",
                table: "AuditLog",
                type: "text",
                nullable: true);

            migrationBuilder.AddColumn<bool>(
                name: "IsDeleted",
                table: "AuditLog",
                type: "boolean",
                nullable: false,
                defaultValue: false);

            migrationBuilder.AddColumn<Guid>(
                name: "OrgId",
                table: "AuditLog",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "ChangeType",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "ChangedBy",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "EntityId",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "EntityType",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "Field",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "IsDeleted",
                table: "AuditLog");

            migrationBuilder.DropColumn(
                name: "OrgId",
                table: "AuditLog");

            migrationBuilder.RenameColumn(
                name: "ChangedOn",
                table: "AuditLog",
                newName: "DateTime");

            migrationBuilder.AlterColumn<int>(
                name: "Id",
                table: "AuditLog",
                type: "integer",
                nullable: false,
                oldClrType: typeof(Guid),
                oldType: "uuid")
                .Annotation("Npgsql:ValueGenerationStrategy", NpgsqlValueGenerationStrategy.IdentityByDefaultColumn);

            migrationBuilder.AddColumn<string>(
                name: "ColumnName",
                table: "AuditLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "TableName",
                table: "AuditLog",
                type: "text",
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "Username",
                table: "AuditLog",
                type: "text",
                nullable: false,
                defaultValue: "");
        }
    }
}
