using DJLocalAPI.Api;
using DJScannerLib.Models;
using DJScannerLib.Services;
using System.Security.Permissions;

namespace DJLocalAPI
{
    public partial class FrmDJLocalAPI : Form
    {
        //ScannerService sc;
        private bool scanStart = false;

        private string[] args;
        private ApiServer? apiServer;
        private string apiLocal = "http://localhost:";
        private int apiPort = 22431;
        private string apiUrl;
        //private IScannerService scannerService;

        /// <summary>
        /// «Øºc
        /// </summary>
        public FrmDJLocalAPI()
        {
            InitializeComponent();
            apiUrl = apiLocal + apiPort.ToString();
        }

        /// <summary>
        /// «Øºc
        /// </summary>
        /// <param name="args"></param>
        public FrmDJLocalAPI(string[] args) : this()
        {
            this.args = args;
        }

        private void DJLLocalAPI_Load(object sender, EventArgs e)
        {
            //scannerService = apiServer.ScannerService;
            WindowState = FormWindowState.Minimized;
            apiServer = new(args, Handle);
            Task.Run(() => apiServer!.StartAsync()); 
        }

        private void FrmDJLLocalAPI_FormClosing(object sender, FormClosingEventArgs e) => apiServer!.StopAsync();
    }
}