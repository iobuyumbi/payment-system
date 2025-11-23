using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class locationProfileFields : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_LocationProfile_Location_LocationId",
                table: "LocationProfile");

            migrationBuilder.DropIndex(
                name: "IX_LocationProfile_LocationId",
                table: "LocationProfile");

            migrationBuilder.RenameColumn(
                name: "LocationId",
                table: "LocationProfile",
                newName: "CountryId");

            migrationBuilder.AddColumn<string>(
                name: "AlternateNumber",
                table: "LocationProfile",
                type: "character varying(20)",
                maxLength: 20,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "AttachmentFileId",
                table: "LocationProfile",
                type: "uuid",
                nullable: true);

            migrationBuilder.AddColumn<string>(
                name: "Website",
                table: "LocationProfile",
                type: "character varying(256)",
                maxLength: 256,
                nullable: true);

            migrationBuilder.AddColumn<Guid>(
                name: "LocationProfileId",
                table: "Location",
                type: "uuid",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_LocationProfile_AttachmentFileId",
                table: "LocationProfile",
                column: "AttachmentFileId");

            migrationBuilder.CreateIndex(
                name: "IX_Location_LocationProfileId",
                table: "Location",
                column: "LocationProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Location_LocationProfile_LocationProfileId",
                table: "Location",
                column: "LocationProfileId",
                principalTable: "LocationProfile",
                principalColumn: "Id");

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProfile_AttachmentFile_AttachmentFileId",
                table: "LocationProfile",
                column: "AttachmentFileId",
                principalTable: "AttachmentFile",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Location_LocationProfile_LocationProfileId",
                table: "Location");

            migrationBuilder.DropForeignKey(
                name: "FK_LocationProfile_AttachmentFile_AttachmentFileId",
                table: "LocationProfile");

            migrationBuilder.DropIndex(
                name: "IX_LocationProfile_AttachmentFileId",
                table: "LocationProfile");

            migrationBuilder.DropIndex(
                name: "IX_Location_LocationProfileId",
                table: "Location");

            migrationBuilder.DropColumn(
                name: "AlternateNumber",
                table: "LocationProfile");

            migrationBuilder.DropColumn(
                name: "AttachmentFileId",
                table: "LocationProfile");

            migrationBuilder.DropColumn(
                name: "Website",
                table: "LocationProfile");

            migrationBuilder.DropColumn(
                name: "LocationProfileId",
                table: "Location");

            migrationBuilder.RenameColumn(
                name: "CountryId",
                table: "LocationProfile",
                newName: "LocationId");

            migrationBuilder.CreateIndex(
                name: "IX_LocationProfile_LocationId",
                table: "LocationProfile",
                column: "LocationId",
                unique: true);

            migrationBuilder.AddForeignKey(
                name: "FK_LocationProfile_Location_LocationId",
                table: "LocationProfile",
                column: "LocationId",
                principalTable: "Location",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
