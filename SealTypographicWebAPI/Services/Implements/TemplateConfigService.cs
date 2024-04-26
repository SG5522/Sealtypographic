using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Models.TemplateConfig;
using Microsoft.OpenApi.Extensions;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 取得印鑑類型列表 (客戶印鑑、會計師簽印)
    /// </summary>
    public class TemplateConfigService
    {
        private readonly IStringLocalizer<TemplateConfigService> localizer;
        private readonly ILogger<TemplateConfigService> logger;

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>
        /// <param name="logger"></param>      
        public TemplateConfigService(IStringLocalizer<TemplateConfigService> localizer, ILogger<TemplateConfigService> logger)
        {
            this.localizer = localizer;
            this.logger = logger;
        }

        /// <summary>
        /// 取得樣板使用的Enum參數
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public TemplateConfigResponseList GetEnumData(Type enumType)
        {
            TemplateConfigResponseList templateConfigResponseList = new();

            try
            {
                Array enumValues = Enum.GetValues(enumType);

                foreach (Enum enumValue in enumValues)
                {
                    TemplateConfigViewModel viewModel = new()
                    {
                        Id = Convert.ToInt32(enumValue),
                        Name = enumValue.GetDisplayName(),
                        Localizer = localizer[enumValue.GetDisplayName()]
                    };
                    templateConfigResponseList.ViewModels.Add(viewModel);
                }

                templateConfigResponseList.Success();
            }
            catch (Exception ex) 
            {
                logger.LogError("GetEnumData error {@Error}", ex.Message);
                templateConfigResponseList.Error();
            }
            return templateConfigResponseList;
        }

    }
}
