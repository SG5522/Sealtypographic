using DBEntities.Consts;

namespace DBEntities.Entities.Base
{
    /// <summary>
    /// 客戶印鑑季度、會計師簽印審核基本資料
    /// </summary>
    public abstract class BaseReviewData : BaseData
    {
        /// <summary>
        /// 檢核人員Id
        /// </summary>
        public int? ReviewUserId { get; set; }

        /// <summary>
        /// 審查時間
        /// </summary>
        public DateTimeOffset? ReviewDate { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTimeOffset StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTimeOffset EndDate { get; set; }

        /// <summary>
        /// 審核狀態狀態
        /// 0.通過(審核完成)
        /// 10.待審查
        /// 20.退件
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}
