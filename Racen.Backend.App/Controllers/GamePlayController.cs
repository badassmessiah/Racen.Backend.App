using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.Gameplay;
using Racen.Backend.App.Services.GamePlay;
using Racen.Backend.App.Services;

namespace Racen.Backend.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class GamePlayController : ControllerBase
    {
        private readonly MatchThePlayers _matchThePlayers;
        private readonly MotorcycleService _motorcycleService;

        public GamePlayController(MatchThePlayers matchThePlayers, MotorcycleService motorcycleService)
        {
            _matchThePlayers = matchThePlayers;
            _motorcycleService = motorcycleService;
        }

        [HttpPost("find-match")]
        public async Task<IActionResult> FindMatch([FromBody] FindMatchRequest request)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Retrieve the motorcycle from the database
            var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(request.MotorcycleId);
            if (motorcycle == null)
            {
                return NotFound("Motorcycle not found.");
            }

            // Find a match
            var matchResult = await _matchThePlayers.FindMatchAsync(motorcycle, request.GameMode);
            if (matchResult == null)
            {
                return NotFound("No suitable match found.");
            }

            // Prepare the result
            var result = new
            {
                Initiator = new
                {
                    Id = matchResult.Initiator.Id,
                    Name = matchResult.Initiator.Name,
                    OwnerId = matchResult.Initiator.OwnerId
                },
                Opponent = new
                {
                    Id = matchResult.Opponent.Id,
                    Name = matchResult.Opponent.Name,
                    OwnerId = matchResult.Opponent.OwnerId
                },
                IsInitiatorWinner = matchResult.IsInitiatorWinner,
                PointsEarned = matchResult.PointsEarned,
                TotalMatches = matchResult.Initiator.Owner.MatchesPlayed,
                TotalWins = matchResult.Initiator.Owner.Wins,
                TotalLosses = matchResult.Initiator.Owner.Losses
            };

            return Ok(result);
        }
    }
}