using Microsoft.AspNetCore.Mvc;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;
using Serilog;
using DBEntities.Consts;
using SealTypographicWebAPI.Services;

namespace SealTypographicWebAPI.Controllers
{
    /// <summary>
    /// 上傳
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UploadController : ControllerBase
    {
        private readonly IUploadService uploadService;        

        /// <summary>
        /// 注入UploadService
        /// </summary>
        /// <param name="uploadService"></param>              
        public UploadController(IUploadService uploadService)
        {
            this.uploadService = uploadService;            
        }

        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public UploadTypeResponse UploadType() => uploadService.GetUploadType();

        /// <summary>
        /// 取得上傳重複檔名處理模式
        /// </summary>
        /// <returns></returns>
        [HttpGet("[Action]")]
        public DuplicateFileProcessModeResponse DuplicateFileProcessMode() => uploadService.GetDuplicateFileProcessMode();

        /// <summary>
        /// 取得檔案列表
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public UploadFileResponse Files(UploadType uploadType) => uploadService.GetFile(uploadType);

        /// <summary>
        /// 顯示上傳圖檔(PDF類別此階段不處理)
        /// </summary>
        /// <param name="uploadFileId">上傳Id</param>
        /// <returns></returns>
        [HttpGet("{uploadFileId}")]
        public UploadFileImageView FileImage(int uploadFileId) => uploadService.GetFileImage(uploadFileId);

        /// <summary>
        /// 確認上傳是否有重複檔案
        /// </summary>
        /// <param name="uploadType">上傳類別 1.客戶印鑑授權書 2.會計印鑑簽名授權書 3.信頭 4.PDF 5.會計師證明書 6.臨時檔</param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public DuplicateFileResponse CheckDuplicateFileName(UploadType uploadType, [FromForm] List<IFormFile> formFiles) => uploadService.CheckDuplicateFileName(uploadType, formFiles);

        /// <summary>
        /// 上傳掃描圖檔
        /// </summary>
        /// <param name="uploadScanData">圖檔資料</param>
        /// <returns></returns>
        [HttpPost("[Action]")]
        public async Task<ResponseViewModel> ScanNew(UploadScanData uploadScanData) => await uploadService.SaveScanFile(uploadScanData);

        /// <summary>
        /// 上傳檔案
        /// </summary>
        /// <param name="upladData">檔案資料</param>              
        /// <returns></returns>
        [HttpPost]        
        public async Task<ResponseViewModel> New([FromForm]UploadData upladData) => await uploadService.SaveFormFile(upladData);

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>                    
        /// <returns></returns>
        [HttpPut]
        public ResponseViewModel FileWorkStatusToDone(int uploadFileId) => uploadService.ChangeFileWorkStatusToDone(uploadFileId);

        /// <summary>
        /// 刪除上傳檔案
        /// </summary>
        /// <param name="uploadFileIds"></param>
        /// <returns></returns>
        [HttpDelete]
        public ResponseViewModel Delete(List<int> uploadFileIds) => uploadService.Delete(uploadFileIds);

    }
}
