

using System.ComponentModel.DataAnnotations;

namespace SealTypographicWebAPI.Models.TemporarySeal
{
    /// <summary>
    /// 異動臨時章
    /// </summary>
    public class TemporarySealUpdateForm
    {
        /// <summary>
        /// 臨時章Id
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// 刪除臨時章印鑑組的ID清單
        /// </summary>
        public List<int> SealIdsToDelete { get; set; }

        /// <summary>
        /// 更新臨時章印鑑組
        /// </summary>        
        public List<TemporarySealUpdate> SealsToUpdate { get; set; }

        /// <summary>
        /// 新增臨時章印鑑組
        /// </summary>
        public List<TemporarySeal> SealsToCreate { get; set; }
    }
}
