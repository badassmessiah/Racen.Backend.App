using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.MotorcycleDTOs;
using AutoMapper;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Services;
using Microsoft.AspNetCore.Identity;
using Racen.Backend.App.Models.User;


namespace Racen.Backend.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly IMapper _mapper;

        public MotorcycleController(MotorcycleService motorcycleService, IMapper mapper, UserManager<ApplicationUser> userManager)
        {
            _motorcycleService = motorcycleService;
            _mapper = mapper;
            _userManager = userManager;
        }

        [Authorize]
        [HttpGet("all")]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            var motorcycles = await _motorcycleService.GetAllMotorcyclesAsync();
            var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleReadDto>>(motorcycles);
            return Ok(motorcycleDtos);
        }

        [Authorize]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetMotorcycleById(string id)
        {
            try
            {
                var motorcycle = await _motorcycleService.GetMotorcycleByIdAsync(id);
                var motorcycleDto = _mapper.Map<MotorcycleReadDto>(motorcycle);
                return Ok(motorcycleDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle([FromBody] MotorcycleCreateDto motorcycleDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            motorcycle.Id = Guid.NewGuid().ToString();
            DefaultProperties.SetDefaultProperties(motorcycle);
            var applicationUser = await _userManager.FindByIdAsync(motorcycleDto.OwnerId);

            if (applicationUser == null)
            {
                return BadRequest("Owner not found.");
            }

            try
            {
                await _motorcycleService.CreateMotorcycleAsync(motorcycleDto.Name, motorcycleDto.OwnerId, motorcycle.Rarity, applicationUser);
                return CreatedAtAction(nameof(GetMotorcycleById), new { id = motorcycle.Id }, motorcycle);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.\n" + ex.Message);
            }
        }

        [Authorize]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateMotorcycle(string id, [FromBody] MotorcycleUpdateDto motorcycleDto)
        {
            if (id != motorcycleDto.Id)
            {
                return BadRequest("ID mismatch.");
            }

            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);

            try
            {
                await _motorcycleService.UpdateMotorcycleAsync(id, motorcycle);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteMotorcycle(string id)
        {
            try
            {
                await _motorcycleService.DeleteMotorcycleAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [Authorize]
        [HttpPost("{motorcycleId}/items/{itemId}")]
        public async Task<IActionResult> AssignItemToMotorcycle(string motorcycleId, string itemId)
        {
            try
            {
                await _motorcycleService.AssignItemToMotorcycleAsync(motorcycleId, itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpDelete("{motorcycleId}/items/{itemId}")]
        public async Task<IActionResult> RemoveItemFromMotorcycle(string motorcycleId, string itemId)
        {
            try
            {
                await _motorcycleService.RemoveItemFromMotorcycleAsync(motorcycleId, itemId);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ex.Message);
            }
        }

        [Authorize]
        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMotorcycles(string userId)
        {
            var motorcycles = await _motorcycleService.GetUserMotorcyclesAsync(userId);
            var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleReadDto>>(motorcycles);
            return Ok(motorcycleDtos);
        }

        [Authorize]
        [HttpGet("{motorcycleId}/items")]
        public async Task<IActionResult> GetMotorcycleItems(string motorcycleId)
        {
            try
            {
                var items = await _motorcycleService.GetMotorcycleItemsAsync(motorcycleId);
                return Ok(items);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
        }
    }

}