using Lagoon.Application.Utilities;
using Lagoon.Domain.Entities;
using Lagoon.Web.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace Lagoon.Web.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(UserManager<ApplicationUser> userManager,
                                 SignInManager<ApplicationUser> signInManager,
                                 RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _roleManager = roleManager;
        }

        public IActionResult Login(string? redirectUrl = null)
        {

            redirectUrl ??= Url.Content("~/");

            LoginVM loginVM = new()
            {
                RedirectUrl = redirectUrl
            };

            return View(loginVM);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginVM loginVM)
        {
            if (!ModelState.IsValid) return View(loginVM);

            var result = await _signInManager
                .PasswordSignInAsync(loginVM.Email!, loginVM.Password!, loginVM.RememberMe, lockoutOnFailure: false);

            if (!result.Succeeded)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");

                return View(loginVM);
            }

            ApplicationUser? user = await _userManager.FindByEmailAsync(loginVM.Email!);

            if (user is null)
            {
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(loginVM);
            }

            if (await _userManager.IsInRoleAsync(user, SD.AdminEndUser)) return RedirectToAction(nameof(Index), "Dashboard");

            if (string.IsNullOrEmpty(loginVM.RedirectUrl)) return RedirectToAction(nameof(Index), "Home");

            return LocalRedirect(loginVM.RedirectUrl);
        }

        public IActionResult Register(string? returnUrl = null)
        {
            RegisterVM registerVM = new()
            {
                RoleList = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name))
            };

            return View(registerVM);
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterVM registerVM)
        {
            if (!ModelState.IsValid) return View(registerVM);

            var user = new ApplicationUser
            {
                UserName = registerVM.Email,
                Email = registerVM.Email,
                NormalizedEmail = registerVM.Email?.ToUpper(),
                Name = registerVM.Name,
                PhoneNumber = registerVM.PhoneNumber,
                EmailConfirmed = true,
            };

            var result = await _userManager.CreateAsync(user, registerVM.Password!);

            if (!result.Succeeded)
            {
                foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);

                registerVM.RoleList = _roleManager.Roles.Select(r => new SelectListItem(r.Name, r.Name));

                return View(registerVM);
            }

            await _userManager.AddToRoleAsync(user, registerVM.Role ?? SD.CustomerEndUser);
            await _signInManager.SignInAsync(user, isPersistent: false);

            if (!string.IsNullOrEmpty(registerVM.RedirectUrl)) return LocalRedirect(registerVM.RedirectUrl);

            return RedirectToAction("Index", "Home");
        }

        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction(nameof(Index), "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

    }
}
