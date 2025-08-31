using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_network_server.Models;

namespace social_network_server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfileController : ControllerBase
    {
        private readonly SocialNetworkContext _context;
        public ProfileController(SocialNetworkContext context)
        {
            _context = context;
        }
        [HttpGet("/api/get_profile/{userId}")]
        public async Task<IActionResult> GetProfile(int userId)
        {
            var profile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == userId);
            if (profile == null)
            {
                return NotFound();
            }
            return Ok(profile);
        }

        [HttpPost("/api/create_profile")]
        public async Task<IActionResult> CreateProfile([FromBody] Profile profile)
        {
            if (profile == null)
            {
                return BadRequest("Profile data is required.");
            }
            await _context.Profiles.AddAsync(profile);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProfile), new { userId = profile.UserId }, profile);
        }
    }
}
