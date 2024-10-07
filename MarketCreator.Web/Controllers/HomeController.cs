using System.Diagnostics;
using MarketCreator.Web.Models;
using Microsoft.AspNetCore.Mvc;

namespace MarketCreator.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
 

        public IActionResult Index()
        {
            return View();
        }
          
    }
}
