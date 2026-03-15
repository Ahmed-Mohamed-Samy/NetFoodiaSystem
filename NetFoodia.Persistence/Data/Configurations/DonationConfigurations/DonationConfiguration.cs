using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.DonationModule;

namespace NetFoodia.Persistence.Data.Configurations.DonationConfigurations
{
    public class DonationConfiguration : IEntityTypeConfiguration<Donation>
    {
        public void Configure(EntityTypeBuilder<Donation> builder)
        {
            builder.HasKey(d => d.Id);

            builder.Property(d => d.FoodType)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(d => d.Quantity)
                   .IsRequired();

            builder.Property(d => d.PreparedTime)
                   .IsRequired();

            builder.Property(d => d.ExpirationTime)
                   .IsRequired();

            builder.Property(d => d.Notes)
                   .HasMaxLength(1000);

            builder.Property(d => d.UrgencyScore)
                   .HasColumnType("real");

            builder.Property(d => d.Status)
                   .IsRequired();

            builder.Property(d => d.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.Property(d => d.AcceptedAt);
            builder.Property(d => d.AssignedAt);
            builder.Property(d => d.PickedUpAt);
            builder.Property(d => d.DeliveredAt);

            builder.OwnsOne(d => d.PickupLocation, location =>
            {
                location.Property(l => l.Latitude)
                        .HasColumnName("PickupLatitude")
                        .HasColumnType("decimal(18,6)");

                location.Property(l => l.Longitude)
                        .HasColumnName("PickupLongitude")
                        .HasColumnType("decimal(18,6)");
            });

            builder.HasOne(d => d.Donor)
                   .WithMany()
                   .HasForeignKey(d => d.DonorId)
                   .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(d => d.Charity)
                   .WithMany()
                   .HasForeignKey(d => d.CharityId)
                   .OnDelete(DeleteBehavior.Restrict);
        }
    }
}