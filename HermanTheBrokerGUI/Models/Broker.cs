using HermanTheBrokerGUI.Models;
using Microsoft.AspNetCore.Identity;

namespace HermanTheBrokerGUI.Models
{
	public class Broker : IdentityUser
	{
        public string Name { get; set; }
		public long PhoneNumber { get; set; }
		public ICollection<House> Houses { get; set; }
	}
}
