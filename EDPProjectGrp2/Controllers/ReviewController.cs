using EDPProjectGrp2.Models;
using Microsoft.AspNetCore.Mvc;
using LearningAPI;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ReviewsController : ControllerBase
    {
        private readonly MyDbContext _context;

        public ReviewsController(MyDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<IEnumerable<Review>> GetReviews()
        {
            return _context.Reviews.ToList();
        }

        [HttpGet("{id}")]
        public ActionResult<Review> GetReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }

            return review;
        }

        [HttpPost]
        public IActionResult CreateReview([FromBody] Review review)
        {
            _context.Reviews.Add(review);
            _context.SaveChanges();

            return CreatedAtAction(nameof(GetReview), new { id = review.Id }, review);
        }

        [HttpPut("{id}")]
        public IActionResult UpdateReview(int id, [FromBody] Review updatedReview)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }

            review.Name = updatedReview.Name;
            review.Comment = updatedReview.Comment;
            review.Subject = updatedReview.Subject;
            review.Rating = updatedReview.Rating;

            _context.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.Find(id);

            if (review == null)
            {
                return NotFound();
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return NoContent();
        }
    }

}

