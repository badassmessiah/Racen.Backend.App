namespace Racen.Backend.App.DTOs.Car
{
    public class CarReadDto
    {
        public required string Id { get; set; }
        public required string Name { get; set; }
        public int Speed { get; set; }
        public int Acceleration { get; set; }
        public int Aerodynamics { get; set; }
        public int TyreGrip { get; set; }
        public int Weight { get; set; }
        public int Power { get; set; }
        public int FuelConsumption { get; set; }
        public int Level { get; set; }
        public required string OwnerId { get; set; }
    }
}