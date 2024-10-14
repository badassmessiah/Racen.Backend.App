namespace Racen.Backend.App.DTOs.Car
{
    public class AddAccessoryDto
    {
        public required string UserId { get; set; }
        public required string AccessoryId { get; set; }
    }
}