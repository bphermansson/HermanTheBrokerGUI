
namespace HermanTheBrokerGUI.Models
{
    public enum Category
    {
        Bostadsrättslägenhet,
        Bostadsrättsradhus,
        Villa,
        Fritidshus
    }
    public class House
    {
        public int HouseId { get; set; }
        public Category? Category { get; set; }
        public string PostalAddress { get; set; }
        public string City { get; set; }
        public int AskingPrice { get; set; }
        public int LivingArea { get; set; }
        public int SecondaryArea { get; set; }
        public int GardenArea { get; set; }
        public string Description { get; set; }
        public int NoOfRooms { get; set; }
        public int MonthlyFee { get; set; }
        public int YearlyRunningCosts { get; set; }
        public int BuildYear { get; set; }
        public bool Error { get; set; }
        public string? BrokerId { get; set; } = null!;
        public Broker? Broker { get; set; } = null!;    // Value in this field isn't required.
    }
}
