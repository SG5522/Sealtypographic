using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師簽印建立日期歷程表
    /// </summary>
    public class AccountantSignGroupJournal : BaseReviewData
    {
        /// <summary>
        /// 會計師基本資料
        /// </summary>
        public Accountant Accountant { get; set; }

        /// <summary>
        /// 會計師簽印歷程表
        /// </summary>
        public List<AccountantSignJournal> AccountantSignJournals { get; set; }

    }
}
