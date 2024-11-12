using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Racen.Backend.App.Models.MotorcycleRelated;

namespace Racen.Backend.App.DTOs.Motorcycle
{
    public class MotorcycleReadDto
    {
        public string Id { get; set; } = string.Empty;

        public string Name { get; set; } = string.Empty;

        public int Speed { get; set; }

        public int Power { get; set; }

        public int Handling { get; set; }

        public Rarity Rarity { get; set; }

        public bool Enabled { get; set; }

        public string OwnerId { get; set; } = string.Empty;

        public List<ItemReadDto>? Items { get; set; }
    }

    public class ItemReadDto
    {
    }
}