using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models.MotorcycleRelated;
using Racen.Backend.App.Models.User;

namespace Racen.Backend.App.Services
{
    public class MotorcycleService
    {
        private readonly AppDbContext _context;

        public MotorcycleService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Motorcycle>> GetAllMotorcyclesAsync()
        {
            return await _context.Motorcycles.Where(m => m.Enabled).ToListAsync();
        }

        public async Task<List<Motorcycle>> GetDisabledMotorcyclesAsync()
        {
            return await _context.Motorcycles.Where(m => !m.Enabled).ToListAsync();
        }

        public async Task<Motorcycle> GetMotorcycleByIdAsync(string id)
        {
            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id && m.Enabled);
            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");
            }
            return motorcycle;
        }

        public async Task<Motorcycle> CreateMotorcycleAsync(string name,string userId, Rarity rarity, ApplicationUser owner)
        {
            var motorcycle = new Motorcycle
            {
                Id = Guid.NewGuid().ToString(),
                Name = name,
                Rarity = rarity,
                OwnerId = userId,
                Owner = owner
            };
            
            DefaultProperties.SetDefaultProperties(motorcycle);

            await _context.Motorcycles.AddAsync(motorcycle);
            await _context.SaveChangesAsync();
            return motorcycle;
        }

        public async Task<Motorcycle> UpdateMotorcycleAsync(string id, Motorcycle motorcycle)
        {
            if (id != motorcycle.Id)
            {
                throw new InvalidOperationException("ID mismatch.");
            }

            var existingMotorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == id && m.Enabled);
            if (existingMotorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {id} not found.");
            }

            _context.Entry(existingMotorcycle).CurrentValues.SetValues(motorcycle);
            await _context.SaveChangesAsync();
            return existingMotorcycle;
        }

        public async Task DeleteMotorcycleAsync(string id)
        {
            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(c => c.Id == id);
            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle not found.");
            }
            motorcycle.Enabled = false;
            await _context.SaveChangesAsync();
        }

        public async Task AssignItemToMotorcycleAsync(string motorcycleId, string itemId)
        {
            // Fetch the motorcycle along with its items
            var motorcycle = await _context.Motorcycles
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == motorcycleId && m.Enabled);

            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");
            }

            // Check if the motorcycle already has 6 items
            if (motorcycle.Items != null && motorcycle.Items.Count >= 6)
            {
                throw new InvalidOperationException("Motorcycle already has the maximum number of items.");
            }

            // Fetch the item
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId && i.Enabled);

            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found.");
            }

            // Check if the item is already assigned to a motorcycle
            if (!string.IsNullOrEmpty(item.MotorcycleId))
            {
                throw new InvalidOperationException("Item is already assigned to a motorcycle.");
            }

            // Assign the item to the motorcycle
            item.MotorcycleId = motorcycle.Id;
            motorcycle.Items ??= new List<Items>();
            motorcycle.Items.Add(item);

            await _context.SaveChangesAsync();
        }

        public async Task RemoveItemFromMotorcycleAsync(string motorcycleId, string itemId)
        {
            // Fetch the motorcycle including its items
            var motorcycle = await _context.Motorcycles
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == motorcycleId && m.Enabled);

            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");
            }

            // Fetch the item
            var item = await _context.Items.FirstOrDefaultAsync(i => i.Id == itemId && i.Enabled);

            if (item == null)
            {
                throw new KeyNotFoundException($"Item with ID {itemId} not found.");
            }

            // Check if the item is assigned to the motorcycle
            if (item.MotorcycleId != motorcycleId)
            {
                throw new InvalidOperationException("Item is not assigned to this motorcycle.");
            }

            // Remove the association
            item.MotorcycleId = null;
            motorcycle.Items?.Remove(item);

            await _context.SaveChangesAsync();
        }

        public async Task<List<Motorcycle>> GetUserMotorcyclesAsync(string userId)
        {
            return await _context.Motorcycles
                .Where(m => m.OwnerId == userId && m.Enabled)
                .ToListAsync();
        }

        public async Task<List<Items>> GetMotorcycleItemsAsync(string motorcycleId)
        {
            var motorcycle = await _context.Motorcycles
                .Include(m => m.Items)
                .FirstOrDefaultAsync(m => m.Id == motorcycleId && m.Enabled);

            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");
            }

            return motorcycle.Items ?? new List<Items>();
        }

        public async Task LevelUpMotorcycleAsync(string motorcycleId)
        {
            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(m => m.Id == motorcycleId && m.Enabled);
            if (motorcycle == null)
            {
                throw new KeyNotFoundException($"Motorcycle with ID {motorcycleId} not found.");
            }

            motorcycle.Level += 0.1m;
            await _context.SaveChangesAsync();
        }

    }
}