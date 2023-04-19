using DBEntities.Base;
using DBEntities.Consts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntities
{
    /// <summary>
    /// 客戶印鑑樣板
    /// </summary>
    public class CustomerSealTemplate : BaseTemplate
    {
        /// <summary>
        /// 樣板疊放方式
        /// </summary>
        public StackMode StackMode { get; set; }

        /// <summary>
        /// 樣板疊放位移
        /// </summary>
        public int StackShift { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public List<CustomerSealTemplateLocation> CustomerTempTemplateLocations { get; set; }
    }
}
