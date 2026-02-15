using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.CharityModule;

namespace NetFoodia.Persistence.Data.Configurations.CharityConfigurations
{
    public class CharityAdminProfileConfiguration : IEntityTypeConfiguration<CharityAdminProfile>
    {
        public void Configure(EntityTypeBuilder<CharityAdminProfile> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Ignore(x => x.Id);

            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(p => p.Charity)
                   .WithOne(c => c.AdminProfile)
                   .HasForeignKey<CharityAdminProfile>(p => p.CharityId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }

    }
}
