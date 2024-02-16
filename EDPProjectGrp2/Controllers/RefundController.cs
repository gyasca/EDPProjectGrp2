using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;
using System.Security.Claims;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text.Json.Serialization;
using System.Text.Json;
using LearningAPI;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("/Refund")]
    public class RefundController : ControllerBase
    {
        private readonly MyDbContext _context;

        public RefundController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetRefund(int orderId)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var refund = await _context.Refunds
                .Where(r => r.OrderId == orderId && r.UserId == userId)
                .FirstOrDefaultAsync();

            if (refund == null)
            {
                return NotFound();
            }

            var options = new JsonSerializerOptions
            {
                ReferenceHandler = ReferenceHandler.Preserve,
                // Other serialization options if needed
            };

            return Ok(JsonSerializer.Serialize(refund, options));
        }

        [HttpPost]
        public async Task<IActionResult> RequestRefund([FromBody] RefundRequestModel refundRequest)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var order = await _context.Orders
                .Where(o => o.Id == refundRequest.OrderId && o.UserId == userId)
                .FirstOrDefaultAsync();

            if (order == null)
            {
                return NotFound("Order not found");
            }

            // Set order status to "Refund Processing"
            order.OrderStatus = "Refund Processing";
            await _context.SaveChangesAsync();

            // Create a refund record
            var refund = new Refund
            {
                RequestRefundDate = DateTime.Now,
                RefundReason = refundRequest.RefundReason,
                RefundAmount = refundRequest.RefundAmount,
                RefundStatus = "Pending", 
                OrderId = refundRequest.OrderId,
                UserId = userId
            };

            _context.Refunds.Add(refund);
            await _context.SaveChangesAsync();

            return Ok(refund);
        }
    }

    public class RefundRequestModel
    {
        [Required]
        public int OrderId { get; set; }

        [Required]
        public string RefundReason { get; set; }

        [Required]
        public decimal RefundAmount { get; set; }
    }
}
