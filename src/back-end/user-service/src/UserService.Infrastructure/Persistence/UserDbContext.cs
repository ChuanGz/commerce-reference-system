namespace UserService.Infrastructure.Persistence;

public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
{
    public DbSet<User> Users => Set<User>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        var seedUsers = new[]
        {
            new User { Id = new Guid("00000000-0000-0000-0000-000000000001"), Name = "User Test 001", Email = "user001@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000002"), Name = "User Test 002", Email = "user002@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000003"), Name = "User Test 003", Email = "user003@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000004"), Name = "User Test 004", Email = "user004@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000005"), Name = "User Test 005", Email = "user005@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000006"), Name = "User Test 006", Email = "user006@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000007"), Name = "User Test 007", Email = "user007@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000008"), Name = "User Test 008", Email = "user008@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000009"), Name = "User Test 009", Email = "user009@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000010"), Name = "User Test 010", Email = "user010@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000011"), Name = "User Test 011", Email = "user011@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000012"), Name = "User Test 012", Email = "user012@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000013"), Name = "User Test 013", Email = "user013@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000014"), Name = "User Test 014", Email = "user014@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000015"), Name = "User Test 015", Email = "user015@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000016"), Name = "User Test 016", Email = "user016@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000017"), Name = "User Test 017", Email = "user017@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000018"), Name = "User Test 018", Email = "user018@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000019"), Name = "User Test 019", Email = "user019@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000020"), Name = "User Test 020", Email = "user020@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000021"), Name = "User Test 021", Email = "user021@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000022"), Name = "User Test 022", Email = "user022@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000023"), Name = "User Test 023", Email = "user023@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000024"), Name = "User Test 024", Email = "user024@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000025"), Name = "User Test 025", Email = "user025@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000026"), Name = "User Test 026", Email = "user026@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000027"), Name = "User Test 027", Email = "user027@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000028"), Name = "User Test 028", Email = "user028@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000029"), Name = "User Test 029", Email = "user029@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000030"), Name = "User Test 030", Email = "user030@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000031"), Name = "User Test 031", Email = "user031@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000032"), Name = "User Test 032", Email = "user032@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000033"), Name = "User Test 033", Email = "user033@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000034"), Name = "User Test 034", Email = "user034@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000035"), Name = "User Test 035", Email = "user035@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000036"), Name = "User Test 036", Email = "user036@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000037"), Name = "User Test 037", Email = "user037@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000038"), Name = "User Test 038", Email = "user038@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000039"), Name = "User Test 039", Email = "user039@example.com" },
            new User { Id = new Guid("00000000-0000-0000-0000-000000000040"), Name = "User Test 040", Email = "user040@example.com" }
        };

        modelBuilder.Entity<User>().HasData(seedUsers);
    }
}
