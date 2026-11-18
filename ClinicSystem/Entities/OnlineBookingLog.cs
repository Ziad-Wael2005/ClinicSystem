namespace ClinicSystem.API.Entities
{
    public class OnlineBookingLog
    {
        public long Id { get; set; }

        public string ClientHash { get; set; } = null!;

        public DateTimeOffset CreatedAt { get; set; }
    }
}
