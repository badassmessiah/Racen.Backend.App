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

            SetDefaultProperties();
        }

        private static readonly Random random = new Random();
        private void SetDefaultProperties()
        {
            Level = 1;

            Enabled = true;

            Speed = Rarity switch
            {
                Rarity.Basic => random.Next(40, 79),
                Rarity.Common => random.Next(80, 110),
                Rarity.Rare => random.Next(111, 160),
                Rarity.VeryRare => random.Next(161, 210),
                Rarity.Super => random.Next(211, 260),
                Rarity.Hyper => random.Next(261, 310),
                Rarity.Legendary => random.Next(311, 400),
                _ => random.Next(40, 60)
            };

            Power = Rarity switch
            {
                Rarity.Basic => random.Next(15, 35),
                Rarity.Common => random.Next(36, 56),
                Rarity.Rare => random.Next(57, 75),
                Rarity.VeryRare => random.Next(76, 96),
                Rarity.Super => random.Next(97, 120),
                Rarity.Hyper => random.Next(121, 160),
                Rarity.Legendary => random.Next(161, 300),
                _ => random.Next(15, 35)
            };

            Handling = Rarity switch
            {
                Rarity.Basic => random.Next(5, 10),
                Rarity.Common => random.Next(11, 22),
                Rarity.Rare => random.Next(23, 34),
                Rarity.VeryRare => random.Next(35, 45),
                Rarity.Super => random.Next(46, 56),
                Rarity.Hyper => random.Next(57, 67),
                Rarity.Legendary => random.Next(68, 78),
                _ => random.Next(5, 10)
            };

            FuelCapacity = Rarity switch
            {
                Rarity.Basic => 3,
                Rarity.Common => 4,
                Rarity.Rare => 5,
                Rarity.VeryRare => 6,
                Rarity.Super => 7,
                Rarity.Hyper => 8,
                Rarity.Legendary => 9,
                _ => 3
            };
        }
    }
}