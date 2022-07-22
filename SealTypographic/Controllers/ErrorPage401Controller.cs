using Microsoft.AspNetCore.Mvc;

namespace SealTypographic.Controllers
{
    public class ErrorPage401Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
