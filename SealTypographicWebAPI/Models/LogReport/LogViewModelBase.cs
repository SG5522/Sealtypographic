using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.LogReport
{
    /// <summary>
    /// LogViewModel基本結構
    /// </summary>
    public abstract class LogViewModelBase
    {
        private DateTime dateTime;

        /// <summary>
        /// 使用者id
        /// </summary>
        [Display(Order = 1)]
        public string UserId { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        [Display(Order = 2)]
        public string UserName { get; set; }

        /// <summary>
        /// 紀錄日期
        /// </summary>        
        [Display(Order = 3)]
        public DateTime DateTime 
        {
            //MongoDB預設紀錄的時間為UTC，顯示在畫面時要轉成本地時間。
            get => dateTime;
            set => dateTime = value.ToLocalTime();
        }
    }
}
