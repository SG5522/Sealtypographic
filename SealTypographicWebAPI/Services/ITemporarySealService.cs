using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.TemporarySeal;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 管理臨時章資料
    /// </summary>
    public interface ITemporarySealService
    {
        /// <summary>
        /// 取得臨時章詳細基本資料
        /// </summary>
        /// <param name="temporaryId">臨時章ID</param>
        /// <returns></returns>
        TemporarySealDetailViewModel GetDetail(int temporaryId);

        /// <summary>
        /// 取得臨時章資料列表(分頁)
        /// </summary>
        /// <param name="temporarySealSearch">臨時章分頁搜尋</param>        
        /// <returns></returns>
        TemporarySealPaginateViewModel GetPaginate(TemporarySealSearch temporarySealSearch);

        /// <summary>
        /// 新增客戶基本資料
        /// </summary>
        /// <param name="temporarySealForm">臨時章資料</param>
        Task<ResponseViewModel> New(TemporarySealForm temporarySealForm);

        /// <summary>
        /// 更新臨時章
        /// </summary>        
        /// <param name="temporarySealUpdateForm">臨時章資料</param>
        Task<ResponseViewModel> Update(TemporarySealUpdateForm temporarySealUpdateForm);

        /// <summary>
        /// 刪除臨時章。
        /// </summary>
        /// <param name="Id"></param>
        ResponseViewModel Delete(int Id);
    }
}
