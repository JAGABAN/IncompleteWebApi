using Microsoft.AspNetCore.Identity;

namespace MyPersonalProject.Models
{
    public class ApplicationUser:IdentityUser
    {
        public string Name { get; set; }
    }
}
