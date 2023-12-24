using BlogHub.Identity.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Identity.Controllers;

[Route("[controller]")]
public class UserController : ControllerBase
{
    private readonly UserManager<ApplicationUser> _userManager;

    public UserController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    [HttpGet("id/{userId}")]
    public async Task<IActionResult> GetUserNameById(string userId)
    {
        var user = await _userManager.FindByIdAsync(userId);

        if (user is null) return NotFound();

        return Ok(new { username = $"{user.UserName}"});
    }
}