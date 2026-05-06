namespace ScanAndSavor.PlatformModels;

public class Tenant
{
    public int Id { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Subdomain { get; set; } = string.Empty;

    public string CustomDomain { get; set; } = string.Empty;

    public string DatabaseName { get; set; } = string.Empty;

    public bool IsActive { get; set; } = true;

    public SubscriptionStatus SubscriptionStatus { get; set; } = SubscriptionStatus.Trial;

    public DateTime? SubscriptionStartedAt { get; set; }

    public DateTime? SubscriptionExpiresAt { get; set; }

    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

    public ICollection<TenantSubscription> Subscriptions { get; set; } = new List<TenantSubscription>();
}
