using Microsoft.AspNetCore.Builder;

namespace OrderServiceApp
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var app = builder.Build();
            app.MapGet("/", () => "Order Service is running...");
            app.Run();
        }
    }
}
