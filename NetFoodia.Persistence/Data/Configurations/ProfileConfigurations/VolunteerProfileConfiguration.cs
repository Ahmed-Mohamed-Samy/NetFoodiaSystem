using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Persistence.Data.Configurations.ProfileConfigurations
{
    public class VolunteerProfileConfiguration : IEntityTypeConfiguration<VolunteerProfile>
    {
        public void Configure(EntityTypeBuilder<VolunteerProfile> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Ignore(x => x.Id);

            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.VehicleType)
                   .HasMaxLength(100);

            builder.OwnsOne(c => c.Location, loc =>
            {
                loc.Property(p => p.Latitude).HasColumnName("Latitude").HasPrecision(9, 6);
                loc.Property(p => p.Longitude).HasColumnName("Longitude").HasPrecision(9, 6);
            });
        }
    }
}
