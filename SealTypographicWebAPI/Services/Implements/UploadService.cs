using EFCore.BulkExtensions;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;

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
                    Name = localizer[uploadType.GetDescription()]
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
                    Name = localizer[duplicateFileProcessMode.GetDescription()]
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
            int companyId = 1;
            List<UploadFile> uploadFiles = dbContext.UploadFiles
                                            .Where
                                            (
                                                uploadFile => uploadFile.UploadType == uploadType
                                                && uploadFile.Company.Id == companyId
                                                && uploadFile.FileWorkStatus == FileWorkStatus.Unprocessed
                                                && uploadFile.DeleteStatus == DeleteStatus.No
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
            }
            uploadFileResponse.Success();

            return uploadFileResponse;
        }

        /// <summary>
        /// 取得上傳檔案圖片
        /// </summary>
        /// <param name="uploadFileId"></param>
        /// <returns></returns>
        public UploadFileImageView GetFileImage(int uploadFileId)
        {
            UploadFileImageView uploadFileImageView = new();
            UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
            if(uploadFile != null)
            {
                uploadFileImageView.ImageBase64 = imageSharpService.GetPathToBase64(uploadFile.FullPath);                
            }
            uploadFileImageView.Success();

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
                                             && uploadFile.OriginalFileName == formFile.FileName
                                             && uploadFile.DeleteStatus == DeleteStatus.No
                                            && uploadFile.FileWorkStatus == FileWorkStatus.Unprocessed);//檢查重複時連同工作狀態一起檢查(未來製作檔案管理時可能要拔掉這塊)

                List<UploadFile> test = dbContext.UploadFiles.ToList();

                if (uploadFileQuery != null)
                {

                    uploadDuplicateFiles.ViewModel.Add(new DuplicateFileViewModel { Id = uploadFileQuery.Id, FileName = uploadFileQuery.OriginalFileName });
                }
            }
            uploadDuplicateFiles.Success();
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
            List<UploadFile> uploadfiles = new ();
            int userId = 0; //帳號驗證取得ID
            int companyId = 1; //公司Id

            //尋找公司並與上傳檔案關聯
            Company? companyQuery = dbContext.Companys.Include(x => x.UploadFiles)
                                    .Select(x => new Company
                                    {
                                        Id = x.Id,
                                        Code = x.Code,
                                        UploadFiles = new List<UploadFile>()
                                    })
                                    .FirstOrDefault(x => x.Id == companyId);
            if(companyQuery != null) 
            {
                if (uploadData.DuplicateFileIds != null)
                {
                    foreach (int duplicateFileId in uploadData.DuplicateFileIds)
                    {                        
                        UploadFile? uploadFile = companyQuery.UploadFiles.FirstOrDefault(x => x.Id == duplicateFileId);

                        if (uploadFile != null)
                        {
                            //找出重複的檔案
                            IFormFile formFile = uploadData.FormFiles.Single(f => f.FileName == uploadFile.OriginalFileName);
                            //上傳重複檔名處理模式為覆蓋模式則將原來資料標上刪除狀態。
                            if (uploadData.DuplicateFileProcessMode == DuplicateFileProcessMode.Overlay)
                            {
                                //原檔案的刪除狀態變更為Yes
                                uploadFile.DeleteStatus = DeleteStatus.Yes;
                                BaseInput(uploadFile, false, userId);
                            }
                            //新增上傳的檔案
                            await SaveFile(formFile, uploadData.UploadType, uploadData.DuplicateFileProcessMode, userId, uploadfiles);
                            uploadData.FormFiles.Remove(formFile);
                        }
                    }
                }

                foreach (IFormFile formFile in uploadData.FormFiles)
                {
                    await SaveFile(formFile, uploadData.UploadType, DuplicateFileProcessMode.NoRepeat, userId, uploadfiles);
                }

                if (uploadfiles.Any())
                {                    
                    //companyQuery.UploadFiles.AddRange<UploadFile>(uploadfiles);
                    //dbContext.Entry(companyQuery).State = EntityState.Unchanged;
                    dbContext.UploadFiles.AddRange(uploadfiles);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.FileUploadFailed();
                }
            }

            return response;
        }

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>
        /// <returns></returns>
        public ResponseViewModel ChangeFileWorkStatusToDone(int uploadFileId)
        {
            ResponseViewModel response = new();
            int userid = 0;
            UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
            if (uploadFile != null)
            {
                uploadFile.FileWorkStatus = FileWorkStatus.Done;
                BaseInput(uploadFile, false, userid);
                dbContext.SaveChanges();
                response.Success();
            }
            else
            {
                response.FileUploadNoData();
            }

            return response;
        }

        /// <summary>
        /// 刪除上傳檔案(隱藏)
        /// </summary>
        /// <param name="uploadFileIds"></param>
        /// <returns></returns>
        public ResponseViewModel Delete(List<int> uploadFileIds)
        {
            ResponseViewModel response = new();
            int userid = 0;
            
            foreach(int uploadFileId in uploadFileIds)
            {
                UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
                if (uploadFile != null)
                {
                    uploadFile.DeleteStatus = DeleteStatus.Yes;
                    BaseInput(uploadFile, false, userid);                    
                }
                else
                {
                    response.ErrorItem += $"{uploadFileId},";
                }
            }
            if(response.ErrorItem == null)
            {
                response.Success();
                dbContext.SaveChanges();
            }
            else
            {
                response.ErrorItem = response.ErrorItem.Remove(response.ErrorItem.Length - 1, 1);                
                response.FileUploadNoData();
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
            //後續在DuplicateFileProcessMode.Reserve模式時客戶要求檔名要區分時在另做調整。
            uploadFile.OriginalFileName = formFile.FileName;
            BaseInput(uploadFile, true, userid);
            uploadfiles.Add(uploadFile);
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        /// <param name="uploadType">印鑑類別</param>
        /// <param name="userid"></param>
        /// <param name="originalfileName">原始檔名</param>        
        /// <returns></returns>
        private string GetSavePath(UploadType uploadType,int userid, string originalfileName)
        {            
            string folder = string.Empty;            
            DateTime dateTime = DateTime.Now;            

            switch (uploadType)
            {
                case UploadType.CustomerSealAuthorization:
                    folder = uploadConfigPath.CustomerSealAuthorization;
                    break;
                case UploadType.AccountantSignAuthorization:
                    folder = uploadConfigPath.AccountantSignAuthorization;
                    break;
                case UploadType.LetterheadImage:
                    folder = uploadConfigPath.LetterheadImage;
                    break;
                case UploadType.PDF:
                    folder = uploadConfigPath.PDF;
                    break;
                case UploadType.AccountantSignCertificate:
                    folder = uploadConfigPath.AccountantSignCertificate;
                    break;
                case UploadType.Temporary:
                    folder = uploadConfigPath.Temporary;
                    break;
            }

            if (!Directory.Exists(folder))
            {
                Directory.CreateDirectory(folder);
            }
            
            return Path.Combine(folder, $"{userid}{dateTime:yyyyMMddHHmmssffff}{Path.GetExtension(originalfileName)}");
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
                uploadFile.DeleteStatus = DeleteStatus.No;
                uploadFile.FileWorkStatus = FileWorkStatus.Unprocessed;
            }
            else
            {
                uploadFile.UpdateUserId = userid;
                uploadFile.UpdateDate = DateTime.Now;
            }
        }
    }
}
