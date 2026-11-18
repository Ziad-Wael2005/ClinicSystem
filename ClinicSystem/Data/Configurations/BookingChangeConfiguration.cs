using ClinicSystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSystem.API.Data.Configurations
{
    public class BookingChangeConfiguration : IEntityTypeConfiguration<BookingChange>
    {
        public void Configure(EntityTypeBuilder<BookingChange> builder)
        {
            builder.ToTable("booking_changes", t =>
            {
                t.HasCheckConstraint(
                    "CK_BookingChanges_Action",
                    "action IN ('create', 'change_type', 'cancel')");

                t.HasCheckConstraint(
                    "CK_BookingChanges_Reason",
                    "LEN(reason + N'#') - 1 BETWEEN 3 AND 200");
            });

            // Primary Key
            builder.HasKey(b => b.Id);

            // ID
            builder.Property(b => b.Id)
                .HasColumnName("id")
                .UseIdentityColumn();

            // Booking ID
            builder.Property(b => b.BookingId)
                .HasColumnName("booking_id");

            // Action
            builder.Property(b => b.Action)
                .HasColumnName("action")
                .HasMaxLength(20)
                .IsRequired();

            // Old Type
            builder.Property(b => b.OldType)
                .HasColumnName("old_type")
                .HasMaxLength(50);

            // New Type
            builder.Property(b => b.NewType)
                .HasColumnName("new_type")
                .HasMaxLength(50);

            // Old Price
            builder.Property(b => b.OldPrice)
                .HasColumnName("old_price");

            // New Price
            builder.Property(b => b.NewPrice)
                .HasColumnName("new_price");

            // Reason
            builder.Property(b => b.Reason)
                .HasColumnName("reason")
                .HasMaxLength(200)
                .IsRequired();

            // Changed By
            builder.Property(b => b.ChangedBy)
                .HasColumnName("changed_by");

            // Changed By Name
            builder.Property(b => b.ChangedByName)
                .HasColumnName("changed_by_name")
                .IsRequired();

            // Changed At
            builder.Property(b => b.ChangedAt)
                .HasColumnName("changed_at")
                .HasDefaultValueSql(
                    "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");

            // Relationships
            builder.HasOne(b => b.Booking)
                .WithMany(b => b.Changes)
                .HasForeignKey(b => b.BookingId)
                .OnDelete(DeleteBehavior.Restrict);

            builder.HasOne(b => b.ChangedByStaff)
                .WithMany(s => s.BookingChanges)
                .HasForeignKey(b => b.ChangedBy)
                .OnDelete(DeleteBehavior.Restrict);

            // Indexes
            builder.HasIndex(b => b.BookingId)
                .HasDatabaseName("booking_changes_booking_idx");
            builder.HasIndex(b => b.ChangedAt)
                .IsDescending(true)
                .HasDatabaseName("booking_changes_time_idx")
                .HasAnnotation(
                    "Relational:IndexSortOrder",
                    new[] { true });
        }
    }
}
