using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models.MotorcycleRelated;


namespace Racen.Backend.App.Services
{
    public class ItemsService
    {
        private readonly AppDbContext _context;

        public ItemsService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Items>> GetAllItemsAsync()
        {
            return await _context.Items.ToListAsync();
        }

        public async Task<Items> GetItemByIdAsync(string id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }
            return item;
        }

        public async Task<Items> CreateItemAsync(Items item)
        {
            _context.Items.Add(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task<Items> UpdateItemAsync(string id, Items item)
        {
            if (id != item.Id)
            {
                throw new InvalidOperationException("ID mismatch.");
            }

            var existingItem = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            if (existingItem == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }

            _context.Entry(existingItem).CurrentValues.SetValues(item);
            await _context.SaveChangesAsync();
            return item;
        }

        public async Task DeleteItemAsync(string id)
        {
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == id);
            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {id} not found.");
            }

            item.Enabled = false;
            await _context.SaveChangesAsync();
        }
    }
}