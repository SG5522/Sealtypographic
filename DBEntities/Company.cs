using DBEntities.Base;

namespace DBEntities
{
    /// <summary>
    /// 會計師事務所(公司)
    /// </summary>
    public class Company : BaseDetail
    {
        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> Customers { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public IList<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public IList<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public IList<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 客戶印鑑樣板
        /// </summary>
        public IList<Template> Templates { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public IList<UploadFile> UploadFiles { get; set; }

        /// <summary>
        /// 使用者資料表
        /// </summary>
        public IList<User> Users { get; set; }

        /// <summary>
        /// 圖片截取範圍設定
        /// </summary>
        public IList<ImageRangeSetting>? ImageRangeSettings { get; set; }

    }
}
