using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Consts;

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
        public DbSet<AccountantSignLocation> AccountantSignLocaltions { get; set; }

        /// <summary>
        /// 信頭圖片排版位置
        /// </summary>
        public DbSet<LetterheadImageLocation> LetterheadImageLocaltions { get; set; }

        /// <summary>
        /// 使用者
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SealTypographicDbContext(DbContextOptions<SealTypographicDbContext> options) : base(options)
        {
        }

        #region Required
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            //建立DB前先建置SealMappingConfig資料
            modelBuilder.Entity<SealMappingConfig>().HasData(new SealMappingConfig
            {
                Id = 1,
                SealType = SealType.Customer,
                SubId = "companySeal",
                Name = "公司章"
            },
            new SealMappingConfig
            {
                Id = 2,
                SealType = SealType.Customer,
                SubId = "ceoSeal",
                Name = "負責人"
            },
            new SealMappingConfig
            {
                Id = 3,
                SealType = SealType.Customer,
                SubId = "managerSeal",
                Name = "經理"
            },
            new SealMappingConfig
            {
                Id = 4,
                SealType = SealType.Customer,
                SubId = "accountantDirectorSeal",
                Name = "會計主管"
            },
            new SealMappingConfig
            {
                Id = 5,
                SealType = SealType.Customer,
                SubId = "customerOther",
                Name = "其他"
            },
            new SealMappingConfig
            {
                Id = 6,
                SealType = SealType.Accountant,
                SubId = "accountantSeal",
                Name = "會計師印鑑"
            },
            new SealMappingConfig
            {
                Id = 7,
                SealType = SealType.Accountant,
                SubId = "accountantCHSign",
                Name = "中文簽名"
            },
            new SealMappingConfig
            {
                Id = 8,
                SealType = SealType.Accountant,
                SubId = "accountantENSign",
                Name = "英文簽名"
            },
            new SealMappingConfig
            {
                Id = 9,
                SealType = SealType.Accountant,
                SubId = "accountantOldSign",
                Name = "舊式簽名"
            },
            new SealMappingConfig
            {
                Id = 10,
                SealType = SealType.Accountant,
                SubId = "accountantOther",
                Name = "其他"
            });

            //建立DB前先建置AccountantGroup無群組資料
            modelBuilder.Entity<AccountantGroup>().HasData(new AccountantGroup
            {
                Id = 1,
                AccountantGroupNumber = "NO000",
                //CreateDate = DateTime.Parse("0001/01/01 00:00:00"),
                //UpdateDate = DateTime.Parse("0001/01/01 00:00:00"),
                CreateUserId = 0,
                UpdateUserId = 0,
                DeleteStatus = 0,
                Name = "無群組"
            });
        }
        #endregion
    }
}
