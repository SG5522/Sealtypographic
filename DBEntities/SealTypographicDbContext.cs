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
        /// 使用者資料表
        /// </summary>
        public DbSet<User> Users { get; set; }

        /// <summary>
        /// 圖片截取設定資料表
        /// </summary>
        public DbSet<ImageRangeSetting> ImageRangeSettings {get; set;}

        /// <summary>
        /// 圖片截取範圍設定資料表
        /// </summary>
        public DbSet<ImageRangeLocation> ImageRangeLocations { get; set; }

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

            modelBuilder.Entity<Accountant>()
                .HasOne(e => e.CreateUser)                
                .WithMany(e => e.AccountantsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Accountant>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<AccountantGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.AccountantGroupsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<AccountantGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantGroupsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<AccountantSignGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.AccountantSignGroupsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<AccountantSignGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantSignGroupsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CustomersCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CustomersUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<CustomerSealGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CustomerSealGroupsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<CustomerSealGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CustomerSealGroupsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Letterhead>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.LetterheadsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Letterhead>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.LetterheadsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TemporarySealGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TemporarySealGroupsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TemporarySealGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TemporarySealGroupsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TypographicResource>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TypographicResourcesCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TypographicResource>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TypographicResourcesUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<UploadFile>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.UploadFilesCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<UploadFile>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.UploadFilessUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TypographicPDF>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TypographicPDFsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TypographicPDF>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TypographicPDFsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Template>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TemplatesCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Template>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TemplatesUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Company>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CompanysCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Company>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CompanysUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ImageRangeSetting>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.ImageRangeSettingsCreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ImageRangeSetting>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.ImageRangeSettingsUpdateUser)
                .HasForeignKey(e => e.UpdateUserId);
        }
        #endregion
    }
}
