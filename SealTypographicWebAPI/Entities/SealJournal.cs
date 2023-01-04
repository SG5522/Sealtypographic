using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 客戶印鑑組歷程資料表
    /// </summary>
    public class SealJournal : BaseData
    {
        ///// <summary>
        ///// 客戶ID
        ///// </summary>
        //public int CustomerId { get; set; }

        ///// <summary>
        ///// 會計師ID
        ///// </summary>
        //public int AccountantId { get; set; }

        ///// <summary>
        ///// 信頭ID
        ///// </summary>
        //public int LetterheadId { get; set; }

        /// <summary>
        /// 印鑑類別
        /// </summary>
        public SealMappingConfigType SealMappingConfigType { get; set; }

        /// <summary>
        /// 印鑑路徑
        /// </summary>
        public string ImagePath { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public Customer? Customer { get; set; }

        /// <summary>
        /// 會計資料表
        /// </summary>
        public Accountant? Accountant { get; set; }

        /// <summary>
        /// 信頭資料表
        /// </summary>
        public Letterhead? Letterhead { get; set; }

        /// <summary>
        /// 印鑑審核歷程資料表
        /// </summary>
        public SealReviewJournal SealReviewJournal { get; set; }

    }
}
