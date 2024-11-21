using Garage_3.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
    }

    // GET: /Account/Login?returnUrl=...
    [HttpGet]
    public IActionResult Login(string returnUrl = null)
    {
        var model = new LoginViewModel { ReturnUrl = returnUrl };
        return View(model);
    }

    // POST: /Account/Login
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // Validation av ReturnUrl
        if (string.IsNullOrWhiteSpace(model.ReturnUrl))
        {
            ModelState.AddModelError(nameof(model.ReturnUrl), "ReturnUrl cannot be empty.");
        }

        if (ModelState.IsValid)
        {
            // Här utförs autentisering med Identity
            var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, model.RememberMe, false);
            if (result.Succeeded)
            {
                // Redirect till ReturnUrl eller hem
                return Redirect(model.ReturnUrl ?? Url.Action("Index", "Home"));
            }
            ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        }

        // Återvänd till inloggningsvyn vid fel
        return View(model);
    }

    // GET: /Account/Register
    [HttpGet]
    public IActionResult Register(string returnUrl = null)
    {
        ViewData["ReturnUrl"] = returnUrl; // Skicka med ReturnUrl till vyn
        return View();
    }

    // POST: /Account/Register
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Register(RegisterViewModel model, string returnUrl = null)
    {
        if (ModelState.IsValid)
        {
            var user = new ApplicationUser { UserName = model.UserName, Email = model.Email };

            // Skapa användare i systemet
            var result = await _userManager.CreateAsync(user, model.Password);
            if (result.Succeeded)
            {
                // Logga in användaren
                await _signInManager.SignInAsync(user, isPersistent: false);
                return LocalRedirect(returnUrl ?? Url.Action("Index", "Home")); // Skicka med ReturnUrl
            }

            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

        // Returnera till registreringsvyn om det är några fel
        return View(model);
    }

    // POST: /Account/Logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
}