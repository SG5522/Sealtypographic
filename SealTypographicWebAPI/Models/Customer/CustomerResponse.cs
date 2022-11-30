namespace SealTypographicWebAPI.Models.Customer
{
    /// <summary>
    /// 取得客戶資料以及回應訊息
    /// </summary>
    public class CustomerResponseViewModel : ResponseViewModel
    {
        /// <summary>
        /// 顧客基本資料
        /// </summary>
        public CustomerForm? Data { get; set; }
    }
}
