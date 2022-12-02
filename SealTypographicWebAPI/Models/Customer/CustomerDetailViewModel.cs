using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 客戶資料
    /// </summary>
    public class CustomerDetailViewModel : ResponseViewModel
    {
        /// <summary>
        /// 客戶基本資料
        /// </summary>
        public CustomerForm CustomerForm { get; set; }
    }
}
