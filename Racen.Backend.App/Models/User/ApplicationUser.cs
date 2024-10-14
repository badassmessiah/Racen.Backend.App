using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Models.Accessories;

namespace Racen.Backend.App.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserAccessoryModel> UserAccessories { get; set; } = new();
    }
}