using Microsoft.AspNetCore.Mvc;

namespace MarketCreator.Web.Controllers
{
    public class AccountController : Controller
    {


        #region Register

        [HttpGet("register-user")]
        public IActionResult Register()
        {
            return View();
        }
        #endregion
    }
}
