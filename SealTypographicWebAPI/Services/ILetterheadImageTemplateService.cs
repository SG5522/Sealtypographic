using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.LetterheadImageTemplate;

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
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<LetterheadImageTemplateDetailViewModel> GetDetail(int id, int userId = 0);

        /// <summary>
        /// 信頭簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        Task<LetterheadImageTemplateImageView> GetImage(int id, int userId = 0);

        /// <summary>
        /// 信頭樣板分頁顯示
        /// </summary>
        /// <param name="letterheadTemplateSearch">信頭樣板分頁搜尋</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <param name="companyId">公司ID 預設為1</param>
        /// <returns></returns>
        Task<LetterheadImageTemplatePaginate> GetPaginate(LetterheadImageTemplateSearch letterheadTemplateSearch, int userId = 1, int companyId = 1);

        /// <summary>
        /// 新增信頭樣板
        /// </summary>
        /// <param name="letterheadTemplateForm">信頭樣板</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <param name="companyId">公司ID 預設為1</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(LetterheadImageTemplateForm letterheadTemplateForm, int userId = 1, int companyId = 1);

        /// <summary>
        /// 更新信頭樣板
        /// </summary>        
        /// <param name="letterheadTemplateUpdateForm">信頭樣板</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(LetterheadImageTemplateUpdateForm letterheadTemplateUpdateForm, int userId = 1);

        /// <summary>
        /// 刪除信頭樣板
        /// </summary>
        /// <param name="Id">信頭樣板Id</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <returns></returns>
        Task<ResponseViewModel> Delete(int Id, int userId = 1);

    }
}
