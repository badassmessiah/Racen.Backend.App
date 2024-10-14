using System.ComponentModel.DataAnnotations;
using Racen.Backend.App.Models.Car;

namespace Racen.Backend.App.DTOs.Car
{
    public class CarCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; }

        [Range(0, 500, ErrorMessage = "Speed must be between 0 and 500.")]
        public int Speed { get; set; }

        [Range(0, 100, ErrorMessage = "Acceleration must be between 0 and 100.")]
        public int Acceleration { get; set; }

        [Range(0, 100, ErrorMessage = "Aerodynamics must be between 0 and 100.")]
        public int Aerodynamics { get; set; }

        [Range(0, 100, ErrorMessage = "TyreGrip must be between 0 and 100.")]
        public int TyreGrip { get; set; }

        [Range(500, 2000, ErrorMessage = "Weight must be between 500 and 2000.")]
        public int Weight { get; set; }

        [Range(50, 1000, ErrorMessage = "Power must be between 50 and 1000.")]
        public int Power { get; set; }

        [Range(1, 50, ErrorMessage = "FuelConsumption must be between 1 and 50.")]
        public int FuelConsumption { get; set; }

        [Range(1, 10, ErrorMessage = "Level must be between 1 and 10.")]
        public int Level { get; set; }

        [Required]
        public string OwnerId { get; set; }

        [Required]
        public CarRarity Rarity { get; set; }
    }
}