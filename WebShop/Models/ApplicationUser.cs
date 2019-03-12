using Microsoft.AspNetCore.Identity;

namespace WebShop.Models
{
    // Add UserProfile data for application users by adding properties to the ApplicationUser class
    public class ApplicationUser : IdentityUser
    {
        public virtual UserProfile UserProfile { get; set; }
    }
}
