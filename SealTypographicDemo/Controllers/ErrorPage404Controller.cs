using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class ErrorPage404Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
