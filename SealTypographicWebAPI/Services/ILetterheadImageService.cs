using SealTypographicWebAPI.Models.Letterhead;
using SealTypographicWebAPI.Models;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 信頭圖片管理
    /// </summary>
    public interface ILetterheadImageService
    {
        /// <summary>
        /// 取得信頭圖案狀態列表
        /// </summary>
        /// <returns></returns>
        LetterheadImageStatusResponse GetStatus();

        /// <summary>
        /// 取得信頭名稱與圖片建立日期
        /// </summary>
        /// <param name="letterheadImageSearch">信頭圖片歷程分頁搜尋</param>        
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<LetterheadImageCreateDateViews> GetNameAndCreateDate(LetterheadImageSearch letterheadImageSearch, int userId = 1);

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="id">信頭圖片Id</param>
        /// <param name="isTransparent">是否白底透明化</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<LetterheadImageViewModel> GetImageViewModel(int id, bool isTransparent, int userId = 1);

        /// <summary>
        /// 異動信頭圖片
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(LetterheadImageForm letterheadImageForms, int userId = 1);

        /// <summary>
        /// 異動信頭圖片的處理(審查狀態是草稿才進行修改)
        /// </summary>
        /// <param name="letterheadImageUpdate">異動信頭圖片資料</param>
        /// <param name="userId">登入的使用者Id</param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(LetterheadImageUpdate letterheadImageUpdate, int userId = 1);
    }
}
