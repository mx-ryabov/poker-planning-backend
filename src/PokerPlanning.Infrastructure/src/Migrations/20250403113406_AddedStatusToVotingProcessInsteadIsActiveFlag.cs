using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class AddedStatusToVotingProcessInsteadIsActiveFlag : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingProcess_IsActive",
                table: "Games");

            migrationBuilder.AddColumn<int>(
                name: "VotingProcess_Status",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 0);
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "VotingProcess_Status",
                table: "Games");

            migrationBuilder.AddColumn<bool>(
                name: "VotingProcess_IsActive",
                table: "Games",
                type: "boolean",
                nullable: false,
                defaultValue: false);
        }
    }
}
