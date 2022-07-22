using Microsoft.AspNetCore.Mvc;

namespace SealTypographic.Controllers
{
    public class ErrorPage404Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
