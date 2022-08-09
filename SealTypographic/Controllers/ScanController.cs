using Microsoft.AspNetCore.Mvc;
using DJTWAINLib;

namespace SealTypographic.Controllers
{
    public class ScanController : Controller
    {

        public IActionResult Index()
        {
            return View();
        }
        public void ScanSourcecheck()
        {
            
        }
        public void ScanSelect(List<string> identitys, string defaultSource)
        {

        }
    }
}
