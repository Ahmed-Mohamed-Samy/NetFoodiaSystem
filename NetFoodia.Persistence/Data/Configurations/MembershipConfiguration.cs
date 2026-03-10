using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.MembershipModule;

namespace NetFoodia.Persistence.Data.Configurations
{
    public class MembershipConfiguration : IEntityTypeConfiguration<VolunteerMembership>
    {
        public void Configure(EntityTypeBuilder<VolunteerMembership> builder)
        {

            builder.HasIndex(x => x.VolunteerId)
                   .IsUnique()
                   .HasFilter("[Status] IN (0,1,2)");

            builder.Property(rt => rt.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}
