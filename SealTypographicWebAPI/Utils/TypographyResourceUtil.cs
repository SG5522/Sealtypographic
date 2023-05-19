using DBEntitiesExtension;
using DBEntitiesExtension.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// Db上TypographyResourceUtil的一些常用的新增與修改的參數
    /// </summary>
    public class TypographyResourceUtil
    {
        /// <summary>
        /// 客戶印鑑新增修改時基本的資料輸入
        /// </summary>
        /// <param name="typographyResource">Db上的印鑑資料</param>
        /// <param name="isCreate">對Db所做的行動</param>
        /// <param name="userId">userId</param>
        public static void BaseInputTypographyResource (TypographyResource typographyResource, bool isCreate, int userId)
        {
            if (isCreate)
            {
                typographyResource.CreateUserId = userId;
                typographyResource.CreateDate = DateTime.Now;
                typographyResource.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                typographyResource.UpdateUserId = userId;
                typographyResource.UpdateDate = DateTime.Now;
            }
        }        
    }
}
