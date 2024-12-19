using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_OnlineStatus_For_Participant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AddColumn<bool>(
                name: "Online",
                table: "Participants",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "Online",
                table: "Participants");
        }
    }
}
