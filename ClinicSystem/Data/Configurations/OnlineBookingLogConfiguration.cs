using ClinicSystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSystem.API.Data.Configurations
{
    public class OnlineBookingLogConfiguration
        : IEntityTypeConfiguration<OnlineBookingLog>
    {
        public void Configure(EntityTypeBuilder<OnlineBookingLog> builder)
        {
            builder.ToTable("online_booking_log");

            builder.HasKey(x => x.Id);

            builder.Property(x => x.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            builder.Property(x => x.ClientHash)
    .HasColumnName("client_hash")
    .HasMaxLength(64)
    .IsRequired();
            builder.Property(x => x.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql(
                    "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");

            // Index
            builder.HasIndex(x => new { x.ClientHash, x.CreatedAt })
                .IsDescending(false, true)
                .HasDatabaseName("online_booking_log_client_time");
        }
    }
}
