namespace ScanAndSavor.Models;

public class OrderItem
{
    public int Id { get; set; }

    public int OrderId { get; set; }
    public Order? Order { get; set; }

    public int MenuItemId { get; set; }
    public MenuItem? MenuItem { get; set; }

    public string NameSnapshot { get; set; } = string.Empty;

    public decimal UnitPriceSnapshot { get; set; }

    public decimal CostPriceSnapshot { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }

    public string Notes { get; set; } = string.Empty;
}
