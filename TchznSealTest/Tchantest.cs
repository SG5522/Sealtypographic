using TchznSeal;
using DJLib;
using DJSharpZipLib;
using DJEncryption;
using System.Data.SqlTypes;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Security.Cryptography.X509Certificates;

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

        private static void OpenDialog(OpenFileDialog dialog)
        {
            dialog.Multiselect = false;//該值確定是否可以選擇多個檔案
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "所有檔案(*.*)|*.*";
        }

        private void ButtonOpenimage_Click(object sender, EventArgs e)
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
        private void Button1_Click(object sender, EventArgs e)
        {
            
            OpenFileDialog dialog = new();
            OpenDialog(dialog);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                RSAEncryption rSAEncryption = new();
                string filepath = dialog.FileName;
                string ImageBase64 = ImageSharpUtil.PathImageFileToBase64(filepath);
                string base64String = ImageBase64[(ImageBase64.IndexOf("base64,") + 7)..];
                byte[] bytes = Convert.FromBase64String(base64String);
                AESEncryption encryptionWithAES = AESEncryption.Encrypte(bytes, 12);
                // 生成 RSA 公鑰和私鑰
                string publicKey = rSAEncryption.GetPublicKey();
                string privateKey = rSAEncryption.GetPrivateKey();

                encryptionWithAES.SaltString = rSAEncryption.Encrypt(encryptionWithAES.SaltString, publicKey);
                encryptionWithAES.Password = rSAEncryption.Encrypt(encryptionWithAES.Password, publicKey);

                Dictionary<string, byte[]> src = new()
                {
                    [$"{dialog.SafeFileName}"] = encryptionWithAES.EncryptionData,
                    [$"{dialog.SafeFileName}.txt"] = Encoding.UTF8.GetBytes($"{encryptionWithAES.SaltString}{LF}{encryptionWithAES.Password}")
                };
                label1.Text = publicKey;
                label2.Text = privateKey;

                byte[] outbytes = DJZip.Compress(src);

                
                File.WriteAllBytes(Path.Combine($"C:/DJimage/TestSealcard", "ziptest.zip"), outbytes);
            }
        }

        /// <summary>
        /// 解密 解壓縮
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Button2_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new();
            AESEncryption encryptionWithAES = new();
            OpenDialog(dialog);
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                string filepath = dialog.FileName;
                RSAEncryption rSAEncryption = new();
                //encryptionWithAES.EncryptionData = File.ReadAllBytes(filepath);                
                //byte[] outBytes = EncryptionWithAES.Decrypt(encryptionWithAES);
                //outBytes = DJGZip.DecompressBytes(outBytes);
                Dictionary<string, byte[]> src = DJZip.Decompress(File.ReadAllBytes(filepath));
                string txt = Encoding.UTF8.GetString(src.Last().Value);
                string[] lines = txt.Split(new[] { NewLine, LF }, StringSplitOptions.None);

                lines[0] = rSAEncryption.Decrypt(lines[0], label2.Text);
                lines[1] = rSAEncryption.Decrypt(lines[1], label2.Text);

                encryptionWithAES.EncryptionData = src.First().Value;
                encryptionWithAES.SaltString = lines[0];
                encryptionWithAES.Password = lines[1];
                byte[] outBytes = AESEncryption.Decrypt(encryptionWithAES);
                File.WriteAllBytes(Path.Combine($"D:/", "gztest.bmp"), outBytes);
            }
        }
    }
}
