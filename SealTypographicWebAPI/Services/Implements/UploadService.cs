using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Utils;
using DBEntities.Entities;
using DBEntities;
using DJImageLib.Utils;
using DJSpire.Utils;
using DJSpire.Models;
using CommonLib.Extensions;
using System.IO;
using SealTypographicWebAPI.Extensions;
using k8s.Models;
using SealTypographicWebAPI.Models.CustomerSeal;
using DJImageLib.Extensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 上傳檔案管理
    /// </summary>
    public class UploadService : IUploadService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly ImageService imageService;
        private readonly IStringLocalizer<UploadService> localizer;
        private readonly ILogger<UploadData> logger;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private UploadPathOption uploadConfigPath;

        /// <summary>
        /// 建構
        /// </summary>        
        /// <param name="dbContext">注入資料庫</param>
        /// <param name="imageService"></param>        
        /// <param name="localizer">注入多國語系處理</param>
        /// <param name="options">注入uploadConfigPath</param>
        /// <param name="logger">注入logger</param>
        /// <param name="mapper"></param>       
        public UploadService(SealTypographicDbContext dbContext
            , ImageService imageService
            , IStringLocalizer<UploadService> localizer
            , ILogger<UploadData> logger
            , IMapper mapper
            , IOptionsMonitor<UploadPathOption> options)
        {
            this.dbContext = dbContext;
            this.imageService = imageService;
            this.localizer = localizer;
            this.logger = logger;
            configurationProvider = mapper.ConfigurationProvider;
            uploadConfigPath = options.CurrentValue;

            options.OnChange(options =>
            {
                uploadConfigPath = options;
            });
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        public UploadTypeResponse GetUploadType()
        {
            UploadTypeResponse uploadTypeResponse = new();
            try
            {
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
                logger.LogInformation("GetUploadType output {@Output}", uploadTypeResponse);
            }
            catch (Exception ex)
            {
                uploadTypeResponse.Error();
                logger.LogError("GetUploadType error {@Error}", ex.Message);
            }

            return uploadTypeResponse;
        }

        /// <inheritdoc/>
        public int GetMaxUploadSize() => uploadConfigPath.MaxUploadSize;

        /// <inheritdoc/> 
        public DuplicateFileProcessModeResponse GetDuplicateFileProcessMode()
        {
            DuplicateFileProcessModeResponse duplicateFileProcessModeResponse = new();
            try
            {
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
                logger.LogInformation("GetDuplicateFileProcessMode output {@output}", duplicateFileProcessModeResponse);
            }
            catch (Exception ex)
            {
                duplicateFileProcessModeResponse.Error();
                logger.LogError("GetUploadType error {@Error}", ex.Message);
            }

            return duplicateFileProcessModeResponse;
        }

        /// <inheritdoc/>     
        public UploadPaginateViewModel GetUploadPaginate(UploadSearch uploadSearch, int companyId = 1)
        {
            logger.LogInformation("GetUploadPaginate input uploadSearch: {@uploadSearch} companyId: {@companyId}", uploadSearch, companyId);

            UploadPaginateViewModel uploadPaginateViewModel = new();

            try
            {
                IQueryable<UploadFile> uploadFilesQuery = dbContext.UploadFiles.Where
                                                        (
                                                            x => x.Company.Id == companyId
                                                            && x.DeleteStatus == DeleteStatus.No
                                                        );

                if (!string.IsNullOrEmpty(uploadSearch.KeyWord))
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                        (
                                            x => x.OriginalFileName.Contains(uploadSearch.KeyWord)
                                        );
                }

                if (uploadSearch.UploadType != null)
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                       (
                                           x => x.UploadType == uploadSearch.UploadType
                                       );
                }

                if (uploadSearch.FileWorkStatus != null)
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                       (
                                           x => x.FileWorkStatus == uploadSearch.FileWorkStatus
                                       );
                }

                uploadFilesQuery = uploadFilesQuery.OrderBy(uploadFile => uploadFile.Id);

                if (uploadFilesQuery.Any())
                {
                    uploadPaginateViewModel.ViewModels = uploadFilesQuery
                                                        .Skip((uploadSearch.PageNumber - 1) * uploadSearch.PageSize)
                                                        .Take(uploadSearch.PageSize)
                                                        .ProjectTo<UploadViewModel>(configurationProvider)
                                                        .ToList();

                    PageUtil.SetPaginate(uploadPaginateViewModel, uploadSearch.PageNumber, uploadSearch.PageSize, uploadFilesQuery.Count());
                    uploadPaginateViewModel.Success();
                }
                else
                {
                    uploadPaginateViewModel.DbNoData();
                }
                logger.LogInformation("GetUploadPaginate output {@Output}", uploadPaginateViewModel);
            }
            catch (Exception ex)
            {
                uploadPaginateViewModel.Error();
                logger.LogError("GetUploadPaginate error {@Error}", ex.Message);
            }
            return uploadPaginateViewModel;
        }


        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        public UploadFileResponse GetFile(UploadType uploadType)
        {
            logger.LogInformation("GetFile input uploadType: {@uploadType}", uploadType);

            UploadFileResponse uploadFileResponse = new();
            int companyId = 1;

            try
            {
                IQueryable<UploadFileViewModel> uploadFileQuery = dbContext.UploadFiles.Where
                                                                (
                                                                    uploadFile => uploadFile.UploadType == uploadType
                                                                    && uploadFile.Company.Id == companyId
                                                                    && uploadFile.FileWorkStatus == FileWorkStatus.Unprocessed
                                                                    && uploadFile.DeleteStatus == DeleteStatus.No
                                                                ).ProjectTo<UploadFileViewModel>(configurationProvider);
                if (uploadFileQuery.Any())
                {
                    uploadFileResponse.ViewModel = uploadFileQuery.ToList();
                    uploadFileResponse.Success();
                }
                else
                {
                    uploadFileResponse.DbNoData();
                }
                logger.LogInformation("GetFile output {@output}", uploadFileResponse);
            }
            catch (Exception ex)
            {
                uploadFileResponse.Error();
                logger.LogError("GetFile error {@Error}", ex.Message);
            }

            return uploadFileResponse;
        }



        /// <summary>
        /// 取得上傳檔案圖片
        /// </summary>
        /// <param name="uploadFileId"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public UploadFileImageView GetFileImage(int uploadFileId, int userId = 1)
        {
            logger.LogInformation("GetFileImage input uploadFileId: {@uploadFileId}", uploadFileId);

            UploadFileImageView uploadFileImageView = new();

            try
            {
                UploadEncryptFile? uploadEncryptFile = dbContext.UploadFiles                                                        
                                                        .Where(x => x.Id == uploadFileId)
                                                        .Select(x => new UploadEncryptFile
                                                        {
                                                            FullPath = x.FullPath,
                                                            EncryptKey = x.EncryptKey,
                                                            UploadType = x.UploadType,
                                                            RSAKey = imageService.GetRasKey(userId)
                                                        }).FirstOrDefault();
                if (uploadEncryptFile != null)
                {                    
                    string base64 = imageService.DecryptFile(uploadEncryptFile.FullPath, uploadEncryptFile.EncryptKey, uploadEncryptFile.RSAKey);

                    if (uploadEncryptFile.UploadType == UploadType.FinancialReport || uploadEncryptFile.UploadType == UploadType.TaxReport)
                    {
                        //取得單頁PDF圖檔
                        PdfPageImageInfo pDFImageInfo = PdfImageUtil.GetPdfPageImageInfo(base64.ToBytes(), 0);
                        uploadFileImageView.ImageBase64 = pDFImageInfo.ImageDataUrl;
                    }
                    else
                    {
                        //解密圖檔
                        uploadFileImageView.ImageBase64 = ImageUtil.ToDataUrl(base64.ToBytes());
                    }
                    uploadFileImageView.Success();
                }
                else
                {
                    uploadFileImageView.DbNoData();
                }

                logger.LogInformation("GetFileImage output {@output}", uploadFileImageView);
            }
            catch (Exception ex)
            {
                uploadFileImageView.Error();
                logger.LogError("GetFileImage error {@error}", ex.Message);
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
            logger.LogInformation("CheckDuplicateFileName input uploadType: {@uploadType} formFiles: {@formFiles}", uploadType, formFiles);

            DuplicateFileResponse uploadDuplicateFiles = new();

            try
            {
                foreach (IFormFile formFile in formFiles)
                {
                    UploadFile? uploadFileQuery = dbContext.UploadFiles
                                                    .FirstOrDefault(uploadFile => uploadFile.UploadType == uploadType
                                                    && uploadFile.OriginalFileName == formFile.FileName
                                                    && uploadFile.DeleteStatus == DeleteStatus.No
                                                    && uploadFile.FileWorkStatus == FileWorkStatus.Unprocessed);//檢查重複時連同工作狀態一起檢查(未來製作檔案管理時可能要拔掉這塊)                    

                    if (uploadFileQuery != null)
                    {
                        uploadDuplicateFiles.ViewModel.Add(new DuplicateFileViewModel { Id = uploadFileQuery.Id, FileName = uploadFileQuery.OriginalFileName });
                    }
                }
                uploadDuplicateFiles.Success();
                logger.LogInformation("CheckDuplicateFileName output {@output}", uploadDuplicateFiles);
            }
            catch (Exception ex)
            {
                uploadDuplicateFiles.Error();
                logger.LogError("CheckDuplicateFileName error {@error}", ex.Message);
            }

            return uploadDuplicateFiles;
        }

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadBase64Data">上傳檔案內容</param>
        /// <param name="userId">使用者Id</param>              
        /// <returns></returns>
        public async Task<ResponseViewModel> SaveScanFile(UploadScanData uploadBase64Data, int userId = 1)
        {
            logger.LogInformation("SaveScanFile input {@Input} userId: {@userid}", uploadBase64Data, userId);
            ResponseViewModel response = new();
            int companyId = 1; //公司Id

            try
            {
                //尋找公司並與上傳檔案關聯
                Company? companyQuery = dbContext.Companys.Include(x => x.UploadFiles).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    ImageSaveInfo imageSaveInfo = new()
                    {
                        RootPath = GetRootPath(uploadBase64Data.UploadType),
                        Code = userId.ToString(),
                        RSAKey = imageService.GetRasKey(userId)
                    };
                    foreach (string imagebase64 in uploadBase64Data.ImageBase64Strings)
                    {
                        //掃描完存在資料庫的原始檔名
                        string originalFileName = $"ScanFile{imageSaveInfo.FullPath}";
                        await imageService.EncryptImageAsync(imageSaveInfo);
                        companyQuery.UploadFiles.Add(NewUploadFile(imageSaveInfo, uploadBase64Data.UploadType, userId, originalFileName));
                    }
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.FileUploadFailed();
                }
                logger.LogInformation("SaveScanFile output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("SaveScanFile Db error {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("SaveScanFile error {@error}", ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadData">上傳檔案內容</param>
        /// <param name="userId">帳號驗證取得ID</param>              
        /// <returns></returns>
        public async Task<ResponseViewModel> SaveFormFile(UploadData uploadData, int userId = 1)
        {
            logger.LogInformation("SaveFormFile input {@Input} userId: {@userid}", uploadData, userId);

            ResponseViewModel response = new();
            int companyId = 1; //公司Id

            try
            {
                //尋找公司並與上傳檔案關聯
                Company? companyQuery = dbContext.Companys.Include(x => x.UploadFiles).FirstOrDefault(x => x.Id == companyId);

                if (companyQuery != null)
                {
                    ImageSaveInfo imageSaveInfo = new()
                    {
                        RootPath = GetRootPath(uploadData.UploadType),
                        Code = userId.ToString(),
                        RSAKey = imageService.GetRasKey(userId)
                    };

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
                                companyQuery.UploadFiles.Add(await SaveFile(formFile, uploadData.UploadType, userId, imageSaveInfo));
                                uploadData.FormFiles.Remove(formFile);
                            }
                        }
                    }

                    foreach (IFormFile formFile in uploadData.FormFiles)
                    {
                        companyQuery.UploadFiles.Add(await SaveFile(formFile, uploadData.UploadType, userId, imageSaveInfo));
                    }
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.FileUploadFailed();
                }
                logger.LogInformation("SaveFormFile output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogInformation("SaveScanFile Db error {@dbError}", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("SaveFormFile error {@Error}", ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseViewModel ChangeFileWorkStatusToDone(int uploadFileId, int userId = 1)
        {
            logger.LogInformation("ChangeFileWorkStatusToDone input uploadFileId: {@uploadFileId} userId: {@userid}", uploadFileId, userId);

            ResponseViewModel response = new();

            try
            {
                UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
                if (uploadFile != null)
                {
                    uploadFile.FileWorkStatus = FileWorkStatus.Done;
                    BaseInput(uploadFile, false, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.FileUploadNoData();
                }
                logger.LogInformation("ChangeFileWorkStatusToDone output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("ChangeFileWorkStatusToDone error {@error}", ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 刪除上傳檔案(隱藏)
        /// </summary>
        /// <param name="uploadFileIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseViewModel Delete(List<int> uploadFileIds, int userId = 1)
        {
            logger.LogInformation("Delete input uploadFileIds {@uploadFileIds} userId: {@userid}", uploadFileIds, userId);

            ResponseViewModel response = new();

            try
            {
                foreach (int uploadFileId in uploadFileIds)
                {
                    UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
                    if (uploadFile != null)
                    {
                        uploadFile.DeleteStatus = DeleteStatus.Yes;
                        BaseInput(uploadFile, false, userId);
                    }
                    else
                    {
                        response.ErrorItem += $"{uploadFileId},";
                    }
                }
                if (response.ErrorItem == null)
                {
                    response.Success();
                    dbContext.SaveChanges();
                }
                else
                {
                    response.ErrorItem = response.ErrorItem.Remove(response.ErrorItem.Length - 1, 1);
                    response.FileUploadNoData();
                }
                logger.LogInformation("Delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("Delete error {@Error}", ex.Message);
            }

            return response;
        }

        /// <summary>
        /// 存檔處理       
        /// </summary>
        /// <param name="formFile">上傳的檔案</param>
        /// <param name="uploadType">檔案類型</param>
        /// <param name="userId">使用者 ID</param>
        /// <param name="imageSaveInfo">Image存檔資訊</param>
        /// <returns>上傳檔案的詳細信息</returns>              
        private async Task<UploadFile> SaveFile(IFormFile formFile, UploadType uploadType, int userId, ImageSaveInfo imageSaveInfo)
        {
            using MemoryStream memoryStream = new();
            await formFile.CopyToAsync(memoryStream);

            if (uploadType >= UploadType.FinancialReport || uploadType <= UploadType.AccountantSignCertificate)
            {
                // 使用擴充方法將 MemoryStream 轉換為 Base64 字串
                imageSaveInfo.Base64 = memoryStream.ToBase64String();
                await imageService.EncryptFileAsync(imageSaveInfo);
            }
            else
            {
                // 使用擴充方法將 MemoryStream 轉換為 Base64 字串
                imageSaveInfo.ImageBase64 = memoryStream.ToBase64String();
                await imageService.EncryptImageAsync(imageSaveInfo);
            }
            return NewUploadFile(imageSaveInfo, uploadType, userId, formFile.FileName);
        }

        /// <summary>
        /// 新增上傳檔案
        /// </summary>
        /// <param name="imageSaveInfo">存檔路徑</param>
        /// <param name="uploadType">上傳檔案類別</param>
        /// <param name="userId">userId</param>
        /// <param name="originalFileName">原檔名稱</param>
        /// <returns></returns>
        private static UploadFile NewUploadFile(ImageSaveInfo imageSaveInfo, UploadType uploadType, int userId, string originalFileName)
        {
            // TODO:後續在DuplicateFileProcessMode.Reserve(保留原檔名)模式時客戶要求檔名要區分時在另做調整。
            UploadFile uploadFile = new()
            {
                OriginalFileName = originalFileName,
                UploadType = uploadType,
                FullPath = imageSaveInfo.FullPath,
                EncryptKey = imageSaveInfo.EncryptKey
            };
            BaseInput(uploadFile, true, userId);
            return uploadFile;
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        /// <param name="uploadType">印鑑類別</param>          
        /// <returns></returns>
        private string GetRootPath(UploadType uploadType)
        {
            string folder = string.Empty;

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
                case UploadType.FinancialReport:
                    folder = uploadConfigPath.FinancialReport;
                    break;
                case UploadType.TaxReport:
                    folder = uploadConfigPath.TaxReport;
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
            return folder;
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
