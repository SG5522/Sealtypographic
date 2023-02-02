using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Options;
using SealTypographicWebAPI.Config;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Entities;
using SealTypographicWebAPI.Models.SealMappingConfig;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得印鑑類型列表 (客戶印鑑、會計師簽印)
    /// </summary>
    public class SealMappingConfigService
    {
        private readonly IStringLocalizer<SealMappingConfigService> localizer;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>      
        public SealMappingConfigService(IStringLocalizer<SealMappingConfigService> localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// 取得印鑑類型列表
        /// </summary>
        /// <param name="sealType">印鑑類型 1.客戶 2.會計師</param>
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
                            Name = localizer[customerSealType.GetDescription()]
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
                            Name = localizer[accountantSignType.GetDescription()]
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
