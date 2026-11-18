using ClinicSystem.API.Data.Configurations;
using ClinicSystem.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace ClinicSystem.API.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        }
        public DbSet<VisitType> VisitTypes { get; set; }
        public DbSet<Staff> Staffs { get; set; }
        public DbSet<Booking> Bookings { get; set; }
        public DbSet<OnlineBookingLog> OnlineBookingLogs { get; set; }
        public DbSet<BookingChange> BookingChanges { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new VisitTypeConfiguration());

            modelBuilder.ApplyConfiguration(new StaffConfiguration());

            modelBuilder.ApplyConfiguration(new BookingConfiguration());
            modelBuilder.ApplyConfiguration(new BookingChangeConfiguration());
            modelBuilder.ApplyConfiguration(new OnlineBookingLogConfiguration());
        }
    }
}
