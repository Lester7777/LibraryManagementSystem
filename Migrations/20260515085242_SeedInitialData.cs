using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class SeedInitialData : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Authors",
                columns: new[] { "Id", "Biography", "Name" },
                values: new object[,]
                {
                    { 1, "British author best known for Harry Potter series", "J.K. Rowling" },
                    { 2, "English novelist and essayist", "George Orwell" },
                    { 3, "American novelist known for To Kill a Mockingbird", "Harper Lee" },
                    { 4, "English writer and philologist", "J.R.R. Tolkien" }
                });

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "JoinedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 5, 15, 8, 52, 41, 882, DateTimeKind.Utc).AddTicks(217), "tsNaxjQv0P+StXyBQCCkzwydK2aIf8WimAJ0raTuPyA=", "W3m1bE893YEUKmdAS0f41Q==" });

            migrationBuilder.InsertData(
                table: "Books",
                columns: new[] { "Id", "AuthorId", "AvailableCopies", "Genre", "ISBN", "Title", "TotalCopies" },
                values: new object[,]
                {
                    { 1, 1, 5, "Fantasy", "978-0747532699", "Harry Potter and the Philosopher's Stone", 5 },
                    { 2, 2, 3, "Dystopian", "978-0451524935", "1984", 3 },
                    { 3, 3, 4, "Fiction", "978-0061120084", "To Kill a Mockingbird", 4 },
                    { 4, 4, 3, "Fantasy", "978-0547928227", "The Hobbit", 3 }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Books",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 1);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 2);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 3);

            migrationBuilder.DeleteData(
                table: "Authors",
                keyColumn: "Id",
                keyValue: 4);

            migrationBuilder.UpdateData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 999,
                columns: new[] { "JoinedDate", "PasswordHash", "PasswordSalt" },
                values: new object[] { new DateTime(2026, 5, 15, 8, 40, 13, 60, DateTimeKind.Utc).AddTicks(2800), "1ujM17+eLlW6kT3IfCDejSL3c4CWrY824GROLTEg66o=", "2bD8EDuBsMq6Uw6bgbHNOA==" });
        }
    }
}
