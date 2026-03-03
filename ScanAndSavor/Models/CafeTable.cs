namespace ScanAndSavor.Models
{

    public class CafeTable
    {
        public int Id { get; set; }

        public int RestaurantId { get; set; }
        public Restaurant? Restaurant { get; set; }

        public int TableNumber { get; set; }
    }
}
