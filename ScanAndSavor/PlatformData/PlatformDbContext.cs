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
}