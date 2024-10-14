using Microsoft.AspNetCore.Mvc;
using Racen.Backend.App.DTOs.Car;
using Racen.Backend.App.Models.Car;
using Racen.Backend.App.Services;

namespace Racen.Backend.App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CarController : ControllerBase
    {
        private readonly CarService _carService;

        public CarController(CarService carService)
        {
            _carService = carService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllCars()
        {
            var cars = await _carService.GetAllCarsAsync();
            var carDtos = cars.Select(car => new CarReadDto
            {
                Id = car.Id,
                Name = car.Name,
                Speed = car.Speed,
                Acceleration = car.Acceleration,
                Aerodynamics = car.Aerodynamics,
                TyreGrip = car.TyreGrip,
                Weight = car.Weight,
                Power = car.Power,
                FuelConsumption = car.FuelConsumption,
                Level = car.Level,
                OwnerId = car.OwnerId
            }).ToList();
            return Ok(carDtos);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCarById(string id)
        {
            var car = await _carService.GetCarByIdAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            var carDto = new CarReadDto
            {
                Id = car.Id,
                Name = car.Name,
                Speed = car.Speed,
                Acceleration = car.Acceleration,
                Aerodynamics = car.Aerodynamics,
                TyreGrip = car.TyreGrip,
                Weight = car.Weight,
                Power = car.Power,
                FuelConsumption = car.FuelConsumption,
                Level = car.Level,
                OwnerId = car.OwnerId
            };
            return Ok(carDto);
        }

        [HttpPost]
        public async Task<IActionResult> CreateCar(CarCreateDto carDto)
        {
            var car = new CarModel
            {
                Id = Guid.NewGuid().ToString(),
                Name = carDto.Name,
                Speed = carDto.Speed,
                Acceleration = carDto.Acceleration,
                Aerodynamics = carDto.Aerodynamics,
                TyreGrip = carDto.TyreGrip,
                Weight = carDto.Weight,
                Power = carDto.Power,
                FuelConsumption = carDto.FuelConsumption,
                Level = carDto.Level,
                OwnerId = carDto.OwnerId,
                Rarity = carDto.Rarity
            };
            var createdCar = await _carService.CreateCarAsync(car);
            var createdCarDto = new CarReadDto
            {
                Id = createdCar.Id,
                Name = createdCar.Name,
                Speed = createdCar.Speed,
                Acceleration = createdCar.Acceleration,
                Aerodynamics = createdCar.Aerodynamics,
                TyreGrip = createdCar.TyreGrip,
                Weight = createdCar.Weight,
                Power = createdCar.Power,
                FuelConsumption = createdCar.FuelConsumption,
                Level = createdCar.Level,
                OwnerId = createdCar.OwnerId,
                Rarity = createdCar.Rarity
            };
            return CreatedAtAction(nameof(GetCarById), new { id = createdCarDto.Id }, createdCarDto);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCar(string id, CarUpdateDto carDto)
        {
            if (id != carDto.Id)
            {
                return BadRequest();
            }

            var car = new CarModel
            {
                Id = carDto.Id,
                Name = carDto.Name,
                Speed = carDto.Speed,
                Acceleration = carDto.Acceleration,
                Aerodynamics = carDto.Aerodynamics,
                TyreGrip = carDto.TyreGrip,
                Weight = carDto.Weight,
                Power = carDto.Power,
                FuelConsumption = carDto.FuelConsumption,
                Level = carDto.Level,
                OwnerId = carDto.OwnerId
            };

            var updatedCar = await _carService.UpdateCarAsync(car);
            var updatedCarDto = new CarReadDto
            {
                Id = updatedCar.Id,
                Name = updatedCar.Name,
                Speed = updatedCar.Speed,
                Acceleration = updatedCar.Acceleration,
                Aerodynamics = updatedCar.Aerodynamics,
                TyreGrip = updatedCar.TyreGrip,
                Weight = updatedCar.Weight,
                Power = updatedCar.Power,
                FuelConsumption = updatedCar.FuelConsumption,
                Level = updatedCar.Level,
                OwnerId = updatedCar.OwnerId
            };
            return Ok(updatedCarDto);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCar(string id)
        {
            await _carService.DeleteCarAsync(id);
            return NoContent();
        }

        [HttpPost("{carId}/accessories")]
        public async Task<IActionResult> AddAccessoryToCar(string carId, [FromBody] AddAccessoryDto addAccessoryDto)
        {
            try
            {
                await _carService.AddAccessoryToCarAsync(carId, addAccessoryDto.UserId, addAccessoryDto.AccessoryId);
                return Ok();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}