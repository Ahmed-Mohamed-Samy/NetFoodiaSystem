using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Entities.IdentityModule;
using System.Reflection;

namespace NetFoodia.Persistence.Data.DbContexts
{
    public class NetFoodiaDbContext : IdentityDbContext<ApplicationUser>
    {
        public NetFoodiaDbContext(DbContextOptions<NetFoodiaDbContext> options) : base(options) { }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);
            builder.Entity<ApplicationUser>().ToTable("Users")
                                             .Property(u => u.CreatedAt)
                                             .HasDefaultValueSql("GETDATE()");
            builder.Entity<IdentityRole>().ToTable("Roles");
            builder.Entity<IdentityUserRole<string>>().ToTable("UserRoles");
            builder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
        }

        public DbSet<RefreshToken> RefreshTokens { get; set; }

    }
}
