using DBEntities.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntities
{
    /// <summary>
    /// 會計師事務所(公司)
    /// </summary>
    public class Company : BaseNameData
    {
        /// <summary>
        /// 公司編號
        /// </summary>
        public string Code { get; set; }

        /// <summary>
        /// 統一編號 (business administration number)
        /// </summary>
        public string? BAN { get; set; }

        /// <summary>
        /// 客戶資料表
        /// </summary>
        public List<Customer> Customers { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public List<Accountant> Accountants { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public List<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 客戶印鑑樣板
        /// </summary>
        public List<CustomerSealTemplate> CustomerSealTemplates { get; set; }

        /// <summary>
        /// 會計師印鑑樣板
        /// </summary>
        public List<AccountSignSealTemplate> AccountSignSealTemplates { get; set; }

        /// <summary>
        /// 信頭圖片樣板
        /// </summary>
        public List<LetterheadImageTemplate> LetterheadImageTemplates { get; set; }

    }
}
