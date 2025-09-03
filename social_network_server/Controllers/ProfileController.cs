using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using social_network_server.Models;
using social_network_server.Models.ModelBase;

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
        public async Task<IActionResult> CreateProfile([FromBody] profile_req profile)
        {
            if (profile == null)
            {
                return BadRequest("Profile data is required.");
            }

            var existingProfile = await _context.Profiles
                .FirstOrDefaultAsync(p => p.UserId == profile.UserId);
            if (existingProfile != null)
            {
                return Ok(existingProfile);
            }
            Profile newProfile = new Profile
            {
                UserId = profile.UserId,
                FullName = profile.FullName,
                Address = profile.Address,
                AvatarUrl = profile.AvatarUrl,
                Bio = profile.Bio,
                CreateAt = DateTime.Now,
                UpdateAt = DateTime.Now,
                DateOfBirth = profile.DateOfBirth,
                Gender = profile.Gender,
                Phone = profile.Phone
            };

            await _context.Profiles.AddAsync(newProfile);
            await _context.SaveChangesAsync();
            return CreatedAtAction(nameof(GetProfile), new { userId = profile.UserId }, profile);
        }
    }
}
