using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Models;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 取得圖片(Base64)
    /// </summary>
    [Route("api/[controller]")]
    [Produces("application/json")]
    [ApiController]
    public class ImageController : ControllerBase
    {
        private readonly ScanConfigPath _scanConfig;

        private readonly ImageSharpService imageService;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>
        /// <param name="imageService"></param>
        public ImageController(IOptionsMonitor<ScanConfigPath> options, ImageSharpService imageService)
        {
            _scanConfig = options.CurrentValue;
            this.imageService = imageService;
        }
       
        /// <summary>
        /// 取得Base64字串
        /// </summary>
        /// <param name="imagePath"></param>
        /// <returns></returns>
        [HttpGet("{imagePath}")]
        public string GetImageBase64(string imagePath)
        {
            //測試存圖用
            //string base64 = ImageSharpUtil.PathImageFileToBase64(imagePath);
            //ImageSharpUtil.Base64ToSaveImage(base64);

            return ImageSharpUtil.PathImageFileToBase64(imagePath);

        }
    }
}
