using System;
using System.Runtime.InteropServices;

namespace SealAPIWrap
{
    /// <summary>
    /// 呼叫天創C++ Dll元件
    /// </summary>
    public class ApiWrap
    {
        /// <summary>
        /// 模型ID 預設0
        /// </summary>
        public static long DEFAULT_MODID = 0;

        /// <summary>
        /// 執行結果 0: 成功
        /// </summary>
        public static int RETURN_SUCCESS = 0;

        /// <summary>
        /// 執行結果 1: 失敗，其他詳見ErrorMsg內容
        /// </summary>
        public static int RETURN_FIALURE = 0;

        /// <summary>
        /// 建印函數  擷取印鑑
        /// 
        /// 1. 自動建印： 根據傳入的顏色(限紅色、藍色，黑色無法自動建印)
        /// 2. 指定區域建印： 根據傳入的區域座標，擷取該區域的印章，預設此區域內只有一顆印章。
        /// </summary>
        /// <param name="modID">模型 ID【預留參數，先預設傳入 0】</param>
        /// <param name="input">輸入JSON</param>
        /// <returns></returns>
        [DllImport(@"Resources\lib\libsealinterface.dll", EntryPoint = "seal_build", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SealBuild(long modID, string input);

        /// <summary>
        /// 驗印函數
        /// 
        /// 1. 自動驗印： 根據傳入的顏色，自動比對輸入的原始印章和圖像，比對後進行判別。
        /// 2. 指定區域驗印： 根據傳入的區域座標，擷取該區域的印章，比對輸入的原始印章，比對後進行判別。
        /// </summary>
        /// <param name="modID">模型 ID【預留參數，先預設傳入 0】</param>
        /// <param name="input">輸入JSON</param>
        /// <returns></returns>
        [DllImport(@"Resources\lib\libsealinterface.dll", EntryPoint = "seal_identify", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SealIdentify(long modID, string input);

        /// <summary>
        /// 圖像處理函數
        /// 
        /// 有三個功能：
        /// 1. 圖像裁剪(需傳入裁剪區域資料)
        /// 2. 圖像DPI轉換(需傳入圖像原始DPI和圖像目標DPI)
        /// 3. 圖像旋轉(需傳入旋轉角度，順時針0 - 360)
        /// 若相對應參數都有傳入，執行順序為：圖像裁剪>圖像DPI轉換>圖像旋轉
        /// </summary>
        /// <param name="input">輸入JSON</param>
        /// <returns></returns>
        [DllImport(@"Resources\lib\libsealinterface.dll", EntryPoint = "image_process", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr ImageProcess(string input);

        /// <summary>
        /// 驗印結果輔助顯示函數
        /// 
        /// 有三個功能：
        /// 1. 輪廓圖查看
        /// 2. 折角圖查看
        /// 3. 殘像對比查看
        /// </summary>
        /// <param name="input">輸入JSON</param>
        /// <returns></returns>
        [DllImport(@"Resources\lib\libsealinterface.dll", EntryPoint = "seal_show", CallingConvention = CallingConvention.StdCall)]
        public static extern IntPtr SealShow(string input);

        /// <summary>
        /// 釋放資料函數
        /// </summary>
        /// <param name="output"></param>
        [DllImport(@"Resources\lib\libsealinterface.dll", EntryPoint = "release_seal_data", CallingConvention = CallingConvention.StdCall)]
        public static extern void ReleaseSealData(out IntPtr output);
    }
}
