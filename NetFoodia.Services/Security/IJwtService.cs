using NetFoodia.Domain.Entities.IdentityModule;

namespace NetFoodia.Services.Security
{
    public interface IJwtService
    {
        Task<string> GenerateTokenAsync(ApplicationUser user);
    }
}
