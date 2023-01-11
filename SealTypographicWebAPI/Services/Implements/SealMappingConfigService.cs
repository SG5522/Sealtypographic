using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.SealMappingConfig;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得印鑑類型列表 (客戶印鑑、會計師簽印)
    /// </summary>
    public class SealMappingConfigService
    {
        /// <summary>
        /// 取得印鑑類型列表
        /// </summary>
        /// <param name="sealType">印鑑類型</param>
        /// <returns></returns>
        public SealMappingConfigResponseList Get(SealType sealType)
        {
            SealMappingConfigResponseList sealMappingConfigResponseList = new();
            
            switch (sealType)
            {
                case SealType.Customer:
                    foreach (CustomerSealType customerSealType in (CustomerSealType[])Enum.GetValues(typeof(CustomerSealType)))
                    {
                        SealMappingConfigViewModel sealMappingConfigViewModel = new()
                        {
                            Id = (int)customerSealType,
                            Name = Enum.GetName(customerSealType)
                        };
                        sealMappingConfigResponseList.SealMappingConfigViewModels.Add(sealMappingConfigViewModel);
                    }
                    break;
                case SealType.Accountant:
                    foreach (AccountantSignType accountantSignType in (AccountantSignType[])Enum.GetValues(typeof(AccountantSignType)))
                    {
                        SealMappingConfigViewModel sealMappingConfigViewModel = new()
                        {
                            Id = (int)accountantSignType,
                            Name = Enum.GetName(accountantSignType)
                        };
                        sealMappingConfigResponseList.SealMappingConfigViewModels.Add(sealMappingConfigViewModel);
                    }
                    break;
            }

            sealMappingConfigResponseList.SealType = Enum.GetName(sealType);

            return sealMappingConfigResponseList;
        }        
    }
}
