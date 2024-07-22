using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace HermanTheBrokerGUI.Models
{
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
	}
}
