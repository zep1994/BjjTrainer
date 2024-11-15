using BjjTrainer_API.Models.User;
using System.ComponentModel.DataAnnotations;

namespace BjjTrainer_API.Models.Users
{
    public class RefreshToken
    {
        [Key]
        public int Id { get; set; }
        public string? Token { get; set; }
        public string? UserId { get; set; }
        public DateTime? Expires { get; set; }
        public DateTime? Created { get; set; }
        public DateTime? Revoked { get; set; }

        public bool IsActive => Revoked == null && !IsExpired;
        public bool IsExpired => DateTime.UtcNow >= Expires;

        public ApplicationUser? User { get; set; }
    }
}
