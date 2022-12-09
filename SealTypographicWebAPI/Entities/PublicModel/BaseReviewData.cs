using SealTypographicWebAPI.Consts;

namespace SealTypographicWebAPI.Entities.PublicModel
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
        /// 審核狀態狀態
        /// 0.通過(審核完成)
        /// 10.待審查
        /// 20.退件
        /// </summary>
        public ReviewStatus ReviewStatus { get; set; }

        /// <summary>
        /// 刪除狀態
        /// 0.無標記
        /// 1.刪除或隱藏
        /// </summary>
        public DeleteStatus DeleteStatus { get; set; }
    }
}
