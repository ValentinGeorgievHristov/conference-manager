using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace ConferenceManager.API.Migrations
{
    /// <inheritdoc />
    public partial class FixRegistrationModel : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "id",
                table: "Registrations",
                newName: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.RenameColumn(
                name: "Id",
                table: "Registrations",
                newName: "id");
        }
    }
}
