using Microsoft.AspNetCore.Identity;

namespace GuitarShop.Models
{
    public class AppUser : IdentityUser
    {
        public byte[] AvatarImage { get; set; }
    }
}
