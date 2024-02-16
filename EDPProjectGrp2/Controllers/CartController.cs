using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("/Cart")]
    [Authorize] // Ensuring that all cart operations require user authentication
    public class CartController : ControllerBase
    {
        private readonly MyDbContext _context;

        public CartController(MyDbContext context)
        {
            _context = context;
        }

        // Get all items in the user's cart
        [HttpGet]
        public IActionResult GetCartItems()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var cartItems = _context.Carts.Where(c => c.UserId == userId)
                            .Select(c => new
                            {
                                c.Id,
                                c.EventId,
                                c.Event.EventName, 
                                c.Quantity,
                                c.Event.EventPrice,
                                c.Event.EventStatus,
                                c.Event,
                            }).ToList();

            return Ok(cartItems);
        }

        // Add an item to the cart
        [HttpPost]
        public IActionResult AddToCart([FromBody] CartItemModel cartItem)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var existingCartItem = _context.Carts.FirstOrDefault(c => c.EventId == cartItem.EventId && c.UserId == userId);

            if (existingCartItem != null)
            {
                existingCartItem.Quantity += cartItem.Quantity;
            }
            else
            {
                var newCartItem = new Cart
                {
                    UserId = userId,
                    EventId = cartItem.EventId,
                    Quantity = cartItem.Quantity
                };
                _context.Carts.Add(newCartItem);
            }

            _context.SaveChanges();
            return Ok();
        }

        // Update a cart item's quantity
        [HttpPut("{id}")]
        public IActionResult UpdateCartItem(int id, [FromBody] CartItemModel model)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);

            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound("Cart item not found.");
            }

            cartItem.Quantity = model.Quantity;

            _context.SaveChanges();

            return Ok("Cart item quantity updated successfully.");
        }

        // Remove a cart item
        [HttpDelete("{id}")]
        public IActionResult RemoveCartItem(int id)
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var cartItem = _context.Carts.FirstOrDefault(c => c.Id == id && c.UserId == userId);

            if (cartItem == null)
            {
                return NotFound();
            }

            _context.Carts.Remove(cartItem);
            _context.SaveChanges();
            return Ok();
        }

        // Clear the user's cart
        [HttpDelete]
        public IActionResult ClearCart()
        {
            var userId = int.Parse(User.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value);
            var userCartItems = _context.Carts.Where(c => c.UserId == userId);

            _context.Carts.RemoveRange(userCartItems);
            _context.SaveChanges();
            return Ok();
        }
    }

    public class CartItemModel
    {
        public int EventId { get; set; }
        public int Quantity { get; set; }
    }
}