using DJLocalAPI.Api;
using DJScannerLib.Services;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using ScannerLib.Services;
using System.Windows.Forms.VisualStyles;
using TWAINWorkingGroup;

namespace DJLocalAPI
{
    public partial class FrmDJLLocalAPI : Form, IMessageFilter
    {
        //ScannerService sc;
        private bool scanStart = false;

        private string[]? args;
        private ApiServerService? apiServerService;
        private ScannerService scannerService;
        public FrmDJLLocalAPI()
        {
            InitializeComponent();
        }
        public FrmDJLLocalAPI(string[]? args) : this()
        {

        }
        private void DJLLocalAPI_Load(object sender, EventArgs e)
        {
            apiServerService = new(args); 
            apiServerService!.StartServer();
            scannerService.InitTwain(this.Handle);
            SetMessageFilter(true);
        }
        private void FrmDJLLocalAPI_FormClosing(object sender, FormClosingEventArgs e) => apiServerService!.StopServer();
 
        private void btnGetDrivers_Click(object sender, EventArgs e)
        {
            List<string> drivers = new List<string>();
            drivers = scannerService.GetAllDrivers();
            foreach (string driver in drivers) 
            {
                lbDriver.Items.Add(driver);
            }
        }

        private void lbDriver_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            bool result = scannerService.SetSelectDriver((string)lbDriver.SelectedItem);
            if (result) 
            {
                lblSetResult.Text = "設置狀態：成功";
            }
            else
            {
                lblSetResult.Text = "設置狀態：失敗";
            }
        }

        private void btnSetDriver_Click(object sender, EventArgs e)
        {
            bool result = scannerService.SetSelectDriver((string)lbDriver.SelectedItem);
            if (result)
            {
                lblSetResult.Text = "設置狀態：成功";
            }
            else
            {
                lblSetResult.Text = "設置狀態：失敗";
            }
        }

        private void btnScan_Click(object sender, EventArgs e)
        {
            scannerService.Scan();
        }
        /// <summary>
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY), this
        /// function is only triggered when SetMessageFilter() is called with 'true'...
        /// </summary>
        /// <param name="message">Message to process</param>
        /// <returns>Result of the processing</returns>
        //[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message message)
        {
            IntPtr a_intptrHwnd = message.HWnd;
            int a_iMsg = message.Msg;
            IntPtr a_intptrWparam = message.WParam;
            IntPtr a_intptrLparam = message.LParam;
            bool scanEnd = scannerService.PreFilterMessage(a_intptrHwnd, a_iMsg, a_intptrWparam, a_intptrLparam);

            if (scanEnd && scanStart)
            {
                scanStart = false;
                //scanImageDatas = dJTWAIN.LoadImageDatas();

                //foreach (ScanImageData scanImageData in scanImageDatas)
                //{
                //    foreach (var socket in allSockets.ToList())
                //    {
                //        socket.Send(JsonConvert.SerializeObject(scanImageData));
                //        //socket.Send(scanImageData.Data);
                //    }
                //}
                //this.WindowState = FormWindowState.Minimized;
                //@清空ImageDatas
                //dJTWAIN.ClearImageDatas();
                //OpenScanImageListView(scanImagePath,"bmp");
                return true;
            }
            return false;
        }
        /// <summary>
        /// Turn message filtering on or off, we use this to capture stuff
        /// like MSG_XFERREADY.  If it's off, then it's assumed we're getting
        /// this info through DAT_CALLBACK2...
        /// </summary>
        /// <param name="openCheck">True to turn it on</param>
        public void SetMessageFilter(bool openCheck)
        {
            if (openCheck)
            {
                Application.AddMessageFilter(this);
            }
            else
            {
                Application.RemoveMessageFilter(this);
            }
        }
    }
}