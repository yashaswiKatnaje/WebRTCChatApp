using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class MessagingController : ControllerBase
{
    private readonly ChatDbContext _context;
    private readonly IConfiguration _configuration;

    public MessagingController( ChatDbContext context, IConfiguration configuration)
    {
        _context = context;
        _configuration = configuration;
    }

    [HttpPost("SendMessage")]
    public async Task<IActionResult> SendMessage(Message message)
    {
        _context.Messages.Add(message);
        await _context.SaveChangesAsync();
        return Ok();
    }

    [HttpGet("{roomId}")]
    public async Task<IActionResult> GetMessages(string roomId)
    {
        var messages = await _context.Messages.Where(m => m.RoomId == roomId).ToListAsync();
        return Ok(messages);
    }
}
