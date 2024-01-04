using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;
using LearningAPI;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        public UserController(MyDbContext context)
        {
            _context = context;
        }


        // Create user (Sign up)
        [HttpPost]
        public IActionResult AddTutorial(User user)
        {
            var now = DateTime.Now;
            var myUser = new User()
            {
                RoleName = user.RoleName,
                MembershipStatus = user.MembershipStatus,
                MobileNumber = user.MobileNumber,
                Email = user.Email.Trim(),
                Password = user.Password.Trim(),
                ProfilePhotoFile = user.ProfilePhotoFile,
                FirstName = user.FirstName.Trim(),
                LastName = user.LastName.Trim(),
                Gender = user.Gender,
                OccupationType = user.OccupationType,
                Address = user.Address.Trim(),
                PostalCode = user.PostalCode.Trim(),
                NewsletterSubscriptionStatus = user.NewsletterSubscriptionStatus,
                TwoFactorAuthStatus = user.TwoFactorAuthStatus,
                VerificationStatus = user.VerificationStatus,
                DateOfBirth = user.DateOfBirth,
                CreatedAt = now,
                UpdatedAt = now
            };
            _context.Users.Add(myUser);
            _context.SaveChanges();
            return Ok(myUser);
        }

        // Get all users
        [HttpGet("getAllNoSearch")]
        public IActionResult GetAllNoSearch()
        {
            IQueryable<User> result = _context.Users;
            var list = result.OrderByDescending(x => x.CreatedAt).ToList();
            return Ok(list);
        }


        // Get by search IF NOT get all users. Search by Email field
        [HttpGet]
        public IActionResult GetAll(string? search)
        {
            IQueryable<User> result = _context.Users;
            if (search != null)
            {
                //result = result.Where(x => x.Email.Contains(search)
                //|| x.FirstName.Contains(search));

                result = result.Where(x => x.Email.Contains(search));
            }
            var list = result.OrderByDescending(x => x.CreatedAt).ToList();
            return Ok(list);
        }
    }
}
