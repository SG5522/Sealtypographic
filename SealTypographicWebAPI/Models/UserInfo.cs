namespace SealTypographicWebAPI.Models
{
    /// <summary>
    /// 登入使用者基本資訊
    /// </summary>
    public class UserInfo
    {
        /// <summary>
        /// 使用者Id
        /// </summary>
        public int ApplicationUserId { get; set; }

        /// <summary>
        /// 使用者名稱(帳號)
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string? FirstName { get; set; }

        /// <summary>
        /// 使用者暱稱
        /// </summary>
        public string? LastName { get; set; }

    }
}
