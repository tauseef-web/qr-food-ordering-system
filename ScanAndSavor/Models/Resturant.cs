namespace ScanAndSavor.Models
{
    public class Restaurant
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty; // used in URL
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}