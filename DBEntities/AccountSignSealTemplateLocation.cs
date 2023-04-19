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
    /// 會計師簽印樣板位置
    /// </summary>
    public class AccountSignSealTemplateLocation : BaseLocation
    {
        /// <summary>
        /// 簽印類別
        /// </summary>
        public AccountantSignType ConfigType { get; set; }

        /// <summary>
        /// 會計師簽印樣板表
        /// </summary>
        public AccountSignSealTemplate AccountSignSealTemplate { get; set; }
    }
}
