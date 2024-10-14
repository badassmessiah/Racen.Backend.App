using Racen.Backend.App.Data;
using Racen.Backend.App.Models.Car;
using Microsoft.EntityFrameworkCore;

namespace Racen.Backend.App.Services
{
    public class CarService
    {
        private readonly AppDbContext _context;

        public CarService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<CarModel>> GetAllCarsAsync()
        {
            return await _context.Cars.ToListAsync();
        }

        public async Task<CarModel> GetCarByIdAsync(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found.");
            }
            return car;
        }

        public async Task<CarModel> CreateCarAsync(CarModel car)
        {
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<CarModel> UpdateCarAsync(CarModel car)
        {
            _context.Cars.Update(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task DeleteCarAsync(string id)
        {
            var car = await _context.Cars.FindAsync(id);
            if (car != null)
            {
                _context.Cars.Remove(car);
                await _context.SaveChangesAsync();
            }
        }
    }
}