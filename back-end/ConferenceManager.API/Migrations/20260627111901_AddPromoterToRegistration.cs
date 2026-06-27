using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceManager.API.Migrations
{
    /// <inheritdoc />
    public partial class AddPromoterToRegistration : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<int>(
                name: "PromoterProfileId",
                table: "Registrations",
                type: "integer",
                nullable: true);

            migrationBuilder.CreateIndex(
                name: "IX_Registrations_PromoterProfileId",
                table: "Registrations",
                column: "PromoterProfileId");

            migrationBuilder.AddForeignKey(
                name: "FK_Registrations_PromoterProfiles_PromoterProfileId",
                table: "Registrations",
                column: "PromoterProfileId",
                principalTable: "PromoterProfiles",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Registrations_PromoterProfiles_PromoterProfileId",
                table: "Registrations");

            migrationBuilder.DropIndex(
                name: "IX_Registrations_PromoterProfileId",
                table: "Registrations");

            migrationBuilder.DropColumn(
                name: "PromoterProfileId",
                table: "Registrations");
        }
    }
}
