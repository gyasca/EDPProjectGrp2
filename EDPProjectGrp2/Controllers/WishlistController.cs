using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Linq;
using System.Security.Claims;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")] // Changed from "/Cart" to use the controller name by convention
    [Authorize] // Ensuring that all wishlist operations require user authentication
    public class WishlistController : ControllerBase
    {
        private readonly MyDbContext _context;

        public WishlistController(MyDbContext context)
        {
            _context = context;
        }

        // Get all items in the user's wishlist
        [HttpGet]
        public IActionResult GetWishlistItems()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var wishlistItems = _context.Wishlists.Where(w => w.UserId == userId)
                .Select(w => new
                {
                    w.Id,
                    w.EventId,
                    w.Event.EventName, 
                    w.Event.EventPrice,
                    w.Event.EventStatus,
                    w.Event
                }).ToList();

            return Ok(wishlistItems);
        }

        // Add an item to the wishlist
        [HttpPost]
        public IActionResult AddToWishlist([FromBody] WishlistItemModel wishlistItem)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var existingWishlistItem = _context.Wishlists.FirstOrDefault(w => w.EventId == wishlistItem.EventId && w.UserId == userId);

            if (existingWishlistItem == null)
            {
                var newWishlistItem = new Wishlist
                {
                    UserId = userId,
                    EventId = wishlistItem.EventId
                };
                _context.Wishlists.Add(newWishlistItem);
                _context.SaveChanges();
                return Ok("Item added to wishlist");
            }

            return BadRequest("Item already in wishlist");
        }

        // Remove a single item from the wishlist
        [HttpDelete("{id}")]
        public IActionResult RemoveWishlistItem([FromBody] WishlistItemModel wishlistItem)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var deleteWishlistItem = _context.Wishlists.FirstOrDefault(w => w.EventId == wishlistItem.EventId && w.UserId == userId);

            if (wishlistItem == null)
            {
                return NotFound("Item not found in wishlist");
            }

            _context.Wishlists.Remove(deleteWishlistItem);
            _context.SaveChanges();
            return Ok("Item removed from wishlist");
        }

        // Clear the user's wishlist
        [HttpDelete]
        public IActionResult ClearWishlist()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var userWishlistItems = _context.Wishlists.Where(w => w.UserId == userId);

            _context.Wishlists.RemoveRange(userWishlistItems);
            _context.SaveChanges();
            return Ok("Wishlist cleared");
        }
    }

    public class WishlistItemModel
    {
        public int EventId { get; set; }
    }
}
