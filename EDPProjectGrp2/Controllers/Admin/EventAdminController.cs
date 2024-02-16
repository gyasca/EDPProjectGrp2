using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EDPProjectGrp2.Controllers.Admin
{
    [ApiController]
    [Route("Admin/Event")]
    public class EventAdminController : Controller
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public EventAdminController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // GET: All Events with optional search
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
                e.EventDuration,
                e.EventEndDate,
                e.EventSale,
                e.EventStatus
            });
            return Ok(data);
        }

        // GET: Single Event by ID
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

        // Create an Event
        [HttpPost]
        public IActionResult AddEvent(Event newEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            // Assuming you might have a similar logic for setting a user ID or other properties
            //int userId = GetUserId(); Replace with actual logic to get the user ID
            var now = DateTime.Now;

            var myEvent = new Event()
            {
                EventName = newEvent.EventName.Trim(),
                EventDescription = newEvent.EventDescription.Trim(),
                EventCategory = newEvent.EventCategory.Trim(),
                EventLocation = newEvent.EventLocation.Trim(),
                EventTicketStock = newEvent.EventTicketStock,
                EventPicture = newEvent.EventPicture,
                EventPrice = newEvent.EventPrice,
                EventDiscountPrice = newEvent.EventDiscountPrice,
                EventUplayMemberPrice = newEvent.EventUplayMemberPrice,
                EventNtucClubPrice = newEvent.EventNtucClubPrice,
                EventDate = newEvent.EventDate,
                EventEndDate = newEvent.EventEndDate,
                EventDuration = newEvent.EventDuration,
                EventSale = newEvent.EventSale,
                EventStatus = newEvent.EventStatus,
                // UserId = userId // Include this line if you have a UserId property in the Event model

            };

            _context.Events.Add(myEvent);
            _context.SaveChanges();
            return Ok(myEvent);
        }


        //Edit Single Event
        [HttpPut("{id}")]
        public IActionResult UpdateEvent(int id, Event updatedEvent)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var myEvent = _context.Events.Find(id);
            if (myEvent == null)
            {
                return NotFound();
            }
            myEvent.EventName = updatedEvent.EventName;
            myEvent.EventDescription = updatedEvent.EventDescription;
            myEvent.EventCategory = updatedEvent.EventCategory;
            myEvent.EventLocation = updatedEvent.EventLocation;
            myEvent.EventTicketStock = updatedEvent.EventTicketStock;
            myEvent.EventPicture = updatedEvent.EventPicture;
            myEvent.EventPrice = updatedEvent.EventPrice;
            myEvent.EventUplayMemberPrice = updatedEvent.EventUplayMemberPrice;
            myEvent.EventNtucClubPrice = updatedEvent.EventNtucClubPrice;
            myEvent.EventDate = updatedEvent.EventDate;
            myEvent.EventEndDate = updatedEvent.EventEndDate;
            myEvent.EventDuration = updatedEvent.EventDuration;
            myEvent.EventSale = updatedEvent.EventSale;
            myEvent.EventStatus = updatedEvent.EventStatus;

            _context.SaveChanges();
            return Ok();
        }

        // PUT: Deactivate Event Status
        [HttpPut("deactivate/{id}")]
        public async Task<IActionResult> DeactivateEventStatus(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.EventStatus = false;

            _context.SaveChanges();
            return Ok();
        }

        // PUT: Activate Event Status
        [HttpPut("activate/{id}")]
        public async Task<IActionResult> ActivateEventStatus(int id)
        {
            var eventItem = await _context.Events.FindAsync(id);
            if (eventItem == null)
            {
                return NotFound();
            }

            eventItem.EventStatus = true;

            _context.SaveChanges();
            return Ok();
        }

    }
}
