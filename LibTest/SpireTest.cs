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

        private async void BtnStart_Click(object sender, EventArgs e)
        {
            
            Stream stream = OpenCvUtil.TransparentToStream(Dialog.FileName, 160);

            using (FileStream fileStream = File.Create(@"D:\123.png"))
            {
                stream.Position = 0;
                await stream.CopyToAsync(fileStream);
            }

            //pDFService.GetEditPDFBase64(editPages, true);
        }

        private void BtnToImageBas64_Click(object sender, EventArgs e)
        {
            ImageSharpUtil.PathImageFileToBase64(Dialog.FileName);
        }
    }
}