using DJKeycloakLib.Model.BaseModel;

namespace DJKeycloakLib.Model
{
    /// <summary>
    /// 顯示KeycloakUser 資料
    /// </summary>
    public class UserDataPaginate : PaginateViewModel
    {
        /// <summary>
        /// 建置
        /// </summary>
        public UserDataPaginate()
        {
            UserDatas = new();
        }

        /// <summary>
        /// 顯示KeycloakUser 分頁資料
        /// </summary>
        public List<UserDetailViewModel> UserDatas { get; set; }
    }
}
