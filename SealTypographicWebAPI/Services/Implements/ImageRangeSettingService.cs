using AutoMapper;
using AutoMapper.QueryableExtensions;
using DBEntities;
using DBEntities.Consts;
using DBEntities.Utils;
using Microsoft.EntityFrameworkCore;
using SealTypographicWebAPI.Models;
using SealTypographicWebAPI.Models.BaseModels;
using SealTypographicWebAPI.Models.ImageRangeSetting;

namespace SealTypographicWebAPI.Services.Implements
{
    /// <summary>
    /// 印鑑截取範圍設定
    /// </summary>
    public class ImageRangeSettingService : IImageRangeSettingService
    {
        private readonly SealTypographicDbContext dbContext;
        private readonly IMapper mapper;
        private readonly AutoMapper.IConfigurationProvider configurationProvider;
        private readonly ILogger<ImageRangeSettingService> logger;

        /// <summary>
        /// 建置
        /// </summary>
        public ImageRangeSettingService(SealTypographicDbContext dbContext, IMapper mapper, ILogger<ImageRangeSettingService> logger)
        {
            this.dbContext = dbContext;
            this.mapper = mapper;
            configurationProvider = mapper.ConfigurationProvider;
            this.logger = logger;
        }

        ///<inheritdoc />
        public CustomerSealRangeSettingResponse GetCustomerSealRangeSetting(int companyId = 1)
        {
            logger.LogInformation("GetCustomerSealCapture input userId: {@userId}", companyId);

            CustomerSealRangeSettingResponse result = new ();

            try
            {
                CustomerSealRangeSetting? ImageCaptureSetting = dbContext.ImageRangeSettings
                                                                .Include(x => x.ImageRangeLocations)
                                                                .Where(x => x.Company!.Id == companyId && x.ImageRangeLocations.Any(location => location.SealType == SealType.Customer))
                                                                .ProjectTo<CustomerSealRangeSetting>(configurationProvider)
                                                                .FirstOrDefault();

                if (ImageCaptureSetting != null) 
                {
                    result.CustomerSealRangeSetting = ImageCaptureSetting;
                    result.Success();
                }

                logger.LogInformation("GetCustomerSealCapture output {@output}", result);
            }
            catch (Exception ex) 
            {
                result.Error();
                logger.LogError("GetCustomerSealCapture error {@error}", ex.Message);
            }
            return result;
        }

        ///<inheritdoc />
        public AccountantSignRangeSettingResponse GetAccountantSignRangeSetting(int companyId = 1)
        {
            logger.LogInformation("GetAccountantSignCapturee input companyId: {@companyId}", companyId);

            AccountantSignRangeSettingResponse result = new();

            try
            {
                AccountantSignRangeSetting? ImageCaptureSetting = dbContext.ImageRangeSettings
                                                                    .Include(x => x.ImageRangeLocations)
                                                                    .Where(x => x.Company!.Id == companyId && x.ImageRangeLocations.Any(location => location.SealType == SealType.Accountant))
                                                                    .ProjectTo<AccountantSignRangeSetting>(configurationProvider)
                                                                    .FirstOrDefault();

                if (ImageCaptureSetting != null)
                {
                    result.AccountantSignRangeSetting = ImageCaptureSetting;
                    result.Success();
                }

                logger.LogInformation("GetAccountantSignCapturee output {@output}", result);
            }
            catch (Exception ex)
            {
                result.Error();
                logger.LogError("GetAccountantSignCapturee error {@error}", ex.Message);
            }
            return result;
        }

        ///<inheritdoc />
        public ResponseViewModel New<T>(T captureSetting, int companyId = 1)
        {
            logger.LogInformation("New input {@captureSetting} companyId: {@companyId}", captureSetting, companyId);

            ResponseViewModel response = new ();

            try
            {                
                ImageRangeSetting imageCaptureSetting = mapper.Map<ImageRangeSetting>(captureSetting);
                imageCaptureSetting.Company = dbContext.Companys.Single(x => x.Id == companyId);
                InputUtil.Base(imageCaptureSetting, true, companyId);
                dbContext.ImageRangeSettings.Add(imageCaptureSetting);
                dbContext.SaveChanges();
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

        ///<inheritdoc />
        public ResponseViewModel UpdateCustomerSealRangeSetting(CustomerSealRangeSetting customerSealRangeSetting, int userId = 1)
        {
            logger.LogInformation("UpdateCustomerSealRangeSetting input {@customerSealRangeSetting} userId: {@userId}", customerSealRangeSetting, userId);

            ResponseViewModel response = new();

            try
            {
                ImageRangeSetting? imageRangeSetting = dbContext.ImageRangeSettings
                                                                .Include(x => x.ImageRangeLocations)
                                                                .Where(x => x.Id == customerSealRangeSetting.Id)                                                                        
                                                                .FirstOrDefault();
                if (imageRangeSetting != null)
                {
                    imageRangeSetting.PageSize = customerSealRangeSetting.PageSize;
                    imageRangeSetting.PaperOrientation = customerSealRangeSetting.PaperOrientation;
                    foreach (CustomerSealRangeLocation customerSealRangeLocation in customerSealRangeSetting.CustomerSealRangeLocations)
                    {
                        ImageRangeLocation imageRangeLocation = imageRangeSetting.ImageRangeLocations.Single(x => x.Id == customerSealRangeLocation.Id);                       
                        mapper.Map(customerSealRangeLocation, imageRangeLocation);                        
                    }
                    InputUtil.Base(imageRangeSetting, false, userId);                    
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("UpdateCustomerSealRangeSetting output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("UpdateCustomerSealRangeSetting error {@error}", ex.Message);
            }
            return response;
        }

        ///<inheritdoc />
        public ResponseViewModel UpdateAccountantSignRangeSetting(AccountantSignRangeSetting accountantSignRangeSetting, int userId = 1)
        {
            logger.LogInformation("UpdateAccountantSignRangeSetting input {@accountantSignRangeSetting} userId: {@userId}", accountantSignRangeSetting, userId);

            ResponseViewModel response = new();

            try
            {
                ImageRangeSetting? imageRangeSetting = dbContext.ImageRangeSettings
                                                                .Include(x => x.ImageRangeLocations)
                                                                .Where(x => x.Id == accountantSignRangeSetting.Id)
                                                                .FirstOrDefault();
                if (imageRangeSetting != null)
                {
                    foreach (AccountantSignRangeLocation accountantSignRangeLocation in accountantSignRangeSetting.AccountantSignRangeLocations)
                    {
                        ImageRangeLocation imageRangeLocation = imageRangeSetting.ImageRangeLocations.Single(x => x.Id == accountantSignRangeLocation.Id);
                        mapper.Map(accountantSignRangeLocation, imageRangeLocation);                        
                    }                    
                    InputUtil.Base(imageRangeSetting, false, userId);
                    dbContext.SaveChanges();
                    response.Success();
                }
                else
                {
                    response.DbNoData();
                }
                logger.LogInformation("UpdateAccountantSignRangeSetting output {@output}", response);
            }
            catch (Exception ex)
            {
                response.Error();
                logger.LogError("UpdateAccountantSignRangeSetting error {@error}", ex.Message);
            }
            return response;
        }
    }
}
