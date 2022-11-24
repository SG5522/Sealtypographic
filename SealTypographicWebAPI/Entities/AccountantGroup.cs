namespace SealTypographicWebAPI.Entities
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class AccountantGroup
    {
        /// <summary>
        /// 群組ID
        /// </summary>
        public string Id { get; set; } = null!;

        /// <summary>
        /// 群組名稱
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public List<Accountant> Accountants { get; set; }
    }
}
