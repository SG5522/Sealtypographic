using Microsoft.AspNetCore.Identity;

namespace DBEntities
{
    /// <summary>
    /// 使用者
    /// </summary>
    public class User : IdentityUser<int>
    {
        public User() : base()
        {
            Id = 0;
        }

        /// <summary>
        /// Keycloak上的UserId
        /// </summary>
        public string KeycloakUserId { get; set; }

        /// <summary>
        /// 會計師事務所(公司)
        /// </summary>
        public Company Company { get; set; }

        /// <summary>
        /// 圖片截取範圍設定
        /// </summary>
        public IList<ImageCaptureSetting>? ImageCaptureSettings { get; set; }
    }
}
