using Azure.Core;
using Microsoft.AspNetCore.Mvc;
using social_network_server.Models;
using social_network_server.Models.ModelBase;

[ApiController]
[Route("api/[controller]")]
public class UploadController : ControllerBase
{
    private readonly SocialNetworkContext _context;
    public UploadController(SocialNetworkContext context)
    {
        _context = context;
    }
    [HttpPost]
    [Consumes("multipart/form-data")]
    public async Task<IActionResult> Upload([FromForm] FileUploadModel model)
    {
        if (model.File == null || model.File.Length == 0)
            return BadRequest("No file.");

        var root = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "uploads");
        Directory.CreateDirectory(root);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(model.File.FileName)}";
        var path = Path.Combine(root, fileName);

        using (var stream = System.IO.File.Create(path))
        {
            await model.File.CopyToAsync(stream);
        }

        var url = $"{Request.Scheme}://{Request.Host}/uploads/{fileName}";
        return Ok(new { url });
    }
}
