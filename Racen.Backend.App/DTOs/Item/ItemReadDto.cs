using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.Item
{
    public class ItemReadDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public Rarity Rarity { get; set; }

        public int Speed { get; set; }

        public int Power { get; set; }

        public int Handling { get; set; }
    }
}