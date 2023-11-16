using Microsoft.AspNetCore.Identity;

namespace DBEntities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class User : IdentityUser<int>
    {
        public User() : base()
        {
            Id = 0;
        }

        /// <summary>
        /// Keycloak上的UserId
        /// </summary>
        public string KeycloakUserId { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> Customers { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroups { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public IList<Accountant> AccountantsCreateUser { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public IList<Accountant> AccountantsUpdateUser { get; set; }

        /// <summary>
        /// 會計師群組
        /// </summary>
        public IList<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public IList<UploadFile> UploadFiles { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroups { get; set; }

        /// <summary>
        /// 事務所信頭
        /// </summary>
        public IList<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 排版資源
        /// </summary>
        public IList<TypographicResource> TypographicResources { get; set; }

        /// <summary>
        /// 臨時章群組
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroups { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public IList<TypographicPDF> TypographicPDFs { get; set; }

        /// <summary>
        /// 樣板
        /// </summary>
        public IList<Template> Templates { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 圖片截取範圍設定
        /// </summary>
        public IList<ImageRangeSetting>? ImageRangeSettings { get; set; }
    }
}
