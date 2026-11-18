namespace ClinicSystem.API.Entities
{
    public class BookingChange
    {
        public long Id { get; set; }

        public Guid BookingId { get; set; }

        public string Action { get; set; } = null!;

        public string? OldType { get; set; }

        public string? NewType { get; set; }

        public int? OldPrice { get; set; }

        public int? NewPrice { get; set; }

        public string Reason { get; set; } = null!;

        public Guid ChangedBy { get; set; }

        public string ChangedByName { get; set; } = null!;

        public DateTimeOffset ChangedAt { get; set; }

        // Navigation Properties
        public Booking Booking { get; set; } = null!;

        public Staff ChangedByStaff { get; set; } = null!;
    }
}
