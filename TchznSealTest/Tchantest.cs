using TchznSeal;
using DJLib;
using DJSharpZipLib;
using DJEncryption;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;

namespace TchznSealTest
{
    public partial class Tchantest : Form
    {
        private readonly AutoSealSplit autoSealSplit = new();
        private const string NewLine = @"\r\n";
        private const string LF = @"\n";
        public Tchantest()
        {
            InitializeComponent();
        }

        private void OpenDialog(OpenFileDialog dialog)
        {
            dialog.Multiselect = false;//該值確定是否可以選擇多個檔案
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "所有檔案(*.*)|*.*";
        }

        private void buttonOpenimage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            OpenDialog(dialog);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                autoSealSplit.SplitSeal(filepath, @"C:\temp\", "Test", "R");
            }
        }


        /// <summary>
        /// 加密 壓縮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button1_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            OpenDialog(dialog);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                string ImageBase64 = ImageSharpUtil.PathImageFileToBase64(filepath);
                string base64String = ImageBase64.Substring(ImageBase64.IndexOf("base64,") + 7);
                byte[] bytes = Convert.FromBase64String(base64String);
                EncryptionWithAES encryptionWithAES = EncryptionWithAES.Encrypte(bytes, 12);
                Dictionary<string, byte[]> src = new()
                {
                    [$"{dialog.SafeFileName}"] = encryptionWithAES.EncryptionData,
                    [$"{dialog.SafeFileName}.txt"] = Encoding.UTF8.GetBytes($"{encryptionWithAES.SaltString}{LF}{encryptionWithAES.Password}")
                };
                //byte[] outbytes = DJGZip.CompressBytes(encryptionWithAES.EncryptionData);
                //byte[] outbytes = DJZip.Compress(src);
                byte[] outbytes = DJ7Zip.Compress(src);


                //File.WriteAllBytes(Path.Combine($"D:/", "gztest.gz"), outbytes);
                File.WriteAllBytes(Path.Combine($"C:/DJimage/TestSealcard", "ziptest.7z"), outbytes);
            }
        }

        /// <summary>
        /// 解密 解壓縮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            EncryptionWithAES encryptionWithAES = new();
            OpenDialog(dialog);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                //encryptionWithAES.EncryptionData = File.ReadAllBytes(filepath);                
                //byte[] outBytes = EncryptionWithAES.Decrypt(encryptionWithAES);
                //outBytes = DJGZip.DecompressBytes(outBytes);
                Dictionary<string, byte[]> src = DJZip.Decompress(File.ReadAllBytes(filepath));
                string txt = Encoding.UTF8.GetString(src.Last().Value);
                string[] lines = txt.Split(new[] { NewLine, LF }, StringSplitOptions.None);
                encryptionWithAES.EncryptionData = src.First().Value;
                encryptionWithAES.SaltString = lines[0];
                encryptionWithAES.Password = lines[1];
                byte[] outBytes = EncryptionWithAES.Decrypt(encryptionWithAES);
                File.WriteAllBytes(Path.Combine($"D:/", "gztest.bmp"), outBytes);
            }
        }
    }
}
