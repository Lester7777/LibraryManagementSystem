using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

namespace LibraryManagement.Migrations
{
    /// <inheritdoc />
    public partial class SeedAdminUser : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Members",
                columns: new[] { "Id", "Email", "FirstName", "IsAdmin", "JoinedDate", "LastName", "PasswordHash", "PasswordSalt", "PhoneNumber" },
                values: new object[] { 999, "admin@library.com", "Admin", true, new DateTime(2026, 5, 15, 8, 40, 13, 60, DateTimeKind.Utc).AddTicks(2800), "User", "1ujM17+eLlW6kT3IfCDejSL3c4CWrY824GROLTEg66o=", "2bD8EDuBsMq6Uw6bgbHNOA==", null });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Members",
                keyColumn: "Id",
                keyValue: 999);
        }
    }
}
