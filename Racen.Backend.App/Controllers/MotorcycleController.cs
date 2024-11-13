using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.Motorcycle;
using AutoMapper;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Services;


namespace Racen.Backend.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MotorcycleController : ControllerBase
    {
        private readonly MotorcycleService _motorcycleService;
        private readonly IMapper _mapper;

        public MotorcycleController(MotorcycleService motorcycleService, IMapper mapper)
        {
            _motorcycleService = motorcycleService;
            _mapper = mapper;
        }

        [HttpGet("all")]
        public async Task<IActionResult> GetAllMotorcycles()
        {
            var motorcycles = await _motorcycleService.GetAllMotorcyclesAsync();
            var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleReadDto>>(motorcycles);
            return Ok(motorcycleDtos);
        }

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

        [HttpPost]
        public async Task<IActionResult> CreateMotorcycle([FromBody] MotorcycleCreateDto motorcycleDto)
        {
            var motorcycle = _mapper.Map<Motorcycle>(motorcycleDto);
            motorcycle.Id = Guid.NewGuid().ToString();
            await _motorcycleService.CreateMotorcycleAsync(motorcycle);
            var createdMotorcycleDto = _mapper.Map<MotorcycleReadDto>(motorcycle);
            return CreatedAtAction(nameof(GetMotorcycleById), new { id = motorcycle.Id }, createdMotorcycleDto);
        }

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

        [HttpGet("user/{userId}")]
        public async Task<IActionResult> GetUserMotorcycles(string userId)
        {
            var motorcycles = await _motorcycleService.GetUserMotorcyclesAsync(userId);
            var motorcycleDtos = _mapper.Map<IEnumerable<MotorcycleReadDto>>(motorcycles);
            return Ok(motorcycleDtos);
        }
    }

}