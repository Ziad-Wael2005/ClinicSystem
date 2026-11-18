namespace ClinicSystem.API.Entities
{
    public class Staff
    {
        public Guid Id { get; set; }

        public string Email { get; set; } = null!;

        public string PasswordHash { get; set; } = null!;

        public string Name { get; set; } = null!;

        public string? Shift { get; set; }

        public bool IsActive { get; set; } = true;

        public DateTimeOffset CreatedAt { get; set; }

        // Navigation Properties
        public ICollection<Booking> CreatedBookings { get; set; } = new List<Booking>();
        public ICollection<BookingChange> BookingChanges { get; set; }
    = new List<BookingChange>();
    }
}
