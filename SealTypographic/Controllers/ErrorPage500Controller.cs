using Microsoft.AspNetCore.Mvc;

namespace SealTypographic.Controllers
{
    public class ErrorPage500Controller : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
