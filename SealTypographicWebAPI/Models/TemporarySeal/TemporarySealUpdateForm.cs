

using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 異動臨時章
    /// </summary>
    public class TemporarySealUpdateForm
    {
        /// <summary>
        /// 刪除簽印列表(ID)
        /// </summary>
        public List<int> SealIdsToDelete { get; set; }

        /// <summary>
        /// 更新臨時印鑑組
        /// </summary>        
        public List<TemporarySealUpdate> SealsToUpdate { get; set; }

        /// <summary>
        /// 新增臨時印鑑組
        /// </summary>
        public List<TemporarySeal> SealsToCreate { get; set; }
    }
}
