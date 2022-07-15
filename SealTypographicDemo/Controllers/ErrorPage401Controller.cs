using Microsoft.AspNetCore.Mvc;

namespace SealTypographicDemo.Controllers
{
    public class ErrorPage401Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
