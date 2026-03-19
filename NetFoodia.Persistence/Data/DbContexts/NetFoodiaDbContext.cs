using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using NetFoodia.Domain.Entities.CharityModule;
using NetFoodia.Domain.Entities.IdentityModule;
using NetFoodia.Domain.Entities.MembershipModule;
using NetFoodia.Domain.Entities.ProfileModule;
using NetFoodia.Domain.Entities.DonationModule;
using NetFoodia.Domain.Entities.DeliveryModule;
using NetFoodia.Domain.Entities.NotificationModule;
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
        public DbSet<Charity> Charities { get; set; }
        public DbSet<CharityAdminProfile> CharityAdminProfiles { get; set; }
        public DbSet<VolunteerProfile> VolunteerProfiles { get; set; }
        public DbSet<DonorProfile> DonorProfiles { get; set; }
        public DbSet<VolunteerMembership> Memberships { get; set; }
        public DbSet<Donation> Donations { get; set; }
        public DbSet<PickupTask> PickupTasks { get; set; }
        public DbSet<AssignmentAttempt> AssignmentAttempts { get; set; }
        public DbSet<Notification> Notifications { get; set; }

    }
}
