using System.ComponentModel.DataAnnotations;

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
		[Key]
		public int HouseId { get; set; }
        public string Street { get; set; }
		public string City { get; set; }
		public int Area { get; set; }
		public int BuildYear { get; set; }
        public int NoOfFloors { get; set; }
        public int NoOfRooms { get; set; }
        public Category? Category { get; set; }
    }
}
