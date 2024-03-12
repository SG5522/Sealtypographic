using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.AccountantSignTemplate;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師簽印樣板管理
    /// </summary>
    public interface IAccountantSignTemplateService
    {
        /// <summary>
        /// 會計師簽印樣板詳細資料
        /// </summary>
        /// <param name="id">會計師簽印樣本Id</param>
        /// <param name="userId">帳號驗證取得ID</param>        
        /// <returns></returns>
        Task<AccountantSignTemplateDetailViewModel> GetDetail(int id, int userId = 1);

        /// <summary>
        /// 會計師簽印樣板圖片顯示
        /// </summary>
        /// <param name="id"></param>
        /// <param name="userId">帳號驗證取得ID</param>        
        /// <returns></returns>
        Task<AccountantSignTemplateImageView> GetImage(int id, int userId = 1);

        /// <summary>
        /// 會計師簽印樣板分頁顯示
        /// </summary>
        /// <param name="accountantSignTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <param name="userId">帳號驗證取得ID</param>
        /// <param name="companyId">公司ID 預設為1</param>
        /// <returns></returns>
        Task<AccountantSignTemplatePaginate> GetPaginate(AccountantSignTemplateSearch accountantSignTemplateSearch, int userId = 1, int companyId = 1);

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="accountantSignTemplateForm">會計師簽印樣板</param>
        /// <param name="userId">userId</param>
        /// <param name="companyId"></param>
        /// <returns></returns>
        Task<ResponseViewModel> New(AccountantSignTemplateForm accountantSignTemplateForm, int userId = 1, int companyId = 1);

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>        
        /// <param name="accountantSignTemplateUpdateForm"></param>
        /// <param name="userId">帳號驗證取得ID</param>        
        /// <returns></returns>
        Task<ResponseViewModel> Update(AccountantSignTemplateUpdateForm accountantSignTemplateUpdateForm, int userId = 1);

        /// <summary>
        /// 刪除會計師簽印樣板
        /// </summary>
        /// <param name="id">會計師簽印樣板Id</param>
        /// <param name="userId">帳號驗證取得ID</param>        
        /// <returns></returns>
        Task<ResponseViewModel> Delete(int id, int userId = 1);

    }
}
