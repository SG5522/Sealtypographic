using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.AccountantGroupMember
{
    /// <summary>
    /// 會計師群組變更資料
    /// </summary>
    public class AccountantGroupChangeForm : BaseData
    {
        /// <summary>
        /// 會計師群組ID        
        /// </summary>
        /// <example>1</example>
        public int AccountantGroupId { get; set; }
    }
}
