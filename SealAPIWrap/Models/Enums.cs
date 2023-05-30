using System.ComponentModel;

namespace SealAPIWrap.Models
{
    /// <summary>
    /// 操作模式
    /// </summary>
    public enum OPMode
    {
        /// <summary>
        /// 建印
        /// </summary>
        SealBuild,
        /// <summary>
        /// 驗印
        /// </summary>
        SealIdentify,
        /// <summary>
        /// 圖像處理
        /// </summary>
        ImageProcess,
        /// <summary>
        /// 驗印結果輔助顯示
        /// </summary>
        SealShow
    }

    /// <summary>
    /// 顏色
    /// </summary>
    public enum Color
    {
        /// <summary>
        /// 紅色
        /// </summary>
        [Description("紅色")]
        Red,
        /// <summary>
        /// 藍色
        /// </summary>
        [Description("藍色")]
        Blue,
        /// <summary>
        /// 黑色
        /// </summary>
        [Description("黑色")]
        Black
    }

    /// <summary>
    /// 旋轉
    /// </summary>
    public enum Rotate
    {
        /// <summary>
        /// 不自動旋轉
        /// </summary>
        [Description("不自動旋轉")]
        No,
        /// <summary>
        /// 自動旋轉
        /// </summary>
        [Description("自動旋轉")]
        Yes
    }

    /// <summary>
    /// 檔案類型
    /// </summary>
    public enum FileType
    {
        /// <summary>
        /// bmp or jpg
        /// </summary>
        [Description("bmp or jpg")]
        BmpJpg,
        /// <summary>
        /// pdf
        /// </summary>
        [Description("pdf")]
        PDF
    }

    /// <summary>
    /// 驗印鬆緊度: 分為1-5，逐漸變緊，預設等級為3
    /// </summary>
    public enum TightLevel
    {
        /// <summary>
        /// 1: 最鬆
        /// </summary>
        [Description("1: 最鬆")]
        Level1 = 1,
        /// <summary>
        /// 2: 鬆
        /// </summary>
        [Description("2: 鬆")]
        Level2 = 2,
        /// <summary>
        /// 3: 正常(預設)
        /// </summary>
        [Description("3: 正常(預設)")]
        Level3 = 3,
        /// <summary>
        /// 4: 緊
        /// </summary>
        [Description("4: 緊")]
        Level4 = 4,
        /// <summary>
        /// 5: 最緊
        /// </summary>
        [Description("5: 最緊")]
        Level5 = 5
    }

    /// <summary>
    /// 驗印結果輔助查看操作類型
    /// </summary>
    public enum ShowType
    {
        /// <summary>
        /// 1: 輪廓圖查看
        /// </summary>
        [Description("1: 輪廓圖查看")]
        OutlineView = 1,
        /// <summary>
        /// 2: 折角圖查看
        /// </summary>
        [Description("2: 折角圖查看")]
        ChamferView = 2,
        /// <summary>
        /// 3: 殘像對比查看
        /// </summary>
        [Description("3: 殘像對比查看")]
        AfterimageView = 3
    }
}
