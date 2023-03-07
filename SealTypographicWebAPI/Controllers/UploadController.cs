using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using SealTypographicWebAPI.Services;
using SealTypographicWebAPI.Services.Implements;
using Serilog;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 上傳
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly UploadService uploadService;        

        /// <summary>
        /// 注入UploadService
        /// </summary>
        /// <param name="uploadService"></param>              
        public UploadController(UploadService uploadService)
        {
            this.uploadService = uploadService;            
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public UploadTypeResponse UploadType()
        {
            UploadTypeResponse uploadTypeResponse = new();
            try
            {
                Log.Information("Upload uploadType input {@Input}", uploadTypeResponse);
                uploadTypeResponse = uploadService.GetUploadType();
                Log.Information("Upload uploadType output {@Output}", uploadTypeResponse);
            }
            catch (Exception ex)
            {
                Log.Error("Upload uploadType error {@Error}", ex);
                uploadTypeResponse.DbError();
            }
            return uploadTypeResponse;
        }

        /// <summary>
        /// 取得上傳重複檔名處理模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public DuplicateFileProcessModeResponse DuplicateFileProcessMode()
        {
            DuplicateFileProcessModeResponse duplicateFileProcessModeResponse = new();
            try
            {
                Log.Information("Upload uploadType input {@Input}", duplicateFileProcessModeResponse);
                duplicateFileProcessModeResponse = uploadService.GetDuplicateFileProcessMode();
                Log.Information("Upload uploadType output {@Output}", duplicateFileProcessModeResponse);
            }
            catch (Exception ex)
            {
                Log.Error("Upload uploadType error {@Error}", ex);
                duplicateFileProcessModeResponse.DbError();
            }
            return duplicateFileProcessModeResponse;
        }


        /// <summary>
        /// 取得檔案列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UploadFileResponse Files(UploadType uploadType)
        {
            UploadFileResponse uploadTypeResponse = new();
            try
            {
                Log.Information("Upload files input {@Input}", uploadType);
                uploadTypeResponse = uploadService.GetFile(uploadType);
                Log.Information("Upload files output {@Output}", uploadTypeResponse);
            }
            catch (Exception ex)
            {
                Log.Error("Upload files error {@Error}", ex);
                uploadTypeResponse.DbError();
            }
            return uploadTypeResponse;
        }

        /// <summary>
        /// 顯示上傳圖檔(PDF類別此階段不處理)
        /// </summary>
        /// <param name="uploadFileId">上傳Id</param>
        /// <returns></returns>
        [HttpGet("{uploadFileId}")]
        public UploadFileImageView FileImage(int uploadFileId)
        {
            UploadFileImageView uploadFileImageView = new();
            try
            {
                Log.Information("Upload fileImage input {@Input}", uploadFileId);
                uploadFileImageView = uploadService.GetFileImage(uploadFileId);
                Log.Information("Upload fileImage output {@Output}", uploadFileImageView);
            }
            catch (Exception ex)
            {
                Log.Error("Upload fileImage error {@Error}", ex);
                uploadFileImageView.DbError();
            }
            return uploadFileImageView;
        }

        /// <summary>
        /// 確認上傳是否有重複檔案
        /// </summary>
        /// <param name="uploadType">上傳類別 1.客戶印鑑授權書 2.會計印鑑簽名授權書 3.信頭 4.PDF 5.會計師證明書 6.臨時檔</param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public DuplicateFileResponse CheckDuplicateFileName(UploadType uploadType, [FromForm] List<IFormFile> formFiles)
        {
            DuplicateFileResponse uploadDuplicateFileNames = new();
            try
            {
                Log.Information("Upload CheckDuplicateFileName input {@Input}", uploadDuplicateFileNames);
                uploadDuplicateFileNames = uploadService.CheckDuplicateFileName(uploadType, formFiles);
                Log.Information("Upload CheckDuplicateFileName output {@Output}", uploadDuplicateFileNames);
            }
            catch (Exception ex)
            {
                Log.Error("Upload getUploadType error {@Error}", ex);
                uploadDuplicateFileNames.DbError();
            }
            return uploadDuplicateFileNames;
        }

        /// <summary>
        /// 掃描上傳(尚未完成)
        /// </summary>
        /// <param name="uploadScanForms">圖檔資料</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public ResponseViewModel Scan(List<UploadScanForm> uploadScanForms)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Upload Scan  input {@Input}", uploadScanForms);                
                //ResponseViewModel response = uploadService.SaveImageBase64(uploadScanForms);

                Log.Information("Upload Scan  output {@Output}", response);                
            }
            catch (Exception ex)
            {
                Log.Error("Upload Scan error {@Error}", ex);
                response.DbError();                
            }
            return response;
        }

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="upladData">檔案資料</param>              
        /// <returns></returns>
        [HttpPost]        
        public async Task<ResponseViewModel> New([FromForm]UploadData upladData)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Upload new input {@Input}", upladData);
                response = await uploadService.SaveFormFile(upladData);
                Log.Information("Upload new output {@Output}", upladData);                       
            }
            catch (Exception ex)
            {
                Log.Error("Upload new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>                    
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel FileWorkStatusToDone(int uploadFileId)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Upload new input {@Input}", uploadFileId);
                response = uploadService.ChangeFileWorkStatusToDone(uploadFileId);
                Log.Information("Upload new output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("Upload new error {@Error}", ex);
                response.DbError();
            }
            return response;
        }

        /// <summary>
        /// 刪除上傳檔案
        /// </summary>
        /// <param name="uploadFileIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseViewModel Delete(List<int> uploadFileIds)
        {
            ResponseViewModel response = new();
            try
            {
                Log.Information("Upload Delete input {@Input}", uploadFileIds);
                response = uploadService.Delete(uploadFileIds);
                Log.Information("Upload Delete output {@Output}", response);
            }
            catch (Exception ex)
            {
                Log.Error("Upload Delete error {@Error}", ex);
                response.DbError();
            }
            return response;
        }
    }
}
