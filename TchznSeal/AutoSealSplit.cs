using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TchznSeal
{
    public class AutoSealSplit
    {        
        private readonly string exePath = @".\";
        //const string DJFileName = "DJSealResult.txt";   // 天創元件 結果文字檔
        private readonly string xmlname = "build.xml";
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// 整張圖截取
        /// </summary>
        /// <param name="sealCardPath">印鑑卡檔名</param>
        /// <param name="destFilePath">目標資料夾路徑(印章 + XML擺放位置)</param>    
        /// <param name="LibPreFixName">分離印章檔案名稱命名</param>
        /// <param name="sealColor">印鑑顏色</param>
        /// <returns></returns>
        //@使用DLL分離
        public int SealSplit(string sealCardPath,string destFilePath,string LibPreFixName, string sealColor)
        {
            //var dll_t = new DJ_TzchznSeal();
            //dll_t.Seal_Split(Seal_name, SealColor);
            var sealunit = new TchznSealUnit();
            KeyValuePair<int, TchznSealUnit.ResultSealStatus> result;

            if (File.Exists(sealCardPath))
            {
                if (!Directory.Exists(destFilePath))
                {
                    Directory.CreateDirectory(destFilePath);
                }

                // 印鑑建檔 build.xml 路徑
                string buildXmlFilePath = destFilePath + xmlname;
                int sealIndex = 0;

                //@set TchznSealUnit path
                SetDllDirectory(exePath + @"include");
                //準備擷取印鑑......
                if (sealColor == "R")
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Red, sealCardPath, buildXmlFilePath, destFilePath, 300, LibPreFixName, sealIndex);
                }
                else
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Blue, sealCardPath, buildXmlFilePath, destFilePath, 300, LibPreFixName, sealIndex);
                }
                
                return result.Key;                               
            }            
            return 9999;
        }
               
    }
}
