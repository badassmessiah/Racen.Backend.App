using Racen.Backend.App.Models.Accessories;
using Racen.Backend.App.Models.User;
using Racen.Backend.App.Models.Car; 

namespace Racen.Backend.App.Models.Accessories
{
    public class UserAccessoryModel
    {
        public required string Id { get; set; }
        public required string UserId { get; set; }
        public ApplicationUser User { get; set; } = default!;
        public required string AccessoryId { get; set; }
        public AccessoryModel Accessory { get; set; } = default!;
    }
}