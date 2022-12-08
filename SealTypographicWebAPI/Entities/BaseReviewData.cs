using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 各類別基本資料
    /// </summary>
    public class BaseReviewData : BaseData
    {
        /// <summary>
        /// 檢核人員Id
        /// </summary>
        public int? ReviewUserId { get; set; }

        /// <summary>
        /// 啟用日期
        /// </summary>
        public DateTime StartDate { get; set; }

        /// <summary>
        /// 結束日期
        /// </summary>
        public DateTime EndDate { get; set; }

        /// <summary>
        /// 狀態
        /// 0.待審查
        /// 1.通過(審核完成)
        /// 2.退件
        /// 3.刪除(系統管理員可以看到資料)
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }
    }
}
