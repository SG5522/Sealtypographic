namespace DJKeycloakLib.Model.BaseModel
{
    /// <summary>
    /// 錯誤訊息使用的Class
    /// </summary>
    public class ResponseBaseModel
    {

        /// <summary>
        /// 狀態號碼
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// 回傳訊息
        /// </summary>
        public string? Message { get; set; }

        /// <summary>
        /// 成功
        /// </summary>
        public void Success()
        {
            Code = 0;
            Message = "Success";
        }
        /// <summary>
        /// 回傳失敗
        /// </summary>
        public void NoData()
        {
            Code = 1;
            Message = "No Data";
        }

        /// <summary>
        /// 回傳錯誤
        /// </summary>
        public void KeycloakAPIError()
        {
            Code = 99;
        }

        /// <summary>
        /// 回傳錯誤
        /// </summary>
        public void SystemError()
        {
            Code = 999;
        }
    }
}
