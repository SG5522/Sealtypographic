using Microsoft.EntityFrameworkCore;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// EF Core SealTypographic DbContext
    /// </summary>
    public class SealTypographicDbContext : DbContext
    {
        /// <summary>
        /// 顧客資料表
        /// </summary>
        public DbSet<Customer> Customers { get; set; }
        /// <summary>
        /// 顧客印鑑歷程資料表
        /// </summary>
        public DbSet<CustomerSealJournal> CustomerSealJournals { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public DbSet<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public DbSet<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 會計師印鑑簽名組歷程資料表
        /// </summary>
        public DbSet<AccountantSignJournal> AccountantSignJournals { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public DbSet<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 事務所信頭圖片歷程資料表
        /// </summary>
        public DbSet<LetterheadImageJournal> LetterheadImageJournals { get; set; }

        /// <summary>
        /// 圖片群組資料表 (使用印鑑、簽名、LOGO)
        /// </summary>
        public DbSet<SealMappingConfig> SealMappingConfigs { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public DbSet<TypographicPDF> TypographicPDFs { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public DbSet<TypographicPage> TypographicPages { get; set; }

        /// <summary>
        /// 客戶印鑑排版位置
        /// </summary>
        public DbSet<CustomerSealLocation> CustomerSealLocaltions { get; set; }

        /// <summary>
        /// 會計師印鑑簽名ID
        /// </summary>
        public DbSet<AccountantSingLocation> AccountantSingLocaltions { get; set; }

        /// <summary>
        /// 信頭圖片排版位置
        /// </summary>
        public DbSet<LetterheadImageLocation> LetterheadImageLocaltions { get; set; }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SealTypographicDbContext(DbContextOptions<SealTypographicDbContext> options) : base(options)
        {
        }
    }
}
