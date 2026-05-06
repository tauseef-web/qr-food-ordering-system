namespace ScanAndSavor.Models;

public class MenuItem
{
    public int Id { get; set; }

    // Kept nullable for compatibility while the app moves from Restaurant to Outlet terminology.
    public int? RestaurantId { get; set; }
    public Restaurant? Restaurant { get; set; }

    public int? OutletId { get; set; }
    public Outlet? Outlet { get; set; }

    public int? MenuCategoryId { get; set; }
    public MenuCategory? MenuCategory { get; set; }

    public string Name { get; set; } = string.Empty;

    public string Description { get; set; } = string.Empty;

    public string ImageUrl { get; set; } = string.Empty;

    // Existing scaffolded pages bind Price; new code should use SellingPrice.
    public decimal Price { get; set; }

    public decimal CostPrice { get; set; }

    public decimal SellingPrice { get; set; }

    public bool IsAvailable { get; set; } = true;

    public bool IsActive { get; set; } = true;

    public ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
}
