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
    public class AssignmentAttemptConfiguration : IEntityTypeConfiguration<AssignmentAttempt>
    {
        public void Configure(EntityTypeBuilder<AssignmentAttempt> builder)
        {
            builder.HasKey(a => a.Id);

            builder.Property(a => a.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(a => a.DistanceKm)
                   .HasColumnType("real");

            builder.Property(a => a.EtaMinutes)
                   .HasColumnType("real");

            builder.Property(a => a.ScoreAtOffer)
                   .HasColumnType("real");

            builder.Property(a => a.CandidateLoad)
                   .IsRequired();

            builder.Property(a => a.Response)
                   .IsRequired();

            builder.Property(a => a.Outcome);

            builder.HasOne(a => a.PickupTask)
                   .WithMany()
                   .HasForeignKey(a => a.PickupTaskId)
                   .OnDelete(DeleteBehavior.Cascade);

            builder.HasOne(a => a.Donation)
                   .WithMany()
                   .HasForeignKey(a => a.DonationId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(a => a.Volunteer)
                   .WithMany()
                   .HasForeignKey(a => a.VolunteerId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}
