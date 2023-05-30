using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using YasinFinal.Models;
using YasinFinal.ViewModels;

namespace YasinFinal.Areas.Bizlandadmin.Controllers
{
    [Area("Bizlandadmin")]
    public class AccountController : Controller
    {
        readonly UserManager<AppUser> _userManager;
        readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }
        public IActionResult Register()
        {
            return View();
        }
        [HttpPost]  
        public async Task< IActionResult> Register(RegisterVM newuser)
        {
            AppUser user = new AppUser
            {
                Name = newuser.Name,
                Email = newuser.Email,
                Surname = newuser.Surname,
                UserName = newuser.Username
            };

            IdentityResult error = await _userManager.CreateAsync(user, newuser.Password);
            if (!error.Succeeded)
            {
                foreach (var item in error.Errors)
                {
                    ModelState.AddModelError(string.Empty, item.Description);
                    return View();
                }
            }

            await _signInManager.SignInAsync(user,  false);


            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home", new { area = "" });

        }
        public IActionResult Login()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Login(LoginVM user)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            AppUser existed = await _userManager.FindByEmailAsync(user.UsernameorEmail);
            if(existed == null)
            {
                existed = await _userManager.FindByNameAsync(user.UsernameorEmail);
                if(existed == null)
                {
                    ModelState.AddModelError(string.Empty, "Bu adda istifadeci tapilmadi");
                    return View();
                }
            }
            var result = await _signInManager.PasswordSignInAsync(existed, user.Password, user.RememberMe, false);
            if(result == null)
            {
                ModelState.AddModelError(string.Empty, "Bu adda istifadeci tapilmadi");
                return View();
            }
            return RedirectToAction("Index", "Home", new { area = "" });
        }

    }
}
