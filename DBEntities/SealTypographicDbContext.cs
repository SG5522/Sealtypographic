using Microsoft.EntityFrameworkCore;

namespace DBEntities
{
    /// <summary>
    /// EF Core SealTypographic DbContext
    /// </summary>
    public class SealTypographicDbContext : DbContext
    {
        /// <summary>
        /// 客戶資料表
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// 客戶印鑑歷程資料表
        /// </summary>
        public DbSet<CustomerSealJournal> CustomerSealJournals { get; set; }

        /// <summary>
        /// 客戶印鑑季度歷程資料表
        /// </summary>
        public DbSet<CustomerSealQuarterJournal> CustomerSealQuarterJournals { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public DbSet<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public DbSet<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 會計師簽印歷程表
        /// </summary>
        public DbSet<AccountantSignJournal> AccountantSignJournals { get; set; }

        /// <summary>
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public DbSet<AccountantSignGroupJournal> AccountantSignGroupJournals { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public DbSet<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 事務所信頭圖片歷程資料表
        /// </summary>
        public DbSet<LetterheadImageJournal> LetterheadImageJournals { get; set; }

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
        /// 上傳檔案資料表
        /// </summary>
        public DbSet<UploadFile> UploadFiles { get; set; }

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
            //為了確保每次執行OnModelCreating都是正確的重寫DbContext設定，
            //需要將下列這行加入，但如果沒有其他的OnModelCreating是也可以不加，
            //只是怕未來需要維護繼承此DbContext怕會忘記故此加入。
            base.OnModelCreating(modelBuilder);
        }
        #endregion
    }
}
