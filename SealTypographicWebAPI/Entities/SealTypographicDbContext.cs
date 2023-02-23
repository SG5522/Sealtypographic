using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Entities.BaseEntities;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// EF Core SealTypographic DbContext
    /// </summary>
    public class SealTypographicDbContext : DataContext
    {        
        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        public SealTypographicDbContext(DbContextOptions<DataContext> options) : base(options)
        {
        }

        #region Required
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelBuilder"></param>
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {           
            //建立DB前先建置AccountantGroup無群組資料
            modelBuilder.Entity<AccountantGroup>().HasData(new AccountantGroup
            {
                Id = 1,
                AccountantGroupNumber = "NO000",
                CreateUserId = 0,
                UpdateUserId = 0,
                DeleteStatus = 0,
                Name = "預設群組"
            });
        }
        #endregion
    }
}
