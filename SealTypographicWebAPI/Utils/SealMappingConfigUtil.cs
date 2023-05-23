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
        /// SubSealType轉為AccountantSignType
        /// </summary>
        /// <param name="subSealType"></param>
        /// <returns></returns>
        public static AccountantSignType GetAccountantSignType(SubSealType subSealType)
        {
            switch (subSealType)
            {
                case SubSealType.Seal:
                    return AccountantSignType.Seal;
                case SubSealType.CHSign:
                    return AccountantSignType.CHSign;
                case SubSealType.ENSign:
                    return AccountantSignType.ENSign;
                case SubSealType.OldSign:
                    return AccountantSignType.OldSign;
                case SubSealType.Other:
                    return AccountantSignType.Other;
                default:
                    throw new ArgumentException("Unhandled subSealType : " + subSealType.ToString());
            }
        }

        /// <summary>
        /// AccountantSignType轉換SubSealType
        /// </summary>
        /// <param name="customerSealType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SubSealType GetSubSealTypeWithCustomer(CustomerSealType customerSealType)
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
                    throw new ArgumentException("Unhandled accountantSignType : " + customerSealType.ToString());
            }            
        }

        /// <summary>
        /// AccountantSignType轉換SubSealType
        /// </summary>
        /// <param name="accountantSignType"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public static SubSealType GetSubSealTypeWithAccountant(AccountantSignType accountantSignType)
        {
            switch (accountantSignType)
            {
                case AccountantSignType.Seal:
                    return SubSealType.Seal;
                case AccountantSignType.CHSign:
                    return SubSealType.CHSign;
                case AccountantSignType.ENSign:
                    return SubSealType.ENSign;
                case AccountantSignType.OldSign:
                    return SubSealType.OldSign;
                case AccountantSignType.Other:
                    return SubSealType.Other;
                default:
                    throw new ArgumentException("Unhandled accountantSignType : " + accountantSignType.ToString());
            }
        }
    }
}
