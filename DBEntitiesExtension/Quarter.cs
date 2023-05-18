using DBEntitiesExtension.Base;

namespace DBEntitiesExtension
{
    /// <summary>
    /// 季度
    /// </summary>
    public class Quarter : BaseData
    {
        /// <summary>
        /// 西元年(公曆)
        /// </summary>
        public string GregorianYear { get; set; }

        /// <summary>
        /// 民國年
        /// </summary>
        public string TaiwanYear { get; set; }

        /// <summary>
        /// 季度時期(Q1,Q2,Q3,Q4)
        /// </summary>
        public string Period { get; set; }
    }
}
