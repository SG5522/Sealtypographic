using DJLocalApp.Utils;
using DJScannerLib.Services;
using System.Drawing.Imaging;

namespace DJLocalApp
{
    public partial class FrmMain : Form
    {
        /// <summary>
        /// ±Ω¥yæπ§∏•ÛService
        /// </summary>
        private readonly IScannerService scannerService;

        /// <summary>
        /// The tiff codec information
        /// </summary>
        private ImageCodecInfo imageCodecInfo;

        /// <summary>
        /// 
        /// </summary>
        public FrmMain(IScannerService scannerService)
        {
            InitializeComponent();
            Text += (NTwain.PlatformInfo.Current.IsApp64Bit)? " (64bit)" : " (32bit)";
            
            foreach (ImageCodecInfo imageCodecInfo in ImageCodecInfo.GetImageEncoders())
            {
                if (imageCodecInfo.MimeType == "image/tiff") 
                { 
                    this.imageCodecInfo = imageCodecInfo; 
                    break; 
                }
            }

            this.scannerService = scannerService;
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            RefreshUrl();
        }

        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
        }

        private void RefreshUrl()
        {
            LstUrl.Items.Clear();
            List<string> urls = Program.WebApp.Urls.ToList();
            foreach(string url in urls)
            {
                LstUrl.Items.Add(url);
            }
        }
    }
}