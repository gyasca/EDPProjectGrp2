using Microsoft.AspNetCore.Mvc;
using Stripe;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using EDPProjectGrp2.Models; 
using Microsoft.EntityFrameworkCore; 
using LearningAPI;
using System.Security.Claims;

namespace EDPProjectGrp2.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class StripeWebhookController : Controller
    {
        private readonly MyDbContext _context;

        public StripeWebhookController(MyDbContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<IActionResult> Index()
        {
            var json = await new StreamReader(HttpContext.Request.Body).ReadToEndAsync();

            try
            {
                const string endpointSecret = "whsec_b42f5835a2160c4a7ad230c069946672784c52d1a22df318e44c1c965fe55eab";
                var stripeEvent = EventUtility.ConstructEvent(json, Request.Headers["Stripe-Signature"], endpointSecret);

                // Handle the event
                if (stripeEvent.Type == Events.PaymentIntentSucceeded)
                {
                    var paymentIntent = stripeEvent.Data.Object as PaymentIntent;

                    // Retrieve order ID and user ID from payment intent metadata
                    if (paymentIntent.Metadata.TryGetValue("OrderId", out var orderIdStr) && int.TryParse(orderIdStr, out var orderId)
                        && paymentIntent.Metadata.TryGetValue("UserId", out var userIdStr) && int.TryParse(userIdStr, out var userId))
                    {
                        // Update the order status
                        await UpdateOrder(orderId, userId);

                    }

          
                }
                else if (stripeEvent.Type == Events.PaymentMethodAttached)
                {
                    var paymentMethod = stripeEvent.Data.Object as PaymentMethod;
                    // Handle payment method attachment here
                }
                // Add more event types to handle here if needed

                return Ok();
            }
            catch (StripeException e)
            {
                return BadRequest(e.Message);
            }
        }

        private async Task<IActionResult> UpdateOrder(int orderId, int userId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Event)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                order.OrderStatus = "Order Processing";
                order.OrderPaymentMethod = "Stripe";

                foreach (var item in order.OrderItems)
                {
                    var eventItem = item.Event;
                    if (eventItem != null)
                    {
                        eventItem.EventTicketStock -= item.Quantity;
                        if (eventItem.EventTicketStock <= 0)
                        {
                            eventItem.EventStatus = false;
                        }
                    }
                }

                await _context.SaveChangesAsync();
                await ClearCart(userId, orderId);
                return Ok("Order updated successfully");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating order: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


        private async Task<IActionResult> ClearCart(int userId, int orderId)
        {
            try
            {
                var order = await _context.Orders
                    .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Event)
                    .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

                if (order == null)
                {
                    return NotFound("Order not found");
                }

                var orderItems = order.OrderItems;

                foreach (var item in orderItems)
                {
                    var cartItem = await _context.Carts
                        .FirstOrDefaultAsync(c => c.UserId == userId && c.EventId == item.EventId);

                    if (cartItem != null)
                    {
                        _context.Carts.Remove(cartItem);
                    }
                }

                await _context.SaveChangesAsync();

                return Ok();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error clearing cart items: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
        }


    }
}
