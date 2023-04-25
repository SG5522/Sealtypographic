using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;
using SealTypographicWebAPI.Models.AccountantSignTemplate;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 會計師簽印樣板
    /// </summary>
    public interface IAccountantSignTemplateService
    {
        /// <summary>
        /// 會計師簽印樣板詳細資料
        /// </summary>
        /// <param name="id">會計師簽印樣本Id</param>
        /// <returns></returns>
        AccountantSignTemplateDetailViewModel GetDetail(int id);

        /// <summary>
        /// 會計師簽印樣板分頁顯示
        /// </summary>
        /// <param name="customerSealTemplateSearch">會計師簽印樣板分頁搜尋</param>
        /// <returns></returns>
        CustomerSealTemplatePaginate GetPaginate(CustomerSealTemplateSearch customerSealTemplateSearch);

        /// <summary>
        /// 新增會計師簽印樣板
        /// </summary>
        /// <param name="customerSealTemplateForm">會計師簽印樣板</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(CustomerSealTemplateForm customerSealTemplateForm);

        /// <summary>
        /// 更新會計師簽印樣板
        /// </summary>        
        /// <param name="customerSealTemplateUpdateForm"></param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm);
        
    }
}
