using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Racen.Backend.App.Models.User;

namespace Racen.Backend.App.Models.MotorcycleRelated
{
    public class Motorcycle
    {
        [Key]
        [Required]
        public required string Id { get; set; }

        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public string Name { get; set; } = string.Empty;

        [Required]
        [Range(0, 420, ErrorMessage = "Speed must be between 0 and 420.")]
        public int Speed { get; set; }

        [Required]
        [Range(0, 320, ErrorMessage = "Power must be between 0 and 320.")]
        public int Power { get; set; }

        [Required]
        [Range(0, 100, ErrorMessage = "Handling must be between 0 and 100.")]
        public int Handling { get; set; }

        [Required]
        public Rarity Rarity { get; set; }

        [Required]
        public bool Enabled { get; set; }

        [Required]
        public required string OwnerId { get; set; }

        [ForeignKey("OwnerId")]
        public required ApplicationUser Owner { get; set; }

        private List<Items>? _items;
        public List<Items>? Items
        {
            get => _items;
            set
            {
                if (value != null && value.Count > 6)
                {
                    throw new InvalidOperationException("A motorcycle can have up to 6 items.");
                }
                _items = value;
            }
        }

        public byte FuelCapacity { get; set; }
        public decimal Level { get; set; }
        public Motorcycle()
        {

        }

        private static readonly Random random = new Random();
        
    }
}