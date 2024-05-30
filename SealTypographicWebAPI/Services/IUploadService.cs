using DBEntities.Consts;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Upload;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 上傳檔案管理
    /// </summary>
    public interface IUploadService
    {
        /// <summary>
        /// 取得上傳類別
        /// </summary>
        /// <returns></returns>
        UploadTypeResponse GetUploadType();

        /// <summary>
        /// 取得上傳檔案上限
        /// </summary>
        /// <returns></returns>
        int GetMaxUploadSize();

       /// <summary>
       /// 取得上傳類別
       /// </summary>
       /// <returns></returns>
       DuplicateFileProcessModeResponse GetDuplicateFileProcessMode();

        /// <summary>
        /// 依關鑑字與搜尋條件取得上傳檔案列表
        /// </summary>
        /// <param name="uploadSearch">搜尋條件</param>
        /// <param name="companyId">公司Id</param>
        /// <returns></returns>
        UploadPaginateViewModel GetUploadPaginate(UploadSearch uploadSearch, int companyId = 1);

        /// <summary>
        /// 取得檔案名稱
        /// </summary>
        /// <param name="uploadType"></param>
        /// <returns></returns>
        UploadFileResponse GetFile(UploadType uploadType);

        /// <summary>
        /// 取得上傳檔案圖片
        /// </summary>
        /// <param name="uploadFileId"></param>
        /// <returns></returns>
        UploadFileImageView GetFileImage(int uploadFileId);

        /// <summary>
        /// 確認上傳是否有重複檔案
        /// </summary>
        /// <param name="uploadType"></param>
        /// <param name="formFiles"></param>
        /// <returns></returns>
        DuplicateFileResponse CheckDuplicateFileName(UploadType uploadType, List<IFormFile> formFiles);

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadBase64Data">上傳檔案內容</param>
        /// <param name="userId">使用者Id</param>              
        /// <returns></returns>
        Task<ResponseViewModel> SaveScanFile(UploadScanData uploadBase64Data, int userId = 1);

        /// <summary>
        /// 上傳圖檔(IFormFile)
        /// </summary>
        /// <param name="uploadData">上傳檔案內容</param>
        /// <param name="userId"></param>              
        /// <returns></returns>
        Task<ResponseViewModel> SaveFormFile(UploadData uploadData, int userId = 1);

        /// <summary>
        /// 變更檔案工作狀態為已處理
        /// </summary>
        /// <param name="uploadFileId">上傳檔案Id</param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseViewModel ChangeFileWorkStatusToDone(int uploadFileId, int userId = 1);

        /// <summary>
        /// 刪除上傳檔案(隱藏)
        /// </summary>
        /// <param name="uploadFileIds"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        ResponseViewModel Delete(List<int> uploadFileIds, int userId = 1);
    }
}
