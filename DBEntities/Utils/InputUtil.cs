using DBEntities.Consts;
using DBEntities.Entities.Base;

namespace DBEntities.Utils
{
    /// <summary>
    /// 各類資料的基本輸入
    /// </summary>
    public static class InputUtil
    {
        /// <summary>
        /// 各類資料表的基本輸入處理
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="input">輸入class</param>
        /// <param name="isCreate">是否為建立新表</param>
        /// <param name="userId">使用者Id</param>
        public static void Set<T>(T input, bool isCreate, int userId) where T : BaseData
        {
            if (isCreate)
            {
                input.CreateUserId = userId;                
                input.CreateDate = DateTime.Now;
                input.DeleteStatus = DeleteStatus.No;
            }
            else
            {
                input.UpdateUserId = userId;
                input.UpdateDate = DateTime.Now;
            }
        }
    }
}
