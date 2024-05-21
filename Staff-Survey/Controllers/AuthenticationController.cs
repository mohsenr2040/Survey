using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using Staff_Survey.Models.Dtos;
using Staff_Survey.Models.Interfaces;
using Microsoft.AspNetCore.Identity;
using Staff_Survey.Models.Entities;

namespace Staff_Survey.Controllers
{
    public class AuthenticationController : Controller
    {
        private readonly IUserLoginService _userLoginService;
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        public AuthenticationController(IUserLoginService userLoginService, SignInManager<User> signInManager, UserManager<User> userManager)
        {
            _userLoginService = userLoginService;
            _signInManager = signInManager;
            _userManager = userManager; 
        }
        public IActionResult Index()
        {
            return View();
        }

        [Route("Signin")]
        public IActionResult Signin()
        {
            return View();
        }

        [HttpPost]
        [Route("Signin")]
        public IActionResult Signin(UserLoginDto loginDto)
        {
           
            var signinResult = _userLoginService.SignIn(loginDto);

            if (signinResult.IsSuccess != true)
            {
                ViewBag.Message = signinResult.Message;
                return View("Signin");
            }
            if (signinResult.Data.SignInResult.RequiresTwoFactor == true)
            {
                return RedirectToAction("TwoFactorLogin", new { loginDto.UserName, loginDto.IsPersistent });
            }

            return RedirectToAction("index", "home");
          
        }

        [Route("Signout")]
        public IActionResult Signout()
        {
            _userLoginService.SignOut();
            HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction("Signin", "Authentication");
        }
    }

   
}
