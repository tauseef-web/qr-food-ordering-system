namespace ScanAndSavor.Models;

public class Payment
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public string Provider { get; set; } = "Razorpay";

    public string Method { get; set; } = string.Empty;

    public PaymentStatus Status { get; set; } = PaymentStatus.Pending;

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "INR";

    public string ProviderOrderId { get; set; } = string.Empty;

    public string ProviderPaymentId { get; set; } = string.Empty;

    public string ProviderSignature { get; set; } = string.Empty;

    public DateTime? PaidAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}
