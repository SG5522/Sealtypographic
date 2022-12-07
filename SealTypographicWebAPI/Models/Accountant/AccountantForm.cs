namespace SealTypographicWebAPI.Models.Accountant
{
    /// <summary>
    /// 會計資料
    /// </summary>
    public class AccountantForm : BaseForm
    {
        /// <summary>
        /// 會計師編號
        /// </summary>   
        /// <example>ACC001</example>
        public string AccountantNumber { get; set; }

        /// <summary>
        /// 名稱
        /// </summary>
        /// <example>測試</example>
        public string Name { get; set; }

        /// <summary>
        /// 會計師群組ID
        /// 0 無群組
        /// </summary>
        /// <example>0</example>
        public string AccountantGroupId { get; set; }
    }
}
