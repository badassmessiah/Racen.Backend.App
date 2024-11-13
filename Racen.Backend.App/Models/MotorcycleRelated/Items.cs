using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Racen.Backend.App.Models.User;

namespace Racen.Backend.App.Models.MotorcycleRelated
{
    public class Items
    {
        [Key]
        [Required]
        public required string Id { get; set; }
        [Required]
        [StringLength(100, MinimumLength = 1, ErrorMessage = "Name must be between 1 and 100 characters.")]
        public required string Name { get; set; }
        [Required]
        public Rarity Rarity { get; set; }
        [Required]
        [Range(0, 50, ErrorMessage = "Speed must be between 0 and 50.")]
        public int Speed { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Power must be between 0 and 100.")]
        public int Power { get; set; }
        [Required]
        [Range(0, 100, ErrorMessage = "Handling must be between 0 and 100.")]
        public int Handling { get; set; }
        [Required]
        public bool Enabled { get; set; }
        [Required]
        public required string OwnerId { get; set; }

        // Navigation property to ApplicationUser
        [ForeignKey("OwnerId")]
        public required ApplicationUser Owner { get; set; }

        // Foreign key to Motorcycle
        public string? MotorcycleId { get; set; }

        // Navigation property to Motorcycle
        [ForeignKey("MotorcycleId")]
        public Motorcycle? Motorcycle { get; set; }
    }
}