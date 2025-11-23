using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class adminslevels : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Villages");

            migrationBuilder.DropTable(
                name: "Wards");

            migrationBuilder.DropTable(
                name: "SubCounties");

            migrationBuilder.DropTable(
                name: "Counties");

            migrationBuilder.DropColumn(
                name: "AdminLevel1",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel2",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel3",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel4",
                table: "Farmers");

            migrationBuilder.AddColumn<Guid>(
                name: "AdminLevel1Id",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdminLevel2Id",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdminLevel3Id",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            migrationBuilder.AddColumn<Guid>(
                name: "AdminLevel4Id",
                table: "Farmers",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"));

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "AdminLevel4",
            //    table: "Addresses",
            //    type: "uuid",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "AdminLevel3",
            //    table: "Addresses",
            //    type: "uuid",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "AdminLevel2",
            //    table: "Addresses",
            //    type: "uuid",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "AdminLevel1",
            //    table: "Addresses",
            //    type: "uuid",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            //migrationBuilder.AlterColumn<Guid>(
            //    name: "AdminLevel0",
            //    table: "Addresses",
            //    type: "uuid",
            //    nullable: false,
            //    defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
            //    oldClrType: typeof(string),
            //    oldType: "text",
            //    oldNullable: true);

            migrationBuilder.CreateTable(
                name: "AdminLevel1",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountyName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    CountyCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevel1", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLevel1_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminLevel2",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubCountyName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    SubCountyCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CountyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevel2", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLevel2_AdminLevel1_CountyId",
                        column: x => x.CountyId,
                        principalTable: "AdminLevel1",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminLevel3",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WardName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    WardCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SubCountyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevel3", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLevel3_AdminLevel2_SubCountyId",
                        column: x => x.SubCountyId,
                        principalTable: "AdminLevel2",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "AdminLevel4",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VillageName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    VillageCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    WardId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_AdminLevel4", x => x.Id);
                    table.ForeignKey(
                        name: "FK_AdminLevel4_AdminLevel3_WardId",
                        column: x => x.WardId,
                        principalTable: "AdminLevel3",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel1Id",
                table: "Farmers",
                column: "AdminLevel1Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel2Id",
                table: "Farmers",
                column: "AdminLevel2Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel3Id",
                table: "Farmers",
                column: "AdminLevel3Id");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers",
                column: "AdminLevel4Id");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLevel1_CountryId",
                table: "AdminLevel1",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLevel2_CountyId",
                table: "AdminLevel2",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLevel3_SubCountyId",
                table: "AdminLevel3",
                column: "SubCountyId");

            migrationBuilder.CreateIndex(
                name: "IX_AdminLevel4_WardId",
                table: "AdminLevel4",
                column: "WardId");

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel1_AdminLevel1Id",
                table: "Farmers",
                column: "AdminLevel1Id",
                principalTable: "AdminLevel1",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel2_AdminLevel2Id",
                table: "Farmers",
                column: "AdminLevel2Id",
                principalTable: "AdminLevel2",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel3_AdminLevel3Id",
                table: "Farmers",
                column: "AdminLevel3Id",
                principalTable: "AdminLevel3",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);

            migrationBuilder.AddForeignKey(
                name: "FK_Farmers_AdminLevel4_AdminLevel4Id",
                table: "Farmers",
                column: "AdminLevel4Id",
                principalTable: "AdminLevel4",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel1_AdminLevel1Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel2_AdminLevel2Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel3_AdminLevel3Id",
                table: "Farmers");

            migrationBuilder.DropForeignKey(
                name: "FK_Farmers_AdminLevel4_AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropTable(
                name: "AdminLevel4");

            migrationBuilder.DropTable(
                name: "AdminLevel3");

            migrationBuilder.DropTable(
                name: "AdminLevel2");

            migrationBuilder.DropTable(
                name: "AdminLevel1");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel1Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel2Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel3Id",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel1Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel2Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel3Id",
                table: "Farmers");

            migrationBuilder.DropColumn(
                name: "AdminLevel4Id",
                table: "Farmers");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel1",
                table: "Farmers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel2",
                table: "Farmers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel3",
                table: "Farmers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AddColumn<string>(
                name: "AdminLevel4",
                table: "Farmers",
                type: "character varying(100)",
                maxLength: 100,
                nullable: false,
                defaultValue: "");

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel4",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel3",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel2",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel1",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AlterColumn<string>(
                name: "AdminLevel0",
                table: "Addresses",
                type: "text",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.CreateTable(
                name: "Counties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountryId = table.Column<Guid>(type: "uuid", nullable: false),
                    CountyCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    CountyName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Counties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Counties_Countries_CountryId",
                        column: x => x.CountryId,
                        principalTable: "Countries",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "SubCounties",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    CountyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    SubCountyCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    SubCountyName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_SubCounties", x => x.Id);
                    table.ForeignKey(
                        name: "FK_SubCounties_Counties_CountyId",
                        column: x => x.CountyId,
                        principalTable: "Counties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Wards",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    SubCountyId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    WardCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    WardName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Wards", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Wards_SubCounties_SubCountyId",
                        column: x => x.SubCountyId,
                        principalTable: "SubCounties",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Villages",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    WardId = table.Column<Guid>(type: "uuid", nullable: false),
                    IsActive = table.Column<bool>(type: "boolean", nullable: false),
                    VillageCode = table.Column<string>(type: "character varying(5)", maxLength: 5, nullable: false),
                    VillageName = table.Column<string>(type: "character varying(126)", maxLength: 126, nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Villages", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Villages_Wards_WardId",
                        column: x => x.WardId,
                        principalTable: "Wards",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Counties_CountryId",
                table: "Counties",
                column: "CountryId");

            migrationBuilder.CreateIndex(
                name: "IX_SubCounties_CountyId",
                table: "SubCounties",
                column: "CountyId");

            migrationBuilder.CreateIndex(
                name: "IX_Villages_WardId",
                table: "Villages",
                column: "WardId");

            migrationBuilder.CreateIndex(
                name: "IX_Wards_SubCountyId",
                table: "Wards",
                column: "SubCountyId");
        }
    }
}
