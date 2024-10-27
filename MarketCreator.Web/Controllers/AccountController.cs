using GoogleReCaptcha.V3.Interface;
using MarketCreator.Application.Services.Interfaces;
using MarketCreator.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.BlazorIdentity.Pages;
using System.Security.Claims;

namespace MarketCreator.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Constructor

        private readonly IUserService _userService;
        private readonly ICaptchaValidator _captchaValidator; 

        public AccountController(IUserService userService, ICaptchaValidator captchaValidator)
        {
            _userService = userService;
            _captchaValidator = captchaValidator;
        }

        #endregion

        #region Register

        [HttpGet("register-user")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost("register-user"), ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterUsrDTO register)
        {

            if (!await _captchaValidator.IsCaptchaPassedAsync(register.Captcha))
            {
                TempData[ErrorMessage] = "شما بعنوان یک کاربر واقعی تایید نشدید";
                return View(register);
            }

            //برای چک کردن مقادیر وارد شده 
            //که نیاز به کدهای جاوااسکریپتی هم در صفحه داریم
            if(!ModelState.IsValid)
                return View(register);

            if (register != null)
            {
                var result = await _userService.RegisterUserAsync(register);

                switch (result) 
                {
                    case ResultRegisterUser.Error:
                        TempData[ErrorMessage] = "متاسفانه ثبت نام انجام  نشد!"; 
                        break;
                        case ResultRegisterUser.MobileExists:
                        TempData[ErrorMessage] = "متاسفانه ثبت نام انجام  نشد!";
                        TempData[InfoMessage] = "با این شماره قبلا ثبت نام انجام شده است";
                        break;
                    case ResultRegisterUser.Success:
                        TempData[SuccessMessage] = "تا اینجای کار ثبت نام با موفقیت انجام شد";
                        TempData[InfoMessage] = "لطفا در ادامه کد ارسالی به موبایلتان را در قسمت تعیین شده وارد نمایید";
                        return RedirectToAction("Login");

                }

            }
                return View(register);
        }
        #endregion

        #region Login

        [HttpGet("login-user")]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost("login-user"),ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginUserDTO obj)
        {

            if (!await _captchaValidator.IsCaptchaPassedAsync(obj.Captcha))
            {
                TempData[ErrorMessage] = "شما بعنوان یک کاربر واقعی تایید نشدید";
                return View(obj);
            }


            if (!ModelState.IsValid)
                return View(obj);

            var result = await _userService.LoginUserAsync(obj);

            switch (result)
            {
                case ResultLoginUser.NotFound:
                    TempData[ErrorMessage] = "کاربر یافت نشد!";
                    break;

                case ResultLoginUser.NotActivatedMobile:
                    TempData[ErrorMessage] = "ثبت نام شما کامل انجام نشده و شماره موبایل تایید نشده";
                    TempData[InfoMessage] = "لطفا از نو ثبت نام کنید";
                    break;
                case ResultLoginUser.Success:

                    //اینجا عمل لاگین رو انجام میدیم و ریدایرکت میکنیم به صفحه ی اصلی
                    var user = await _userService.GetUserByMobileAsync(obj.Mobile.Trim());
                    
                    var claims = new List<Claim>()
                    {
                        new Claim(ClaimTypes.NameIdentifier,user.Id.ToString()),
                        new Claim(ClaimTypes.Name, user.FirstName),
                        new Claim(ClaimTypes.MobilePhone,user.Mobile)
                    }; 

                    var identity = new ClaimsIdentity(claims,CookieAuthenticationDefaults.AuthenticationScheme);

                    var principal = new ClaimsPrincipal(identity);

                    var properties = new AuthenticationProperties
                    {
                        IsPersistent = obj.RememberMe
                    };

                    await HttpContext.SignInAsync(principal, properties);

                    TempData[SuccessMessage] = "خوش گلیب سوز";
                    TempData[InfoMessage] = "خونه ی خودت بدون :)";

                    return Redirect("/");


            }


            return View();
        }
        #endregion

        #region Logout

        [HttpGet("log-out")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}