using Microsoft.AspNetCore.Identity;

namespace NetFoodia.Domain.Entities.IdentityModule
{
    public class ApplicationUser : IdentityUser
    {
        public string FullName { get; set; } = default!;
        public string Role { get; set; } = default!;
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        public bool IsActive { get; set; }
        public ICollection<RefreshToken> RefreshTokens { get; set; } = new List<RefreshToken>();
    }
}
