using Domain.Users;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Web.Endpoint.Models.ViewModels.User;

namespace Endpoint.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;
        //private readonly IBasketService basketService;


        public AccountController(UserManager<User> userManager, SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }


        public IActionResult Login(string returnUrl = "/")
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl,
            });
        }


        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }
            var user = _userManager.FindByNameAsync(model.Email).Result;
            if (user == null)
            {
                ModelState.AddModelError("", "کاربر یافت نشد");
                return View(model);
            }
            _signInManager.SignOutAsync();
            var result = _signInManager.PasswordSignInAsync(user, model.Password
                , model.IsPersistent, true).Result;

            //if (result.Succeeded)
            //{
            //    TransferBasketForuser(user.Id);
            //    return Redirect(model?.ReturnUrl ?? "/");
            //}
            //if (result.RequiresTwoFactor)
            //{
            //    //
            //}

            return View(model);
        }


        public IActionResult Register()
        {
            return View();
        }
    }
}
