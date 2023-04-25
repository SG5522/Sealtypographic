using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.CustomerSealTemplate;

namespace SealTypographicWebAPI.Services
{
    /// <summary>
    /// 客戶印鑑樣板
    /// </summary>
    public interface ICustomerSealTemplateService
    {
        /// <summary>
        /// 客戶印鑑樣板詳細資料
        /// </summary>
        /// <param name="id">客戶印鑑樣本Id</param>
        /// <returns></returns>
        CustomerSealTemplateDetailViewModel GetDetail(int id);

        /// <summary>
        /// 客戶印鑑樣板分頁顯示
        /// </summary>
        /// <param name="customerSealTemplateSearch">客戶印鑑樣板分頁搜尋</param>
        /// <returns></returns>
        CustomerSealTemplatePaginate GetPaginate(CustomerSealTemplateSearch customerSealTemplateSearch);

        /// <summary>
        /// 新增客戶印鑑樣板
        /// </summary>
        /// <param name="customerSealTemplateForm">客戶印鑑樣板</param>
        /// <returns></returns>
        Task<ResponseViewModel> New(CustomerSealTemplateForm customerSealTemplateForm);

        /// <summary>
        /// 更新客戶印鑑樣板
        /// </summary>        
        /// <param name="customerSealTemplateUpdateForm"></param>
        /// <returns></returns>
        Task<ResponseViewModel> Update(CustomerSealTemplateUpdateForm customerSealTemplateUpdateForm);

        /// <summary>
        /// 刪除客戶印鑑樣板
        /// </summary>
        /// <param name="Id">客戶印鑑樣板Id</param>
        /// <returns></returns>
        ResponseViewModel Delete(int Id);


    }
}
