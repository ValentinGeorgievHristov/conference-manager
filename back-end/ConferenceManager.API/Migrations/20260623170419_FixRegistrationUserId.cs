using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceManager.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRegistrationUserId : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UsertId",
                table: "Registrations",
                newName: "UserId");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "UserId",
                table: "Registrations",
                newName: "UsertId");
        }
    }
}
