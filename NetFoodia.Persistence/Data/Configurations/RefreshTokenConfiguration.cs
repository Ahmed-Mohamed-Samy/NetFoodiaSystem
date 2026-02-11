using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using NetFoodia.Domain.Entities.IdentityModule;

namespace NetFoodia.Persistence.Data.Configurations
{
    public class RefreshTokenConfiguration : IEntityTypeConfiguration<RefreshToken>
    {
        public void Configure(EntityTypeBuilder<RefreshToken> builder)
        {
            builder.HasIndex(rt => rt.Token);

            builder.HasOne(rt => rt.User)
                .WithMany(u => u.RefreshTokens)
                .HasForeignKey(rt => rt.UserId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(rt => rt.CreatedAt)
                .HasDefaultValueSql("GETDATE()");
        }
    }
}
