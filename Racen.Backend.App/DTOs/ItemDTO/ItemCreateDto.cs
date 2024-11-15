using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.ItemDTO
{
    public class ItemCreateDto
    {
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public required string Name { get; set; }
        [Required]
        public Rarity Rarity { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        public required string OwnerId { get; set; }
        public string? MotorcycleId { get; set; }

    }
}