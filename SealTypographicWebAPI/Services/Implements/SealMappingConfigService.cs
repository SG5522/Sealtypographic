using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Models.SealMappingConfig;
using DBEntities.Consts;
using CommonLib.Extensions;
using Serilog;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得印鑑類型列表 (客戶印鑑、會計師簽印)
    /// </summary>
    public class SealMappingConfigService
    {
        private readonly IStringLocalizer<SealMappingConfigService> localizer;
        private readonly ILogger<SealMappingConfigService> logger;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>      
        public SealMappingConfigService(IStringLocalizer<SealMappingConfigService> localizer, ILogger<SealMappingConfigService> logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }

        /// <summary>
        /// 取得印鑑類型列表
        /// </summary>
        /// <param name="sealType">印鑑類型 1.客戶 2.會計師</param>
        /// <returns></returns>
        public SealMappingConfigResponseList Get(SealType sealType)
        {
            logger.LogInformation("Get input {@Input}", sealType);

            SealMappingConfigResponseList sealMappingConfigResponseList = new();

            try
            {
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
                sealMappingConfigResponseList.Success();
                logger.LogInformation("Get output {@Output}", sealMappingConfigResponseList);
            }
            catch (Exception ex)
            {
                logger.LogError("Get error {@Error}", ex.Message);
                sealMappingConfigResponseList.DbError();
            }            
            return sealMappingConfigResponseList;
        }        
    }
}
