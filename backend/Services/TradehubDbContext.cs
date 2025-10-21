namespace Backend.Services;
using Backend.Models;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
using MongoDB.EntityFrameworkCore.Extensions;

public class TradehubDbContext : DbContext
{
    public DbSet<User> Users { get; set; } = null!;
    public DbSet<Listing> Listings { get; set; } = null!;

    public TradehubDbContext(DbContextOptions options) : base(options) { }

    public static TradehubDbContext Create(IMongoDatabase database) =>
        new(new DbContextOptionsBuilder<TradehubDbContext>()
            .UseMongoDB(database.Client, database.DatabaseNamespace.DatabaseName)
            .Options);

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<User>().ToCollection("users");
        modelBuilder.Entity<Listing>().ToCollection("listings");
    }
}
