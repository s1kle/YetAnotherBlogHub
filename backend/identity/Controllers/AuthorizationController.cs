using BlogHub.Identity.Models;
using IdentityModel;
using IdentityModel.Client;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Identity.Controllers;

[Route("Auth")]
public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private string _authCode; // TODO: AuthCode Manager

    public AuthorizationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _authCode = ""; // TODO: AuthCode Manager
    }

    [HttpPost("Register")]
    public async Task<IActionResult> Register([FromBody] RegisterModel model)
    {
        var user = new ApplicationUser
        {
            UserName = model.Username
        };

        var registerResult = await _userManager.CreateAsync(user, model.Password);

        if (registerResult.Succeeded is false)
        {
            var errorUrl = $"{model.ErrorUri}&ErrorMsg={registerResult.Errors}";
            return Redirect(errorUrl);
        }

        return Redirect(model.RedirectUri);
    }

    [HttpPost("Login")]
    public async Task<IActionResult> Login([FromBody] LoginModel model)
    {
        var user = await _userManager.FindByNameAsync(model.Username);

        if (user is null) return NotFound("Invalid login");

        var loginResult = await _signInManager.PasswordSignInAsync(user, model.Password, false, false);

        if (loginResult.Succeeded is false)
        {
            var errorUrl = $"{model.ErrorUri}&ErrorMsg=InvalidCredentials";
            return Redirect(errorUrl);
        }

        _authCode = CryptoRandom.CreateUniqueId(); // TODO: AuthCode Manager

        var redirectUrl = $"{model.RedirectUri}&AuthCode={_authCode}";
        return Redirect(redirectUrl);
    }

    [HttpPost("connect/token")]
    public IActionResult GenerateToken([FromBody] AuthorizationCodeTokenRequest tokenRequest)
    {
        throw new NotImplementedException();
    }
}