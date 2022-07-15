using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class LoginController : Controller
    {
        public IActionResult Index()
        {
            //return View();PartialView
            return PartialView();
        }
    }
}
