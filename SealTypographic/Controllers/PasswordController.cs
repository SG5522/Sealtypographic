using Microsoft.AspNetCore.Mvc;

namespace SealTypographic.Controllers
{
    public class PasswordController : Controller
    {
        public IActionResult Index()
        {
            return PartialView();
        }
    }
}
