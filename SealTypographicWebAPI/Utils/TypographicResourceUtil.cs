using DBEntities;
using DBEntities.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// Db上TypographyResourceUtil的一些常用的新增與修改的參數
    /// </summary>
    public class TypographicResourceUtil
    {
        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="typographicResource">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        public static void BaseInputTypographyResource (TypographicResource typographicResource, bool isCreate, int userId)
        {
            if (isCreate)
            {
                typographicResource.CreateUserId = userId;
                typographicResource.CreateDate = DateTime.Now;
                typographicResource.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                typographicResource.UpdateUserId = userId;
                typographicResource.UpdateDate = DateTime.Now;
            }
        }

        /// <summary>        
        /// 過濾刪除及重新排序
        /// </summary>
        /// <param name="typographicResources"></param>
        public static void FilterDeleteResource(List<TypographicResource> typographicResources)
        {
            typographicResources = typographicResources
                                .Where(x => x.DeleteStatus == DeleteStatus.No)
                                .OrderBy(x => x.SubSealType)
                                .ThenBy(x => x.Sequence)
                                .ToList();
        }
    }
}
