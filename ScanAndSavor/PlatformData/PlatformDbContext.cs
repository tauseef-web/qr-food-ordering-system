using Microsoft.EntityFrameworkCore;
using ScanAndSavor.PlatformModels;

namespace ScanAndSavor.PlatformData;

public class PlatformDbContext : DbContext
{
    public PlatformDbContext(DbContextOptions<PlatformDbContext> options)
        : base(options)
    {
    }

    public DbSet<Tenant> Tenants => Set<Tenant>();
    public DbSet<TenantSubscription> TenantSubscriptions => Set<TenantSubscription>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.Subdomain)
            .IsUnique();

        modelBuilder.Entity<Tenant>()
            .HasIndex(t => t.CustomDomain)
            .IsUnique()
            .HasFilter("[CustomDomain] <> ''");

        modelBuilder.Entity<TenantSubscription>()
            .Property(s => s.Amount)
            .HasPrecision(18, 2);
    }
}
