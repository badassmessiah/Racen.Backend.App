using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.Gameplay;

namespace Racen.Backend.App.DTOs.Gameplay
{
    public class FindMatchRequest
    {
        [Required]
        public string MotorcycleId { get; set; }

        [Required]
        [EnumDataType(typeof(GameMode))]
        public GameMode GameMode { get; set; }
    }
}