using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace PokerPlanning.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class Add_VotingSystem_DefaultData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "VotingSystems",
                columns: new[] { "Id", "CreatorId", "Name" },
                values: new object[,]
                {
                    { new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c"), null, "Fibonacci" },
                    { new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480"), null, "T-shirts" }
                });

            migrationBuilder.InsertData(
                table: "VotingSystemVotes",
                columns: new[] { "Id", "Order", "Suit", "Value", "VotingSystemId" },
                values: new object[,]
                {
                    { new Guid("07746553-c20a-4337-9fea-d37d9a473e78"), 6m, "☠️", "13", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("0bc7aa91-6a87-4386-994d-105fb13b6d98"), 6m, "☠️", "xxl", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("28e4cc55-2acf-4138-b781-8ae4da22582a"), 7m, "🤡", "?", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("2c60634e-02ae-4b06-9894-c4f928d3037b"), 7m, "🤡", "?", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("3c25140f-c8fb-409f-b54d-04835c143319"), 3m, "🤔", "m", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("46c84b9b-a443-4ac3-8550-02d63c57c6d7"), 5m, "😵", "xl", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("46d6301b-4fc9-4e78-a797-891592197821"), 2m, "🚀", "s", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("5addfe74-4e6d-4dde-8c1b-01b856a57b2a"), 1m, "⚡", "1", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("66de38f8-1e99-4e39-9eef-ceeff15dd938"), 0m, "🏖️", "xss", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("8d23dd52-a81e-41b5-a9ac-2df12cd6d667"), 5m, "😵", "8", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("9875f3f1-cbb0-40ee-b649-de48b706b7ba"), 4m, "😬", "5", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("a05dcc39-4781-4407-9255-94d8cc847657"), 3m, "🤔", "3", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("bf0d4051-84a7-4162-8f48-580d4e488df2"), 0m, "🏖️", "0", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") },
                    { new Guid("c5a7d8f5-432a-447a-81f6-9870e07e237a"), 1m, "⚡", "xs", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("ee166192-1c6d-4e8a-81d2-b111a0ec0a6b"), 4m, "😬", "l", new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480") },
                    { new Guid("f1f6b021-7f60-4b21-b297-6be2b93f0cb9"), 2m, "🚀", "2", new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c") }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("07746553-c20a-4337-9fea-d37d9a473e78"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("0bc7aa91-6a87-4386-994d-105fb13b6d98"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("28e4cc55-2acf-4138-b781-8ae4da22582a"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("2c60634e-02ae-4b06-9894-c4f928d3037b"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("3c25140f-c8fb-409f-b54d-04835c143319"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("46c84b9b-a443-4ac3-8550-02d63c57c6d7"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("46d6301b-4fc9-4e78-a797-891592197821"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("5addfe74-4e6d-4dde-8c1b-01b856a57b2a"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("66de38f8-1e99-4e39-9eef-ceeff15dd938"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("8d23dd52-a81e-41b5-a9ac-2df12cd6d667"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("9875f3f1-cbb0-40ee-b649-de48b706b7ba"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("a05dcc39-4781-4407-9255-94d8cc847657"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("bf0d4051-84a7-4162-8f48-580d4e488df2"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("c5a7d8f5-432a-447a-81f6-9870e07e237a"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("ee166192-1c6d-4e8a-81d2-b111a0ec0a6b"));

            migrationBuilder.DeleteData(
                table: "VotingSystemVotes",
                keyColumn: "Id",
                keyValue: new Guid("f1f6b021-7f60-4b21-b297-6be2b93f0cb9"));

            migrationBuilder.DeleteData(
                table: "VotingSystems",
                keyColumn: "Id",
                keyValue: new Guid("6a113d25-34c9-4b49-985c-2df6dd67650c"));

            migrationBuilder.DeleteData(
                table: "VotingSystems",
                keyColumn: "Id",
                keyValue: new Guid("8eccd01f-53e5-4e6f-8553-64c81448a480"));
        }
    }
}
