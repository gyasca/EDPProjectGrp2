using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using NanoidDotNet;
namespace LearningAPI.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class FileController : ControllerBase
    {
        private readonly IWebHostEnvironment _environment;
        public FileController(IWebHostEnvironment environment)
        {
            _environment = environment;
        }

        [HttpPost("upload"), Authorize]
        public IActionResult Upload(IFormFile file)
        {
            if (file.Length > 1024 * 1024)
            {
                var message = "Maximum file size is 1MB";
                return BadRequest(new { message });
            }
            var id = Nanoid.Generate(size: 10);
            var filename = id + Path.GetExtension(file.FileName);
            var imagePath = Path.Combine(_environment.ContentRootPath,
            @"wwwroot/uploads", filename);
            using var fileStream = new FileStream(imagePath, FileMode.Create);
            file.CopyTo(fileStream);
            return Ok(new { filename });
        }

        [HttpDelete("delete/{filename}"), Authorize]
        public IActionResult Delete(string filename)
        {
            var imagePath = Path.Combine(_environment.ContentRootPath,
                @"wwwroot/uploads", filename);
            if (System.IO.File.Exists(imagePath))
            {
                System.IO.File.Delete(imagePath);
                return Ok();
            }
            else
            {
                return NotFound(); // File not found
            }
        }
    }
}