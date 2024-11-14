using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.Gameplay;
using Racen.Backend.App.Models.Gameplay;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Services.GamePlay;
using Racen.Backend.App.Data;
using System.Threading.Tasks;
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
            var match = await _matchThePlayers.FindMatchAsync(motorcycle, request.GameMode);
            if (match == null)
            {
                return NotFound("No suitable match found.");
            }

            // Prepare the result
            var result = new
            {
                Motorcycle1 = new
                {
                    Id = match.Motorcycle1.Id,
                    Name = match.Motorcycle1.Name,
                    OwnerId = match.Motorcycle1.OwnerId
                },
                Motorcycle2 = new
                {
                    Id = match.Motorcycle2.Id,
                    Name = match.Motorcycle2.Name,
                    OwnerId = match.Motorcycle2.OwnerId
                },
                Winner = match.Winner != null ? new
                {
                    Id = match.Winner.Id,
                    Name = match.Winner.Name,
                    OwnerId = match.Winner.OwnerId
                } : null,
                GameMode = match.GameMode.ToString()
            };

            return Ok(result);
        }
    }
}