using Microsoft.EntityFrameworkCore;

namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// EF Core DbContext
    /// </summary>
    public class MySqlDbContext : SealTypographicDbContext
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="configuration"></param>
        public MySqlDbContext(IConfiguration configuration) : base(configuration)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="options"></param>
        protected override void OnConfiguring(DbContextOptionsBuilder options)
        {
            MySqlServerVersion mySqlServerVersion = new(new Version(8, 0, 32));
            options.UseMySql(Configuration.GetConnectionString("MySql"), mySqlServerVersion);
        }
    }
}
