using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.Motorcycle
{
    public class MotorcycleUpdateDto
    {
        [Required]
        public string Id { get; set; } = string.Empty;
        [Required]
        public decimal Level { get; set; }

        [Required]
        public bool Enabled { get; set; }
    }
}