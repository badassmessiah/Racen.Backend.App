using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models.Gameplay;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Services.GamePlay
{
    public class MatchThePlayers
    {
        private readonly AppDbContext _context;
        private readonly MotorcycleService _motorcycleService;

        public MatchThePlayers(AppDbContext context, MotorcycleService motorcycleService)
        {
            _context = context;
            _motorcycleService = motorcycleService;
        }

        public async Task<NewMatch?> FindMatchAsync(Motorcycle motorcycle, GameMode gameMode)
        {
            // Define acceptable level and rarity differences
            const decimal levelTolerance = 1.0m;
            const int rarityTolerance = 1;

            // Get the numerical value of the motorcycle's rarity
            int motorcycleRarityValue = (int)motorcycle.Rarity;

            // Find a matching motorcycle
            var matchedMotorcycle = await _context.Motorcycles
                .Where(m => m.Id != motorcycle.Id && m.Enabled)
                .Where(m => Math.Abs(m.Level - motorcycle.Level) <= levelTolerance)
                .Where(m => Math.Abs((int)m.Rarity - motorcycleRarityValue) <= rarityTolerance)
                .OrderBy(m => Math.Abs(m.Level - motorcycle.Level)) // Optional: prioritize closer matches
                .FirstOrDefaultAsync();

            if (matchedMotorcycle != null)
            {
                // Create a new match with the game mode
                var newMatch = new NewMatch
                {
                    Motorcycle1 = motorcycle,
                    Motorcycle2 = matchedMotorcycle,
                    GameMode = gameMode
                };

                // Calculate the winner
                newMatch.CalculateWinner();

                


                return newMatch;
            }

            // No match found
            return null;
        }

    }
}