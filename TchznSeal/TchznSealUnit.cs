using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace TchznSeal
{
    public class TchznSealUnit : IDisposable
    {
        /// <summary>
        /// 印鑑元件錯誤訊息 Dictionary
        /// </summary>
        private Dictionary<int, ResultSealStatus> m_Result_NumMsg_Dict = new Dictionary<int, ResultSealStatus>();

        /// <summary>
        /// 印章顏色
        /// </summary>
        public enum SealColor
        {
            Red, Blue
        }

        /// <summary>
        /// 天創驗印狀態
        /// </summary>
        public enum ResultSealStatus
        {
            成功 = 0, 創建文件失敗 = -1000, 打開文件失敗 = -1001, 通道圖轉換失敗 = -1002, 二值化失敗 = -1003,
            圖像居中失敗 = -1004, 切組圖失敗 = -1005, 提取印鑑失敗 = -1006, 解析XML失敗 = -1007,
            印鑑識別失敗 = -1008, 創建輪廓圖失敗 = -1009, 折角變換失敗 = -1010, 打開註冊表失敗 = -1011,
            寫註冊表失敗 = -1012, Key生成失敗 = -1013, Key檢查失敗 = -1014, 讀取配置文件失敗 = -1015,
            連接失敗 = -1016, 發送失敗 = -1017, 接收失敗 = -1018, 圖像縮放失敗 = -1019, 圖像旋轉失敗 = -1020,
            沒有產生驗印結果 = -1021, 印鑑分離紅藍章失敗 = -1022, DJSeal程式不存在 = -1023, 檢查ZTSN是否開啟 = -1024,
            檢查主機IP是否和ZTSN_cfg相同SERVER_IP = -1025, 呼叫元件失敗 = -1026, 印鑑擷取紅藍章失敗 = -1027, ZTSN_CFG不存在 = -1028
        }

        //private const string SealDLL = @".\libTC_sealinterface.dll";
        private const string SealDLL = @"libTC_sealinterface.dll";
        //P/invone
        #region DLLImport
        /// <summary>
        /// 印鑒提取建庫函數
        /// </summary>
        /// <param name="milSys"></param>
        /// <param name="color"></param>
        /// <param name="fileinput">對應輸入的 xml檔案名( 含絕對路徑 )</param>
        /// <param name="xmloutput">對應輸出的 xml檔案名( 含絕對路徑 )</param>
        /// <param name="dpi">圖像精度</param>
        /// <param name="libprefix">組成印鑒庫檔案名</param>
        /// <param name="libindex"></param>
        /// <param name="left">圖像左上角座標</param>
        /// <param name="top">圖像左上角座標</param>
        /// <param name="width">圖像寬度</param>
        /// <param name="height">圖像高度</param>
        /// <param name="binarize">是否進行二值化           0- 不二值化 1-二值化</param>
        /// <param name="rotate">是否對分離印鑒進行旋轉   0-不轉正   1- 轉正</param>
        /// <param name="path">路徑</param>
        /// <param name="calx"></param>
        /// <param name="caly"></param>
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int seal_build
            (out long milSys, int color, string fileinput, string xmloutput,
            int dpi, string libprefix, int libindex, int left, int top, int width, int height, int binarize,
            int rotate, string path, double calx = 1.0, double caly = 1.0);

        /// <summary>
        /// 自動比對函數
        /// </summary>
        /// <param name="milSys"></param>
        /// <param name="tightLevel">驗印鬆緊度 (分為1-5 個等級，從 1到5 逐漸變緊，預設等級為 3)</param>
        /// <param name="input">輸入xml檔案名 (含絕對路徑)</param>
        /// <param name="output">輸出xml檔案名 (含絕對路徑)</param>
        /// <param name="dpi">圖像精度</param>
        /// <param name="left">圖像左上角座標</param>
        /// <param name="top">圖像左上角座標</param>
        /// <param name="width">圖像寬度</param>
        /// <param name="height">圖像高度 </param>
        /// <param name="binarize"></param>
        /// <param name="path">路徑</param>
        /// <param name="calx">X 方向校準值</param>
        /// <param name="caly">Y 方向校準值</param>
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int seal_identify
            (out long milSys, int tightLevel, string input, string output, int dpi, int left, int top,
            int width, int height, int binarize, string path, double calx = 1.0, double caly = 1.0);

        /// <summary>
        /// 輪廓圖查看函數
        /// </summary>
        /// <param name="milSys"></param>
        /// <param name="filein">輸入的影像檔名</param>
        /// <param name="fileout">輸出的影像檔名</param>        
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int seal_shapeimg(out long milSys, string filein, string fileout);

        /// <summary>
        /// 折角圖查看函數
        /// </summary>
        /// <param name="filein">輸入的影像檔名</param>
        /// <param name="angle">輸入旋轉角度</param>
        /// <param name="fileout"></param>
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int seal_angleimg(string filein, int angle, string fileout);

        ///////////// <summary>
        ///////////// 掃描圖查看函數
        ///////////// </summary>
        ///////////// <param name="filein"></param>
        ///////////// <param name="mode">掃描的方向 ,mode 0 縱向,1 橫向</param>
        ///////////// <param name="location">掃描的初始位置</param>
        ///////////// <returns></returns>
        //////////[DllImport(SealDLL)]
        //////////static extern int seal_scanimg(string filein, int mode, int location);

        /// <summary>
        /// 紅藍章分離 一張影像分開兩張   (res_xxxxxxxxxxx.bmp)
        /// </summary>
        /// <param name="szBmpFilePath">res_xxxxxxxxxxxxx.bmp Path</param>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern void dynamic_image(string szBmpFilePath);

        /// <summary>
        /// dpi轉換函數
        /// </summary>
        /// <param name="milSys"></param>
        /// <param name="input">200 DPI 圖像名 </param>
        /// <param name="output">300 DPI 圖像名</param>
        /// <param name="scale">轉換比例</param>
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int trans_image(out long milSys, string input, string output, double scale);

        /// <summary>
        /// 旋轉
        /// </summary>
        /// <param name="milSys"></param>
        /// <param name="input">原始 圖像名 </param>
        /// <param name="output">結果 圖像名</param>
        /// <param name="angle"></param>
        /// <returns></returns>
        [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
        public static extern int rotate_image(out long milSys, string input, string output, int angle);
        #endregion

        public TchznSealUnit()
        {
            m_Result_NumMsg_Dict.Add(0, ResultSealStatus.成功);
            m_Result_NumMsg_Dict.Add(-1000, ResultSealStatus.創建文件失敗);
            m_Result_NumMsg_Dict.Add(-1001, ResultSealStatus.打開文件失敗);
            m_Result_NumMsg_Dict.Add(-1002, ResultSealStatus.通道圖轉換失敗);
            m_Result_NumMsg_Dict.Add(-1003, ResultSealStatus.二值化失敗);
            m_Result_NumMsg_Dict.Add(-1004, ResultSealStatus.圖像居中失敗);
            m_Result_NumMsg_Dict.Add(-1005, ResultSealStatus.切組圖失敗);
            m_Result_NumMsg_Dict.Add(-1006, ResultSealStatus.提取印鑑失敗);
            m_Result_NumMsg_Dict.Add(-1007, ResultSealStatus.解析XML失敗);
            m_Result_NumMsg_Dict.Add(-1008, ResultSealStatus.印鑑識別失敗);
            m_Result_NumMsg_Dict.Add(-1009, ResultSealStatus.創建輪廓圖失敗);
            m_Result_NumMsg_Dict.Add(-1010, ResultSealStatus.折角變換失敗);
            m_Result_NumMsg_Dict.Add(-1011, ResultSealStatus.打開註冊表失敗);
            m_Result_NumMsg_Dict.Add(-1012, ResultSealStatus.寫註冊表失敗);
            m_Result_NumMsg_Dict.Add(-1013, ResultSealStatus.Key生成失敗);
            m_Result_NumMsg_Dict.Add(-1014, ResultSealStatus.Key檢查失敗);
            m_Result_NumMsg_Dict.Add(-1015, ResultSealStatus.讀取配置文件失敗);
            m_Result_NumMsg_Dict.Add(-1016, ResultSealStatus.連接失敗);
            m_Result_NumMsg_Dict.Add(-1017, ResultSealStatus.發送失敗);
            m_Result_NumMsg_Dict.Add(-1018, ResultSealStatus.接收失敗);
            m_Result_NumMsg_Dict.Add(-1019, ResultSealStatus.圖像縮放失敗);
            m_Result_NumMsg_Dict.Add(-1020, ResultSealStatus.圖像旋轉失敗);
            m_Result_NumMsg_Dict.Add(-1021, ResultSealStatus.沒有產生驗印結果);
            m_Result_NumMsg_Dict.Add(-1022, ResultSealStatus.印鑑分離紅藍章失敗);
            m_Result_NumMsg_Dict.Add(-1023, ResultSealStatus.DJSeal程式不存在);
            m_Result_NumMsg_Dict.Add(-1024, ResultSealStatus.檢查ZTSN是否開啟);
            m_Result_NumMsg_Dict.Add(-1025, ResultSealStatus.檢查主機IP是否和ZTSN_cfg相同SERVER_IP);
            m_Result_NumMsg_Dict.Add(-1026, ResultSealStatus.呼叫元件失敗);
            m_Result_NumMsg_Dict.Add(-1027, ResultSealStatus.印鑑擷取紅藍章失敗);
            m_Result_NumMsg_Dict.Add(-1028, ResultSealStatus.ZTSN_CFG不存在);
        }

        #region Base DLL Function

        /// <summary>
        /// 擷取函數
        /// </summary>
        /// <param name="color">分離紅或藍 (0:r 1:b)</param>
        /// <param name="szBmpFilePath">印鑑卡路徑</param>
        /// <param name="szOutXmlFilePath">build.xml</param>
        /// <param name="TempPath">驗印暫存路徑</param>
        /// <param name="dpi">DPI</param>
        /// <param name="LibPreFix">分離印章檔案名稱命名</param>
        /// <param name="LibIndex">分離印章起始編號</param>
        /// <param name="left">Image left</param>
        /// <param name="top">Image top</param>
        /// <param name="width">Image width</param>
        /// <param name="height">Image height</param>
        /// <param name="binarize">是否二值化 (0:不二值化 1:二值化)</param>
        /// <param name="rotate">是否對印鑑分離圖旋轉 (0:不轉正 1:轉正)</param>
        /// <returns></returns>
        public KeyValuePair<int, ResultSealStatus> SealBuild(SealColor color, string szBmpFilePath, string szOutXmlFilePath,
                                                             string TempPath, int dpi = 300, string LibPreFix = "", int LibIndex = 0,
                                                             int left = 0, int top = 0, int width = 0, int height = 0,
                                                             int binarize = 0, int rotate = 0)
        {
            long i = 0;
            if (color == SealColor.Red)
                return ZtErrorMessage(seal_build(out i, 0, szBmpFilePath, szOutXmlFilePath, dpi, LibPreFix, LibIndex, left, top, width, height, binarize, rotate, TempPath));
            else
                return ZtErrorMessage(seal_build(out i, 1, szBmpFilePath, szOutXmlFilePath, dpi, LibPreFix, LibIndex, left, top, width, height, binarize, rotate, TempPath));
        }        

        /// <summary>
        /// 驗印函數
        /// </summary>
        /// <param name="szInXmlFilePath">build.xml</param>
        /// <param name="szOutXmlFilePath">identify.xml</param>
        /// <param name="TempPath">驗印暫存目錄</param>
        /// <param name="TightLevel">鬆緊度</param>
        /// <param name="left">image left</param>
        /// <param name="top">image top</param>
        /// <param name="width">image width</param>
        /// <param name="height">image height</param>
        /// <param name="binarize">是否二值化 (0:不二值化 1:二值化)</param>
        /// <param name="dpi">DPI</param>
        /// <returns></returns>
        public KeyValuePair<int, ResultSealStatus> SealIdentify(string szInXmlFilePath, string szOutXmlFilePath, string TempPath,
                                                                int TightLevel, int dpi = 300,
                                                                int left = 0, int top = 0, int width = 0, int height = 0, int binarize = 0)
        {
            long i = 0;
            return ZtErrorMessage(seal_identify(out i, TightLevel, szInXmlFilePath, szOutXmlFilePath, dpi, left, top, width, height, binarize, TempPath));
        }
        /// <summary>
        /// 輪廓圖查看函數
        /// </summary>
        /// <param name="srcBmpPath">欲生成來源圖檔(限定只能使用 天創生成的  res_xxxxxx_x.bmp)</param>
        /// <param name="destBmpPath">輸出輪廓圖路徑</param>
        public KeyValuePair<int, ResultSealStatus> SealShape(string srcBmpPath, string destBmpPath)
        {
            long i = 0;
            return ZtErrorMessage(seal_shapeimg(out i, srcBmpPath, destBmpPath));
        }

        /// <summary>
        /// 殘像比對函數 (將一張影像分開兩張影像 (res_xxxxxxxxxxxxxx.bmp))
        /// </summary>
        /// <param name="szBmpFilePath"></param>
        public void SealDynamic(string szBmpFilePath)
        {
            dynamic_image(szBmpFilePath);
        }

        /// <summary>
        /// 折角圖查看函數
        /// </summary>
        /// <param name="srcBmpPath">欲生成來源(限定只能使用 天創生成的  res_xxxxxx_x.bmp)</param>
        /// <param name="destBmpPath">輸出折角圖路徑</param>
        /// <param name="angle">角度</param>
        /// <returns>KeyValuePair<int, ResultSealStatus></returns>
        public KeyValuePair<int, ResultSealStatus> SealAngle(string srcBmpPath, string destBmpPath, int angle)
        {
            return ZtErrorMessage(seal_angleimg(srcBmpPath, angle, destBmpPath));
        }

        /// <summary>
        /// DPI轉換
        /// </summary>
        /// <param name="srcBmpPath">來源圖檔路徑</param>
        /// <param name="destBmpPath">目的圖檔路徑</param>
        /// <param name="scale">比率</param>
        /// <returns>KeyValuePair<int, ResultSealStatus></returns>
        public KeyValuePair<int, ResultSealStatus> SealTrans(string srcBmpPath, string destBmpPath, double scale)
        {
            long i = 0;
            return ZtErrorMessage(trans_image(out i, srcBmpPath, destBmpPath, scale));
        }

        /// <summary>
        /// 旋轉
        /// </summary>
        /// <param name="srcBmpPath">來源圖檔路徑</param>
        /// <param name="destBmpPath">目的圖檔路徑</param>
        /// <param name="angle">角度</param>
        /// <returns>KeyValuePait<int, ResultSealStatus></returns>
        public KeyValuePair<int, ResultSealStatus> SealRotate(string srcBmpPath, string destBmpPath, int angle)
        {
            long i = 0;
            return ZtErrorMessage(rotate_image(out i, srcBmpPath, destBmpPath, angle));
        }
        #endregion

        public KeyValuePair<int, ResultSealStatus> ZtErrorMessage(int result)
        {
            foreach (KeyValuePair<int, ResultSealStatus> item in m_Result_NumMsg_Dict)
            {
                if (item.Key == result)
                    return item;
            }
            return new KeyValuePair<int, ResultSealStatus>(-1021, ResultSealStatus.檢查ZTSN是否開啟);
        }

        ///// <summary>
        ///// 修改 Build.xml 
        ///// </summary>
        ///// <param name="szBuildXmlFilePath">Build.xml 路徑</param>
        ///// <param name="szBmpFilePath">要修改的路徑內容</param>
        ///// <param name="Paramkey">節點參數 屬性參數</param>
        //public void ModifyBuildXmlPathValue(string szBuildXmlFilePath, string szBmpFilePath, params string[] Paramkey)
        //{
        //    try
        //    {
        //        XmlDocument xmldoc = new XmlDocument();
        //        xmldoc.Load(szBuildXmlFilePath);
        //        XmlNode xmlnode = xmldoc.SelectSingleNode(Paramkey[0]);

        //        foreach (XmlNode node in xmlnode)
        //        {
        //            if (node == null)
        //                continue;
        //            if (node.Attributes == null)
        //                continue;
        //            if (node.Name == Paramkey[1])
        //            {
        //                node.Attributes[Paramkey[2]].Value = szBmpFilePath;
        //                break;
        //            }
        //        }
        //        xmldoc.Save(szBuildXmlFilePath);
        //    }
        //    catch (System.Xml.XmlException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ModifyBuildXmlValue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ModifyBuildXmlValue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ModifyBuildXmlValue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ModifyBuildXmlValue", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        ///// <summary>
        ///// 讀取 build.xml 內容
        ///// </summary>
        ///// <param name="szXmlPath">印鑑卡或傳票build.xml路徑</param>
        ///// <param name="SealPathList">印鑑卡或傳票擷取印章集合路徑</param>
        ///// <param name="SealColorList">印鑑卡或傳票擷取印章集合顏色</param>
        ///// <param name="SealRectList">印鑑卡或傳票擷取印章集合Rectangle</param>
        //public void ReadBuildXmlValue(string szXmlPath, ref List<string> SealPathList, ref List<SealColor> SealColorList, ref List<SealInfo.RECT> SealRectList)
        //{
        //    try
        //    {
        //        XmlDocument xmldoc = new XmlDocument();
        //        xmldoc.Load(szXmlPath);
        //        XmlNode xmlnode = xmldoc.SelectSingleNode("input").SelectSingleNode("seallibs");
        //        SealPathList.Clear();
        //        SealColorList.Clear();
        //        foreach (XmlNode node in xmlnode.ChildNodes)
        //        {
        //            if (node.Attributes == null)
        //                continue;
        //            if (SealPathList != null)
        //                SealPathList.Add(node.Attributes["filename"].Value);
        //            if (SealColorList != null)
        //            {
        //                if (node.Attributes["color"].Value == "0")
        //                    SealColorList.Add(SealColor.Red);
        //                else
        //                    SealColorList.Add(SealColor.Blue);
        //            }
        //            SealInfo.RECT rect = new SealInfo.RECT();
        //            rect.w = Convert.ToInt32(node.Attributes["x"].Value);
        //            rect.h = Convert.ToInt32(node.Attributes["y"].Value);
        //            rect.x = Convert.ToInt32(node.Attributes["centx"].Value) - Convert.ToInt32(rect.w / 2);
        //            rect.y = Convert.ToInt32(node.Attributes["centy"].Value) - Convert.ToInt32(rect.h / 2);
        //            rect.centx = Convert.ToInt32(node.Attributes["centx"].Value);
        //            rect.centy = Convert.ToInt32(node.Attributes["centy"].Value);
        //            if (SealRectList != null)
        //                SealRectList.Add(rect);
        //        }
        //    }
        //    catch (XmlException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ReadBuildXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ReadBuildXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ReadBuildXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ReadBuildXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}
        ///// <summary>
        ///// 讀取 identify.xml 內容
        ///// </summary>
        ///// <param name="szXmlPath">印鑑卡或傳票identify.xml路徑</param>
        ///// <param name="IdentifyPathList">印鑑卡或傳票擷取印章集合路徑</param>
        ///// <param name="IdentifyRectList">印鑑卡或傳票擷取印章集合Rectangle</param>
        ///// <param name="IdentifyAngleList">印鑑卡或傳票擷取印章集合角度</param>
        ///// <param name="IdentifyScoreList">印鑑卡或傳票擷取印章集合分數</param>
        ///// <param name="IdentifyResultList">印鑑卡或傳票擷取印章集合結果</param>
        //public void ReadIdentifyXmlValue(string szXmlPath, ref List<string> IdentifyPathList, ref List<SealInfo.RECT> IdentifyRectList, ref List<double> IdentifyAngleList, ref List<int> IdentifyScoreList, ref List<int> IdentifyResultList)
        //{
        //    try
        //    {
        //        XmlDocument xmldoc = new XmlDocument();
        //        xmldoc.Load(szXmlPath);
        //        XmlNode xmlnode = xmldoc.SelectSingleNode("output");

        //        foreach (XmlNode node in xmlnode.ChildNodes)
        //        {
        //            if (node.Attributes == null)
        //                continue;
        //            if (IdentifyPathList != null)
        //                IdentifyPathList.Add(node.Attributes["filename"].Value);
        //            SealInfo.RECT rect = new SealInfo.RECT();
        //            rect.w = Convert.ToInt32(node.Attributes["x"].Value);
        //            rect.h = Convert.ToInt32(node.Attributes["y"].Value);
        //            rect.x = Convert.ToInt32(node.Attributes["centx"].Value) - Convert.ToInt32(rect.w / 2);
        //            rect.y = Convert.ToInt32(node.Attributes["centy"].Value) - Convert.ToInt32(rect.h / 2);
        //            rect.centx = Convert.ToInt32(node.Attributes["centx"].Value);
        //            rect.centy = Convert.ToInt32(node.Attributes["centy"].Value);
        //            if (IdentifyRectList != null)
        //                IdentifyRectList.Add(rect);
        //            double angle = 0;
        //            angle = Convert.ToDouble(node.Attributes["angle"].Value);
        //            if (IdentifyAngleList != null)
        //                IdentifyAngleList.Add(angle);
        //            int score = Convert.ToInt32(node.Attributes["fraction"].Value);
        //            if (IdentifyScoreList != null)
        //                IdentifyScoreList.Add(score);
        //            int result = Convert.ToInt32(node.Attributes["result"].Value);
        //            if (IdentifyResultList != null)
        //                IdentifyResultList.Add(result);
        //        }
        //    }
        //    catch (XmlException ex)
        //    {
        //        MessageBox.Show(ex.Message, "ReadIdentifyXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ReadIdentifyXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //    catch (Exception ex)
        //    {
        //        MessageBox.Show(ex.Message, "ReadIdentifyXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //        MessageBox.Show(ex.StackTrace, "ReadIdentifyXml_FilePath", MessageBoxButtons.OK, MessageBoxIcon.Error);
        //    }
        //}

        bool disposed = false;

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            if (disposed)
                return;
            if (disposing)
            {

            }
            disposed = true;
        }

        ~TchznSealUnit()
        {
            Dispose(false);
        }
    }
}
