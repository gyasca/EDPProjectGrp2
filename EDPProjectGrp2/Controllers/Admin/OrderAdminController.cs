using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;

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
                .Include(o => o.User) // Assuming you have a User entity related to the Order
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Event) // Include Event details if necessary
                .ToListAsync();

            // You might want to select specific fields to return, similar to the EventAdminController
            return Ok(orders);
        }

        // GET: Single Order by ID
        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(int id)
        {
            var order = await _context.Orders
                .Include(o => o.User) // Include User details
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Event) // Include Event details if necessary
                .FirstOrDefaultAsync(o => o.Id == id);

            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // You might want to format the response similarly to the EventAdminController
            return Ok(order);
        }

        // PUT: Update Order Status
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateOrderStatus(int id, [FromBody] OrderStatusUpdateModel model)
        {
            var order = await _context.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound("Order not found.");
            }

            // Assuming you have an enum or a set of constants for order statuses
            order.OrderStatus = model.NewStatus;
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
