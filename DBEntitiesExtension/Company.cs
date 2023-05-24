using DBEntitiesExtension.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DBEntitiesExtension
{
    /// <summary>
    /// 會計師事務所(公司)
    /// </summary>
    public class Company : BaseDetail
    {
        /// <summary>
        /// 客戶資料表
        /// </summary>
        public List<Customer> Customers { get; set; }

        /// <summary>
        /// 會計師資料表
        /// </summary>
        public List<Accountant> Accountants { get; set; }

        /// <summary>
        /// 會計師群組資料表
        /// </summary>
        public List<AccountantGroup> AccountantGroups { get; set; }

        /// <summary>
        /// 事務所信頭資料表
        /// </summary>
        public List<Letterhead> Letterheads { get; set; }

        /// <summary>
        /// 客戶印鑑樣板
        /// </summary>
        public List<Template> Templates { get; set; }

        /// <summary>
        /// 上傳檔案資料表
        /// </summary>
        public List<UploadFile> UploadFiles { get; set; }

        /// <summary>
        /// 使用者資料表
        /// </summary>
        public List<User> Users { get; set; }

    }
}
