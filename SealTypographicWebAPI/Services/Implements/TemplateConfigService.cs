using Microsoft.Extensions.Localization;
using SealTypographicWebAPI.Config;
using DBEntities.Consts;
using SealTypographicWebAPI.Models.SealMappingConfig;
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

        /// <summary>
        /// IStringLocalizer
        /// </summary>
        /// <param name="localizer"></param>      
        public TemplateConfigService(IStringLocalizer<TemplateConfigService> localizer)
        {
            this.localizer = localizer;
        }

        /// <summary>
        /// 取得樣板使用的Enum參數
        /// </summary>
        /// <param name="enumType"></param>
        /// <returns></returns>
        public TemplateConfigResponseList GetEnumData(Type enumType)
        {
            TemplateConfigResponseList templateConfigResponseList = new();

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

            return templateConfigResponseList;
        }

    }
}
