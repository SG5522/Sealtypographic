using Fleck;
using DJTWAINLib;
using System.Windows.Forms;
using Newtonsoft.Json;
using System.Drawing.Imaging;
using System.IO;
using System.Net.Sockets;

namespace DJTWAINScan
{
    public partial class Scan : Form, IMessageFilter
    {
        private DJTWAIN dJTWAIN = new();
        private bool scanStart = false;
        private string scanImagePath = "";
        private List<IWebSocketConnection> allSockets = new();
        private WebSocketServer? server;
        private List<ScanImageData> scanImageDatas = new();

        public Scan()
        {
            InitializeComponent();

            //Open ScanSetting
            dJTWAIN.TwainSet(this, this.Handle);
            SetMessageFilter(true);
            DefaultScan();

            //預設縮到最小
            this.WindowState = FormWindowState.Minimized;
            this.ShowInTaskbar = false;

            WebSocket();

        }   
        public void WebSocket()
        {
            //設定WebSocketServer            
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
                    GetData? getData = JsonConvert.DeserializeObject<GetData>(message);
                    if (getData != null)
                    {
                        //等到網頁確定呼叫1100就將視窗還原
                        if (getData.Opencode == "1100")
                        {
                            if (getData.ScanSavePath != null)
                            {
                                scanImagePath = getData.ScanSavePath;
                                if (!Directory.Exists(scanImagePath))
                                {
                                    Directory.CreateDirectory(scanImagePath);
                                }
                                this.Invoke(new Action(() =>
                                {
                                    this.WindowState = FormWindowState.Normal;
                                    this.Show();
                                    this.ShowIcon = true;
                                    notifyIcon1.Visible = false;
                                }));
                            }
                            else
                            {
                                socket.Send("沒有指定掃描路徑");
                            }
                        }
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
                scanImageDatas = dJTWAIN.LoadImageDatas();
                
                foreach (ScanImageData scanImageData in scanImageDatas)
                {
                    foreach (var socket in allSockets.ToList())
                    {
                        socket.Send(JsonConvert.SerializeObject(scanImageData));
                        //socket.Send(scanImageData.Data);
                    }
                }
                this.WindowState = FormWindowState.Minimized;
                //@清空ImageDatas
                dJTWAIN.ClearImageDatas();
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
        private void DefaultScan()
        {
            ScanSourceDataList scanSourceDataList = new();
            dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);
            ScanSourceData defaultData = dJTWAIN.SourceData(scanSourceDataList.DefaultScan);
            dJTWAIN.ScanSource(defaultData.TwidentityProductName, scanSourceDataList);
            if (scanSourceDataList.ErrorMessage == null)
            {
                this.Text = "Scan Device (" + defaultData.TwidentityProductName + ")";
                ButtonSetup.Enabled = true;
                ButtonScan.Enabled = true;
            }
            else
            {
                MessageBox.Show(scanSourceDataList.ErrorMessage + "\n請確認掃描機是否有開啟");
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
            ScanSelect = new ScanSelect(scanSourceDataList.LszIdentity, scanSourceDataList.DefaultScan)
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
                this.Text = "Scan Device (" + selectScan + ")";
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

        private void Scan_Resize(object sender, EventArgs e)
        {
            if (WindowState == FormWindowState.Minimized)
            {
                this.ShowIcon = false;
                this.Hide();
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

        private void button1_Click(object sender, EventArgs e)
        {
            //scanImageDatas = img(@"D:\example\");
            scanImageDatas.Add(new ScanImageData
            {
                ImageName = "Seal1.jpg",
                Base64Data = ImageDataToBase64("Bmp",img(@"D:\example\Seal1.jpg")),                
            });

            //foreach (var socket in allSockets.ToList())
            //{                
            //    socket.Send(img(@"D:\example\Seal1.jpg"));
            //}
            
            
            foreach (ScanImageData scanImageData in scanImageDatas)
            {
                foreach (var socket in allSockets.ToList())
                {
                    socket.Send(JsonConvert.SerializeObject(scanImageData));
                    //socket.Send(img(@"D:\example\Seal1.jpg"));
                }
            }
            this.WindowState = FormWindowState.Minimized;
            scanImageDatas.Clear();
        }
        public string ImageDataToBase64(string contentType , byte[] imagebytes)
        {            
            string imageBase64String = "data:" + contentType + ";base64," + Convert.ToBase64String(imagebytes, 0, imagebytes.Length);
            return imageBase64String;   
        }
        private byte[] img(string path)
        {
            FileStream fileStream = File.OpenRead(path);
            Image image = Image.FromStream(fileStream);
            MemoryStream memoryStream = new MemoryStream();
            image.Save(memoryStream, ImageFormat.Png);

            byte[] imageBytes = new byte[memoryStream.Length];
            memoryStream.Position = 0;
            memoryStream.Read(imageBytes, 0, (int)memoryStream.Length);
            memoryStream.Close();
            fileStream.Close();
            return imageBytes;
        }
    }
}
