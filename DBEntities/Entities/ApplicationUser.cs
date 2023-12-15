using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.ImageRangeModels;
using DBEntities.Entities.TemplateModels;
using DBEntities.Entities.TypographicModels;
using Microsoft.AspNetCore.Identity;

namespace DBEntities.Entities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class ApplicationUser : IdentityUser<int>
    {
        public ApplicationUser() : base()
        {
            Id = 0;
        }

        /// <summary>
        /// 使用者姓氏
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 使用者名稱
        /// </summary>
        public string? LastName { get; set; }

        /// <summary>
        /// 建立日期
        /// </summary>
        public DateTime CreateDate { get; set; }

        /// <summary>
        /// 更新日期
        /// </summary>
        public DateTime? UpdateDate { get; set; }

        /// <summary>
        /// 建立此筆資料的User
        /// </summary>        
        public int? CreateUserId { get; set; }

        /// <summary>
        /// 建立的關聯User表
        /// </summary>        
        public ApplicationUser? CreateUser { get; set; }

        /// <summary>
        /// 更新此筆資料的User
        /// </summary>        
        public int? UpdateUserId { get; set; }

        /// <summary>
        /// 更新的關聯User表
        /// </summary>       
        public ApplicationUser? UpdateUser { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company? Company { get; set; }

        /// <summary>
        /// 使用者資料表(Create)
        /// </summary>
        public IList<ApplicationUser> CreateUsers { get; set; }

        /// <summary>
        /// 使用者資料表(Update)
        /// </summary>
        public IList<ApplicationUser> UpdateUsers { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> CustomerCreateUsers { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public IList<Customer> CustomerUpdateUsers { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroupCreateUsers { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public IList<CustomerSealGroup> CustomerSealGroupUpdateUsers { get; set; }

        /// <summary>
        /// 會計師資料表關連CreateUser
        /// </summary>
        public IList<Accountant> AccountantCreateUsers { get; set; }

        /// <summary>
        /// 會計師資料表關連UpdateUser
        /// </summary>
        public IList<Accountant> AccountantUpdateUsers { get; set; }

        /// <summary>
        /// 會計師群組關連CreateUser
        /// </summary>
        public IList<AccountantGroup> AccountantGroupCreateUsers { get; set; }

        /// <summary>
        /// 會計師群組關連UpdateUser
        /// </summary>
        public IList<AccountantGroup> AccountantGroupUpdateUsers { get; set; }

        /// <summary>
        /// 上傳檔案資料表關連CreateUser
        /// </summary>
        public IList<UploadFile> UploadFileCreateUsers { get; set; }

        /// <summary>
        /// 上傳檔案資料表關連UpdateUser
        /// </summary>
        public IList<UploadFile> UploadFileUpdateUsers { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表關連CreateUser
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroupCreateUsers { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表關連UpdateUser
        /// </summary>
        public IList<AccountantSignGroup> AccountantSignGroupUpdateUsers { get; set; }

        /// <summary>關連CreateUser
        /// 事務所信頭
        /// </summary>
        public IList<Letterhead> LetterheadCreateUsers { get; set; }

        /// <summary>
        /// 事務所信頭關連UpdateUser
        /// </summary>
        public IList<Letterhead> LetterheadUpdateUsers { get; set; }


        /// <summary>
        /// 排版資源關連CreateUser
        /// </summary>
        public IList<TypographicResource> TypographicResourceCreateUsers { get; set; }

        /// <summary>
        /// 排版資源關連UpdateUser
        /// </summary>
        public IList<TypographicResource> TypographicResourceUpdateUsers { get; set; }

        /// <summary>
        /// 臨時章群組關連CreateUser
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroupCreateUsers { get; set; }

        /// <summary>
        /// 臨時章群組關連UpdateUser
        /// </summary>
        public IList<TemporarySealGroup> TemporarySealGroupUpdateUsers { get; set; }

        /// <summary>
        /// PDF排版資訊關連CreateUser
        /// </summary>
        public IList<TypographicPDF> TypographicPDFCreateUsers { get; set; }

        /// <summary>
        /// PDF排版資訊關連UpdateUser
        /// </summary>
        public IList<TypographicPDF> TypographicPDFUpdateUsers { get; set; }

        /// <summary>
        /// 樣板關連CreateUser
        /// </summary>
        public IList<Template> TemplateCreateUsers { get; set; }

        /// <summary>
        /// 樣板關連UpdateUser
        /// </summary>
        public IList<Template> TemplateUpdateUsers { get; set; }

        /// <summary>
        /// 會計師事務所(公司)關連CreateUser
        /// </summary>
        public IList<Company> CompanyCreateUsers { get; set; }

        /// <summary>
        /// 會計師事務所(公司)關連UpdateUser
        /// </summary>
        public IList<Company> CompanyUpdateUsers { get; set; }

        /// <summary>
        /// 圖片截取範圍設定關連CreateUser
        /// </summary>
        public IList<ImageRangeSetting> ImageRangeSettingCreateUsers { get; set; }

        /// <summary>
        /// 圖片截取範圍設定關連UpdateUser
        /// </summary>
        public IList<ImageRangeSetting> ImageRangeSettingUpdateUsers { get; set; }
    }
}
