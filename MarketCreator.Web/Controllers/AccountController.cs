using MarketCreator.DataLayer.DTOs.Account;
using Microsoft.AspNetCore.Mvc;

namespace MarketCreator.Web.Controllers
{
    public class AccountController : SiteBaseController
    {


        #region Register

        [HttpGet("register-user")]
        public IActionResult Register()
        {
            return View();
        }


        [HttpPost("register-user")]
        public IActionResult Register(RegisterUsrDTO register)
        {
            return View();
        }
        #endregion
    }
}
