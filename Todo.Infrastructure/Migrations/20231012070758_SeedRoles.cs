using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace Todo.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class SeedRoles : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.InsertData(
                table: "Roles",
                columns: new[] { "Id", "ConcurrencyStamp", "Name", "NormalizedName" },
                values: new object[,]
                {
                    { new Guid("3f1aea69-0556-46c7-bd21-4a56e395fadb"), "b8e674d7-291d-4af5-8991-49d56f35a49e", "Admin", "ADMIN" },
                    { new Guid("ec92f867-82c1-4aaa-89f3-a34f30f36a29"), "f2323c60-50e0-4afa-80e2-0c81eb6fcaed", "User", "USER" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("3f1aea69-0556-46c7-bd21-4a56e395fadb"));

            migrationBuilder.DeleteData(
                table: "Roles",
                keyColumn: "Id",
                keyValue: new Guid("ec92f867-82c1-4aaa-89f3-a34f30f36a29"));
        }
    }
}
