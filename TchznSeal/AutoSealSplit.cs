using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
using System.Drawing;

namespace TchznSeal
{
    public class AutoSealSplit
    {
        //private readonly string exePath = @".\"; //@暫時固定路徑
        private readonly string exePath = Directory.GetCurrentDirectory() + @"\";
        //private readonly string exePath = Directory.GetCurrentDirectory() + @"\bin\Debug\net6.0\"; //@暫時固定路徑
        //const string DJFileName = "DJSealResult.txt";   // 天創元件 結果文字檔
        private readonly string xmlname = "build.xml";
        [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
        static extern bool SetDllDirectory(string lpPathName);

        /// <summary>
        /// 分離印鑑(整張圖)
        /// </summary>
        /// <param name="sealCardFullName">印鑑卡檔名</param>
        /// <param name="targetFilePath">目標資料夾路徑</param>    
        /// <param name="LibPreFixName">分離印章檔案名稱命名</param>
        /// <param name="sealColor">印鑑顏色</param>
        /// <returns></returns>
        public int SealSplit(string sealCardFullName,string targetFilePath,string LibPreFixName, string sealColor)
        {
            //var dll_t = new DJ_TzchznSeal();
            //dll_t.Seal_Split(Seal_name, SealColor);
            var sealunit = new TchznSealUnit();
            KeyValuePair<int, TchznSealUnit.ResultSealStatus> result;

            if (File.Exists(sealCardFullName))
            {
                if (!Directory.Exists(targetFilePath))
                {
                    Directory.CreateDirectory(targetFilePath);
                }

                // 印鑑建檔 build.xml 路徑
                string buildXmlFileFullName = targetFilePath + xmlname;
                int sealIndex = 0;

                //@set TchznSealUnit path
                SetDllDirectory(exePath + @"include");
                //準備擷取印鑑......
                if (sealColor == "R")
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Red, sealCardFullName, buildXmlFileFullName, targetFilePath, 300, LibPreFixName, sealIndex);
                }
                else
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Blue, sealCardFullName, buildXmlFileFullName, targetFilePath, 300, LibPreFixName, sealIndex);
                }
                
                return result.Key;                               
            }            
            return 9999;
        }
               
    }
}
