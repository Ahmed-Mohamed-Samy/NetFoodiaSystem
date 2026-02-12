using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.CharityModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NetFoodia.Persistence.Data.Configurations.CharityConfigurations
{
    public class CharityConfiguration : IEntityTypeConfiguration<Charity>
    {
        public void Configure(EntityTypeBuilder<Charity> builder)
        {
            builder.HasIndex(c => c.OrganizationName);

            builder.Property(c => c.OrganizationName)
                   .IsRequired()
                   .HasMaxLength(200);

            builder.Property(c => c.LicenseNumber)
                   .IsRequired()
                   .HasMaxLength(100);

            builder.Property(c => c.CreatedAt)
                   .HasDefaultValueSql("GETDATE()");

            builder.OwnsOne(c => c.Location, loc =>
            {
                loc.Property(p => p.Latitude).HasColumnName("Latitude").HasPrecision(9, 6);
                loc.Property(p => p.Longitude).HasColumnName("Longitude").HasPrecision(9, 6);
            });

        }
    }
}
