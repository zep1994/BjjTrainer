using BjjTrainer_API.Data;
using BjjTrainer_API.Models.DTO.Payments;
using BjjTrainer_API.Models.Payments;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Stripe;
using Stripe.Checkout;

namespace BjjTrainer_API.Controllers.Payments
{
    [ApiController]
    [Route("api/[controller]")]
    public class PaymentsController(IConfiguration configuration) : ControllerBase
    {

        private static readonly Dictionary<string, string> ProductPriceMap = new()
        {
            { "Onboarding/Setup Fee", "price_1RcVjNH0B7ycUErcnEfSuYyu" },
            { "Transaction Fee", "price_1RcVinH0B7ycUErcl2NazffO" },
            { "Drop-in Fee", "price_1RcVd9H0B7ycUErcXaSozpnr" },
            { "Pro Plan", "price_1RcVauH0B7ycUErcPrvcPutj" },
            { "Growth Plan", "price_1RcVaUH0B7ycUErceuHfm6MA" },
            { "Starter Plan", "price_1RcVa5H0B7ycUErcJMWi0EDo" }
        };

        private readonly IConfiguration _configuration = configuration;

        [HttpPost("create-customer")]
        public async Task<IActionResult> CreateCustomer([FromBody] CreateCustomerDto dto)
        {
            var options = new CustomerCreateOptions { Email = dto.Email };
            var service = new CustomerService();
            var customer = await service.CreateAsync(options);
            return Ok(customer.Id);
        }

        [HttpPost("create-subscription-session")]
        public async Task<IActionResult> CreateSubscriptionSession([FromBody] SubscriptionRequestDto request)
        {
            if (!ProductPriceMap.TryGetValue(request.ProductName, out var priceId))
                return BadRequest("Invalid product name.");

            var options = new SessionCreateOptions
            {
                Customer = request.CustomerId,
                PaymentMethodTypes = ["card"],
                Mode = "subscription",
                LineItems =
                [
                    new SessionLineItemOptions
                    {
                        Price = priceId,
                        Quantity = 1,
                    },
                ],
                SuccessUrl = "http://localhost:5057/api/success?session_id={CHECKOUT_SESSION_ID}",
                CancelUrl = "http://localhost:5057/api/cancel",
            };
            var service = new SessionService();
            var session = await service.CreateAsync(options);
            return Ok(new { sessionId = session.Id, url = session.Url });
        }

        [HttpPost("webhook")]
        public async Task<IActionResult> StripeWebhook([FromServices] ApplicationDbContext db)
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();
            var stripeEvent = EventUtility.ConstructEvent(
                json,
                Request.Headers["Stripe-Signature"],
                _configuration["Stripe:WebhookSecret"]
            );

            // Idempotency check
            if (await db.ProcessedStripeEvents.AnyAsync(e => e.EventId == stripeEvent.Id))
                return Ok(); // Already processed

            if (stripeEvent.Type == "customer.subscription.created" ||
                stripeEvent.Type == "customer.subscription.updated" ||
                stripeEvent.Type == "customer.subscription.deleted")
            {
                if (stripeEvent.Data.Object is not Subscription subscription)
                    return Ok();

                var user = db.ApplicationUsers.FirstOrDefault(u => u.StripeCustomerId == subscription.CustomerId);
                if (user == null)
                    return Ok();

                user.StripeSubscriptionId = subscription.Id;
                user.MembershipPlan = subscription.Items.Data.FirstOrDefault()?.Price.Nickname ?? "Unknown";
                user.MembershipStatus = subscription.Status; 

                user.MembershipStartDate = subscription.StartDate.ToUniversalTime();
                user.MembershipEndDate = subscription.EndedAt?.ToUniversalTime();

                await db.SaveChangesAsync();
            }

            // Mark event as processed
            db.ProcessedStripeEvents.Add(new ProcessedStripeEvent { EventId = stripeEvent.Id });
            await db.SaveChangesAsync();

            return Ok();
        }
    }
}