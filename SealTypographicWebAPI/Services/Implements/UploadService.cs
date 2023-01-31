using EFCore.BulkExtensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using SealTypographicWebAPI.Utils;

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
        /// 取得上傳類別
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
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        public DuplicateFileProcessModeResponse GetDuplicateFileProcessMode()
        {
            DuplicateFileProcessModeResponse duplicateFileProcessModeResponse = new();
            foreach (DuplicateFileProcessMode duplicateFileProcessMode in (DuplicateFileProcessMode[])Enum.GetValues(typeof(DuplicateFileProcessMode)))
            {
                DuplicateFileProcessModeViewModel duplicateFileProcessModeViewModel = new()
                {
                    DuplicateFileProcessMode = duplicateFileProcessMode,
                    Name = localizer[EnumExtenstionUtil.GetDescription(duplicateFileProcessMode)]
                };
                duplicateFileProcessModeResponse.ViewModels.Add(duplicateFileProcessModeViewModel);
            }
            duplicateFileProcessModeResponse.Success();
            return duplicateFileProcessModeResponse;
        }

        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        public UploadFileResponse GetFile(UploadType uploadType)
        {
            UploadFileResponse uploadFileResponse = new();
            List<UploadFile> uploadFiles = dbContext.UploadFiles
                                            .Where
                                            (
                                                uploadFile => uploadFile.UploadType == uploadType
                                                && uploadFile.FileWorkStatus == FileWorkStatus.Undone
                                            ).ToList();
            if(uploadFiles.Any())
            {
                foreach(UploadFile uploadFile in uploadFiles) 
                {
                    uploadFileResponse.ViewModel.Add(new UploadFileViewModel
                        {
                            Id = uploadFile.Id,
                            UploadDate = uploadFile.UpdateDate,
                            FileName = uploadFile.OriginalFileName
                        }
                    );
                }
                uploadFileResponse.Success();
            }
            else
            {
                uploadFileResponse.FileUploadNoData();
            }
            return uploadFileResponse;
        }

        /// <summary>
        /// 取得上傳檔案圖片
        /// </summary>
        /// <param name="UploadFileId"></param>
        /// <returns></returns>
        public UploadFileImageView GetFileImage(int UploadFileId)
        {
            UploadFileImageView uploadFileImageView = new();
            UploadFile? uploadFile = dbContext.UploadFiles.Find(UploadFileId);
            if(uploadFile != null)
            {
                uploadFileImageView.ImageBase64 = imageSharpService.GetPathToBase64(uploadFile.FullPath);
                uploadFileImageView.Success();
            }
            else
            {
                uploadFileImageView.FileUploadNoData();
            }
            return uploadFileImageView;
        }

        /// <summary>
        /// 確認上傳是否有重複檔案
        /// </summary>
        /// <param name="uploadType"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        public DuplicateFileResponse CheckDuplicateFileName(UploadType uploadType, List<IFormFile> formFiles)
        {
            DuplicateFileResponse uploadDuplicateFiles = new();
            foreach (IFormFile formFile in formFiles)
            {
                UploadFile? uploadFileQuery = dbContext.UploadFiles
                                             .FirstOrDefault(uploadFile => uploadFile.UploadType == uploadType
                                             && uploadFile.OriginalFileName == formFile.FileName);
                if (uploadFileQuery != null)
                {

                    uploadDuplicateFiles.ViewModel.Add(new DuplicateFileViewModel { Id = uploadFileQuery.Id, FileName = uploadFileQuery.OriginalFileName });
                }
            }
            return uploadDuplicateFiles;
        }


        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadData">上傳檔案內容</param>              
        /// <returns></returns>
        public async Task<ResponseViewModel> SaveFormFile(UploadData uploadData)
        {
            ResponseViewModel response = new();
            List<UploadFile> uploadfiles = new();
            int userid = 0; //帳號驗證取得ID
            if (uploadData.DuplicateFileIds != null)
            {
                switch(uploadData.DuplicateFileProcessMode)
                {
                    case DuplicateFileProcessMode.Reserve:
                        foreach (int duplicateFileId in uploadData.DuplicateFileIds)
                        {
                            UploadFile? uploadFile = dbContext.UploadFiles.Find(duplicateFileId);
                            if(uploadFile != null)
                            {
                                IFormFile formFile = uploadData.FormFiles.Single(f => f.FileName == uploadFile.OriginalFileName);
                                await SaveFile(formFile, uploadData.UploadType, uploadData.DuplicateFileProcessMode, userid, uploadfiles);
                                uploadData.FormFiles.Remove(formFile);
                            }                            
                        }
                        break;
                    case DuplicateFileProcessMode.Overlay:
                        foreach (int duplicateFileId in uploadData.DuplicateFileIds)
                        {
                            UploadFile? uploadFile = dbContext.UploadFiles.Find(duplicateFileId);
                            if (uploadFile != null)                                                                                        
                            {
                                IFormFile formFile = uploadData.FormFiles.Single(f => f.FileName == uploadFile.OriginalFileName);
                                File.Delete(uploadFile.FullPath);

                                string savePath = GetSavePath(uploadData.UploadType, userid, formFile.FileName);
                                using Stream stream = new FileStream(savePath, FileMode.Create);
                                await formFile.CopyToAsync(stream);

                                uploadFile.FullPath = savePath;
                                BaseInput(uploadFile, false, userid);
                                uploadData.FormFiles.Remove(formFile);
                            }
                        }
                        break;
                }
            }
            foreach (IFormFile formFile in uploadData.FormFiles)
            {
                await SaveFile(formFile, uploadData.UploadType, DuplicateFileProcessMode.NoRepeat, userid, uploadfiles);
            }

            if (uploadfiles.Any())
            {
                dbContext.UploadFiles.AddRange(uploadfiles);
                dbContext.BulkSaveChanges();
                response.Success();
            }
            else
            {
                response.FileUploadFailed();
            }

            return response;
        }

        /// <summary>
        /// 存檔處理
        /// </summary>
        /// <param name="formFile"></param>
        /// <param name="uploadType">檔案類型</param>
        /// <param name="processMode">上傳重複檔名處理模式</param>
        /// <param name="userid">使用者ID</param>
        /// <param name="uploadfiles">上傳檔案資料表</param>
        /// <returns></returns>
        private async Task SaveFile(IFormFile formFile, UploadType uploadType, DuplicateFileProcessMode processMode, int userid, List<UploadFile> uploadfiles)
        {
            string savePath = GetSavePath(uploadType, userid, formFile.FileName);
            using Stream stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            UploadFile uploadFile = new()
            {
                OriginalFileName = formFile.FileName,
                UploadType = uploadType,
                FullPath = savePath
            };
            if(processMode == DuplicateFileProcessMode.NoRepeat)
            {
                uploadFile.OriginalFileName = formFile.FileName;
            }
            else
            {
                uploadFile.OriginalFileName = $"{Path.GetFileNameWithoutExtension(formFile.FileName)}(1){Path.GetExtension(formFile.FileName)}";
            }            
            BaseInput(uploadFile, true, userid);
            uploadfiles.Add(uploadFile);
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        /// <param name="uploadType">印鑑類別</param>
        /// <param name="userid"></param>
        /// <param name="OriginalfileName">原始檔名</param>        
        /// <returns></returns>
        private string GetSavePath(UploadType uploadType,int userid, string OriginalfileName)
        {            
            string folder = string.Empty;            
            DateTime dateTime = DateTime.Now;            

            switch (uploadType)
            {
                case UploadType.CustomerSealAuthorization:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.CustomerSealAuthorization}";
                    break;
                case UploadType.AccountantSignAuthorization:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.AccountantSignAuthorization}";
                    break;
                case UploadType.LetterheadImage:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.LetterheadImage}";
                    break;
                case UploadType.PDF:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.PDF}";
                    break;
                case UploadType.AccountantSignCertificate:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.AccountantSignCertificate}";
                    break;
                case UploadType.Temporary:
                    folder = $"{uploadConfigPath.UploadRootPath}{uploadConfigPath.Temporary}";
                    break;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            return $"{folder}{userid}{dateTime:yyyyMMddHHmmssffff}{Path.GetExtension(OriginalfileName)}";
        }

        /// <summary>
        /// 信頭資料新增修改時基本資料輸入
        /// </summary>
        /// <param name="uploadFile">DB上的上傳檔案</param>
        /// <param name="isCreate">確認是否新增的動作</param>
        /// <param name="userid">使用者ID</param>
        private static void BaseInput(UploadFile uploadFile, bool isCreate, int userid)
        {
            if (isCreate)
            {
                uploadFile.CreateUserId = userid;
                uploadFile.CreateDate = DateTime.Now;
                uploadFile.UpdateDate = DateTime.Now;
                uploadFile.DeleteStatus = DeleteStatus.No;
                uploadFile.FileWorkStatus = FileWorkStatus.Undone;
            }
            else
            {
                uploadFile.UpdateUserId = userid;
                uploadFile.UpdateDate = DateTime.Now;
            }
        }
    }
}
