namespace BjjTrainer_API.Models.Payments
{
    public class ProcessedStripeEvent
    {
        public int Id { get; set; }
        public string EventId { get; set; } = default!;
        public DateTime ProcessedAt { get; set; } = DateTime.UtcNow;
    }
}
