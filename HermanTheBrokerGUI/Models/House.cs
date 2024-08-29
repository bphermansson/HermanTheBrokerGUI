
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
        //public string Id { get; set; }
        public int HouseId { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public int Area { get; set; }
        public int BuildYear { get; set; }
        public int NoOfFloors { get; set; }
        public int NoOfRooms { get; set; }
        public Category? Category { get; set; }
        public string? Status { get; set; }
        public bool Error { get; set; }
        public string BrokerId { get; set; }
        public Broker? Broker { get; set; } = null!;    // This field isn't required.

    }
}
