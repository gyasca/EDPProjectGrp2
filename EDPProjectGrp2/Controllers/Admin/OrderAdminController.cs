using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Text.Json.Serialization;
using System.Text.Json;
using System.Threading.Tasks;
using Stripe.Climate;

namespace EDPProjectGrp2.Controllers.Admin
{
    [ApiController]
    [Route("Admin/Order")]
    //[Authorize(Roles = "Admin")] // Ensure this route is accessible only by users in the "Admin" role
    public class OrderAdminController : Controller
    {
        private readonly MyDbContext _context;

        public OrderAdminController(MyDbContext context)
        {
            _context = context;
        }

        // GET: All Orders
        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _context.Orders
                .Include(o => o.User) 
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

        // GET: Single Order by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User) 
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Event) 
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other serialization options if needed
            };

            return Ok(JsonSerializer.Serialize(order, options));
        }

        // PUT: Update Order Status
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, OrderStatusUpdateModel updateStatus)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Assuming you have an enum or a set of constants for order statuses
            order.OrderStatus = updateStatus.NewStatus;
            await _context.SaveChangesAsync();

            return Ok("Order status updated successfully.");
        }

        // Additional administrative functionalities like cancelling an order or processing refunds can be added here

    }

    public class OrderStatusUpdateModel
    {
        public string NewStatus { get; set; }
    }
}
