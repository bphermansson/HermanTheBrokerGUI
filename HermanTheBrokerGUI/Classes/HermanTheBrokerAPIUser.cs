using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HermanTheBrokerGUI.Models;
using Microsoft.AspNetCore.Identity;

namespace HermanTheBrokerAPI.Areas.Identity.Data;

// Add profile data for application users by adding properties to the HermanTheBrokerAPIUser class
public class HermanTheBrokerAPIUser : IdentityUser
{
    public ICollection<House> Houses { get; set; }

}

