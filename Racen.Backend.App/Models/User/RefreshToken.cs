namespace Racen.Backend.App.Models
{
    public class RefreshToken
    {
        public int Id { get; set; } // Primary key
        public string Token { get; set; } = string.Empty;
        public string UserId { get; set; } = string.Empty;
        public DateTime ExpiryDate { get; set; }
        public bool IsRevoked { get; set; }
    }
}