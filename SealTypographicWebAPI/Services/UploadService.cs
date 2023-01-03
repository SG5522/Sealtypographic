using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 上傳文件
    /// </summary>
    public class UploadService
    {
        private readonly ImageSharpService imageSharpService;
        private readonly ScanConfigPath scanConfig;

        /// <summary>
        /// 注入ImageSharpService
        /// </summary>
        /// <param name="imageSharpService"></param>
        /// <param name="options"></param>       
        public UploadService(ImageSharpService imageSharpService, IOptionsMonitor<ScanConfigPath> options)
        {
            this.imageSharpService = imageSharpService;
            scanConfig = options.CurrentValue;
        }

        /// <summary>
        /// 上傳圖檔(BASE64)
        /// </summary>
        /// <param name="uploadScanForms">上傳資料</param>
        /// <returns></returns>
        //public ResponseViewModel SaveImageBase64(List<UploadScanForm> uploadScanForms)
        //{            
        //    foreach(UploadScanForm uploadScanForm in uploadScanForms)
        //    {
        //        SaveImageInfo saveImageInfo = GetSaveImageInfo(uploadScanForm.SealType, uploadScanForm.ClientFileName);
        //        imageSharpService.Base64ToSaveImage(uploadScanForm.ImageBase64, saveImageInfo);
        //    }
        //    return new();
        //}

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="formFiles"></param>
        /// <param name="uploadType"></param>        
        /// <returns></returns>
        //public ResponseViewModel SaveImageIFormFile(int uploadType, List<IFormFile> formFiles)
        //{
        //    foreach (IFormFile formFile in formFiles)
        //    {
        //        SaveImageInfo saveImageInfo = GetSaveImageInfo((SealType)uploadType, formFile.FileName);
        //        imageSharpService.IFromToSaveImage(formFile, saveImageInfo);
        //    }
        //    return new();
        //}

        //private SaveImageInfo GetSaveImageInfo(SealType sealType, string clientFileName)
        //{
        //    SaveImageInfo saveScanForm = new();
        //    string targetFolder = DateTime.Now.ToString("yyyy") + "/" + DateTime.Now.ToString("MM") + "/" + DateTime.Now.ToString("dd") + "/";
        //    string fileName = clientFileName; //暫時使用來源資料之後會變動為SERVER上的名稱
        //    switch (sealType)
        //    {
        //        case SealType.Customer :                    
        //            saveScanForm.Folder = scanConfig.ScanImagePath + scanConfig.CustomerFolder + targetFolder;
        //            break;
        //        case SealType.Accountant:
        //            saveScanForm.Folder = scanConfig.ScanImagePath + scanConfig.AccountantFolder + targetFolder;
        //            break;
        //        case SealType.Letterhead:
        //            saveScanForm.Folder = scanConfig.ScanImagePath + scanConfig.LetterheadFolder + targetFolder;
        //            break;
        //    }
        //    FolderUtil.CheckFolder(saveScanForm.Folder);
        //    saveScanForm.Filename = fileName;

        //    return saveScanForm;
        //}
        
    }
}
