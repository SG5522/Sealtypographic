using Microsoft.AspNetCore.Identity;

namespace DBEntities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class User : IdentityUser
    {
        public User() : base()
        {
            Id = 0;
        }

        /// <summary>
        /// 
        /// </summary>
        public new int Id { get; set; }

        /// <summary>
        /// 密碼
        /// </summary>
        public string Pwaosrsd { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }
    }
}
