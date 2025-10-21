using Backend.Services;
using MongoDB.Driver;
using DotNetEnv;
using MongoDB.EntityFrameworkCore.Extensions;

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("MONGODB");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB' env variable");
    Environment.Exit(0);
}

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();

var app = builder.Build();
var client = new MongoClient(connectionString);
var db = TradehubDbContext.Create(client.GetDatabase("tradehub"));
    
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseHttpsRedirection();

app.Run();
