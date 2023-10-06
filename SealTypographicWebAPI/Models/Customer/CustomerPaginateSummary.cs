using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 顯示多筆簡化的客戶資料 (只有客戶編號與名稱)
    /// </summary>
    public class CustomerPaginateSummary : PaginateViewModel
    {
        /// <summary>
        /// new ViewBases
        /// </summary>
        public CustomerPaginateSummary()
        {
            Summarys = new();
        }
        /// <summary>
        /// 多筆客戶基本資料(簡化)
        /// </summary>
        public List<CustomerSummary> Summarys { get; set; }
    }
}
