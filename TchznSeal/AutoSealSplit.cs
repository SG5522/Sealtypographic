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
        private readonly string exePath = Directory.GetCurrentDirectory();
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
        /// <param name="libPreFixName">分離印章檔案名稱命名</param>
        /// <param name="sealColor">印鑑顏色</param>
        /// <returns></returns>
        public int SplitSeal(string sealCardFullName,string targetFilePath,string libPreFixName, string sealColor)
        {
            //var dll_t = new DJ_TzchznSeal();
            //dll_t.Seal_Split(Seal_name, SealColor);
            TchznSealUnit sealunit = new TchznSealUnit();
            KeyValuePair<int, TchznSealUnit.ResultSealStatus> result;

            if (File.Exists(sealCardFullName))
            {
                if (!Directory.Exists(targetFilePath))
                {
                    Directory.CreateDirectory(targetFilePath);
                }
                else
                {
                    DirectoryInfo directoryInfo = new DirectoryInfo(targetFilePath);                    
                    FileInfo[] files = directoryInfo.GetFiles();
                    foreach (FileInfo file in files)
                    {
                        file.Delete();
                    }
                }

                // 印鑑建檔 build.xml 路徑
                string buildXmlFileFullName = targetFilePath + xmlname;
                int sealIndex = 0;

                //@set TchznSealUnit path
                SetDllDirectory(exePath + @"\include");
                //準備擷取印鑑......
                if (sealColor == "R")
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Red, sealCardFullName, buildXmlFileFullName, targetFilePath, 300, libPreFixName, sealIndex);
                }
                else
                {
                    result = sealunit.SealBuild(TchznSealUnit.SealColor.Blue, sealCardFullName, buildXmlFileFullName, targetFilePath, 300, libPreFixName, sealIndex);
                }
                if(result.Key == 0)
                {
                    MoveXML(targetFilePath, targetFilePath + @"XML\", xmlname);
                }
                return result.Key;                               
            }            
            return 9999;
        }
        private void MoveXML(string originalFilePath,  string targetFilePath, string xmlName)
        {
            try
            {
                if (!Directory.Exists(targetFilePath))
                {
                    Directory.CreateDirectory(targetFilePath);
                }
                if (File.Exists(targetFilePath + xmlname))
                {
                    File.Delete(targetFilePath + xmlname);
                }
                File.Move(originalFilePath + xmlName, targetFilePath + xmlName);                
            }
            catch (FileNotFoundException e)
            {                
                Console.WriteLine($"The file was not found: '{e}'");
            }
        }
        
    }
}
