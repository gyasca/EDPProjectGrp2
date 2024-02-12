using LearningAPI;
using EDPProjectGrp2.Models;
using Microsoft.AspNetCore.Mvc;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public TicketsController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Ticket>> GetTickets()
        {
            return _context.Tickets.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Ticket> GetTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            return ticket;
        }

        [HttpPost]
        public IActionResult CreateTicket([FromBody] Ticket ticket)
        {
            _context.Tickets.Add(ticket);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetTicket), new { id = ticket.Id }, ticket);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateTicket(int id, [FromBody] Ticket updatedTicket)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            ticket.Subject = updatedTicket.Subject;
            ticket.Description = updatedTicket.Description;
            ticket.Status = updatedTicket.Status;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteTicket(int id)
        {
            var ticket = _context.Tickets.Find(id);

            if (ticket == null)
            {
                return NotFound();
            }

            _context.Tickets.Remove(ticket);
            _context.SaveChanges();

            return NoContent();
        }
    }

}

