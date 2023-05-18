using DBEntitiesExtension.Base;

namespace DBEntitiesExtension
{
    /// <summary>
    /// 會計師簽印建立日期歷程表
    /// </summary>
    public class AccountantSignGroup : BaseReviewData
    {
        /// <summary>
        /// 會計師基本資料
        /// </summary>
        public Accountant Accountant { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public UploadFile UploadFile { get; set; }

        /// <summary>
        /// 排版素材
        /// </summary>
        public List<TypographyAsset> TypographyAssets { get; set; }

    }
}
