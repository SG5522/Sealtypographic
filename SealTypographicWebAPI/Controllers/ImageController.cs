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

        private readonly ImageService imageService;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>
        /// <param name="imageService"></param>
        public ImageController(IOptionsMonitor<ScanConfigPath> options, ImageService imageService)
        {
            _scanConfig = options.CurrentValue;
            this.imageService = imageService;
        }
        /// <summary>
        /// 取得圖檔並顯示指定的圖
        /// </summary>
        /// <param name="imageWorks">選擇使用路徑 1.掃描圖檔位置 2.印鑑暫存位置(本機暫存檔位置)</param>
        /// <param name="imageName">圖片檔名</param>
        /// <returns>取得圖片(Base64)</returns>                     
        /// GET api/imageWorks/imageName
        [HttpGet("{imageWorks}/{imageName}")]
        public string GetScanImage(int imageWorks, string imageName)
        {
            string imagepath;
            switch (imageWorks)
            {
                case 1:
                    imagepath = _scanConfig.ScanImagePath;
                    break;
                case 2:
                    imagepath = Path.GetTempPath() + _scanConfig.SealTempPath;
                    break;
                default:
                    imagepath = _scanConfig.ScanImagePath;
                    break;
            }
            ImageModel? imageData = imageService.GetData(imagepath, imageName);
            if (imageData != null)
            {
                //轉成image Base64
                return "data:" + imageData.ContentType + ";base64," + Convert.ToBase64String(imageData.Data, 0, imageData.Data.Length);
            }
            return "~/img/NoImage.svg";
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
            string base64 = ImageSharpUtil.PathImageFileToBase64(imagePath);
            ImageSharpUtil.Base64ToSaveImage(base64);

            return ImageSharpUtil.PathImageFileToBase64(imagePath);

            //return imageService.GetImageBase64(imagePath);
        }
    }
}
