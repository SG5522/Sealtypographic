using DJLib;
using DJLib.Models;
using DJSpireNet6;

namespace SpireTest
{
    public partial class Form1 : Form
    {
        public OpenFileDialog Dialog { get; set; }

        public Form1()
        {
            InitializeComponent();
            Dialog = new OpenFileDialog();
        }

        private void button1_Click(object sender, EventArgs e)
        {

        }

        private void BtnOpenFile_Click(object sender, EventArgs e)
        {
            OpenDialog(Dialog);
            Dialog.ShowDialog();
        }

        private static void OpenDialog(OpenFileDialog dialog)
        {
            dialog.Multiselect = false; //該值確定是否可以選擇多個檔案
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "所有檔案(*.*)|*.*";
        }

        private void BtnStart_Click(object sender, EventArgs e)
        {
            PdfPageToImage pdfPageToImage = new()
            {
                PageIndex = 1,
                Path = Dialog.FileName
            };

            ImageInfo imageInfo = ImageInfo.FromImageBase64(PdfPageToImage.GetImageBase64(pdfPageToImage));

            SaveFullPath saveFullPath = new()
            {
                FileName = "test",
                Folder = "D:\\works\\SealTypographicWebAPI\\"
            };
            ImageSharpUtil.SaveFile(imageInfo.Image, imageInfo.ImageFormat, saveFullPath);
        }

        private void BtnToImageBas64_Click(object sender, EventArgs e)
        {
            ImageSharpUtil.PathImageFileToBase64(Dialog.FileName);
        }
    }
}