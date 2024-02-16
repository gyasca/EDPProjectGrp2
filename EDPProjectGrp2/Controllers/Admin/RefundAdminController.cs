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
    [Route("Admin/Refund")]
    public class AdminRefundController : Controller
    {
        private readonly MyDbContext _context;

        public AdminRefundController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllRefunds()
        {
            var refunds = await _context.Refunds
                .Include(r => r.Order)
                .Include(r => r.User)
                .ToListAsync();

            return Ok(refunds);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetRefund(int id)
        {
            var refund = await _context.Refunds
                .Include(r => r.Order)
                .Include(r => r.User)
                .FirstOrDefaultAsync(r => r.Id == id);

            if (refund == null)
            {
                return NotFound("Refund not found.");
            }

            return Ok(refund);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRefundStatus(RefundStatusUpdateModel updateStatus)
        {
            var refund = await _context.Refunds.FindAsync(updateStatus.RefundId);
            if (refund == null)
            {
                return NotFound("Refund not found.");
            }

            var order = await _context.Orders.FindAsync(refund.OrderId);
            if (order == null)
            {
                return NotFound("Order associated with the refund not found.");
            }

            refund.RefundStatus = updateStatus.NewStatus;

            if (updateStatus.NewStatus == "Refund Approved")
            {
                order.OrderStatus = "Refund Approved";
            }
            else if (updateStatus.NewStatus == "Refund Rejected")
            {
                order.OrderStatus = "Refund Rejected";
            }

            await _context.SaveChangesAsync();

            return Ok("Refund status and associated order status updated successfully.");
        }



    }

    public class RefundStatusUpdateModel
    {
        public int RefundId { get; set; }
        public string NewStatus { get; set; }
    }

}
