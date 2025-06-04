using Microsoft.EntityFrameworkCore.Infrastructure;
using UserService.Infrastructure.Persistence;

#nullable disable

namespace UserService.Infrastructure.Migrations
{
    [DbContext(typeof(UserDbContext))]
    partial class UserDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.5")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity(
                "UserService.Domain.Entities.User",
                b =>
                {
                    b.Property<Guid>("Id").ValueGeneratedOnAdd().HasColumnType("uniqueidentifier");

                    b.Property<string>("Email").IsRequired().HasColumnType("nvarchar(max)");

                    b.Property<string>("Name").IsRequired().HasColumnType("nvarchar(max)");

                    b.HasKey("Id");

                    b.ToTable("Users");

                    b.HasData(
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000001"),
                            Email = "user001@example.com",
                            Name = "User Test 001",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000002"),
                            Email = "user002@example.com",
                            Name = "User Test 002",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000003"),
                            Email = "user003@example.com",
                            Name = "User Test 003",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000004"),
                            Email = "user004@example.com",
                            Name = "User Test 004",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000005"),
                            Email = "user005@example.com",
                            Name = "User Test 005",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000006"),
                            Email = "user006@example.com",
                            Name = "User Test 006",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000007"),
                            Email = "user007@example.com",
                            Name = "User Test 007",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000008"),
                            Email = "user008@example.com",
                            Name = "User Test 008",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000009"),
                            Email = "user009@example.com",
                            Name = "User Test 009",
                        },
                        new
                        {
                            Id = new Guid("00000000-0000-0000-0000-000000000010"),
                            Email = "user010@example.com",
                            Name = "User Test 010",
                        }
                    );
                }
            );
#pragma warning restore 612, 618
        }
    }
}
