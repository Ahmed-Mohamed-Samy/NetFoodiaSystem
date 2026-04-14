using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.InspectionModule;

namespace NetFoodia.Persistence.Data.Configurations
{
    public class FoodInspectionConfiguration : IEntityTypeConfiguration<FoodInspection>
    {
        public void Configure(EntityTypeBuilder<FoodInspection> builder)
        {

            builder.HasOne(i => i.Donation)
                   .WithOne(d => d.Inspection)
                   .HasForeignKey<FoodInspection>(i => i.DonationId)
                   .OnDelete(DeleteBehavior.Cascade);


            builder.HasIndex(i => i.DonationId)
                   .IsUnique();

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");
        }
    }
}