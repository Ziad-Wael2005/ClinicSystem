using ClinicSystem.API.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace ClinicSystem.API.Data.Configurations
{
    public class StaffConfiguration : IEntityTypeConfiguration<Staff>
    {
        public void Configure(EntityTypeBuilder<Staff> builder)
        {
            builder.ToTable("staff", t =>
            {
                t.HasCheckConstraint(
                    "CK_Staff_Name",
                    "LEN(name + N'#') - 1 BETWEEN 2 AND 40");

                t.HasCheckConstraint(
                    "CK_Staff_Shift",
                    "shift IS NULL OR LEN(shift + N'#') - 1 <= 40");
            });

            // Primary Key
            builder.HasKey(s => s.Id);

            // ID
            builder.Property(s => s.Id)
                .HasColumnName("id")
                .HasDefaultValueSql("NEWID()");

            // Email
            builder.Property(s => s.Email)
 .HasColumnName("email")
 .HasMaxLength(254)
 .IsRequired();

            builder.HasIndex(s => s.Email)
                .IsUnique();

            // Password Hash
            builder.Property(s => s.PasswordHash)
                .HasColumnName("password_hash")
                .IsRequired();

            // Name
            builder.Property(s => s.Name)
                .HasColumnName("name")
                .HasMaxLength(40)
                .IsRequired();

            // Shift
            builder.Property(s => s.Shift)
                .HasColumnName("shift")
                .HasMaxLength(40)
                .IsRequired(false);

            // Is Active
            builder.Property(s => s.IsActive)
                .HasColumnName("is_active")
                .HasDefaultValue(true)
                .IsRequired();

            // Created At
            builder.Property(s => s.CreatedAt)
                .HasColumnName("created_at")
                .HasDefaultValueSql(
                    "TODATETIMEOFFSET(SYSUTCDATETIME(), '+00:00')");

         
        }
    }
}
