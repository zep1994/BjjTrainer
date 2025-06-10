using System.ComponentModel.DataAnnotations;

namespace BjjTrainer_API.Models.Users
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }

        public string? Token { get; set; }  // The token string itself
        public string? UserId { get; set; } // User that this refresh token is associated with
        public DateTime Expires { get; set; }  // Expiration date of the refresh token
        public DateTime Created { get; set; }  // When the refresh token was created
        public DateTime? Revoked { get; set; }  // When the token was revoked (null if not revoked)

        // Read-only properties to check token state
        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;

        // Navigation property to the user (optional, but helpful for easier relationships)
        public ApplicationUser? User { get; set; }
    }
}
