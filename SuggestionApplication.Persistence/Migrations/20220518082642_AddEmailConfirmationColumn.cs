using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace SuggestionApplication.Persistence.Migrations
{
    public partial class AddEmailConfirmationColumn : Migration
    {
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "EmailIsConfirmed",
                table: "Users",
                type: "bit",
                nullable: false,
                defaultValue: false);
        }

        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "EmailIsConfirmed",
                table: "Users");
        }
    }
}
