using DBEntitiesExtension.Consts;

namespace SealTypographicWebAPI.Utils
{
    /// <summary>
    /// 印鑑類別轉換
    /// </summary>
    public class SealMappingConfigUtil
    {
        /// <summary>
        /// SubSealType轉為CustomerSealType
        /// </summary>
        /// <param name="subSealType"></param>
        /// <returns></returns>
        public static CustomerSealType GetCustomerSealType (SubSealType subSealType)
        {            
            switch (subSealType)
            {
                case SubSealType.Company:
                    return CustomerSealType.Company;                    
                case SubSealType.President:
                    return CustomerSealType.President;                    
                case SubSealType.Manager:
                    return CustomerSealType.Manager;
                case SubSealType.AccountingDirector:
                    return CustomerSealType.AccountingDirector;
                case SubSealType.Other:
                    return CustomerSealType.Other;
                default:
                    throw new ArgumentException("Unhandled subSealType : " + subSealType.ToString());
            }            
        }

        /// <summary>
        /// CustomerSealType轉回SubSealType
        /// </summary>
        /// <param name="customerSealType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SubSealType GetSubSealType(CustomerSealType customerSealType)
        {
            switch (customerSealType)
            {
                case CustomerSealType.Company:
                    return SubSealType.Company;
                case CustomerSealType.President:
                    return SubSealType.President;
                case CustomerSealType.Manager:
                    return SubSealType.Manager;
                case CustomerSealType.AccountingDirector:
                    return SubSealType.AccountingDirector;
                case CustomerSealType.Other:
                    return SubSealType.Other;
                default:
                    throw new ArgumentException("Unhandled customerSealType : " + customerSealType.ToString());
            }
        }
    }
}
