using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.Letterhead;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 顧客資料處理的interface
    /// </summary>
    public interface ILetterheadService
    {
        /// <summary>
        /// 取得信頭基本資料列表
        /// </summary>
        /// <param name="letterheadSearch">信頭搜尋條件</param>         
        /// <returns></returns>
        LetterheadViewModels GetLetterheadViewModels(LetterheadSearch letterheadSearch);

        /// <summary>
        /// 取得信頭基本資料
        /// </summary>
        /// <param name="litterheadID">信頭ID</param>
        /// <returns></returns>
        LetterheadResponse GetLetterheadViewModel(int litterheadID);

        /// <summary>
        /// 建立信頭資料
        /// </summary>
        /// <param name="letterheadPostData">基本資料</param>
        LetterheadCreateResponse CreateLetterhead(LetterheadForm letterheadPostData);

        /// <summary>
        /// 更新建立信頭資料
        /// </summary>
        /// <param name="letterheadPostData">基本資料</param>
        ResponseViewModel UpdateLetterhead(LetterheadFormUpdate letterheadPostData);

        /// <summary>
        /// 刪除信頭資料(變更狀態使其一般USER無法看到)
        /// </summary>
        /// <param name="litterheadID"></param>
        ResponseViewModel DeleteLetterhead(int litterheadID);
    }
}
