using DJLib;
using DJLib.Models;
using DJSpire.Models;
using DJSpire.Services;

namespace LibTest
{
    public partial class SpireTest : Form
    {
        public OpenFileDialog Dialog { get; set; }

        public SpireTest()
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
            //PDFService pDFService = new()
            //{
            //    PDFPath = "C:\\DJimage\\TestSealcard\\2222.pdf"
            //};

            //ImageInfo imageInfo = ImageInfo.FromPath(Dialog.FileName);
            //ImageInfo imageInfo = ImageInfo.FromPathWithAlpha(Dialog.FileName);
            //imageInfo.Transparent();

            OpenCvUtil.TransparentToStream(Dialog.FileName, 160);

            //pDFService.GetEditPDFBase64(editPages, true);
        }

        private void BtnToImageBas64_Click(object sender, EventArgs e)
        {
            ImageSharpUtil.PathImageFileToBase64(Dialog.FileName);
        }
    }
}