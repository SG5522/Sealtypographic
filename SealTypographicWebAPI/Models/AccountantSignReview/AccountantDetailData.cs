namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>
    /// 會計師基本資料
    /// </summary>
    public class AccountantDetailData
    {
        /// <summary>
        /// 姓名
        /// </summary>
        /// <example>王XX</example>
        public string Name { get; set; }

        /// <summary>
        /// 會計師編號
        /// </summary>
        /// <example>ACC123</example>
        public string Code { get; set; }

        /// <summary>
        /// 會計師群組名稱
        /// </summary>
        /// <example>台北群組</example>
        public string GroupName { get; set; }
    }
}
