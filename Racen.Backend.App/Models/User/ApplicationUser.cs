using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Models.Accessories;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public List<UserAccessoryModel> UserAccessories { get; set; } = new();
        public List<Motorcycle>? Motorcycles { get; set; }
        public List<Items>? Items { get; set; }
    }
}