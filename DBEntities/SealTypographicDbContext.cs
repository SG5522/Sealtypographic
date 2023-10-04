using Microsoft.EntityFrameworkCore;

namespace DBEntities
{
    /// <summary>
    /// EF Core SealTypographic DbContext
    /// </summary>
    public class SealTypographicDbContext : DbContext
    {
        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public DbSet<Company> Companys { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public DbSet<Customer> Customers { get; set; }

        /// <summary>
        /// 季度
        /// </summary>
        public DbSet<QuarterYear> QuarterYears { get; set; }

        /// <summary>
        /// 客戶印鑑季度資料表
        /// </summary>
        public DbSet<CustomerSealGroup> CustomerSealGroups { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public DbSet<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public DbSet<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 會計師 與會計師群組多對多資料表
        /// </summary>
        public DbSet<GroupAccountant> GroupAccountants { get; set; }


        /// <summary>
        /// 會計師簽印建立日期歷程表
        /// </summary>
        public DbSet<AccountantSignGroup> AccountantSignGroups { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public DbSet<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 排版資源
        /// </summary>
        public DbSet<TypographicResource> TypographicResources { get; set; }

        /// <summary>
        /// 臨時章群組表
        /// </summary>
        public DbSet<TemporarySealGroup> TemporarySealGroups { get; set; }

        /// <summary>
        /// PDF排版資訊
        /// </summary>
        public DbSet<TypographicPDF> TypographicPDFs { get; set; }

        /// <summary>
        /// 排版頁
        /// </summary>
        public DbSet<TypographicPage> TypographicPages { get; set; }

        /// <summary>
        /// 各印鑑簽印排版位置
        /// </summary>
        public DbSet<TypographicResourceLocation> TypographicResourceLocations { get; set; }

        /// <summary>
        /// 樣板
        /// </summary>
        public DbSet<Template> Templates { get; set; }

        /// <summary>
        /// 樣板位置
        /// </summary>
        public DbSet<TemplateLocation> TemplateLocations { get; set; }

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
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Accountant>()
                .HasMany(e => e.AccountantGroups)
                .WithMany(e => e.Accountants)
                .UsingEntity<GroupAccountant>();
        }
        #endregion
    }
}
