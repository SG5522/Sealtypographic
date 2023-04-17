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
        /// 新增客戶印鑑組資料
        /// </summary>
        /// <param name="customerSealTemplateForm">客戶印鑑組資料</param>
        /// <returns></returns>
        ResponseViewModel New(CustomerSealTemplateForm customerSealTemplateForm);
    }
}
