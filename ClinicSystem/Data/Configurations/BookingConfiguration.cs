using ClinicSystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSystem.API.Data.Configurations
{
    
        public class BookingConfiguration : IEntityTypeConfiguration<Booking>
        {
            public void Configure(EntityTypeBuilder<Booking> builder)
            {
                builder.ToTable("bookings", t =>
                {
                    t.HasCheckConstraint(
                        "CK_Bookings_PatientName",
                        "LEN(patient_name + N'#') - 1 BETWEEN 2 AND 60");

                    t.HasCheckConstraint(
                        "CK_Bookings_Phone",
                        "LEN(phone + N'#') = 12 AND phone LIKE N'01[0125]%' AND phone NOT LIKE N'%[^0-9]%'");

                    t.HasCheckConstraint(
                        "CK_Bookings_Price",
                        "price >= 0");

                    t.HasCheckConstraint(
                        "CK_Bookings_Status",
                        "status IN ('confirmed', 'cancelled')");

                    t.HasCheckConstraint(
                        "CK_Bookings_Source",
                        "source IN ('online', 'staff')");

                    t.HasCheckConstraint(
                        "CK_Bookings_SlotTime",
                        "DATEPART(MINUTE, slot_time) IN (0, 30) AND DATEPART(SECOND, slot_time) = 0");
                });

                // Primary Key
                builder.HasKey(b => b.Id);

                // ID
                builder.Property(b => b.Id)
                    .HasColumnName("id")
                    .HasDefaultValueSql("NEWID()");

                // Slot Date
                builder.Property(b => b.SlotDate)
                    .HasColumnName("slot_date")
                    .HasColumnType("date")
                    .IsRequired();

                // Slot Time
                builder.Property(b => b.SlotTime)
                    .HasColumnName("slot_time")
                    .HasColumnType("time(0)")
                    .IsRequired();

                // Patient Name
                builder.Property(b => b.PatientName)
                    .HasColumnName("patient_name")
                    .HasMaxLength(60)
                    .IsRequired();

                // Phone
                builder.Property(b => b.Phone)
                    .HasColumnName("phone")
                    .HasMaxLength(11)
                    .IsRequired();

                // Current Visit Type
                builder.Property(b => b.VisitType)
                    .HasColumnName("visit_type")
                    .HasMaxLength(50)
                    .IsRequired();

                // Original Booked Type
                builder.Property(b => b.BookedType)
                    .HasColumnName("booked_type")
                    .HasMaxLength(50)
                    .IsRequired();

                // Price
                builder.Property(b => b.Price)
                    .HasColumnName("price")
                    .IsRequired();

                // Duration
                builder.Property(b => b.DurationMin)
                    .HasColumnName("duration_min")
                    .IsRequired();

                // Status
                builder.Property(b => b.Status)
                    .HasColumnName("status")
                    .HasMaxLength(20)
                    .HasDefaultValue("confirmed")
                    .IsRequired();

                // Source
                builder.Property(b => b.Source)
                    .HasColumnName("source")
                    .HasMaxLength(20)
                    .HasDefaultValue("online")
                    .IsRequired();

                // Created By
                builder.Property(b => b.CreatedBy)
                    .HasColumnName("created_by");

                // Created By Name
                builder.Property(b => b.CreatedByName)
                    .HasColumnName("created_by_name");

                // Created At
                builder.Property(b => b.CreatedAt)
                    .HasColumnName("created_at")
                    .HasDefaultValueSql(
                        "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");

                // Relationships

                builder.HasOne(b => b.CurrentVisitType)
                    .WithMany(v => v.CurrentBookings)
                    .HasForeignKey(b => b.VisitType)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(b => b.OriginalVisitType)
                    .WithMany(v => v.OriginalBookings)
                    .HasForeignKey(b => b.BookedType)
                    .OnDelete(DeleteBehavior.Restrict);

                builder.HasOne(b => b.CreatedByStaff)
                    .WithMany(s => s.CreatedBookings)
                    .HasForeignKey(b => b.CreatedBy)
                    .OnDelete(DeleteBehavior.Restrict);
            // Indexes

            // One active booking per slot
            builder.HasIndex(b => new { b.SlotDate, b.SlotTime })
                .IsUnique()
                .HasFilter("[status] = N'confirmed'")
                .HasDatabaseName("one_booking_per_slot");

            // One active booking per phone per day
            builder.HasIndex(
                    b => new { b.Phone, b.SlotDate },
                    "IX_Bookings_Phone_SlotDate_Unique")
                .IsUnique()
                .HasFilter("[status] = N'confirmed'")
                .HasDatabaseName("one_booking_per_phone_day");

            // Search bookings by phone and date
            builder.HasIndex(
                    b => new { b.Phone, b.SlotDate },
                    "IX_Bookings_Phone_SlotDate_Descending")
                .IsDescending(false, true)
                .HasDatabaseName("bookings_phone_idx");
        }
        }
    }
