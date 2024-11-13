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
        [Range(0, 420, ErrorMessage = "Speed must be between 0 and 500.")]
        public int Speed { get; set; }
        [Required]
        [Range(50, 1000, ErrorMessage = "Power must be between 50 and 1000.")]
        public int Power { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Handling must be between 0 and 100.")]
        public int Handling { get; set; }
        [Required]
        public Rarity Rarity { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        [ForeignKey("OwnerId")]
        public required string OwnerId { get; set; }
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

        private void SetDefaultProperties()
        {
            var random = new Random();

            Enabled = true;

            Speed = Rarity switch
            {
                Rarity.Basic => random.Next(40,60),
                Rarity.Common => random.Next(80,110),
                Rarity.Rare => random.Next(130,160),
                Rarity.VeryRare => random.Next(170, 210),
                Rarity.Super => random.Next(230, 260),
                Rarity.Hyper => random.Next(280, 310),
                Rarity.Legendary => random.Next(330, 360),
                _ => random.Next(40,60)
            };

            Power = Rarity switch
            {
                Rarity.Basic => random.Next(50, 100),
                Rarity.Common => random.Next(100, 150),
                Rarity.Rare => random.Next(150, 200),
                Rarity.VeryRare => random.Next(200, 250),
                Rarity.Super => random.Next(250, 300),
                Rarity.Hyper => random.Next(300, 350),
                Rarity.Legendary => random.Next(350, 400),
                _ => random.Next(50, 100)
            };

            Handling = Rarity switch
            {
                Rarity.Basic => random.Next(5, 20),
                Rarity.Common => random.Next(20, 40),
                Rarity.Rare => random.Next(40, 60),
                Rarity.VeryRare => random.Next(60, 80),
                Rarity.Super => random.Next(80, 90),
                Rarity.Hyper => random.Next(90, 95),
                Rarity.Legendary => random.Next(95, 100),
                _ => random.Next(5, 20)
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
