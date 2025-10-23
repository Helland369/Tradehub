using Backend.Services;
using DotNetEnv;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

Env.Load();

var connectionString = Environment.GetEnvironmentVariable("MONGODB");
if (connectionString == null)
{
    Console.WriteLine("You must set your 'MONGODB' env variable");
    Environment.Exit(0);
}

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

builder.Services.AddCors(options =>
{
    options.AddPolicy("FrontendDev", p =>
        p.WithOrigins("http://localhost:5173")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials());
});

builder.Services.AddDbContext<TradehubDbContext>(opt =>
opt.UseMongoDB(connectionString, databaseName: "tradehub"));

builder.Services.AddSingleton<PasswordHasher>();

var app = builder.Build();

app.UseCors("FrontendDev");
app.UseHttpsRedirection();
app.MapControllers();
app.Run();
