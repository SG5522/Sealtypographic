using AutoMapper;
using DBEntities;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.SealCaptureRange;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public class ImageCaptureSettingService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<ImageCaptureSettingService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        public ImageCaptureSettingService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<ImageCaptureSettingService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }


        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="customerSealSetting"></param>
        /// <param name="userId"></param>
        /// <returns></returns>
        public ResponseViewModel New(CustomerSealSetting customerSealSetting, int userId = 0)
        {
            logger.LogInformation("New input {@customerSealSetting} userId: {@userId}", customerSealSetting, userId);

            ResponseViewModel response = new ();

            try
            {
                ImageCaptureSetting imageCaptureSetting = mapper.Map<ImageCaptureSetting>(customerSealSetting);                
                dbContext.ImageCaptureSettings.Add(imageCaptureSetting);
                logger.LogInformation("New output {@output}", response);
            }
            catch (DbUpdateException ex)
            {
                response.DbError();
                logger.LogError("New dberror {@dberror}", ex.Message);
            }
            catch (Exception ex) 
            {
                response.Error();
                logger.LogError("New error {@error}", ex.Message);
            }
            return response;
        }

    }
}
