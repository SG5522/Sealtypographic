using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.LetterheadTemplate;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 信頭樣板管理
    /// </summary>
    public interface ILetterheadImageTemplateService
    {
        /// <summary>
        /// 信頭簽印樣板詳細資料
        /// </summary>
        /// <param name="id">信頭樣本Id</param>
        /// <returns></returns>
        LetterheadImageTemplateDetailViewModel GetDetail(int id);

        /// <summary>
        /// 信頭樣板分頁顯示
        /// </summary>
        /// <param name="letterheadTemplateSearch">信頭樣板分頁搜尋</param>
        /// <returns></returns>
        LetterheadImageTemplatePaginate GetPaginate(LetterheadImageTemplateSearch letterheadTemplateSearch);

        /// <summary>
        /// 新增信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateForm">信頭樣板</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadTemplateForm);

        /// <summary>
        /// 更新信頭樣板
        /// </summary>        
        /// <param name="letterheadTemplateUpdateForm">信頭樣板</param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadTemplateUpdateForm);

        /// <summary>
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="Id">信頭樣板Id</param>
        /// <returns></returns>
        ResponseViewModel Delete(int Id);

    }
}
