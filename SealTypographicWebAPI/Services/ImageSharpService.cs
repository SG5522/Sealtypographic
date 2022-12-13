using DJLib;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Util;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageSharpService
    {
        /// <summary>
        /// Base64轉圖存檔
        /// </summary>
        /// <param name="ImageBase64">BASE64圖檔字串</param>
        /// <param name="saveImageInfo">存檔資訊</param>        
        /// <returns></returns>
        public ResponseViewModel Base64ToSaveImage(string ImageBase64, SaveImageInfo saveImageInfo)
        {            
            string base64string = ImageBase64[(ImageBase64.IndexOf(",") + 1)..];
            byte[] bytes = Convert.FromBase64String(base64string);
            Image image = Image.Load(bytes, out IImageFormat format);
            return SaveImageFile(image, format, saveImageInfo);
        }

        /// <summary>
        /// IFormFile存圖檔
        /// </summary>
        /// <param name="formFile">IFormFile</param>
        /// <param name="saveImageInfo">存檔資訊</param>
        /// <returns></returns>
        public ResponseViewModel IFromToSaveImage(IFormFile formFile, SaveImageInfo saveImageInfo)
        {
            Image image = Image.Load(formFile.OpenReadStream(), out IImageFormat format);            
            return SaveImageFile(image, format, saveImageInfo);
        }

        /// <summary>
        /// 存成圖檔
        /// </summary>
        /// <param name="image">圖</param>
        /// <param name="format">圖片格式</param>
        /// <param name="saveScanForm">存檔位置資訊</param>
        /// <returns></returns>
        private static ResponseViewModel SaveImageFile(Image image,IImageFormat format, SaveImageInfo saveScanForm)
        {
            switch (format.Name)
            {
                case "BMP":
                    image.SaveAsBmp(saveScanForm.Folder + saveScanForm.Filename);
                    break;
                case "JPEG":
                    image.SaveAsJpeg(saveScanForm.Folder + saveScanForm.Filename);
                    break;
                case "PNG":
                    image.SaveAsPng(saveScanForm.Folder + saveScanForm.Filename);
                    break;
                default:
                    return ResponseUtil.FileUploadFailed();
            }
            return ResponseUtil.Success();
        }

        /// <summary>
        /// 圖片轉base64
        /// </summary>
        /// <param name="image"></param>
        /// <param name="format"></param>        
        /// <returns></returns>
        public static string ImageToBase64(Image image, IImageFormat format)
        {
            return image.ToBase64String(format);
        }

        /// <summary>
        /// 檔案路徑圖片轉base64
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static string PathImageFileToBase64(string fullPath)
        {
            Image image = Image.Load(fullPath, out IImageFormat format);
            return ImageToBase64(image, format);
        }
    }
}
