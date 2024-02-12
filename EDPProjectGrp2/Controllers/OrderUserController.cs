using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;
using System.Security.Claims;
using LearningAPI;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("/Order")]
    public class OrderUserController : ControllerBase
    {
        private readonly MyDbContext _context;

        public OrderUserController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Event)
                .ToListAsync();

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other serialization options if needed
            };

            return Ok(JsonSerializer.Serialize(orders, options));
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var order = await _context.Orders
                .Where(o => o.UserId == userId && o.Id == id)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Event)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other serialization options if needed
            };

            return Ok(JsonSerializer.Serialize(order, options));
        }



        [HttpPost]
        public async Task<IActionResult> CreateOrderWithItems([FromBody] OrderWithItemsModel orderWithItemsModel)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);


            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now,
                OrderStatus = orderWithItemsModel.OrderStatus,
                SubTotalAmount = orderWithItemsModel.SubTotalAmount,
                GstAmount = orderWithItemsModel.GstAmount,
                TotalAmount = orderWithItemsModel.TotalAmount,
                NoOfItems = orderWithItemsModel.OrderItems.Count,
                OrderPaymentMethod = orderWithItemsModel.OrderPaymentMethod,
                OrderItems = orderWithItemsModel.OrderItems.Select(oi => new OrderItem
                {
                    EventId = oi.EventId,
                    Quantity = oi.Quantity,
                    TotalPrice = oi.TotalPrice,
                    Discounted = oi.Discounted,
                    DiscountedTotalPrice = oi.DiscountedTotalPrice
                }).ToList()
            };

            _context.Orders.Add(order);
            await _context.SaveChangesAsync();

            return Ok(new { OrderId = order.Id });
        }

        // POST: /Order/SetReceived/{id}
        [HttpPost("SetReceived/{id}")]
        public async Task<IActionResult> SetReceived(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            // Update order status
            order.OrderStatus = "Received"; 
            await _context.SaveChangesAsync();

            // Log the status change
            var orderLog = new OrderLog
            {
                OrderId = order.Id,
                UserId = order.UserId,
                Timestamp = DateTime.UtcNow, 
                ChangedStatus = order.OrderStatus
            };
            _context.Add(orderLog);
            await _context.SaveChangesAsync();

            return Ok();
        }

        [HttpPost("UpdateOrder/{id}")]
        public async Task<IActionResult> UpdateOrder(int id, [FromBody] UpdateOrderModel model)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Event)
                .FirstOrDefaultAsync(o => o.Id == id && o.UserId == userId);

            if (order == null)
            {
                return NotFound();
            }

            // Update order status and payment method
            order.OrderStatus = model.NewStatus;
            order.OrderPaymentMethod = model.NewPaymentMethod;

            // Update related events
            foreach (var item in order.OrderItems)
            {
                var eventItem = item.Event;
                if (eventItem != null)
                {
                    // Update event stock and status here
                    // For example, decrement stock
                    eventItem.EventTicketStock -= item.Quantity;

                    // Update event status based on your business logic
                    // e.g., if stock is 0, mark as Sold Out
                    if (eventItem.EventTicketStock <= 0)
                    {
                        eventItem.EventStatus = false;
                    }
                }
            }

            await _context.SaveChangesAsync();

            return Ok();
        }
    }

    public class UpdateOrderModel
    {
        public string NewStatus { get; set; }
        public string NewPaymentMethod { get; set; }
    }

    public class OrderWithItemsModel
    {
        // Order details
        [Required]
        public string OrderStatus { get; set; }

        [Required]
        public decimal SubTotalAmount { get; set; }

        [Required]
        public decimal GstAmount { get; set; }

        [Required]
        public decimal TotalAmount { get; set; }

        [Required]
        public int NoOfItems { get; set; }

        [Required]
        public string OrderPaymentMethod { get; set; }

        // Order items
        public List<OrderItemModel> OrderItems { get; set; }
    }


    public class OrderItemModel
    {

        [ForeignKey("Event")]
        public int EventId { get; set; }

        [Required]
        public int Quantity { get; set; }

        [Required]
        public decimal TotalPrice { get; set; }
        [Required]
        public decimal Discounted { get; set; }
        [Required]
        public decimal DiscountedTotalPrice { get; set; }
    }

}
