namespace BjjTrainer_API.Models.DTO.Payments
{
    public class SubscriptionRequestDto
    {
        public string CustomerId { get; set; } = default!;
        public string ProductName { get; set; } = default!;
    }
}
