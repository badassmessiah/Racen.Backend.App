using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.Motorcycle
{
    public class MotorcycleCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        public Rarity Rarity { get; set; }

        [Required]
        public string OwnerId { get; set; } = string.Empty;
    }
}