namespace SealTypographicWebAPI.Consts
{
    /// <summary>
    /// keycloak角色常數
    /// </summary>
    public static class KeycloakRoleConsts
    {
        /// <summary>
        /// 資源存取宣稱類型
        /// </summary>
        public const string CLAIM_TYPE = "resource_access";

        /// <summary>
        /// 客戶端名稱
        /// </summary>
        public const string RESOURCE_NAME = "sealTypographic";

        /// <summary>
        /// 資料管理-會計師資料
        /// </summary>
        public const string DATAMANGE_ACCOUNTANT = "dataManage-accountant";

        /// <summary>
        /// 資料管理-會計師群組管理
        /// </summary>
        public const string DATAMANAGE_ACCOUNTANTGROUP = "dataManage-accountantGroup";

        /// <summary>
        /// 資料管理-客戶資料
        /// </summary>
        public const string DATAMANAGE_CUSTOMER = "dataManage-customer";

        /// <summary>
        /// 資料管理-信頭
        /// </summary>
        public const string DATAMANAGE_LETTERHEAD = "dataManage-letterhead";

        /// <summary>
        /// 資料管理-臨時章資料
        /// </summary>
        public const string DATAMANAGE_TEMPORARYSEAL = "dataManage-temporarySeal";

        /// <summary>
        /// 財報作業-印鑑資料
        /// </summary>
        public const string FINANCIALREPORT_SEALDATA = "financialReport-sealdata";

        /// <summary>
        /// 財報作業-排版
        /// </summary>
        public const string FINANCIALREPORT_TYPOGRAPHIC = "financialReport-typographic";

        /// <summary>
        /// 稅報作業-印鑑資料
        /// </summary>
        public const string TAXREPORT_SEALDATA = "taxReport-sealdata";

        /// <summary>
        /// 稅報作業-排版
        /// </summary>
        public const string TAXREPORT_TYPOGRAPHIC = "taxReport-typographic";

        /// <summary>
        /// 報表作業-會計師清單
        /// </summary>
        public const string REPORT_ACCOUNTANTMEMBER = "report-accountantMember";

        /// <summary>
        /// 報表作業-會計師簽印異動紀錄
        /// </summary>
        public const string REPORT_ACCOUNTANTSIGNEVENTLOG = "report-accountantSignEventLog";

        /// <summary>
        /// 報表作業-財報排版紀錄
        /// </summary>
        public const string REPORT_FINANCIALREPORT = "report-financialReport";

        /// <summary>
        /// 報表作業-財報印鑑異動紀錄
        /// </summary>
        public const string REPORT_FINANCIALREPORTSEALEVENTLOG = "report-financialReportSealEventLog";

        /// <summary>
        /// 報表作業-操作紀錄
        /// </summary>
        public const string REPORT_OPERATIONLOG = "report-operationLog";

        /// <summary>
        /// 報表作業-稅報排版紀錄
        /// </summary>
        public const string REPORT_TAXREPORT = "report-taxReport";

        /// <summary>
        /// 報表作業-稅報印鑑異動紀錄
        /// </summary>
        public const string REPORT_TAXREPORTSEALEVENTLOG = "report-taxReportSealEventLog";

        /// <summary>
        /// 報表作業-系統使用者清單
        /// </summary>
        public const string REPORT_USERMEMBER = "report-userMember";

        /// <summary>
        /// 審核作業-會計師資料審核
        /// </summary>
        public const string REVIEW_ACCOUNTANTSIGNREVIEW = "review-accountantSignReview";

        /// <summary>
        /// 審核作業-客戶財報印鑑審核
        /// </summary>
        public const string REVIEW_CUSTOMERSEALREVIEW = "review-CustomerSealReview";

        /// <summary>
        /// 審核作業-客戶財報印鑑審核
        /// </summary>
        public const string REVIEW_CUSTOMERSEALTAXREVIEW = "review-customerSealTaxReview";

        /// <summary>
        /// 掃描及上傳
        /// </summary>
        public const string SCANUPLOAD = "scanUpload";

        /// <summary>
        /// 系統管理-帳號群組管理
        /// </summary>
        public const string SYSTEM_ACCOUNTGROUPMANAGE = "system-accountGroupManage";

        /// <summary>
        /// 系統管理-帳號管理
        /// </summary>
        public const string SYSTEM_ACCOUNTMANAGE = "system-accountManage";

        /// <summary>
        /// 系統管理-自動取章範圍設定-會計師
        /// </summary>
        public const string SYSTEM_AUTOSEALIMAGERANGESETTING_ACCOUNTANT = "system-autoSealImageRangeSetting-accountant";

        /// <summary>
        /// 系統管理-自動取章範圍設定-客戶
        /// </summary>
        public const string SYSTEM_AUTOSEALIMAGERANGESETTING_CUSTOMER = "system-autoSealImageRangeSetting-customer";

        /// <summary>
        /// 系統管理-個人帳號管理
        /// </summary>
        public const string SYSTEM_PERSONALACCOUNTMANAGE = "system-personalAccountManage";

        /// <summary>
        /// 系統管理-上傳檔案管理
        /// </summary>
        public const string SYSTEM_UPLOADMANAGE = "system-uploadManage";


        /// <summary>
        /// 樣版作業-會計師簽印樣版設計
        /// </summary>
        public const string TEMPLATE_ACCOUNTANTSIGNTEMPLATE = "template-accountantSignTemplate";

        /// <summary>
        /// 樣版作業-客戶印鑑樣版設計
        /// </summary>
        public const string TEMPLATE_CUSTOMERSEALTEMPLATE = "template-customerSealTemplate";

        /// <summary>
        /// 樣版作業-事務所信頭樣版設計
        /// </summary>
        public const string TEMPLATE_LETTERHEADIMAGETEMPLATE = "template-letterheadImageTemplate";

    }
}
