using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Data;
using Racen.Backend.App.DTOs.Gameplay;
using Racen.Backend.App.Models.Gameplay;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.Services.GamePlay
{
    public class MatchThePlayers
    {
        private readonly AppDbContext _context;
        private readonly MotorcycleService _motorcycleService;
        private readonly AccountService _accountService;

        public MatchThePlayers(AppDbContext context, MotorcycleService motorcycleService, AccountService accountService)
        {
            _context = context;
            _motorcycleService = motorcycleService;
            _accountService = accountService;
        }

        private static readonly Dictionary<Rarity, decimal> PrizeAmounts = new Dictionary<Rarity, decimal>
    {
        { Rarity.Basic, 10 },
        { Rarity.Common, 20 },
        { Rarity.Rare, 30 },
        { Rarity.VeryRare, 40 },
        { Rarity.Super, 50 },
        { Rarity.Hyper, 60 },
        { Rarity.Legendary, 70 }
    };

        public async Task<MatchResult?> FindMatchAsync(Motorcycle motorcycle, GameMode gameMode)
        {
            // Calculate the lower and upper bounds of the level range
            decimal lowerBound = Math.Floor(motorcycle.Level);
            decimal upperBound = lowerBound + 1;

            // Find a matching motorcycle within the same level range
            var matchedMotorcycle = await _context.Motorcycles
                .Where(m => m.Id != motorcycle.Id && m.Enabled)
                .Where(m => m.Level >= lowerBound && m.Level < upperBound)
                .FirstOrDefaultAsync();

            if (matchedMotorcycle != null)
            {
                var newMatch = new NewMatch
                {
                    Motorcycle1 = motorcycle,
                    Motorcycle2 = matchedMotorcycle,
                    GameMode = gameMode
                };

                var matchResult = newMatch.CalculateWinnerFromInitiatorPerspective(motorcycle);

                await UpdatePlayerStatsAsync(matchResult);

                return matchResult;
            }

            return null;
        }

        private async Task UpdatePlayerStatsAsync(MatchResult matchResult)
        {
            var initiator = matchResult.Initiator;

            // Update total matches played
            await _accountService.SetTotalMatchesPlayedAsync(initiator.OwnerId);

            if (matchResult.IsInitiatorWinner)
            {
                // Update stats for the winner
                await _accountService.SetPlayerWinAsync(initiator.OwnerId);
            }
            else
            {
                // Update stats for the loser
                await _accountService.SetPlayerLossAsync(initiator.OwnerId);
            }

            // Calculate and set prize money for the winner
            var prizeMoney = CalculateMoneyBasedOnRarityAndLevel(initiator);
            await _accountService.SetPlayerMoneyAsync(initiator.OwnerId, prizeMoney);

        }

        private decimal CalculateMoneyBasedOnRarityAndLevel(Motorcycle motorcycle)
        {
            if (PrizeAmounts.TryGetValue(motorcycle.Rarity, out var basePrizeAmount))
            {
                // Calculate the level percentage (level 1 = 10%, level 10 = 100%)
                decimal levelPercentage = motorcycle.Level / 10;
                return basePrizeAmount * levelPercentage;
            }

            // Default prize amount if rarity is not found (should not happen if all rarities are covered)
            return 0;
        }
    }
}