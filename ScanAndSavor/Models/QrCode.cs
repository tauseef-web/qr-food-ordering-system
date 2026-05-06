namespace ScanAndSavor.Models;

public class QrCode
{
    public int Id { get; set; }

    public int OutletId { get; set; }
    public Outlet? Outlet { get; set; }

    public int CafeTableId { get; set; }
    public CafeTable? CafeTable { get; set; }

    public string Token { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? RevokedAt { get; set; }

    public ICollection<Order> Orders { get; set; } = new List<Order>();
}
