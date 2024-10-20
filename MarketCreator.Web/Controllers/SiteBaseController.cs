using Microsoft.AspNetCore.Mvc;

namespace MarketCreator.Web.Controllers
{
    public class SiteBaseController : Controller
    {
        protected string SuccessMessage = "SuccessMessage";
        protected string InfoMessage = "InfoMessage";
        protected string ErrorMessage = "ErrorMessage";
        protected string WarningMessage = "WarningMessage";

    }
}
