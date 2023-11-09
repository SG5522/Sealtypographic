using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.BaseModels
{
    /// <summary>
    /// 包含日期區間的分頁搜尋
    /// </summary>
    public class PaginateWithDateSearch : PaginateSearch
    {
        private DateTime startDate;
        private DateTime endDate;

        /// <summary>
        /// 日期區間 啟始日期
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime StartDate 
        {
            get
            {
                return startDate;
            }
            set
            {
                startDate = value.Date;                
            }
        }

        /// <summary>
        /// 日期區間 結束日期
        /// </summary>
        [Required]
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:yyyy/MM/dd}", ApplyFormatInEditMode = true)]
        public DateTime EndDate
        {
            get
            {
                return endDate;
            }
            set
            {
                endDate = value.Date;
            }
        }

    }
}
