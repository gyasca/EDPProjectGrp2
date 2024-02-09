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

        // GET: /Order
        [HttpGet]
        public async Task<IActionResult> GetOrders()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var orders = await _context.Orders
                .Where(o => o.UserId == userId)
                .Include(o => o.OrderItems)
                .ThenInclude(oi => oi.Event)
                .ToListAsync();

            return Ok(orders);
        }

        // GET: /Order/{id}
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

            return Ok(order);
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] OrderModel orderModel)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var order = new Order
            {
                UserId = userId,
                OrderDate = DateTime.Now, 
                OrderStatus = orderModel.OrderStatus,
                SubTotalAmount = orderModel.SubTotalAmount,
                GstAmount = orderModel.GstAmount,
                TotalAmount = orderModel.TotalAmount,
                NoOfItems = orderModel.NoOfItems,
                OrderPaymentMethod = orderModel.OrderPaymentMethod,
                OrderItems = orderModel.OrderItems
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
    }


    public class OrderModel
    {
        [Required]
        public DateTime OrderDate { get; set; }

        [Required]
        [StringLength(255)]
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
        [StringLength(255)]
        public string OrderPaymentMethod { get; set; }

        public virtual ICollection<OrderItem> OrderItems { get; set; } = new List<OrderItem>();
    }
    
}
