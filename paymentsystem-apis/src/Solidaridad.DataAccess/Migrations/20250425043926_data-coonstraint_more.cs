using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace Solidaridad.DataAccess.Migrations
{
    /// <inheritdoc />
    public partial class datacoonstraint_more : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateIndex(
                name: "IX_Farmers_BeneficiaryId",
                table: "Farmers",
                column: "BeneficiaryId",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"BeneficiaryId\" IS NOT NULL AND \"BeneficiaryId\" <> ''");

            //migrationBuilder.CreateIndex(
            //    name: "IX_Farmers_Mobile",
            //    table: "Farmers",
            //    column: "Mobile",
            //    unique: true,
            //    filter: "\"IsDeleted\" = false AND \"Mobile\" IS NOT NULL AND \"Mobile\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_ParticipantId",
                table: "Farmers",
                column: "ParticipantId",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"ParticipantId\" IS NOT NULL AND \"ParticipantId\" <> ''");

            migrationBuilder.CreateIndex(
                name: "IX_Farmers_PaymentPhoneNumber",
                table: "Farmers",
                column: "PaymentPhoneNumber",
                unique: true,
                filter: "\"IsDeleted\" = false AND \"PaymentPhoneNumber\" IS NOT NULL AND \"PaymentPhoneNumber\" <> ''");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropIndex(
                name: "IX_Farmers_BeneficiaryId",
                table: "Farmers");

            //migrationBuilder.DropIndex(
            //    name: "IX_Farmers_Mobile",
            //    table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_ParticipantId",
                table: "Farmers");

            migrationBuilder.DropIndex(
                name: "IX_Farmers_PaymentPhoneNumber",
                table: "Farmers");
        }
    }
}
