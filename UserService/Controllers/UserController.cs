using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class UserController : ControllerBase
{
    private readonly UserDbContext _context;

    public UserController(UserDbContext context)
    {
        _context = context;
    }

    [HttpGet("GetUser/{id}")]
    public async Task<IActionResult> GetUser(long id)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        return Ok(user);
    }

    [HttpPut("UpdateUser/{id}")]
    public async Task<IActionResult> UpdateUser(long id, [FromBody] User dto)
    {
        var user = await _context.Users.FindAsync(id);
        if (user == null) return NotFound();

        user.Username = dto.Username ?? user.Username;
        user.Email = dto.Email ?? user.Email;
        user.Password = dto.Password ?? user.Password;

        await _context.SaveChangesAsync();
        return Ok();
    }
}
