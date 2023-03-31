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
    /// 會計師簽印樣板
    /// </summary>
    public class AccountSignSealTemplate : BaseTemplate
    {
        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 會計師簽印樣板位置
        /// </summary>
        public List<AccountSignSealTemplateLocation> AccountSignSealTemplateLocations { get; set; }
    }
}
