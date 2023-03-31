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
    /// 信頭樣板位置
    /// </summary>
    public class LetterheadImageTemplateLocation : BaseLocation
    {
        /// <summary>
        /// 信頭樣板
        /// </summary>
        public LetterheadImageTemplate CustomerTemplate { get; set; }
    }
}
