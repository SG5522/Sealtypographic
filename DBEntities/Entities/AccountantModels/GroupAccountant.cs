namespace DBEntities.Entities.AccountantModels
{
    /// <summary>
    /// 會計師群組
    /// </summary>
    public class GroupAccountant
    {
        /// <summary>
        /// 會計師資料表Id
        /// </summary>
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public Accountant Accountant { get; set; }

        /// <summary>
        /// 會計師群組資料表Id
        /// </summary>
        public int AccountantGroupId { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public AccountantGroup AccountantGroup { get; set; }
    }
}
