using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.ProfileModule;

namespace NetFoodia.Persistence.Data.Configurations.ProfileConfigurations
{
    public class DonorProfileConfiguration : IEntityTypeConfiguration<DonorProfile>
    {
        public void Configure(EntityTypeBuilder<DonorProfile> builder)
        {
            builder.HasKey(x => x.UserId);

            builder.Ignore(x => x.Id);

            builder.HasOne(p => p.User)
                   .WithMany()
                   .HasForeignKey(p => p.UserId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.Property(p => p.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(x => x.BusinessType)
                   .HasMaxLength(100);

            builder.Property(x => x.ReliabilityScore)
                   .HasPrecision(5, 2);

            builder.OwnsOne(c => c.Location, loc =>
            {
                loc.Property(p => p.Latitude).HasColumnName("Latitude").HasPrecision(9, 6);
                loc.Property(p => p.Longitude).HasColumnName("Longitude").HasPrecision(9, 6);
            });

            builder.ToTable(t =>
            {
                t.HasCheckConstraint("CK_DonorProfile_ReliabilityScore_NonNegative", "[ReliabilityScore] >= 0");
                t.HasCheckConstraint("CK_DonorProfile_BusinessRules",
                    "([IsBusiness] = 1 AND [BusinessType] IS NOT NULL) OR ([IsBusiness] = 0 AND [BusinessType] IS NULL AND [IsVerified] = 0)");
            });
        }
    }
}
