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

            //--以下為User關聯處理--Strat//
            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UpdateUsers)
                .WithOne(e => e.UpdateUser)                
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantGroupCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantGroupUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantSignGroupCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.AccountantSignGroupUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CustomerCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CustomerUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CustomerSealGroupCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CustomerSealGroupUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.LetterheadCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.LetterheadUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TemporarySealGroupCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TemporarySealGroupUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TypographicResourceCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TypographicResourceUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UploadFileCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.UploadFileUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TypographicPDFCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TypographicPDFUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TemplateCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.TemplateUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CompanyCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.CompanyUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.ImageRangeSettingCreateUsers)
                .WithOne(e => e.CreateUser)
                .HasForeignKey(e => e.CreateUserId);

            modelBuilder.Entity<ApplicationUser>()
                .HasMany(e => e.ImageRangeSettingUpdateUsers)
                .WithOne(e => e.UpdateUser)
                .HasForeignKey(e => e.UpdateUserId);
            //--User關聯處理--End//
        }
        #endregion
    }
}
