using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.Item;
using Racen.Backend.App.DTOs.ItemDTO;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Services;

namespace Racen.Backend.App.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ItemsController : ControllerBase
    {
        private readonly ItemsService _itemsService;
        private readonly IMapper _mapper;

        public ItemsController(ItemsService itemsService, IMapper mapper)
        {
            _itemsService = itemsService;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllItems()
        {
            var items = await _itemsService.GetAllItemsAsync();
            var itemDtos = _mapper.Map<IEnumerable<ItemReadDto>>(items);
            return Ok(itemDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetItemById(string id)
        {
            try
            {
                var item = await _itemsService.GetItemByIdAsync(id);
                var itemDto = _mapper.Map<ItemReadDto>(item);
                return Ok(itemDto);
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateItem([FromBody] ItemCreateDto itemDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var item = _mapper.Map<Items>(itemDto);
            item.Id = Guid.NewGuid().ToString();
            item.Enabled = true;

            try
            {
                await _itemsService.CreateItemAsync(item);
                var createdItemDto = _mapper.Map<ItemReadDto>(item);
                return CreatedAtAction(nameof(GetItemById), new { id = item.Id }, createdItemDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "An unexpected error occurred.\n" + ex.Message);
            }
        }


        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteItem(string id)
        {
            try
            {
                await _itemsService.DeleteItemAsync(id);
                return NoContent();
            }
            catch (KeyNotFoundException)
            {
                return NotFound();
            }
        }
    }

}