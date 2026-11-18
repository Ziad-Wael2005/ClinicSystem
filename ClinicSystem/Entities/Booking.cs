namespace ClinicSystem.API.Entities
{
    public class Booking
    {
        public Guid Id { get; set; }

        public DateOnly SlotDate { get; set; }

        public TimeOnly SlotTime { get; set; }

        public string PatientName { get; set; } = null!;

        public string Phone { get; set; } = null!;

        public string VisitType { get; set; } = null!;

        public string BookedType { get; set; } = null!;

        public int Price { get; set; }

        public int DurationMin { get; set; }

        public string Status { get; set; } = "confirmed";

        public string Source { get; set; } = "online";

        public Guid? CreatedBy { get; set; }

        public string? CreatedByName { get; set; }

        public DateTimeOffset CreatedAt { get; set; }

        // Navigation Properties
        public VisitType CurrentVisitType { get; set; } = null!;

        public VisitType OriginalVisitType { get; set; } = null!;

        public Staff? CreatedByStaff { get; set; }
        public ICollection<BookingChange> Changes { get; set; }
    = new List<BookingChange>();
    }
}
