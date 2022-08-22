using Fleck;
using DJTWAINLib;
using System.Windows.Forms;

namespace DJTWAINScan
{
    public partial class Scan : Form, IMessageFilter
    {
        private DJTWAIN dJTWAIN = new();
        private bool scanStart = false;
        private string scanImagePath = "";
        private List<IWebSocketConnection> allSockets;
        private WebSocketServer server;
        public Scan()
        {
            InitializeComponent();

            dJTWAIN.TwainSet(this,this.Handle);
            SetMessageFilter(true);

            //預設縮到最小
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;


            
            //設定WebSocketServer
            allSockets = new List<IWebSocketConnection>();
            server = new WebSocketServer("ws://0.0.0.0:8181");
            server.Start(socket =>
            {
                socket.OnOpen = () =>
                {
                    Console.WriteLine("Open!");
                    allSockets.Add(socket);
                };
                socket.OnClose = () =>
                {
                    Console.WriteLine("Close!");
                    allSockets.Remove(socket);
                };
                socket.OnMessage = message =>
                {
                    //等到網頁確定呼叫1100就將視窗還原
                    if (message == "1100")
                    {
                        this.Invoke(new Action(() => {
                            this.WindowState = FormWindowState.Normal;
                        }));
                    }
                };
            });   
            
        }   


        /// <summary>
        /// We're being closed, clean up nicely...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisFormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                this.WindowState = FormWindowState.Minimized;
            }

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
            bool scanEnd = dJTWAIN.PreFilterMessage(a_intptrHwnd, a_iMsg, a_intptrWparam, a_intptrLparam);

            if (scanEnd && scanStart)
            {
                scanStart = false;
                OpenScanImageListView(scanImagePath,"bmp");
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

        private void ButtonScanSource_Click(object sender, EventArgs e)
        {
            
            dJTWAIN.CloseTWAINDriver(); //每次選擇掃描機就關閉前一次開啟的掃描機驅動

            string selectScan; //@被選擇的掃描機
            ScanSelect ScanSelect;
            DialogResult dialogresult;
            ScanSourceDataList scanSourceDataList = new();                       

            dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);


            if(scanSourceDataList.ErrorMessage =="")
            {
                MessageBox.Show(scanSourceDataList.ErrorMessage);
                return;
            }
            
            // Ruh-roh...
            if (scanSourceDataList.LszIdentity.Count == 0)
            {
                //MessageBox.Show("There are no TWAIN drivers installed on this system...");
                dJTWAIN.SuorceSelectCancel();
                dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);
            }

            // Instantiate our form...
            ScanSelect = new ScanSelect(scanSourceDataList.LszIdentity, scanSourceDataList.SzDefault)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            dialogresult = ScanSelect.ShowDialog(this);
            if (dialogresult != System.Windows.Forms.DialogResult.OK)
            {
                //blExit = true;
                return;
            }

            // Get all the identities...
            // Get the selected identity...
            selectScan = ScanSelect.GetSelectedDriver();
            if (selectScan == null)
            {
                //blExit = true;
                return;
            }

            dJTWAIN.ScanSource(selectScan, scanSourceDataList);
            if(scanSourceDataList.ErrorMessage == null)
            {
                this.Text = "TWAIN C# Scan (" + selectScan + ")";
                ButtonSetup.Enabled = true;
                ButtonScan.Enabled = true;                
            }
            else
            {
                MessageBox.Show(scanSourceDataList.ErrorMessage + "\n請確認掃描機是否有開啟");
            }
        }

        private void ButtonScan_Click(object sender, EventArgs e)
        {            
            string imageName = DateTime.Now.ToString("yyMMddHHmmss");
            string imgageType = ".bmp";

            dJTWAIN.StartScan(this.Handle, scanImagePath , imageName, imgageType);
            scanStart = true;
        }

        private void ButtonSetup_Click(object sender, EventArgs e)
        {
            dJTWAIN.Setup(this.Handle);
        }

        private void SelectScanPathButton_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderBrowserDialog = new();
            folderBrowserDialog.Description = "請選擇掃描完成後的資料夾";
            if (folderBrowserDialog.ShowDialog() == DialogResult.OK)
            {
                scanImagePath = folderBrowserDialog.SelectedPath + @"\";
                OpenScanImageListView(scanImagePath, "bmp");
                buttonScanSource.Enabled = true;
            }
        }

        private void OpenScanImageListView(string scanImagePath,string imageType)
        {
            scanImageListView.Items.Clear();
            ScanImageListViewHeaderSetting();
            string[] files = Directory.GetFiles(scanImagePath);
            if (files.Length > 0)
            {
                DirectoryInfo folder = new(scanImagePath);
                foreach (FileInfo file in folder.GetFiles("*F." + imageType))
                {
                    scanImageListView.Items.Add(file.Name);
                    //test

                }
                if (scanImageListView.Items.Count != 0) scanImageListView.Items[0].Selected = true;
            }
        }
        private void ScanImageListViewHeaderSetting()
        {
            scanImageListView.Scrollable = false;
            scanImageListView.HeaderStyle = ColumnHeaderStyle.None;
            ColumnHeader header = new()
            {
                Text = "",
                Name = "col1"
            };
            scanImageListView.Columns.Add(header);
            scanImageListView.AutoResizeColumns(ColumnHeaderAutoResizeStyle.HeaderSize);
        }

        private void ScanImageListView_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (scanImageListView.SelectedItems.Count != 0)
            {
                string Scan_filename_F = scanImageListView.SelectedItems[0].Text;
                string Scan_filename_R = Scan_filename_F.Replace("F", "R");
                pictureBox1.Image = Fromimage(scanImagePath + Scan_filename_F);
                pictureBox2.Image = Fromimage(scanImagePath + Scan_filename_R);
            }
        }
        private static Image Fromimage(string path)
        {
            FileStream fs = File.OpenRead(path);
            var image = Image.FromStream(fs);
            fs.Dispose();
            return image;
        }

        private void Scan_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowIcon = false;
                notifyIcon1.Visible = true;
                //notifyIcon1.ShowBalloonTip(100);
            }
        }

        private void ExitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            // Make sure this thing is off...
            SetMessageFilter(false);
            /// We're being closed, clean up nicely...
            dJTWAIN.FormClosing();
            // This will prevent ReportImage from doing anything as we close...
            //m_graphics1 = null;
            Application.Exit();
        }

        private void NotifyIcon1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Show();
            this.WindowState = FormWindowState.Normal;
        }
    }
}
