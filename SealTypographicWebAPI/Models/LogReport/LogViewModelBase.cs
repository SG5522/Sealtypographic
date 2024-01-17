using MongoDB.Bson.Serialization.Attributes;

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
        public string UserId { get; set; }

        /// <summary>
        /// 使用者姓名
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 紀錄日期
        /// </summary>        
        public DateTime DateTime 
        {
            //MongoDB預設紀錄的時間為UTC，顯示在畫面時要轉成本地時間。
            get => dateTime;
            set => dateTime = value.ToLocalTime();
        }
    }
}
