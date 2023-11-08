using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using DBEntities;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using Microsoft.EntityFrameworkCore;
using DBEntities.Consts;
using DJLib;
using DJLib.Models;
using AutoMapper;
using AutoMapper.QueryableExtensions;
using SealTypographicWebAPI.Utils;
using DJSpire.Services;
using DJSpire.Models;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 上傳檔案管理
    /// </summary>
    public class UploadService : IUploadService
    {        
        private readonly IStringLocalizer<UploadService> localizer;
        private readonly UploadPathOption uploadConfigPath;
        private readonly SealTypographicDbContext dbContext;
        private readonly ILogger<UploadData> logger;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;

        /// <summary>
        /// 建構
        /// </summary>        
        /// <param name="dbContext">注入資料庫</param>
        /// <param name="localizer">注入多國語系處理</param>
        /// <param name="options">注入uploadConfigPath</param>
        /// <param name="logger">注入logger</param>
        /// <param name="mapper"></param>       
        public UploadService(SealTypographicDbContext dbContext
            ,IStringLocalizer<UploadService> localizer
            , IOptionsSnapshot<UploadPathOption> options
            , ILogger<UploadData> logger
            , IMapper mapper)
        {
            this.dbContext = dbContext;
            this.localizer = localizer;
            uploadConfigPath = options.Value;
            this.logger = logger;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
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

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
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
            catch(Exception ex)
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

                if(!string.IsNullOrEmpty(uploadSearch.KeyWord))
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                        (
                                            x => x.OriginalFileName.Contains(uploadSearch.KeyWord)
                                        );
                }

                if(uploadSearch.UploadType != null)
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                       (
                                           x => x.UploadType == uploadSearch.UploadType
                                       );
                }

                if(uploadSearch.FileWorkStatus != null)
                {
                    uploadFilesQuery = uploadFilesQuery.Where
                                       (
                                           x => x.FileWorkStatus == uploadSearch.FileWorkStatus
                                       );
                }

                uploadFilesQuery = uploadFilesQuery.OrderBy(uploadFile => uploadFile.Id);

                if(uploadFilesQuery.Any())
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
            catch (Exception  ex)
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
        /// <returns></returns>
        public UploadFileImageView GetFileImage(int uploadFileId)
        {
            logger.LogInformation("GetFileImage input uploadFileId: {@uploadFileId}", uploadFileId);

            UploadFileImageView uploadFileImageView = new();

            try
            {
                UploadFile? uploadFile = dbContext.UploadFiles.Find(uploadFileId);
                if (uploadFile != null)
                {
                    if(uploadFile.UploadType == UploadType.FinancialReport || uploadFile.UploadType == UploadType.TaxReport)
                    {
                        //取得單頁PDF圖檔
                        PDFService pDFService = new() { PDFPath = uploadFile.FullPath, PageIndex = 1 };
                        PDFImageInfo pDFImageInfo = pDFService.GetPageImageInfo();
                        uploadFileImageView.ImageBase64 = pDFImageInfo.ImageBase64;                        
                    }
                    else
                    {
                        uploadFileImageView.ImageBase64 = ImageSharpUtil.PathImageFileToBase64(uploadFile.FullPath);
                    }
                    uploadFileImageView.Success();
                }
                else
                {
                    uploadFileImageView.DbNoData();
                }
                
                logger.LogInformation("GetFileImage output {@output}", uploadFileImageView);
            }
            catch(Exception ex)
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
        public async Task<ResponseViewModel> SaveScanFile(UploadScanData uploadBase64Data, int userId = 0)
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
                    foreach (string imagebase64 in uploadBase64Data.ImageBase64Strings)
                    {
                        ImageInfo imageInfo = ImageInfo.FromImageBase64(imagebase64);
                        string originalFileName = $"{userId}{DateTime.Now:yyyyMMddHHmmssffff}scanFile.{imageInfo.ImageFormat.FileExtensions.First()}";
                        string savePath = GetSavePath(uploadBase64Data.UploadType, userId, originalFileName);
                        await ImageSharpUtil.SaveFileAsync(imageInfo, savePath);
                        companyQuery.UploadFiles.Add(NewUploadFile(savePath, uploadBase64Data.UploadType, userId, originalFileName));
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
                logger.LogInformation("SaveScanFile Db error (@dbError)", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("SaveScanFile error (@error)", ex.Message);
            }            

            return response;
        }

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadData">上傳檔案內容</param>
        /// <param name="userId">帳號驗證取得ID</param>              
        /// <returns></returns>
        public async Task<ResponseViewModel> SaveFormFile(UploadData uploadData, int userId = 0)
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
                                companyQuery.UploadFiles.Add(await SaveFile(formFile, uploadData.UploadType, userId));
                                uploadData.FormFiles.Remove(formFile);
                            }
                        }
                    }

                    foreach (IFormFile formFile in uploadData.FormFiles)
                    {
                        companyQuery.UploadFiles.Add(await SaveFile(formFile, uploadData.UploadType, userId));
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
                logger.LogInformation("SaveScanFile Db error (@dbError)", ex.Message);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogInformation("SaveFormFile error (@Error)", ex.Message);
            }
            
            return response;
        }

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseViewModel ChangeFileWorkStatusToDone(int uploadFileId, int userId = 0)
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
        public ResponseViewModel Delete(List<int> uploadFileIds, int userId = 0)
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
        /// <param name="formFile"></param>
        /// <param name="uploadType">檔案類型</param>   
        /// <param name="userid">使用者ID</param>
        /// <returns></returns>
        private async Task<UploadFile> SaveFile(IFormFile formFile, UploadType uploadType, int userid)
        {
            string savePath = GetSavePath(uploadType, userid, formFile.FileName);
            using Stream stream = new FileStream(savePath, FileMode.Create);
            await formFile.CopyToAsync(stream);
            return NewUploadFile(savePath, uploadType, userid, formFile.FileName);
        }

        /// <summary>
        /// 新增上傳檔案
        /// </summary>
        /// <param name="savePath">存檔路徑</param>
        /// <param name="uploadType">上傳檔案類別</param>
        /// <param name="userId">userId</param>
        /// <param name="originalFileName">原檔名稱</param>
        /// <returns></returns>
        private static UploadFile NewUploadFile(string savePath, UploadType uploadType, int userId, string originalFileName)
        {
            // TODO: 後續在DuplicateFileProcessMode.Reserve(保留原檔名)模式時客戶要求檔名要區分時在另做調整。
            UploadFile uploadFile = new()
            {
                OriginalFileName = originalFileName,
                UploadType = uploadType,
                FullPath = savePath,                
            };            
            BaseInput(uploadFile, true, userId);
            return uploadFile;
        }

        /// <summary>
        /// 取得存檔路徑
        /// </summary>
        /// <param name="uploadType">印鑑類別</param>
        /// <param name="userid"></param>
        /// <param name="originalFileName">原始檔名</param>        
        /// <returns></returns>
        private string GetSavePath(UploadType uploadType, int userid, string originalFileName)
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
            
            return Path.Combine(folder, $"{userid}{dateTime:yyyyMMddHHmmssffff}{Path.GetExtension(originalFileName)}");
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
