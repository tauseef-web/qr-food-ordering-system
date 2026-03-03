using Microsoft.EntityFrameworkCore;
using ScanAndSavor.Models;

namespace ScanAndSavor.Data;

public class ApplicationDbContext : DbContext
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);
    }

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<CafeTable> Tables => Set<CafeTable>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
}