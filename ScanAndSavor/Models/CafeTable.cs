namespace ScanAndSavor.Models;

public class CafeTable
{
    public int Id { get; set; }

    // Kept nullable for compatibility while the app moves from Restaurant to Outlet terminology.
    public int? RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public int? OutletId { get; set; }
    public Outlet? Outlet { get; set; }

    public int TableNumber { get; set; }

    public string DisplayName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public ICollection<QrCode> QrCodes { get; set; } = new List<QrCode>();

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
