namespace DBEntities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class User
    {
        /// <summary>
        /// ID
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 登入帳號
        /// </summary>
        public string AccountName { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwaosrsd { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }
    }
}
