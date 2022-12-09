namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組資料以及回應
    /// </summary>
    public class AccountantGroupResponse : ResponseViewModel
    {
        /// <summary>
        /// 會計師群組基本資料
        /// </summary>
        public AccountantGroupViewModel AccountantGroupData { get; set; }
    }
}
