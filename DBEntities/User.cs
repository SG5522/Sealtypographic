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
        /// 使用者匿稱
        /// </summary>
        public string? NickName { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime UpdateDate { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> CustomersCreateUser { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> CustomersUpdateUser { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroupsCreateUser { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroupsUpdateUser { get; set; }

        /// <summary>
        /// 會計師資料表關連CreateUser
        /// </summary>
        public IList<Accountant> AccountantsCreateUser { get; set; }

        /// <summary>
        /// 會計師資料表關連UpdateUser
        /// </summary>
        public IList<Accountant> AccountantsUpdateUser { get; set; }

        /// <summary>
        /// 會計師群組關連CreateUser
        /// </summary>
        public IList<AccountantGroup> AccountantGroupsCreateUser { get; set; }

        /// <summary>
        /// 會計師群組關連UpdateUser
        /// </summary>
        public IList<AccountantGroup> AccountantGroupsUpdateUser { get; set; }

        /// <summary>
        /// 上傳檔案資料表關連CreateUser
        /// </summary>
        public IList<UploadFile> UploadFilesCreateUser { get; set; }

        /// <summary>
        /// 上傳檔案資料表關連UpdateUser
        /// </summary>
        public IList<UploadFile> UploadFilessUpdateUser { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表關連CreateUser
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroupsCreateUser { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表關連UpdateUser
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroupsUpdateUser { get; set; }

        /// <summary>關連CreateUser
        /// 事務所信頭
        /// </summary>
        public IList<Letterhead> LetterheadsCreateUser { get; set; }

        /// <summary>
        /// 事務所信頭關連UpdateUser
        /// </summary>
        public IList<Letterhead> LetterheadsUpdateUser { get; set; }


        /// <summary>
        /// 排版資源關連CreateUser
        /// </summary>
        public IList<TypographicResource> TypographicResourcesCreateUser { get; set; }

        /// <summary>
        /// 排版資源關連UpdateUser
        /// </summary>
        public IList<TypographicResource> TypographicResourcesUpdateUser { get; set; }

        /// <summary>
        /// 臨時章群組關連CreateUser
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroupsCreateUser { get; set; }

        /// <summary>
        /// 臨時章群組關連UpdateUser
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroupsUpdateUser { get; set; }

        /// <summary>
        /// PDF排版資訊關連CreateUser
        /// </summary>
        public IList<TypographicPDF> TypographicPDFsCreateUser { get; set; }

        /// <summary>
        /// PDF排版資訊關連UpdateUser
        /// </summary>
        public IList<TypographicPDF> TypographicPDFsUpdateUser { get; set; }

        /// <summary>
        /// 樣板關連CreateUser
        /// </summary>
        public IList<Template> TemplatesCreateUser { get; set; }

        /// <summary>
        /// 樣板關連UpdateUser
        /// </summary>
        public IList<Template> TemplatesUpdateUser { get; set; }

        /// <summary>
        /// 會計師事務所(公司)關連CreateUser
        /// </summary>
        public IList<Company> CompanysCreateUser { get; set; }

        /// <summary>
        /// 會計師事務所(公司)關連UpdateUser
        /// </summary>
        public IList<Company> CompanysUpdateUser { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company? Company { get; set; }

        /// <summary>
        /// 圖片截取範圍設定關連CreateUser
        /// </summary>
        public IList<ImageRangeSetting> ImageRangeSettingsCreateUser { get; set; }

        /// <summary>
        /// 圖片截取範圍設定關連UpdateUser
        /// </summary>
        public IList<ImageRangeSetting> ImageRangeSettingsUpdateUser { get; set; }
    }
}
