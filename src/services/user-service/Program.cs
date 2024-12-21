var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();
app.MapGet("/", () => "User service is running...");
app.Run();