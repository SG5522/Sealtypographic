using TchznSeal;
using DJLib;
using DJSharpZipLib;
using DJEncryption;
using System.Data.SqlTypes;

namespace TchznSealTest
{
    public partial class Tchantest : Form
    {
        private readonly AutoSealSplit autoSealSplit = new();
        private string SaltString = string.Empty;
        private string Password = string.Empty;
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
                //DJGZip.CompressBytes(bytes, Path.Combine($"D:/", "test.bmp"));

                //DJGZip.CompressToFile(filepath, Path.Combine($"D:/", "gztest.gz"));
                byte[] outbytes = DJGZip.CompressBytes(bytes);
                EncryptionWithAES encryptionWithAES = EncryptionWithAES.Encrypte(outbytes, 12);

                label1.Text = encryptionWithAES.SaltString;
                label2.Text = encryptionWithAES.Password;
                //File.WriteAllBytes(Path.Combine($"D:/", "gztest.gz"), outbytes);
                File.WriteAllBytes(Path.Combine($"D:/", "gztest.gz"), encryptionWithAES.EncryptionData);
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
                encryptionWithAES.EncryptionData = File.ReadAllBytes(filepath);
                encryptionWithAES.SaltString = label1.Text;
                encryptionWithAES.Password = label2.Text;
                byte[] outBytes = EncryptionWithAES.Decrypt(encryptionWithAES);
                outBytes = DJGZip.DecompressBytes(outBytes);
                File.WriteAllBytes(Path.Combine($"D:/", "gztest.bmp"), outBytes);
            }
        }
    }
}
