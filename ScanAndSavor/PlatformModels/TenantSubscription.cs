namespace ScanAndSavor.PlatformModels;

public class TenantSubscription
{
    public int Id { get; set; }

    public int TenantId { get; set; }
    public Tenant? Tenant { get; set; }

    public string PlanName { get; set; } = string.Empty;

    public string BillingCycle { get; set; } = "Monthly";

    public decimal Amount { get; set; }

    public string Currency { get; set; } = "INR";

    public SubscriptionStatus Status { get; set; } = SubscriptionStatus.Trial;

    public DateTime StartedAt { get; set; } = DateTime.UtcNow;

    public DateTime? EndsAt { get; set; }

    public string RazorpayCustomerId { get; set; } = string.Empty;

    public string RazorpaySubscriptionId { get; set; } = string.Empty;
}
