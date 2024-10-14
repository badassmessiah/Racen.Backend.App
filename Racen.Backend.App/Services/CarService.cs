using Racen.Backend.App.Data;
using Racen.Backend.App.Models.Car;
using Racen.Backend.App.Models.Accessories;
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
            return await _context.Cars.Include(c => c.Accessories).ToListAsync();
        }

        public async Task<CarModel> GetCarByIdAsync(string id)
        {
            var car = await _context.Cars.Include(c => c.Accessories).FirstOrDefaultAsync(c => c.Id == id);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {id} not found.");
            }
            return car;
        }

        public async Task<CarModel> CreateCarAsync(CarModel car)
        {
            ApplyRarityBonus(car);
            _context.Cars.Add(car);
            await _context.SaveChangesAsync();
            return car;
        }

        public async Task<CarModel> UpdateCarAsync(CarModel car)
        {
            ApplyRarityBonus(car);
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

        public async Task AddAccessoryToCarAsync(string carId, string userId, string accessoryId)
        {
            var car = await _context.Cars.Include(c => c.Accessories).FirstOrDefaultAsync(c => c.Id == carId);
            if (car == null)
            {
                throw new KeyNotFoundException($"Car with ID {carId} not found.");
            }

            var userAccessory = await _context.UserAccessories.Include(ua => ua.Accessory).FirstOrDefaultAsync(ua => ua.UserId == userId && ua.AccessoryId == accessoryId);
            if (userAccessory == null)
            {
                throw new KeyNotFoundException($"Accessory with ID {accessoryId} not found in user's inventory.");
            }

            if (car.Accessories.Any(a => a.Type == userAccessory.Accessory.Type))
            {
                throw new InvalidOperationException($"Car already has an accessory of type {userAccessory.Accessory.Type}.");
            }

            ApplyAccessoryBonus(car, userAccessory.Accessory);
            car.Accessories.Add(userAccessory.Accessory);
            _context.UserAccessories.Remove(userAccessory);
            await _context.SaveChangesAsync();
        }
        private void ApplyRarityBonus(CarModel car)
        {
            var random = new Random();
            var stats = new List<Action<int>>
            {
                bonus => car.Speed += bonus,
                bonus => car.Acceleration += bonus,
                bonus => car.Aerodynamics += bonus,
                bonus => car.TyreGrip += bonus,
                bonus => car.Weight -= bonus, // Assuming lower weight is better
                bonus => car.Power += bonus,
                bonus => car.FuelConsumption -= bonus // Assuming lower consumption is better
            };

            if (car.Rarity > CarRarity.Common)
            {
                var bonusStats = stats.OrderBy(_ => random.Next()).Take(3).ToList();
                foreach (var stat in bonusStats)
                {
                    var bonus = random.Next(10, 21);
                    stat(bonus);
                }
            }
        }

        private void ApplyAccessoryBonus(CarModel car, AccessoryModel accessory)
        {
            switch (accessory.Type)
            {
                case AccessoryType.Speed:
                    ApplySpeedAccessoryBonus(car, accessory);
                    break;
                case AccessoryType.Power:
                    ApplyPowerAccessoryBonus(car, accessory);
                    break;
                case AccessoryType.Aerodynamics:
                    ApplyAerodynamicsAccessoryBonus(car, accessory);
                    break;
            }
        }

        private void ApplySpeedAccessoryBonus(CarModel car, AccessoryModel accessory)
        {
            switch (accessory.Rarity)
            {
                case AccessoryRarity.Basic:
                    car.Speed += (int)(car.Speed * 0.02);
                    break;
                case AccessoryRarity.Rare:
                    car.Speed += (int)(car.Speed * 0.05);
                    car.Aerodynamics += (int)(car.Aerodynamics * 0.05);
                    car.Weight -= (int)(car.Weight * 0.05);
                    break;
                    // Add other cases for different rarities
            }
        }

        private void ApplyPowerAccessoryBonus(CarModel car, AccessoryModel accessory)
        {
            switch (accessory.Rarity)
            {
                case AccessoryRarity.Basic:
                    car.Power += (int)(car.Power * 0.02);
                    break;
                case AccessoryRarity.Rare:
                    car.Power += (int)(car.Power * 0.05);
                    car.Acceleration += (int)(car.Acceleration * 0.05);
                    break;
                    // Add other cases for different rarities
            }
        }

        private void ApplyAerodynamicsAccessoryBonus(CarModel car, AccessoryModel accessory)
        {
            switch (accessory.Rarity)
            {
                case AccessoryRarity.Basic:
                    car.Aerodynamics += (int)(car.Aerodynamics * 0.02);
                    break;
                case AccessoryRarity.Rare:
                    car.Aerodynamics += (int)(car.Aerodynamics * 0.05);
                    car.Speed += (int)(car.Speed * 0.05);
                    break;
                    // Add other cases for different rarities
            }
        }
    }
}