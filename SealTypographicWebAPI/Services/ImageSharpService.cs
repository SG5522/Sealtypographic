using DJLib;
using DJLib.Models;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services
{

    /// <summary>
    /// 取得圖像資料
    /// </summary>
    public class ImageSharpService
    {
        private readonly SealConfigPath sealConfig;

        /// <summary>
        /// 注入appsetting的ScanConfigPath資料
        /// </summary>
        /// <param name="options"></param>

        public ImageSharpService(IOptionsMonitor<SealConfigPath> options)
        {
            sealConfig = options.CurrentValue;
        }

        /// <summary>
        /// 
        /// </summary>        
        /// <param name="fullpath"></param>
        /// <returns></returns>
        public string GetPathToBase64(string fullpath)
        {
            return ImageSharpUtil.PathImageFileToBase64(fullpath);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sealType"></param>
        /// <param name="imageBase64"></param>
        /// <param name="number"></param>        
        public void SaveBase64ToFile(SealType sealType, string imageBase64, string number)
        {
            string savePath = string.Empty;
            DateTime date = DateTime.Now;
            string dateFolder = date.Year + "//" + date.Month + "//" + date.Day + "//" ;            
            switch (sealType)
            {
                case SealType.Customer:
                    savePath = sealConfig.SealImagePath + sealConfig.Customer;
                    break;
                case SealType.Accountant:
                    savePath = sealConfig.SealImagePath + sealConfig.Accountant;
                    break;
                case SealType.Letterhead:
                    savePath = sealConfig.SealImagePath + sealConfig.Letterhead;
                    break;                
            }

            SaveImageInfo saveImageInfo = new()
            {
                Filename = number + date.ToString("HHmmss"),
                Folder = savePath + dateFolder
            };

            ImageSharpUtil.Base64ToSaveImage(imageBase64, saveImageInfo);
        }


        /// <summary>
        /// Base64轉圖存檔
        /// </summary>
        /// <param name="ImageBase64">BASE64圖檔字串</param>
        /// <param name="saveImageInfo">存檔資訊</param>        
        /// <returns></returns>
        //public ResponseViewModel Base64ToSaveImage(string ImageBase64, SaveImageInfo saveImageInfo)
        //{            
        //    string base64string = ImageBase64[(ImageBase64.IndexOf(",") + 1)..];
        //    byte[] bytes = Convert.FromBase64String(base64string);
        //    Image image = Image.Load(bytes, out IImageFormat format);
        //    return SaveImageFile(image, format, saveImageInfo);
        //}

        /// <summary>
        /// IFormFile存圖檔
        /// </summary>
        /// <param name="formFile">IFormFile</param>
        /// <param name="saveImageInfo">存檔資訊</param>
        /// <returns></returns>
        //public ResponseViewModel IFromToSaveImage(IFormFile formFile, SaveImageInfo saveImageInfo)
        //{
        //    Image image = Image.Load(formFile.OpenReadStream(), out IImageFormat format);            
        //    return SaveImageFile(image, format, saveImageInfo);
        //}

        /// <summary>
        /// 存成圖檔
        /// </summary>
        /// <param name="image">圖</param>
        /// <param name="format">圖片格式</param>
        /// <param name="saveScanForm">存檔位置資訊</param>
        /// <returns></returns>
        //private static ResponseViewModel SaveImageFile(Image image,IImageFormat format, SaveImageInfo saveScanForm)
        //{
        //    ResponseViewModel response = new ();
        //    switch (format.Name)
        //    {
        //        case "BMP":
        //            image.SaveAsBmp(saveScanForm.Folder + saveScanForm.Filename);
        //            break;
        //        case "JPEG":
        //            image.SaveAsJpeg(saveScanForm.Folder + saveScanForm.Filename);
        //            break;
        //        case "PNG":
        //            image.SaveAsPng(saveScanForm.Folder + saveScanForm.Filename);
        //            break;
        //        default:
        //            response.FileUploadFailed();
        //            return response;
        //    }
        //    response.Success();
        //    return response;
        //}     
    }
}
