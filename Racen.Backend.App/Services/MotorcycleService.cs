using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Racen.Backend.App.Data;
using Racen.Backend.App.Models.MotorcycleRelated;

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

        public async Task<Motorcycle> CreateMotorcycleAsync(Motorcycle motorcycle)
        {
            _context.Motorcycles.Add(motorcycle);
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

    }
}