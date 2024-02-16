using Microsoft.AspNetCore.Mvc;

using Stripe;
using System.Security.Claims;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PaymentsController : ControllerBase
    {
        private readonly StripeClient _stripeClient;

        public PaymentsController()
        {
            _stripeClient = new StripeClient("sk_test_51OjvsXKiO8MjoyxdDZyv4ZD892042DZYWamVaqxmfSYN0NZ3lFw0KKuuCYVHadYyLqZoWFgTEXed9CEpzzZH5lGe00nc8ZCQPY");
        }

        [HttpPost("create-payment-intent")]
        public async Task<IActionResult> CreatePaymentIntent([FromBody] PaymentIntentRequest request)
        {
            try
            {

                if (request == null || request.Amount <= 0)
                    return BadRequest(new { error = new { message = "Invalid amount" } });

                long amountInCents = (long)request.Amount;

                var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


                var options = new PaymentIntentCreateOptions
                {
                    Currency = "sgd", 
                    Amount = amountInCents,
                    Metadata = new Dictionary<string, string>
                    {
                        { "OrderId", request.OrderId.ToString() },
                        { "UserId", userId.ToString() }
                    }

                };
                var service = new PaymentIntentService(_stripeClient);
                var paymentIntent = await service.CreateAsync(options);

                return Ok(new { clientSecret = paymentIntent.ClientSecret });
            }
            catch (StripeException e)
            {
                return BadRequest(new { error = new { message = e.Message } });
            }
        }


        [HttpGet("config")]
        public IActionResult GetConfig()
        {
            return Ok(new { publishableKey = "pk_test_51OjvsXKiO8MjoyxdpCgkyuqe5Jq6Bepe4ewQ3wpCCU1C6OTL5rBGLUqz3GFTrHLHnGqqhbHs1T8L3jz48phpF8S900KYOY7aJk" });
        }
    }

    public class PaymentIntentRequest
    {
        public double Amount { get; set; }
        public int OrderId { get; set; }
    }

}
