using LearningAPI;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("/Event")]
    public class EventController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public EventController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // get all events
        [HttpGet]
        public IActionResult GetAll(string? search)
        {
            IQueryable<Event> result = _context.Events;
            if (search != null)
            {
                result = result.Where(x => x.EventName.Contains(search)
                    || x.EventDescription.Contains(search));
            }
            var list = result.OrderByDescending(x => x.EventDate).ToList();
            var data = list.Select(e => new
            {
                e.Id,
                e.EventName,
                e.EventDescription,
                e.EventCategory,
                e.EventLocation,
                e.EventTicketStock,
                e.EventPicture,
                e.EventPrice,
                e.EventDiscountPrice,
                e.EventUplayMemberPrice,
                e.EventNtucClubPrice,
                e.EventDate,
                e.EventEndDate,
                e.EventDuration,
                e.EventSale,
                e.EventStatus
            });
            return Ok(data);
        }


        // Single event
        [HttpGet("{id}")]
        public IActionResult GetEvent(int id)
        {
            Event? myEvent = _context.Events.FirstOrDefault(e => e.Id == id);
            if (myEvent == null)
            {
                return NotFound();
            }
            var data = new
            {
                myEvent.Id,
                myEvent.EventName,
                myEvent.EventDescription,
                myEvent.EventCategory,
                myEvent.EventLocation,
                myEvent.EventTicketStock,
                myEvent.EventPicture,
                myEvent.EventPrice,
                myEvent.EventDiscountPrice,
                myEvent.EventUplayMemberPrice,
                myEvent.EventNtucClubPrice,
                myEvent.EventDate,
                myEvent.EventEndDate,
                myEvent.EventDuration,
                myEvent.EventSale,
                myEvent.EventStatus
            };
            return Ok(data);
        }


    }
}
