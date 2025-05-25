using Microsoft.EntityFrameworkCore.Migrations;

#nullable disable

#pragma warning disable CA1814 // Prefer jagged arrays over multidimensional

namespace UserService.Infrastructure.Migrations
{
    /// <inheritdoc />
    public partial class InitialCreate : Migration
    {
        /// <inheritdoc />
        protected override void Up(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.CreateTable(
                name: "Users",
                columns: table =>
                    new
                    {
                        Id = table.Column<Guid>(type: "uniqueidentifier", nullable: false),
                        Name = table.Column<string>(type: "nvarchar(max)", nullable: false),
                        Email = table.Column<string>(type: "nvarchar(max)", nullable: false)
                    },
                constraints: table =>
                {
                    table.PrimaryKey("PK_Users", x => x.Id);
                }
            );

            migrationBuilder.InsertData(
                table: "Users",
                columns: new[] { "Id", "Email", "Name" },
                values: new object[,]
                {
                    {
                        new Guid("00000000-0000-0000-0000-000000000001"),
                        "user001@example.com",
                        "User Test 001"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000002"),
                        "user002@example.com",
                        "User Test 002"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000003"),
                        "user003@example.com",
                        "User Test 003"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000004"),
                        "user004@example.com",
                        "User Test 004"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000005"),
                        "user005@example.com",
                        "User Test 005"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000006"),
                        "user006@example.com",
                        "User Test 006"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000007"),
                        "user007@example.com",
                        "User Test 007"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000008"),
                        "user008@example.com",
                        "User Test 008"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000009"),
                        "user009@example.com",
                        "User Test 009"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000010"),
                        "user010@example.com",
                        "User Test 010"
                    },

                }
            );
        }

        /// <inheritdoc />
        protected override void Down(MigrationBuilder migrationBuilder)
        {
            migrationBuilder.DropTable(name: "Users");
        }
    }
}
