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
                    {
                        new Guid("00000000-0000-0000-0000-000000000011"),
                        "user011@example.com",
                        "User Test 011"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000012"),
                        "user012@example.com",
                        "User Test 012"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000013"),
                        "user013@example.com",
                        "User Test 013"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000014"),
                        "user014@example.com",
                        "User Test 014"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000015"),
                        "user015@example.com",
                        "User Test 015"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000016"),
                        "user016@example.com",
                        "User Test 016"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000017"),
                        "user017@example.com",
                        "User Test 017"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000018"),
                        "user018@example.com",
                        "User Test 018"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000019"),
                        "user019@example.com",
                        "User Test 019"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000020"),
                        "user020@example.com",
                        "User Test 020"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000021"),
                        "user021@example.com",
                        "User Test 021"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000022"),
                        "user022@example.com",
                        "User Test 022"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000023"),
                        "user023@example.com",
                        "User Test 023"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000024"),
                        "user024@example.com",
                        "User Test 024"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000025"),
                        "user025@example.com",
                        "User Test 025"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000026"),
                        "user026@example.com",
                        "User Test 026"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000027"),
                        "user027@example.com",
                        "User Test 027"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000028"),
                        "user028@example.com",
                        "User Test 028"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000029"),
                        "user029@example.com",
                        "User Test 029"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000030"),
                        "user030@example.com",
                        "User Test 030"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000031"),
                        "user031@example.com",
                        "User Test 031"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000032"),
                        "user032@example.com",
                        "User Test 032"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000033"),
                        "user033@example.com",
                        "User Test 033"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000034"),
                        "user034@example.com",
                        "User Test 034"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000035"),
                        "user035@example.com",
                        "User Test 035"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000036"),
                        "user036@example.com",
                        "User Test 036"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000037"),
                        "user037@example.com",
                        "User Test 037"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000038"),
                        "user038@example.com",
                        "User Test 038"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000039"),
                        "user039@example.com",
                        "User Test 039"
                    },
                    {
                        new Guid("00000000-0000-0000-0000-000000000040"),
                        "user040@example.com",
                        "User Test 040"
                    }
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
