using DBEntities.Consts;
using SealTypographicWebAPI.Models.Accountant;
using System.Text.Json.Serialization;

namespace SealTypographicWebAPI.Models.AccountantSignReview
{
    /// <summary>    
    /// 會計師簽印審核詳細資料
    /// </summary>
    public class AccountantSignGroupDetailReviewViewModel : AccountantDetailData
    {
        /// <summary>
        /// new Signs
        /// </summary>
        public AccountantSignGroupDetailReviewViewModel() 
        {
            Signs = new();
        }

        /// <summary>
        /// 會計師簽印組Id
        /// </summary>
        /// <example>1</example>
        public int Id { get; set; }

        //--------------log Save Data-------------//
        /// <summary>
        /// 會計師ID
        /// </summary>
        [JsonIgnore]
        public int AccountantId { get; set; }

        /// <summary>
        /// 會計簽印群組建立日期
        /// </summary>
        [JsonIgnore]
        public DateTime GroupCreateDate { get; set; }
        //--------------log Save Data-------------//

        /// <summary>
        /// 會計師簽印組
        /// </summary>
        public List<AccountantSignViewModel> Signs { get; set; }
    }    
}
