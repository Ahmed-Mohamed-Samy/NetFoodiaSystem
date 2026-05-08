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

            // Composite index to efficiently support the periodic orphaned-task query:
            // WHERE Status = 'Open' AND SlaDueAt IS NOT NULL AND SlaDueAt < @utcnow
            builder.HasIndex(t => new { t.Status, t.SlaDueAt })
                   .HasDatabaseName("IX_PickupTasks_Status_SlaDueAt");

            builder.Property(t => t.RowVersion)
                   .IsRowVersion();
        }
    }
}
