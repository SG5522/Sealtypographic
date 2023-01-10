namespace SealTypographicWebAPI.Models.AccountantGroup
{
    /// <summary>
    /// 會計師群組列表
    /// </summary>
    public class AccountantGroupList : ResponseViewModel
    {
        /// <summary>
        /// new AccountantGroupDatas
        /// </summary>
        public AccountantGroupList ()
        {
            AccountantGroupDatas = new();
        }

        /// <summary>
        /// 會計師群組列表
        /// </summary>
        public List<AccountantGroupViewModel> AccountantGroupDatas { get; set; }
    }
}
