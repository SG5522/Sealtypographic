using SealTypographicWebAPI.Models.BaseModels;

namespace SealTypographicWebAPI.Models.Keycloak
{
    /// <summary>
    /// 顯示KeycloakUser 資料
    /// </summary>
    public class KeycloakUserDataPaginate : PaginateViewModel
    {
        /// <summary>
        /// 建置
        /// </summary>
        public KeycloakUserDataPaginate() 
        {
            UserDatas = new();
        }

        /// <summary>
        /// 顯示KeycloakUser 分頁資料
        /// </summary>
        public List<KeycloakUserDataViewModel> UserDatas { get; set; }
    }
}
