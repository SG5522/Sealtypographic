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
        /// 取得信頭圖片群組創建日期列表
        /// </summary>
        /// <param name="letterheadId">信頭Id</param>
        /// <returns></returns>
        LetterheadGroupCreateDateViews GetLetterheadCreateDateViews(int letterheadId);

        /// <summary>
        /// 取得信頭圖片
        /// </summary>
        /// <param name="letterheadGroupCreateDateSearch">搜尋條件</param>
        /// <returns></returns>
        LetterheadImageViewModels GetLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadGroupCreateDateSearch);

        /// <summary>
        /// 新增信頭圖片組
        /// </summary>
        /// <param name="letterheadImageForms">信頭圖片組</param>
        /// <returns></returns>
        ResponseViewModel Create(LetterheadImageForms letterheadImageForms);

        /// <summary>
        /// 異動信頭圖片的處理(審查狀態是草稿才進行修改)
        /// </summary>
        /// <param name="letterheadImageUpdate">刪除修改新增的list</param>
        /// <returns></returns>
        List<ResponseViewModel> Update(LetterheadImageUpdate letterheadImageUpdate);

        /// <summary>
        /// 變更此群組創建日期的信頭圖片審核為通過(啟用)
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">信頭圖片群組創建日期</param>        
        /// <returns></returns>
        ResponseViewModel ApprovalLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch);

        /// <summary>
        /// 變更此群組創建日期的信頭圖片審核為作廢
        /// </summary>
        /// <param name="letterheadImageGroupCreateDateSearch">信頭圖片群組創建日期</param>        
        ResponseViewModel InvalidLetterheadImages(LetterheadImageGroupCreateDateSearch letterheadImageGroupCreateDateSearch);
    }
}
