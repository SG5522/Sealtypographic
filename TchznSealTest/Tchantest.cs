using TchznSeal;
using DJLib;
using DJSharpZipLib;

namespace TchznSealTest
{
    public partial class Tchantest : Form
    {
        private readonly AutoSealSplit autoSealSplit = new();
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
                DJZip.CompressBytes(bytes, "", Path.Combine($"D:/", "test.bmp.gz"));

            }
        }
    }
}
