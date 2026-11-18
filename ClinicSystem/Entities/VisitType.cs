using System.ComponentModel.DataAnnotations;

namespace ClinicSystem.API.Entities
{
    public class VisitType
    {
        [Key]
        public string Code { get; set; } = null!;

        public string NameAr { get; set; } = null!;

        public int DurationMin { get; set; }

        public int Price { get; set; }

        public bool PatientCanBook { get; set; } = true;

        public int Sort { get; set; }

        // Navigation Properties
        public ICollection<Booking> CurrentBookings { get; set; } = new List<Booking>();

        public ICollection<Booking> OriginalBookings { get; set; } = new List<Booking>();
    }
}
