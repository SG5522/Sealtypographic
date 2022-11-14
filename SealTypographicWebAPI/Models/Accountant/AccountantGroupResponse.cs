namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計師群組資料以及回應
    /// </summary>
    public class AccountantGroupResponse : Response
    {
        /// <summary>
        /// 會計師群組基本資料
        /// </summary>
        public AccountantGroupData AccountantGroupData { get; set; }
    }
}
