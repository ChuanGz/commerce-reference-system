using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.EntityFrameworkCore;

using MediatR;

using UserService.Application.Handlers;
using UserService.Domain.Repositories;
using UserService.Infrastructure.Persistence;
using UserService.Infrastructure.Repositories;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddHealthChecks();
Console.WriteLine("UserService API is starting...");

builder.Services.AddDbContext<UserDbContext>(
    opt => opt.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"))
);
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddMediatR(typeof(CreateUserCommandHandler));

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.MapHealthChecks("/health");

app.Run();
