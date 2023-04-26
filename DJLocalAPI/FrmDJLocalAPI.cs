using DJLocalAPI.Api;

namespace DJLocalAPI
{
    public partial class FrmDJLocalAPI : Form
    {
        private ApiServer? apiServer;
        //private IScannerService scannerService;

        /// <summary>
        /// «Øºc
        /// </summary>
        public FrmDJLocalAPI()
        {
            InitializeComponent();
        }

        /// <summary>
        /// «Øºc
        /// </summary>
        /// <param name="args"></param>
        public FrmDJLocalAPI(string[] args) : this()
        {
            apiServer = new(args, Handle);
        }

        private void DJLocalAPI_Load(object sender, EventArgs e)
        {
            WindowState = FormWindowState.Minimized;
            
            apiServer!.StartAsync();
            //scannerService = apiServer.ScannerService;
        }

        private void FrmDJLocalAPI_FormClosing(object sender, FormClosingEventArgs e) => apiServer!.StopAsync();
    }
}