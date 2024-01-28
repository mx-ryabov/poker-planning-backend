using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_Games_Participants_Tickets_VotingResults : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Games",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Name = table.Column<string>(type: "varchar(255)", nullable: false),
                    Link = table.Column<string>(type: "varchar(255)", nullable: false),
                    Settings_IsAutoRevealCards = table.Column<bool>(type: "boolean", nullable: false),
                    VotingProcess_TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    VotingProcess_IsActive = table.Column<bool>(type: "boolean", nullable: false, defaultValue: false),
                    VotingSystemId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Games", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Games_VotingSystems_VotingSystemId",
                        column: x => x.VotingSystemId,
                        principalTable: "VotingSystems",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "Participants",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    DisplayName = table.Column<string>(type: "varchar(255)", nullable: false),
                    VotingSystemVoteId = table.Column<Guid>(type: "uuid", nullable: true),
                    UserId = table.Column<Guid>(type: "uuid", nullable: true),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    Role = table.Column<string>(type: "varchar(100)", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Participants", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Participants_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_Participants_Users_UserId",
                        column: x => x.UserId,
                        principalTable: "Users",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_Participants_VotingSystemVotes_VotingSystemVoteId",
                        column: x => x.VotingSystemVoteId,
                        principalTable: "VotingSystemVotes",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "Tickets",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    Title = table.Column<string>(type: "varchar(255)", nullable: false),
                    Description = table.Column<string>(type: "text", nullable: false, defaultValue: ""),
                    Link = table.Column<string>(type: "varchar(500)", nullable: true),
                    Type = table.Column<string>(type: "varchar(100)", nullable: true),
                    Identifier = table.Column<string>(type: "varchar(50)", nullable: true),
                    Estimation = table.Column<string>(type: "varchar(50)", nullable: true),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Tickets", x => x.Id);
                    table.ForeignKey(
                        name: "FK_Tickets_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateTable(
                name: "VotingResults",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    GameId = table.Column<Guid>(type: "uuid", nullable: false),
                    TicketId = table.Column<Guid>(type: "uuid", nullable: true),
                    CreatedAt = table.Column<DateTime>(type: "timestamp with time zone", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingResults", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotingResults_Games_GameId",
                        column: x => x.GameId,
                        principalTable: "Games",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VotingResults_Tickets_TicketId",
                        column: x => x.TicketId,
                        principalTable: "Tickets",
                        principalColumn: "Id");
                });

            migrationBuilder.CreateTable(
                name: "VotingResultVotes",
                columns: table => new
                {
                    Id = table.Column<Guid>(type: "uuid", nullable: false),
                    VoteId = table.Column<Guid>(type: "uuid", nullable: false),
                    ParticipantId = table.Column<Guid>(type: "uuid", nullable: false),
                    VotingResultId = table.Column<Guid>(type: "uuid", nullable: true)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_VotingResultVotes", x => x.Id);
                    table.ForeignKey(
                        name: "FK_VotingResultVotes_Participants_ParticipantId",
                        column: x => x.ParticipantId,
                        principalTable: "Participants",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                    table.ForeignKey(
                        name: "FK_VotingResultVotes_VotingResults_VotingResultId",
                        column: x => x.VotingResultId,
                        principalTable: "VotingResults",
                        principalColumn: "Id");
                    table.ForeignKey(
                        name: "FK_VotingResultVotes_VotingSystemVotes_VoteId",
                        column: x => x.VoteId,
                        principalTable: "VotingSystemVotes",
                        principalColumn: "Id",
                        onDelete: ReferentialAction.Cascade);
                });

            migrationBuilder.CreateIndex(
                name: "IX_Games_VotingProcess_TicketId",
                table: "Games",
                column: "VotingProcess_TicketId",
                unique: true);

            migrationBuilder.CreateIndex(
                name: "IX_Games_VotingSystemId",
                table: "Games",
                column: "VotingSystemId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_GameId",
                table: "Participants",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_UserId",
                table: "Participants",
                column: "UserId");

            migrationBuilder.CreateIndex(
                name: "IX_Participants_VotingSystemVoteId",
                table: "Participants",
                column: "VotingSystemVoteId");

            migrationBuilder.CreateIndex(
                name: "IX_Tickets_GameId",
                table: "Tickets",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingResults_GameId",
                table: "VotingResults",
                column: "GameId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingResults_TicketId",
                table: "VotingResults",
                column: "TicketId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingResultVotes_ParticipantId",
                table: "VotingResultVotes",
                column: "ParticipantId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingResultVotes_VoteId",
                table: "VotingResultVotes",
                column: "VoteId");

            migrationBuilder.CreateIndex(
                name: "IX_VotingResultVotes_VotingResultId",
                table: "VotingResultVotes",
                column: "VotingResultId");

            migrationBuilder.AddForeignKey(
                name: "FK_Games_Tickets_VotingProcess_TicketId",
                table: "Games",
                column: "VotingProcess_TicketId",
                principalTable: "Tickets",
                principalColumn: "Id");
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropForeignKey(
                name: "FK_Games_Tickets_VotingProcess_TicketId",
                table: "Games");

            migrationBuilder.DropTable(
                name: "VotingResultVotes");

            migrationBuilder.DropTable(
                name: "Participants");

            migrationBuilder.DropTable(
                name: "VotingResults");

            migrationBuilder.DropTable(
                name: "Tickets");

            migrationBuilder.DropTable(
                name: "Games");
        }
    }
}
