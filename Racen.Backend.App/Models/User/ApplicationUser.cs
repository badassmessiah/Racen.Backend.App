using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public List<Motorcycle>? Motorcycles { get; set; }
        public List<Items>? Items { get; set; }
    }
}