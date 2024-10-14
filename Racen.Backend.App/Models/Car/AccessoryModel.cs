namespace Racen.Backend.App.Models.Car
{
    public enum AccessoryType
    {
        Speed,
        Power,
        Aerodynamics
    }

    public enum AccessoryRarity
    {
        Basic,
        Common,
        Rare,
        VeryRare,
        Super,
        Hyper,
        Legendary
    }
    public class AccessoryModel
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public AccessoryType Type { get; set; }
        public AccessoryRarity Rarity { get; set; }
        public int SpeedBonus { get; set; }
        public int AccelerationBonus { get; set; }
        public int AerodynamicsBonus { get; set; }
        public int TyreGripBonus { get; set; }
        public int WeightBonus { get; set; }
        public int PowerBonus { get; set; }
        public int FuelConsumptionBonus { get; set; }
        public required string CarId { get; set; }
        public CarModel Car { get; set; } = default!;
    }
}