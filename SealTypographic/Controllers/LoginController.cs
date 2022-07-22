using Microsoft.AspNetCore.Mvc;

namespace SealTypographic.Controllers
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
