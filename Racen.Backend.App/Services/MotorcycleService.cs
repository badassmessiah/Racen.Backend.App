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
            return await _context.Motorcycles.ToListAsync();
        }

        public async Task<Motorcycle> GetMotorcycleByIdAsync(string id)
        {
            var motorcycle = await _context.Motorcycles.FirstOrDefaultAsync(c => c.Id == id);
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
        
    }
}