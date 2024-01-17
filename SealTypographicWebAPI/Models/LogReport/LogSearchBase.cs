using SealTypographicWebAPI.Models.BaseModels;
using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// Log基本搜尋
    /// </summary>
    public abstract class LogSearchBase : PaginateSearch
    {
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// 起始日期
        /// </summary>
        [Required]
        public DateTime StartDate
        {
            //MongoDB預設紀錄的時間為utc需要先轉成utc搜尋
            get => startDate;
            set => startDate = value.ToUniversalTime();
        }

        /// <summary>
        /// 結束日期
        /// </summary>
        [Required]
        public DateTime EndDate
        {
            get => endDate;
            set
            {
                // 調整為結束日期的 23:59:59
                //MongoDB預設紀錄的時間為utc需要先轉成utc搜尋
                endDate = value.Date.AddDays(1).AddSeconds(-1).ToUniversalTime();                
            }
        }
    }
}
