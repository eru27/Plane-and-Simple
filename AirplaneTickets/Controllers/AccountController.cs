using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.VisualStudio.Web.CodeGeneration.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Identity;
using System.Threading.Tasks;
using AirplaneTickets.Models.ViewModels;
using AirplaneTickets.Models.Shared;
using AirplaneTickets.Services.Interfaces;
using System.Security.Claims;
using System.Security.Principal;

namespace AirplaneTickets.Controllers
{
    public class AccountController : Controller
    {
        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult Login()
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                var returnUrl = Request.Query["ReturnUrl"];

                if (!string.IsNullOrEmpty(returnUrl))
                {
                    return LocalRedirect(returnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (HttpContext.User.Identity.IsAuthenticated)
            {
                return RedirectToAction("Index", "Home");
            }
            if (ModelState.IsValid)
            {
                var identity = await _userService.CreateIdentitiyAsync(model);

                if (identity != null)
                {
                    await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity));

                    var refererUrl = Request.Headers["Referer"];

                    if (!string.IsNullOrEmpty(refererUrl))
                    {
                        return Redirect(refererUrl);
                    }

                    return RedirectToAction("Index", "Home");
                }
                else
                {
                   ModelState.AddModelError(string.Empty, "Email or password is not correct.");
                }
            }

            return View(model);
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);

            return RedirectToAction("Index", "Home");
        }

        public IActionResult AccessDenied()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpGet]
        public IActionResult CreateAccount()
        {
            return View();
        }

        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<IActionResult> CreateAccount(CreateAccountViewModel model)
        {
            if (ModelState.IsValid)
            {
                var result = await _userService.CreateAccountAsync(model);

                if (result.Succeeded)
                {
                    ModelState.Clear();
                    ViewBag.Message = "Account created successfully.";

                    return View();
                }
                else
                {
                    ModelState.AddModelError(string.Empty, "Email already exists.");
                }

            }

            return View(model);
        }

        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> ListAccounts()
        {
            var users = await _userService.FetchAllAccountsAsync();
            return View(users);
        }
    }
}
