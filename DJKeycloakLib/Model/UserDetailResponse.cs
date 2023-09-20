using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// User詳細資料及回應訊息
    /// </summary>
    public class UserDetailResponse : ResponseBaseModel
    {
        /// <summary>
        /// User詳細資料
        /// </summary>
        public UserDetailViewModel UserDetail { get; set; }
    }
}
