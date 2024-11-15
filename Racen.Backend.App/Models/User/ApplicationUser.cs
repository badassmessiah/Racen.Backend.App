using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Models.User
{
    public class ApplicationUser : IdentityUser
    {
        public List<Motorcycle>? Motorcycles { get; set; } = new List<Motorcycle>();
        public List<Items>? Items { get; set; } = new List<Items>();

        public decimal Money { get; set; }
        public int MatchesPlayed { get; set; }
        public int Wins { get; set; }
        public int Losses { get; set; }
        public double WinLoseRatio => MatchesPlayed == 0 ? 0 : (double)Wins / MatchesPlayed;
    }
}