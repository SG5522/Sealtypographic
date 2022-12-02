namespace SealTypographicWebAPI.Models.Letterhead
{
    /// <summary>
    /// 信頭資料
    /// </summary>
    public class LetterheadForm : PostCreateData
    {
        /// <summary>
        /// 信頭ID
        /// </summary>
        /// <example>Lh001</example>
        public string Id { get; set; }

        /// <summary>
        /// 信頭名稱
        /// </summary>
        /// <example>測試信頭</example>
        public string Name { get; set; }
    }
}
