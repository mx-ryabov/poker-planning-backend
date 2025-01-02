using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class GenerationTicketsIdentifier : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "Tickets",
                type: "varchar(50)",
                nullable: false,
                defaultValue: "",
                oldClrType: typeof(string),
                oldType: "varchar(50)",
                oldNullable: true);

            migrationBuilder.AddColumn<int>(
                name: "TicketsSequenceNumber",
                table: "Games",
                type: "integer",
                nullable: false,
                defaultValue: 1);

            /*
            - function generateGameHash(name)
                - takes first letters of all the words from the name
                - put them together into one word
                - capitalize the result
                - return the result
            - loop through all the games
                - set TicketsSequenceNumber equal to the tickets count
                - loop through all the game's tickets
                    - set Identifier equal to '${generateGameHash(gameName)}-${rowIndex}'
            */

            migrationBuilder.Sql(@"
                CREATE OR REPLACE FUNCTION generate_hash_name(game_name TEXT)
                RETURNS TEXT AS $$
                DECLARE
                    words TEXT[];
                    initials TEXT := '';
                BEGIN
                    words := string_to_array(game_name, ' ');

                    FOR i IN array_lower(words, 1)..array_upper(words, 1) LOOP
                        initials := initials || substr(words[i], 1, 1);
                    END LOOP;

                    RETURN initials;
                END;
                $$ LANGUAGE plpgsql;

                DO $$
                DECLARE 
                    ticketsCount INTEGER;
                    game RECORD;
                    ticket RECORD;
                BEGIN
                    FOR game IN (SELECT ""Id"", ""Name"" FROM ""Games"") LOOP
                        SELECT COUNT(*) INTO ticketsCount FROM ""Tickets"" WHERE ""GameId"" = game.""Id"";
                        UPDATE ""Games"" SET ""TicketsSequenceNumber"" = ticketsCount WHERE ""Id"" = game.""Id"";
                        
                        UPDATE ""Tickets"" 
                        SET ""Identifier"" = generate_hash_name(game.""Name"") || '-' || row_number
                        FROM (
                            SELECT ""Id"", ROW_NUMBER() OVER (ORDER BY ""Id"") AS row_number
                            FROM ""Tickets""
                            WHERE ""GameId"" = game.""Id""
                        ) as ticket_seq
                        WHERE ""Tickets"".""Id"" = ticket_seq.""Id"";
                    END LOOP;
                END;
                $$ LANGUAGE plpgsql;
            ");
        }

        /// <inheritdoc />s
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropColumn(
                name: "TicketsSequenceNumber",
                table: "Games");

            migrationBuilder.AlterColumn<string>(
                name: "Identifier",
                table: "Tickets",
                type: "varchar(50)",
                nullable: true,
                oldClrType: typeof(string),
                oldType: "varchar(50)");
        }
    }
}
