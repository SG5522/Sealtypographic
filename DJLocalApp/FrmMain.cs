using DJLocalApp.Extensions;
using DJScannerLib.Models;
using DJScannerLib.Services;
using System.Drawing.Imaging;
using System.Text.Json;

namespace DJLocalApp
{
    /// <summary>
    /// 主畫面程式
    /// </summary>
    /// <seealso cref="System.Windows.Forms.Form" />
    public partial class FrmMain : Form
    {
        /// <summary>
        /// 掃描器元件Service
        /// </summary>
        private readonly IScannerService scannerService;

        /// <summary>
        /// The tiff codec information
        /// </summary>
        private ImageCodecInfo imageCodecInfo;

        /// <summary>
        /// 建構: 載入Service
        /// </summary>
        public FrmMain(IScannerService scannerService)
        {
            InitializeComponent();

            Text += (NTwain.PlatformInfo.Current.IsApp64Bit) ? " (64bit)" : " (32bit)";

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

        private void RefreshUrl()
        {
            LstUrl.Items.Clear();
            List<string> urls = Program.WebApp.Urls.ToList();
            foreach (string url in urls)
            {
                LstUrl.Items.Add(url);
            }
        }

        private void BtnCurrentDriver_Click(object sender, EventArgs e)
        {
            CurrentDriverResult result = scannerService.GetCurrentDriver();
            TvwShow.LoadJson("Current Drive", JsonSerializer.Serialize(result), true, true);
        }

        private void BtnSetDriver_Click(object sender, EventArgs e)
        {
            bool result = scannerService.SetDriver("123");
            TvwShow.LoadJson("Set Driver", JsonSerializer.Serialize(result), true, true);
        }

        private void BtnAllDrivers_Click(object sender, EventArgs e)
        {
            AllDriversResult result = scannerService.GetAllDrivers();
            TvwShow.LoadJson("All Srivers", JsonSerializer.Serialize(result), true, true);
        }
    }
}