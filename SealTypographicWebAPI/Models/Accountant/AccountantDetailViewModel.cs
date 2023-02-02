namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 顯示會計師基本詳細資料
    /// </summary>
    public class AccountantDetailViewModel : AccountantViewModel
    {
        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        public int AccountantGroupId { get; set; }
    }

    /// <summary>
    /// 會計資料以及回應訊息
    /// </summary>
    public class AccountantDetailResponse : ResponseViewModel
    {
        /// <summary>
        /// 會計基本資料
        /// </summary>
        public AccountantDetailViewModel AccountantDetailViewModel { get; set; }
    }
}
