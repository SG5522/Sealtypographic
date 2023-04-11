using System.ComponentModel.DataAnnotations;
using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶簡化的資料
    /// </summary>
    public class CustomerSummaryResponse : ResponseViewModel
    {
        /// <summary>
        /// new CustomerSummary
        /// </summary>
        public CustomerSummaryResponse ()
        {
            CustomerSummary = new ();
        }
        /// <summary>
        /// 客戶基本資料(簡化)
        /// </summary>
        public CustomerSummary CustomerSummary { get; set; }
    }
}
