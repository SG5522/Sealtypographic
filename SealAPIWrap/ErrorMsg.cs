using System.ComponentModel;

namespace SealAPIWrap
{
    /// <summary>
    /// 錯誤訊息
    /// </summary>
    public enum ErrorMsg
    {
        [Description("創建文件失敗")]
        E1000 = 1000,
        [Description("打開文件失敗")]
        E1001 = 1001,
        [Description("通道圖轉換失敗")]
        E1002 = 1002,
        [Description("二值化失敗")]
        E1003 = 1003,
        [Description("圖像居中失敗")]
        E1004 = 1004,
        [Description("切組圖失敗")]
        E1005 = 1005,
        [Description("提取印鑑失敗")]
        E1006 = 1006,
        [Description("解析JSON失敗")]
        E1007 = 1007,
        [Description("印鑑識別失敗")]
        E1008 = 1008,
        [Description("創建輪廓圖失敗")]
        E1009 = 1009,
        [Description("折角邊煥失敗")]
        E1010 = 1010,
        [Description("打開註冊表失敗")]
        E1011 = 1011,
        [Description("寫註冊表失敗")]
        E1012 = 1012,
        [Description("KEY檢查失敗")]
        E1014 = 1014,
        [Description("配置文件讀取錯誤")]
        E1015 = 1015,
        [Description("圖像縮放失敗")]
        E1019 = 1019,
        [Description("圖像旋轉失敗")]
        E1020 = 1020,
        [Description("圖像裁剪失敗")]
        E1021 = 1021,
        [Description("區域座標不合法")]
        E1022 = 1022,
        [Description("圖像導出失敗")]
        E1023 = 1023,
        [Description("config資料夾不存在")]
        E1024 = 1024,
        [Description("輸入操作類型/參數錯誤")]
        E1025 = 1025
    }
}
