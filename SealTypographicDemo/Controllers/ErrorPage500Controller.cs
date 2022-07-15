using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class ErrorPage500Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
