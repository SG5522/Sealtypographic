using DJLib.Models;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.SealMappingConfig;
using SealTypographicWebAPI.Models.Upload;
using SealTypographicWebAPI.Utils;
using System.Globalization;
using System.Linq;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 上傳檔案
    /// </summary>
    public class UploadService
    {
        private readonly ImageService imageSharpService;
        private readonly IStringLocalizer<UploadService> localizer;
        private readonly UploadPathOption uploadConfigPath;
        private readonly SealTypographicDbContext dbContext;

        /// <summary>
        /// 注入ImageSharpService
        /// </summary>
        /// <param name="imageSharpService"></param>
        /// <param name="dbContext"></param>
        /// <param name="localizer"></param>
        /// <param name="options"></param>       
        public UploadService(ImageService imageSharpService, SealTypographicDbContext dbContext,IStringLocalizer<UploadService> localizer, IOptionsSnapshot<UploadPathOption> options)
        {
            this.imageSharpService = imageSharpService;
            this.dbContext = dbContext;
            this.localizer = localizer;
            uploadConfigPath = options.Value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public UploadTypeResponse GetUploadType() 
        {
            
            UploadTypeResponse uploadTypeResponse = new();            
            foreach (UploadType uploadType in (UploadType[])Enum.GetValues(typeof(UploadType)))
            {                
                UploadTypeViewModel uploadTypeViewModel = new()
                {
                    UploadType = uploadType,
                    Name = localizer[EnumExtenstionUtil.GetDescription(uploadType)]                    
                };
                uploadTypeResponse.ViewModels.Add(uploadTypeViewModel);
            }
            uploadTypeResponse.Success();
            return uploadTypeResponse;
        }

        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <returns></returns>
        public UploadFolderFile GetFileName()
        {
            return new();
        }

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="formFiles"></param>
        /// <param name="uploadType"></param>        
        /// <returns></returns>
        public async Task<ResponseViewModel> SaveImageIFormFile(UploadType uploadType, List<IFormFile> formFiles)
        {
            ResponseViewModel response = new();
            
            

            int count = 0;
            foreach (IFormFile formFile in formFiles)
            {
                IQueryable<UploadFile> uploadFile = dbContext.UploadFiles.Where(uploadFile => uploadFile.UploadType == uploadType);
                string savePath = GetSavePath(uploadType, formFile.FileName, count);
                using Stream stream = new FileStream(savePath, FileMode.Create);
                await formFile.CopyToAsync(stream);
                count++;
            }            
            response.Success();
            return response;
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        /// <param name="uploadType">印鑑類別</param>
        /// <param name="OriginalfileName">原始檔名</param>
        /// <param name="count"></param>
        /// <returns></returns>

        private string GetSavePath(UploadType uploadType, string OriginalfileName, int count)
        {            
            string folder = string.Empty;            
            DateTime dateTime = DateTime.Now;            

            switch (uploadType)
            {
                case UploadType.CustomerSealAuthorization:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.Customer}";
                    break;
                case UploadType.AccountantSignAuthorization:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.Accountant}";
                    break;
                case UploadType.LetterheadImage:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.Letterhead}";
                    break;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }            

            return $"{folder}{dateTime:yyyyMMHHmmss}{count}{Path.GetExtension(OriginalfileName)}";
        }

    }
}
