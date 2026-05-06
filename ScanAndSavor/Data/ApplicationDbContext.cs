using Microsoft.EntityFrameworkCore;
using ScanAndSavor.Models;

namespace ScanAndSavor.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
        : base(options)
    {
    }

    public DbSet<Restaurant> Restaurants => Set<Restaurant>();
    public DbSet<Outlet> Outlets => Set<Outlet>();
    public DbSet<CafeTable> Tables => Set<CafeTable>();
    public DbSet<QrCode> QrCodes => Set<QrCode>();
    public DbSet<MenuCategory> MenuCategories => Set<MenuCategory>();
    public DbSet<MenuItem> MenuItems => Set<MenuItem>();
    public DbSet<Order> Orders => Set<Order>();
    public DbSet<OrderItem> OrderItems => Set<OrderItem>();
    public DbSet<Payment> Payments => Set<Payment>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Outlet>()
            .HasIndex(o => o.Slug)
            .IsUnique();

        modelBuilder.Entity<CafeTable>()
            .HasOne(t => t.Outlet)
            .WithMany(o => o.Tables)
            .HasForeignKey(t => t.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<CafeTable>()
            .HasIndex(t => new { t.OutletId, t.TableNumber })
            .IsUnique()
            .HasFilter("[OutletId] IS NOT NULL");

        modelBuilder.Entity<QrCode>()
            .HasIndex(q => q.Token)
            .IsUnique();

        modelBuilder.Entity<QrCode>()
            .HasOne(q => q.Outlet)
            .WithMany(o => o.QrCodes)
            .HasForeignKey(q => q.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<QrCode>()
            .HasOne(q => q.CafeTable)
            .WithMany(t => t.QrCodes)
            .HasForeignKey(q => q.CafeTableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuCategory>()
            .HasOne(c => c.Outlet)
            .WithMany(o => o.MenuCategories)
            .HasForeignKey(c => c.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.Outlet)
            .WithMany(o => o.MenuItems)
            .HasForeignKey(m => m.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<MenuItem>()
            .HasOne(m => m.MenuCategory)
            .WithMany(c => c.MenuItems)
            .HasForeignKey(m => m.MenuCategoryId)
            .OnDelete(DeleteBehavior.SetNull);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.Price)
            .HasPrecision(18, 2);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.CostPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<MenuItem>()
            .Property(m => m.SellingPrice)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .HasIndex(o => new { o.OutletId, o.OrderNumber })
            .IsUnique();

        modelBuilder.Entity<Order>()
            .HasOne(o => o.Outlet)
            .WithMany(outlet => outlet.Orders)
            .HasForeignKey(o => o.OutletId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.CafeTable)
            .WithMany(t => t.Orders)
            .HasForeignKey(o => o.CafeTableId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .HasOne(o => o.QrCode)
            .WithMany(q => q.Orders)
            .HasForeignKey(o => o.QrCodeId)
            .OnDelete(DeleteBehavior.Restrict);

        modelBuilder.Entity<Order>()
            .Property(o => o.Subtotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.TaxTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.DiscountTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Order>()
            .Property(o => o.GrandTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.UnitPriceSnapshot)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.CostPriceSnapshot)
            .HasPrecision(18, 2);

        modelBuilder.Entity<OrderItem>()
            .Property(i => i.LineTotal)
            .HasPrecision(18, 2);

        modelBuilder.Entity<Payment>()
            .Property(p => p.Amount)
            .HasPrecision(18, 2);
    }
}
