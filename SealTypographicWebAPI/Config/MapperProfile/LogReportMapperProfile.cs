using AutoMapper;
using DBEntities.Consts;
using DBEntities.Entities.CustomerModels;
using DBEntities.Entities.TypographicModels;
using SealTypographicWebAPI.Consts;
using SealTypographicWebAPI.Models.Accountant;
using SealTypographicWebAPI.Models.Customer;
using SealTypographicWebAPI.Models.CustomerSeal;
using SealTypographicWebAPI.Models.CustomerSealReview;
using SealTypographicWebAPI.Models.LogReport.CustomerSealEventLog;
using SealTypographicWebAPI.Models.LogReport.OperationLog;
using SealTypographicWebAPI.Models.LogReport.TypographicReport;
using SealTypographicWebAPI.Models.MongoDBModel;
using SealTypographicWebAPI.Utils;

namespace SealTypographicWebAPI.Config.MapperProfile
{
    /// <summary>
    /// AutoMapper用的LIST
    /// </summary>
    public class LogReport : Profile
    {
        /// <summary>
        /// 建置
        /// </summary>
        public LogReport()
        {
            //會計師分頁顯示Map
            CreateMap<TypographicPDF, TypographicReportViewModel>()
                    ////TODO:透過keycloak取得使用者Id
                    //.ForMember(dst => dst.CreateUserId, opt => opt.MapFrom(src => src.CreateUserId))
                    ////TODO:透過keycloak取得使用者名稱
                    //.ForMember(dst => dst.CreateUserName, opt => opt.MapFrom(src => src.CreateUserId.ToString()))
                    .ForMember(dst => dst.UserName, opt => opt.MapFrom(src => src.UpdateUser!.UserName))
                    .ForMember(dst => dst.UserNickName, opt => opt.MapFrom(src => src.UpdateUser!.LastName))
                    .ForMember(dst => dst.CustomerCode, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))
                    .ForMember(dst => dst.RecordDate, opt => opt.MapFrom(src => src.UpdateDate))
                    .ForMember(dst => dst.EditFileName, opt => opt.MapFrom(src => src.OriginFileName))
                    .ForMember(dst => dst.EditPageCount, opt => opt.MapFrom(src => src.TypographicPages.Where(x => x.BlankCheck == false).Count()))
                    .ForMember(dst => dst.BlankPageCount, opt => opt.MapFrom(src => src.TypographicPages.Where(x => x.BlankCheck == true).Count()));

            //操作紀錄Map
            CreateMap<OperationLog, OperationLogViewModel>()                    
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => src.Data!.ActionType))
                    .ForMember(dst => dst.TargetName, opt => opt.MapFrom(src => 
                        !string.IsNullOrWhiteSpace(src.Data!.AccountantName) ? src.Data.AccountantName : src.Data.CustomerName
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                        !string.IsNullOrWhiteSpace(src.Data!.DisplayQuarterYear) ? src.Data.DisplayQuarterYear : null
                    ));

            //操作紀錄Map客戶資料
            CreateMap<CustomerDetail, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.CustomerQuery))
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Name));                    

            //操作紀錄Map客戶印鑑資料
            CreateMap<CustomerSealViewModels, OperationLogSave>()
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.CustomerSealQuarterId))
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => 
                        src.TypographyType == TypographyType.FinancialReport ? 
                        ActionType.FinancialReportSealQuery : ActionType.TaxReportSealQuery
                    ));

            //操作紀錄Map會計簽印資料
            CreateMap<AccountantViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantQuery))
                    .ForMember(dst => dst.AccountantId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.AccountantName, opt => opt.MapFrom(src => src.Name));

            //客戶印鑑審核查詢
            CreateMap<CustomerSealGroupDetailReviewViewModel, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src =>
                        src.TypographyType == TypographyType.FinancialReport ?
                        ActionType.FinancialReportSealQuery : ActionType.TaxReportSealQuery
                    ))
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.CustomerId))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Name))
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id));


            //操作紀錄Map會計簽印資料
            CreateMap<AccountantSignViewModels, OperationLogSave>()
                    .ForMember(dst => dst.ActionType, opt => opt.MapFrom(src => ActionType.AccountantSignQuery))
                    .ForMember(dst => dst.AccountantSignGroupCreateDate, opt => opt.MapFrom(src => src.GroupCreateDate));

            //印鑑異動建檔紀錄
            CreateMap<CustomerSealGroup, CustomerSealEventLogSave>()                    
                    .ForMember(dst => dst.CustomerId, opt => opt.MapFrom(src => src.Customer.Id))
                    .ForMember(dst => dst.CustomerCode, opt => opt.MapFrom(src => src.Customer.Code))
                    .ForMember(dst => dst.CustomerName, opt => opt.MapFrom(src => src.Customer.Name))                    
                    .ForMember(dst => dst.CustomerSealGroupId, opt => opt.MapFrom(src => src.Id))
                    .ForMember(dst => dst.QuarterYearId, opt => opt.MapFrom(src => src.QuarterYear.Id))
                    .ForMember(dst => dst.GregorainQuarterYear, opt => opt.MapFrom(src =>
                            src.TypographyType == TypographyType.FinancialReport ?
                            QuarterUtil.GetGregorainQuarter(src.QuarterYear) : QuarterUtil.GetGregorainYear(src.QuarterYear)
                    ))
                    .ForMember(dst => dst.DisplayQuarterYear, opt => opt.MapFrom(src =>
                            src.TypographyType == TypographyType.FinancialReport ?
                            QuarterUtil.GetTaiwanYearQuarter(src.QuarterYear) : QuarterUtil.GetTaiwanYear(src.QuarterYear)
                    ));



        }
    }
}
