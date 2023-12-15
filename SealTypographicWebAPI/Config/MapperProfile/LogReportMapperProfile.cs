using AutoMapper;
using DBEntities.Entities.TypographicModels;
using SealTypographicWebAPI.Models.LogReport;
using System.Linq;

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
                    .ForMember(dst => dst.BlankPageCount, opt => opt.MapFrom(src => src.TypographicPages.Where(x => x.BlankCheck == true).Count()))
                    ;
        }
    }
}
