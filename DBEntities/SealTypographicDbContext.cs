using DBEntities.Entities;
using DBEntities.Entities.AccountantModels;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.ImageRangeModels;
using DBEntities.Entities.TemplateModels;
using DBEntities.Entities.TypographicModels;
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
        public DbSet<ApplicationUser> ApplicationUsers { get; set; }

        /// <summary>
        /// 圖片截取設定資料表
        /// </summary>
        public DbSet<ImageRangeSetting> ImageRangeSettings { get; set; }

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
                .WithMany(e => e.AccountantCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Accountant>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<AccountantGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.AccountantGroupCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<AccountantGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantGroupUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<AccountantSignGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.AccountantSignGroupCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<AccountantSignGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.AccountantSignGroupUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CustomerCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Customer>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CustomerUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<CustomerSealGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CustomerSealGroupCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<CustomerSealGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CustomerSealGroupUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Letterhead>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.LetterheadCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Letterhead>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.LetterheadUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TemporarySealGroup>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TemporarySealGroupCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TemporarySealGroup>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TemporarySealGroupUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TypographicResource>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TypographicResourceCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TypographicResource>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TypographicResourceUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<UploadFile>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.UploadFileCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<UploadFile>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.UploadFileUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<TypographicPDF>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TypographicPDFCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<TypographicPDF>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TypographicPDFUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Template>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.TemplateCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Template>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.TemplateUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<Company>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.CompanyCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<Company>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.CompanyUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ImageRangeSetting>()
                .HasOne(e => e.CreateUser)
                .WithMany(e => e.ImageRangeSettingCreateUsers)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ImageRangeSetting>()
                .HasOne(e => e.UpdateUser)
                .WithMany(e => e.ImageRangeSettingUpdateUsers)
                .HasForeignKey(e => e.UpdateUserId);
        }
        #endregion
    }
}
