using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.DeliveryModule;

namespace NetFoodia.Persistence.Data.Configurations.DeliveryConfigurations
{
    public class PickupTaskConfiguration : IEntityTypeConfiguration<PickupTask>
    {
        public void Configure(EntityTypeBuilder<PickupTask> builder)
        {
            builder.HasKey(t => t.Id);

            builder.Property(t => t.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.HasOne(t => t.Donation)
                   .WithMany()
                   .HasForeignKey(t => t.DonationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.Charity)
                   .WithMany()
                   .HasForeignKey(t => t.CharityId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(t => t.AssignedVolunteer)
                   .WithMany()
                   .HasForeignKey(t => t.AssignedVolunteerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
