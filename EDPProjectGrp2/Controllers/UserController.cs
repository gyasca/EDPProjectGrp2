using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;

namespace EDPProjectGrp2.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserController : ControllerBase
    {
        private readonly MyDbContext _context;
        private readonly IConfiguration _configuration;

        public UserController(MyDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }


        // Create user (Sign up)
        [HttpPost("register")]
        public IActionResult AddUser(User user)
        {
            // Create user object
            string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
            var myUser = new User()
            {
                RoleName = user.RoleName,
                MembershipStatus = user.MembershipStatus,
                MobileNumber = user.MobileNumber,
                Email = user.Email.Trim(),
                Password = passwordHash,
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
                CreatedAt = DateTime.Now,
                UpdatedAt = DateTime.Now
        };

            // Check email (Gives error: Capturing variable 'user' that hasn't been captured blah blah)
            var foundUser = _context.Users.Where(
            x => x.Email == myUser.Email).FirstOrDefault();
            if (foundUser != null)
            {
                string message = "Email already exists.";
                return BadRequest(new { message });
            }

            // Add user
            _context.Users.Add(myUser);
            _context.SaveChanges();
            return Ok(myUser);
        }

        //// Register user
        //[HttpPost("registerfrompractical4")]
        //public IActionResult Register(RegisterRequest request)
        //{
        //    // Create user object
        //    var now = DateTime.Now;
        //    string passwordHash = BCrypt.Net.BCrypt.HashPassword(request.Password);
        //    var user = new User()
        //    {
        //        FirstName = request.FirstName,
        //        Email = request.Email,
        //        Password = passwordHash,
        //        CreatedAt = now,
        //        UpdatedAt = now
        //    };

        //    // Check email
        //    var foundUser = _context.Users.Where(
        //    x => x.Email == request.Email).FirstOrDefault();
        //    if (foundUser != null)
        //    {
        //        string message = "Email already exists.";
        //        return BadRequest(new { message });
        //    }

        //    // Add user
        //    _context.Users.Add(user);
        //    _context.SaveChanges();
        //    return Ok();
        //}

        // Login to user account
        [HttpPost("login")]
        public IActionResult Login(LoginRequest request)
        {
            // Trim string values
            request.Email = request.Email.Trim().ToLower();
            request.Password = request.Password.Trim();
            // Check email and password
            string message = "Email or password is not correct.";
            var foundUser = _context.Users.Where(
            x => x.Email == request.Email).FirstOrDefault();
            if (foundUser == null)
            {
                return BadRequest(new { message });
            }
            bool verified = BCrypt.Net.BCrypt.Verify(request.Password, foundUser.Password);
            if (!verified)
            {
                return BadRequest(new { message });
            }
            // Return user info
            var user = new
            {
                foundUser.Id,
                foundUser.Email,
                foundUser.FirstName,
                foundUser.LastName,
            };
            string accessToken = CreateToken(foundUser);
            return Ok(new { user, accessToken });
        }

        private string CreateToken(User user)
        {
            string secret = _configuration.GetValue<string>("Authentication:Secret");
            int tokenExpiresDays = _configuration.GetValue<int>("Authentication:TokenExpiresDays");

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
                    new Claim(ClaimTypes.Name, user.FirstName),
                    new Claim(ClaimTypes.Email, user.Email),
                    new Claim(ClaimTypes.Role, user.RoleName)
                }),
                Expires = DateTime.UtcNow.AddDays(tokenExpiresDays),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var securityToken = tokenHandler.CreateToken(tokenDescriptor);
            string token = tokenHandler.WriteToken(securityToken);

            return token;
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

        // Get User by ID
        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {
            User? userToFind = _context.Users.Find(id);
            if (userToFind == null)
            {
                return NotFound();
            }
            return Ok(userToFind);
        }

        // Authorization/authentication (From Practical 5 page 4, week 6)
        [HttpGet("auth"), Authorize]
        public IActionResult Auth()
        {
            var id = Convert.ToInt32(User.Claims.Where(
            c => c.Type == ClaimTypes.NameIdentifier)
            .Select(c => c.Value).SingleOrDefault());
            var name = User.Claims.Where(c => c.Type == ClaimTypes.Name)
            .Select(c => c.Value).SingleOrDefault();
            var email = User.Claims.Where(c => c.Type == ClaimTypes.Email)
            .Select(c => c.Value).SingleOrDefault();
            if (id != 0 && name != null && email != null)
            {
                var user = new
                {
                    id,
                    email,
                    name
                };
                return Ok(new { user });
            }
            else
            {
                return Unauthorized();
            }
        }

        // Update User by ID
        [HttpPut("{id}")]
        public IActionResult UpdateUser(int id, User user)
        {
            var userToUpdate = _context.Users.Find(id);
            if (userToUpdate == null)
            {
                return NotFound();
            }
            userToUpdate.UpdatedAt = DateTime.Now;
            userToUpdate.RoleName = user.RoleName;
            userToUpdate.MembershipStatus = user.MembershipStatus;
            userToUpdate.MobileNumber = user.MobileNumber;
            userToUpdate.Email = user.Email.Trim();
            userToUpdate.Password = user.Password.Trim();
            userToUpdate.ProfilePhotoFile = user.ProfilePhotoFile;
            userToUpdate.FirstName = user.FirstName.Trim();
            userToUpdate.LastName = user.LastName.Trim();
            userToUpdate.Gender = user.Gender;
            userToUpdate.OccupationType = user.OccupationType;
            userToUpdate.Address = user.Address.Trim();
            userToUpdate.PostalCode = user.PostalCode.Trim();
            userToUpdate.NewsletterSubscriptionStatus = user.NewsletterSubscriptionStatus;
            userToUpdate.TwoFactorAuthStatus = user.TwoFactorAuthStatus;
            userToUpdate.VerificationStatus = user.VerificationStatus;
            userToUpdate.DateOfBirth = user.DateOfBirth;

            // Check if the password is being updated
            if (!string.IsNullOrEmpty(user.Password))
            {
                // Hash the updated password
                string passwordHash = BCrypt.Net.BCrypt.HashPassword(user.Password);
                userToUpdate.Password = passwordHash;
            }

            _context.SaveChanges();
            return Ok(userToUpdate);
        }

        // Delete User by ID
        [HttpDelete("{id}")]
        public IActionResult DeleteUser(int id)
        {
            var userToDelete = _context.Users.Find(id);
            if (userToDelete == null)
            {
                return NotFound();
            }
            _context.Users.Remove(userToDelete);
            _context.SaveChanges();
            return Ok();
        }
    }
}
