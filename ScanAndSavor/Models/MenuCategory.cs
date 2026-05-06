namespace ScanAndSavor.Models;

public class MenuCategory
{
    public int Id { get; set; }

    public int OutletId { get; set; }
    public Outlet? Outlet { get; set; }

    public string Name { get; set; } = string.Empty;

    public int SortOrder { get; set; }

    public bool IsActive { get; set; } = true;

    public ICollection<MenuItem> MenuItems { get; set; } = new List<MenuItem>();
}
