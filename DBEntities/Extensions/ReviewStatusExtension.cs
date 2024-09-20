using DBEntities.Consts;
using DBEntities.Entities.Base;

namespace DBEntities.Extensions
{
    public static class ReviewStatusExtension
    {
        /// <summary>
        /// 設定input的reviewStatus與相關欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="reviewStatus">客戶印鑑、會計師簽印的狀態</param>
        /// <param name="input">包含有BaseReviewData的資料庫model(Entites)</param>
        /// <param name="userId">使用者Id</param>        
        public static void Set<T>(this ReviewStatus reviewStatus, T input, int userId) where T : BaseReviewData
        {
            // For Draft, Pending, and Reject, 不增加任何處理
            //Disabled, Invalid, Refuse 
            if (reviewStatus >= ReviewStatus.Disabled)
            {
                input.DeleteStatus = DeleteStatus.Yes;
                //曾經是通過(啟用)的狀態變成停用 作廢 不受理 就將EndDate改成現在。
                if (input.ReviewStatus == ReviewStatus.Approval) input.EndDate = DateTimeOffset.Now;
            }
            //Approval 
            else if (reviewStatus == ReviewStatus.Approval)
            {
                input.StartDate = DateTimeOffset.Now;
                input.EndDate = DateTimeOffset.Parse("9999/12/31");                
            }

            // 更新草稿狀態以外的審核日期與使用者
            if (reviewStatus != ReviewStatus.Draft)
            {
                input.ReviewDate = DateTimeOffset.Now;
                input.ReviewUserId = userId;
            }

            input.ReviewStatus = reviewStatus;
        }
    }
}
