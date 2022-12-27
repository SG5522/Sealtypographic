using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計群組變更資料
    /// </summary>
    public class AccountantGroupChangeForm : BaseData
    {
        /// <summary>
        /// 會計師群組ID
        /// NO000 無群組
        /// </summary>
        /// <example>1</example>
        public int AccountantGroupId { get; set; }
    }
}
