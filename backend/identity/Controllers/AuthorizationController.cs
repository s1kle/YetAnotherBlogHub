using BlogHub.Identity.Models;
using IdentityServer4.Services;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace BlogHub.Identity.Controllers;

[Route("[controller]")]
public class AuthorizationController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly IIdentityServerInteractionService _interactionService;

    public AuthorizationController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, IIdentityServerInteractionService interactionService)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _interactionService = interactionService;
    }

    [HttpGet("[action]")]
    public IActionResult Login(string returnUrl)
    {
        var viewModel = new LoginViewModel
        {
            Username = string.Empty,
            Password = string.Empty,
            ReturnUrl = returnUrl
        };

        return View(viewModel);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Login(LoginViewModel viewModel)
    {
        if (ModelState.IsValid is false)
        {
            ModelState.AddModelError(string.Empty, "VM is not valid");
            return View(viewModel);
        }

        var user = await _userManager.FindByNameAsync(viewModel.Username);

        if (user is null)
        {
            ModelState.AddModelError(string.Empty, "User not found");
            return View(viewModel);
        }

        var loginResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

        if (loginResult.Succeeded is false)
        {
            ModelState.AddModelError(string.Empty, "Login error");
            return View(viewModel);
        }

        return Redirect(viewModel.ReturnUrl);
    }

    [HttpGet("[action]")]
    public IActionResult Register(string returnUrl)
    {
        var viewModel = new RegisterViewModel
        {
            Username = string.Empty,
            Password = string.Empty,
            ReturnUrl = returnUrl
        };

        return View(viewModel);
    }

    [HttpPost("[action]")]
    public async Task<IActionResult> Register(RegisterViewModel viewModel)
    {
        if (ModelState.IsValid is false)
        {
            ModelState.AddModelError(string.Empty, "VM is not valid");
            return View(viewModel);
        }

        var user = new ApplicationUser()
        {
            UserName = viewModel.Username
        };

        var registerResult = await _userManager.CreateAsync(user, viewModel.Password);

        if (registerResult.Succeeded is false)
        {
            ModelState.AddModelError(string.Empty, $"Login error: {registerResult.Errors}");
            return View(viewModel);
        }

        var loginResult = await _signInManager.PasswordSignInAsync(user, viewModel.Password, false, false);

        if (loginResult.Succeeded is false)
        {
            ModelState.AddModelError(string.Empty, "Login error");
            return View(viewModel);
        }

        return Redirect(viewModel.ReturnUrl);
    }

    [HttpGet("[action]")]
    public async Task<IActionResult> Logout(string logoutId) {
        await _signInManager.SignOutAsync();
        var request = await _interactionService.GetLogoutContextAsync(logoutId);
        return Redirect(request.PostLogoutRedirectUri);
    }
}