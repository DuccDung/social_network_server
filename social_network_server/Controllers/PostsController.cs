using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_network_server.Models;
using social_network_server.Models.ModelBase;
using System.Diagnostics;
using static UploadController;

namespace social_network_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PostsController : ControllerBase
    {
        private readonly SocialNetworkContext _context;
        public PostsController(SocialNetworkContext context)
        {
            _context = context;
        }

        [HttpPost("/api/post/")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> Upload([FromForm] Post_req model)
        // hoặc: Upload([FromForm] List<IFormFile> files)
        {
            var files = model.Files;
            if (files == null || files.Count == 0) return BadRequest("No files.");

            // wwwroot/uploads
            var webroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
            var root = Path.Combine(webroot, "uploads");
            Directory.CreateDirectory(root);

            var allowed = new[] { ".jpg", ".jpeg", ".png", ".gif", ".webp" };
            var urls = new List<string>();

            foreach (var file in files)
            {
                if (file.Length == 0) continue;
                if (!file.ContentType.StartsWith("image/"))
                    return BadRequest("Only image files are allowed.");

                var ext = Path.GetExtension(file.FileName).ToLowerInvariant();
                if (!allowed.Contains(ext))
                    return BadRequest($"Unsupported type: {ext}");

                var fileName = $"{Guid.NewGuid()}{ext}";
                var path = Path.Combine(root, fileName);

                await using var stream = System.IO.File.Create(path);
                await file.CopyToAsync(stream);

                urls.Add($"{Request.Scheme}://{Request.Host}/uploads/{fileName}");
            }

            var post = new Post
            {
                AccountId = model.AccountId,
                Content = model.Content,
                CreateAt = DateTime.UtcNow,
                UpdateAt = DateTime.UtcNow,
                IsRemove = false,
                PostType = "images",
            };
            _context.Posts.Add(post);
            var media = urls.Select(url => new PostMedium
            {
                PostId = post.PostId,
                MediaUrl = url,
                CreateAt = DateTime.UtcNow,
                MediaType = "image"
            }).ToList();
            _context.PostMedia.AddRange(media);

            await _context.SaveChangesAsync();
            return Ok();
        }


        [HttpGet("/api/get-post")]
        public async Task<IActionResult> GetPost(int userId)
        {
            var post = await _context.Posts.Include(p => p.PostMedia).Where(p => p.AccountId == userId).ToListAsync();
            return Ok(post);
        }
    }
}
