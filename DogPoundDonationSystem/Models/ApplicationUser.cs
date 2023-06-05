using Microsoft.AspNetCore.Identity;
using System.Collections.Generic;

namespace DogPoundDonationSystem.Models
{
    public class ApplicationUser : IdentityUser
    {
        public List<Donation>? Donations { get; set; }  
    }
}
