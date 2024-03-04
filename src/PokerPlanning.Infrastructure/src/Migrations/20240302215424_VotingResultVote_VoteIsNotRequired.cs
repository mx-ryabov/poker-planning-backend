using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class VotingResultVote_VoteIsNotRequired : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingResultVotes_VotingSystemVotes_VoteId",
                table: "VotingResultVotes");

            migrationBuilder.AlterColumn<Guid>(
                name: "VoteId",
                table: "VotingResultVotes",
                type: "uuid",
                nullable: true,
                oldClrType: typeof(Guid),
                oldType: "uuid");

            migrationBuilder.AddForeignKey(
                name: "FK_VotingResultVotes_VotingSystemVotes_VoteId",
                table: "VotingResultVotes",
                column: "VoteId",
                principalTable: "VotingSystemVotes",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_VotingResultVotes_VotingSystemVotes_VoteId",
                table: "VotingResultVotes");

            migrationBuilder.AlterColumn<Guid>(
                name: "VoteId",
                table: "VotingResultVotes",
                type: "uuid",
                nullable: false,
                defaultValue: new Guid("00000000-0000-0000-0000-000000000000"),
                oldClrType: typeof(Guid),
                oldType: "uuid",
                oldNullable: true);

            migrationBuilder.AddForeignKey(
                name: "FK_VotingResultVotes_VotingSystemVotes_VoteId",
                table: "VotingResultVotes",
                column: "VoteId",
                principalTable: "VotingSystemVotes",
                principalColumn: "Id",
                onDelete: ReferentialAction.Cascade);
        }
    }
}
