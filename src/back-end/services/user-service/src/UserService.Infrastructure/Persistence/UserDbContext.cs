namespace UserService.Infrastructure.Persistence
{
    public class UserDbContext(DbContextOptions<UserDbContext> options) : DbContext(options)
    {
        public DbSet<User> Users => Set<User>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            ArgumentNullException.ThrowIfNull(modelBuilder, nameof(modelBuilder));
            base.OnModelCreating(modelBuilder);

            var seedUsers = new[]
            {
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000001"),
                    Name = "User Test 001",
                    Email = "user001@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000002"),
                    Name = "User Test 002",
                    Email = "user002@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000003"),
                    Name = "User Test 003",
                    Email = "user003@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000004"),
                    Name = "User Test 004",
                    Email = "user004@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000005"),
                    Name = "User Test 005",
                    Email = "user005@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000006"),
                    Name = "User Test 006",
                    Email = "user006@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000007"),
                    Name = "User Test 007",
                    Email = "user007@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000008"),
                    Name = "User Test 008",
                    Email = "user008@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000009"),
                    Name = "User Test 009",
                    Email = "user009@example.com",
                },
                new User
                {
                    Id = new Guid("00000000-0000-0000-0000-000000000010"),
                    Name = "User Test 010",
                    Email = "user010@example.com",
                },
            };

            modelBuilder.Entity<User>().HasData(seedUsers);
        }
    }
}
