using System.ComponentModel;

namespace SealAPIWrap
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public enum ErrorMsg
    {
        /// <summary>
        /// 1000 創建文件失敗
        /// </summary>
        [Description("創建文件失敗")]
        E1000 = 1000,
        /// <summary>
        /// 1001 打開文件失敗
        /// </summary>
        [Description("打開文件失敗")]
        E1001 = 1001,
        /// <summary>
        /// 1002 通道圖轉換失敗
        /// </summary>
        [Description("通道圖轉換失敗")]
        E1002 = 1002,
        /// <summary>
        /// 1003 二值化失敗
        /// </summary>
        [Description("二值化失敗")]
        E1003 = 1003,
        /// <summary>
        /// 1004 圖像居中失敗
        /// </summary>
        [Description("圖像居中失敗")]
        E1004 = 1004,
        /// <summary>
        /// 1005 切組圖失敗
        /// </summary>
        [Description("切組圖失敗")]
        E1005 = 1005,
        /// <summary>
        /// 1006 提取印鑑失敗
        /// </summary>
        [Description("提取印鑑失敗")]
        E1006 = 1006,
        /// <summary>
        /// 1007 解析JSON失敗
        /// </summary>
        [Description("解析JSON失敗")]
        E1007 = 1007,
        /// <summary>
        /// 1008 印鑑識別失敗
        /// </summary>
        [Description("印鑑識別失敗")]
        E1008 = 1008,
        /// <summary>
        /// 1009 創建輪廓圖失敗
        /// </summary>
        [Description("創建輪廓圖失敗")]
        E1009 = 1009,
        /// <summary>
        /// 1010 折角邊煥失敗
        /// </summary>
        [Description("折角邊煥失敗")]
        E1010 = 1010,
        /// <summary>
        /// 1011 打開註冊表失敗
        /// </summary>
        [Description("打開註冊表失敗")]
        E1011 = 1011,
        /// <summary>
        /// 1012 寫註冊表失敗
        /// </summary>
        [Description("寫註冊表失敗")]
        E1012 = 1012,
        /// <summary>
        /// 1014 KEY檢查失敗
        /// </summary>
        [Description("KEY檢查失敗")]
        E1014 = 1014,
        /// <summary>
        /// 1015 配置文件讀取錯誤
        /// </summary>
        [Description("配置文件讀取錯誤")]
        E1015 = 1015,
        /// <summary>
        /// 1019 圖像縮放失敗
        /// </summary>
        [Description("圖像縮放失敗")]
        E1019 = 1019,
        /// <summary>
        /// 1020 圖像旋轉失敗
        /// </summary>
        [Description("圖像旋轉失敗")]
        E1020 = 1020,
        /// <summary>
        /// 1021 圖像裁剪失敗
        /// </summary>
        [Description("圖像裁剪失敗")]
        E1021 = 1021,
        /// <summary>
        /// 1022 區域座標不合法
        /// </summary>
        [Description("區域座標不合法")]
        E1022 = 1022,
        /// <summary>
        /// 1023 圖像導出失敗
        /// </summary>
        [Description("圖像導出失敗")]
        E1023 = 1023,
        /// <summary>
        /// 1024 config資料夾不存在
        /// </summary>
        [Description("config資料夾不存在")]
        E1024 = 1024,
        /// <summary>
        /// 1025 輸入操作類型/參數錯誤
        /// </summary>
        [Description("輸入操作類型/參數錯誤")]
        E1025 = 1025
    }
}
