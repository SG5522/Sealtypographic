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
        /// 取得信頭名稱與圖片建立日期
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        LetterheadImageCreateDateViews GetNameAndCreateDate(int letterheadId);

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="id">信頭圖片Id</param>
        /// <returns></returns>
        LetterheadImageViewModel GetImageViewModel(int id);

        /// <summary>
        /// 異動信頭圖片
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片</param>
        /// <returns></returns>
        ResponseViewModel New(LetterheadImageForm letterheadImageForms);

        /// <summary>
        /// 異動信頭圖片的處理(審查狀態是草稿才進行修改)
        /// </summary>
        /// <param name="letterheadImageUpdate">異動信頭圖片資料</param>
        /// <returns></returns>
        ResponseViewModel Update(LetterheadImageUpdate letterheadImageUpdate);
    }
}
