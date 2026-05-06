namespace ScanAndSavor.Models;

public class Outlet
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Slug { get; set; } = string.Empty;

    public string Address { get; set; } = string.Empty;

    public string Phone { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<CafeTable> Tables { get; set; } = new List<CafeTable>();

    public ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();

    public ICollection<MenuCategory> MenuCategories { get; set; } = new List<MenuCategory>();

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
