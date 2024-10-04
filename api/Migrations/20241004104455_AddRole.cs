using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace api.Migrations
{
    /// <inheritdoc />
    public partial class AddRole : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "AspNetRoles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { "21d03761-8a11-4169-bfce-64a23337aa75", null, "Admin", "ADMIN" },
                    { "27a2392c-4cc4-4f3b-b239-76fcdaf492b5", null, "Mentor", "MENTOR" },
                    { "a5a53e67-146b-4c63-aec8-e654a5fd4937", null, "Student", "STUDENT" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "21d03761-8a11-4169-bfce-64a23337aa75");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "27a2392c-4cc4-4f3b-b239-76fcdaf492b5");

            migrationBuilder.DeleteData(
                table: "AspNetRoles",
                keyColumn: "Id",
                keyValue: "a5a53e67-146b-4c63-aec8-e654a5fd4937");
        }
    }
}
