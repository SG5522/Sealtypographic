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
    /// 客戶印鑑樣板位置
    /// </summary>
    public class CustomerSealTemplateLocation : BaseLocation
    {
        /// <summary>
        /// 印鑑配置類別
        /// </summary>
        public CustomerSealType ConfigType { get; set; }

        /// <summary>
        /// 
        /// </summary>
        public CustomerSealTemplate CustomerTemplate { get; set; }
    }
}
