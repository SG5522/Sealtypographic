using Microsoft.AspNetCore.Mvc;
using SealTypographicDemo.Models;
using System.Diagnostics;
using DJSpire;

namespace SealTypographicDemo.Controllers
{
    public class HomeController : Controller
    {
        private SpirePDF spirePDF = new();
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        /*
        public IActionResult Privacy()
        {
            return View();
        }
        */

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }



        //[HttpPost]
        public IActionResult PdfOpen()
        {

            //string FileName= "D:\\temp\\myPdf.pdf";
            string pdfPath = @"D:\works\SealTypographic\SealTypographicDemo\wwwroot\pdf\test.pdf";

            MemoryStream stream = spirePDF.PdfLoad(pdfPath);
            stream.Position = 0;
            spirePDF.PdfDocumentClose();
            
            return File(stream, "application/pdf");
        }
    }
}