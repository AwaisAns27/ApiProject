using System;
using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace ApiRevision.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Students",
                columns: table => new
                {
                    Id = table.Column<int>(type: "int", nullable: false)
                        .Annotation("SqlServer:Identity", "1, 1"),
                    GrNo = table.Column<int>(type: "int", nullable: false),
                    Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    Stream = table.Column<string>(type: "nvarchar(max)", nullable: false),
                    DateOfAdmission = table.Column<DateTime>(type: "datetime2", nullable: false)
                },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Students", x => x.Id);
                });

            migrationBuilder.InsertData(
                table: "Students",
                columns: new[] { "Id", "DateOfAdmission", "GrNo", "Name", "Stream" },
                values: new object[,]
                {
                    { 1, new DateTime(2020, 8, 24, 0, 0, 0, 0, DateTimeKind.Unspecified), 234, "Awais Ansari", "Maths" },
                    { 2, new DateTime(2021, 3, 1, 0, 0, 0, 0, DateTimeKind.Unspecified), 454, "Sadain Ansari", "Commerce" },
                    { 3, new DateTime(2024, 7, 22, 18, 25, 33, 119, DateTimeKind.Local).AddTicks(1152), 986, "Arish Khan", "Medical" }
                });
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(
                name: "Students");
        }
    }
}
