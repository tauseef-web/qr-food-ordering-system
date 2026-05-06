namespace ScanAndSavor.Models;

public class Order
{
    public int Id { get; set; }

    public int OutletId { get; set; }
    public Outlet? Outlet { get; set; }

    public int? CafeTableId { get; set; }
    public CafeTable? CafeTable { get; set; }

    public int? QrCodeId { get; set; }
    public QrCode? QrCode { get; set; }

    public string OrderNumber { get; set; } = string.Empty;

    public OrderType OrderType { get; set; } = OrderType.DineIn;

    public OrderStatus Status { get; set; } = OrderStatus.Draft;

    public PaymentStatus PaymentStatus { get; set; } = PaymentStatus.Pending;

    public decimal Subtotal { get; set; }

    public decimal TaxTotal { get; set; }

    public decimal DiscountTotal { get; set; }

    public decimal GrandTotal { get; set; }

    public string CustomerName { get; set; } = string.Empty;

    public string CustomerPhone { get; set; } = string.Empty;

    public string CustomerNote { get; set; } = string.Empty;

    public DateTime? PickupTime { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public DateTime? AcceptedAt { get; set; }

    public DateTime? CompletedAt { get; set; }

    public ICollection<OrderItem> Items { get; set; } = new List<OrderItem>();

    public ICollection<Payment> Payments { get; set; } = new List<Payment>();
}
