using Microsoft.AspNetCore.Mvc;
using EDPProjectGrp2.Models;
using LearningAPI;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Microsoft.AspNetCore.Authorization;
using Microsoft.EntityFrameworkCore;

namespace EDPProjectGrp2.Controllers
{
	[ApiController]
	[Route("[controller]")]
	public class ForumPostController : Controller
	{
		private readonly MyDbContext _context;
		private readonly IConfiguration _configuration;
		private int GetUserId()
		{
			return Convert.ToInt32(User.Claims
			.Where(c => c.Type == ClaimTypes.NameIdentifier)
			.Select(c => c.Value).SingleOrDefault());
		}

		public ForumPostController(MyDbContext context, IConfiguration configuration)
		{
			_context = context;
			_configuration = configuration;
		}

		[HttpPost, Authorize]
		public IActionResult AddPost(ForumPost forumPost)
		{
			int userId = GetUserId();
			var now = DateTime.Now;
			var PostToUpload = new ForumPost()
			{
				PostTopic = forumPost.PostTopic.Trim(),
				Title = forumPost.Title.Trim(),
				Content = forumPost.Content.Trim(),
				Votes = forumPost.Votes,
				DateCreated = now,
				DateEdited = now,
				UserId = userId
			};
			_context.ForumPost.Add(PostToUpload);
			_context.SaveChanges();
			return Ok(PostToUpload);
		}


		[HttpGet]
		public IActionResult GetAllPosts(string? search)
		{
			IQueryable<ForumPost> result = _context.ForumPost.Include(t => t.User);
			
			if (search != null)
			{
				result = result.Where(x => x.PostTopic.Contains(search)
				|| x.Title.Contains(search));
			}

			var list = result.OrderByDescending(x => x.DateCreated).ToList();
			var data = list.Select(t => new
			{
				t.PostId,
				t.PostTopic,
				t.Title,
				t.Content,
				t.DateCreated,
				t.DateEdited,
				t.UserId,
				User = new
				{
					t.User?.Email
				}
			});
			return Ok(data);
		}


		[HttpGet("{id}")]
		public IActionResult GetPostById(int id)
		{
			ForumPost? forumPost = _context.ForumPost.Include(t => t.User).FirstOrDefault(t => t.User.Id == id);

			var data = new
			{
				forumPost.PostId,
				forumPost.PostTopic,
				forumPost.Title,
				forumPost.Content,
				forumPost.DateCreated,
				forumPost.DateEdited,
				forumPost.UserId,
				User = new
				{
					forumPost.User?.Email
				}
			};
			return Ok(data);
		}

	}
}
