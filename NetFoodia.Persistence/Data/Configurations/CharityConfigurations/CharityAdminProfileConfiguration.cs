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
    public class CharityAdminProfileConfiguration : IEntityTypeConfiguration<CharityAdminProfile>
    {
        public void Configure(EntityTypeBuilder<CharityAdminProfile> builder)
        {
            builder.HasIndex(p => p.UserId).IsUnique();
            builder.HasIndex(p => p.CharityId).IsUnique();

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
