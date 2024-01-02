using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_VoteId_For_Participant : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_VotingSystemVotes_VotingSystemVoteId",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "VotingSystemVoteId",
                table: "Participants",
                newName: "VoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_VotingSystemVoteId",
                table: "Participants",
                newName: "IX_Participants_VoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_VotingSystemVotes_VoteId",
                table: "Participants",
                column: "VoteId",
                principalTable: "VotingSystemVotes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Participants_VotingSystemVotes_VoteId",
                table: "Participants");

            migrationBuilder.RenameColumn(
                name: "VoteId",
                table: "Participants",
                newName: "VotingSystemVoteId");

            migrationBuilder.RenameIndex(
                name: "IX_Participants_VoteId",
                table: "Participants",
                newName: "IX_Participants_VotingSystemVoteId");

            migrationBuilder.AddForeignKey(
                name: "FK_Participants_VotingSystemVotes_VotingSystemVoteId",
                table: "Participants",
                column: "VotingSystemVoteId",
                principalTable: "VotingSystemVotes",
                principalColumn: "Id");
        }
    }
}
