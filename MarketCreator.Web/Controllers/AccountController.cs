using MarketCreator.Application.Services.Interfaces;
using MarketCreator.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Mvc;

namespace MarketCreator.Web.Controllers
{
    public class AccountController : SiteBaseController
    {
        #region Constructor

        private readonly IUserService _userService;

        public AccountController(IUserService userService)
        {
            _userService = userService;
        }

        #endregion

        #region Register

        [HttpGet("register-user")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost("register-user")]
        public async Task<IActionResult> Register(RegisterUsrDTO register)
        {

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
        #endregion

        #region Logout

        [HttpGet("log-out")]
        public async Task<IActionResult> Logout()
        {
            return RedirectToAction("Index", "Home");
        }

        #endregion
    }
}