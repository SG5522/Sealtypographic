using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;
using NTwain;
using NTwain.Data;

namespace DJTAWINLibNET45
{
    public class DJTWAIN45
    {
        private ImageCodecInfo _tiffCodecInfo;
        private TwainSession _twain;        
        private bool _stopScan;
        private bool _loadingCaps;
        private ComboBox comboDPI;


        public string NTwainBitCheck()
        {
            string Text = "";
            if (PlatformInfo.Current.IsApp64Bit)
            {
                Text = Text + " (64bit)";
            }
            else
            {
                Text = Text + " (32bit)";
            }
            return Text;
        }
        //@獲得其他呼叫所得到的combobox
        public void comboxinit(ComboBox comboBox)
        {
            comboDPI = comboBox;
            comboDPI.SelectedIndexChanged += new System.EventHandler(this.comboDPI_SelectedIndexChanged);
        }
        public void ImageCodecCheck()
        {
            //@取得tiff的ImageCodecInfo
            foreach (var enc in ImageCodecInfo.GetImageEncoders())
            {
                if (enc.MimeType == "image/tiff")
                {
                    _tiffCodecInfo = enc;
                    break;
                }
            }
        }

        //@初始化NTWAIN的資料
        public void SetupTwain(PictureBox pictureBox)
        {
            var appId = TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly());
            _twain = new TwainSession(appId);

            //NTWAIN狀態變化提供LOG
            _twain.StateChanged += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("State changed to " + _twain.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            //NATWAIN傳輸錯誤提供LOG
            _twain.TransferError += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Got xfer error on thread " + Thread.CurrentThread.ManagedThreadId);
            };

            //@資料傳輸 (圖片)
            _twain.DataTransferred += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferred data event on thread " + Thread.CurrentThread.ManagedThreadId);

                // example on getting ext image info
                var infos = e.GetExtImageInfo(ExtendedImageInfo.Camera).Where(it => it.ReturnCode == ReturnCode.Success);
                foreach (var it in infos)
                {
                    var values = it.ReadValues();
                    PlatformInfo.Current.Log.Info(string.Format("{0} = {1}", it.InfoID, values.FirstOrDefault()));
                    break;
                }

                // handle image data
                Image img = null;
                if (e.NativeData != IntPtr.Zero)
                {
                    var stream = e.GetNativeImageStream();
                    if (stream != null)
                    {
                        img = Image.FromStream(stream);
                    }
                }
                else if (!string.IsNullOrEmpty(e.FileDataPath))
                {
                    img = new Bitmap(e.FileDataPath);
                }
                if (img != null)
                {
                    //NTwainData.ScanImage = img;
                    if (pictureBox.Image != null)
                    {
                        pictureBox.Image.Dispose();
                        pictureBox.Image = null;
                    }
                    pictureBox.Image = img;
                }
            };

            //取消選擇掃描機 (暫無使用)
            _twain.SourceDisabled += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Source disabled event on thread " + Thread.CurrentThread.ManagedThreadId);
                LoadSourceCaps();
            };

            //@取消掃描
            _twain.TransferReady += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferr ready event on thread " + Thread.CurrentThread.ManagedThreadId);
                e.CancelAll = _stopScan;
            };

            // either set sync context and don't worry about threads during events,
            // or don't and use control.invoke during the events yourself
            PlatformInfo.Current.Log.Info("Setup thread = " + Thread.CurrentThread.ManagedThreadId);
            _twain.SynchronizationContext = SynchronizationContext.Current;
            if (_twain.State < 3)
            {
                // use this for internal msg loop
                _twain.Open();
                // use this to hook into current app loop
                //_twain.Open(new WindowsFormsMessageLoopHook(this.Handle));
            }
        }
        //@回傳NTWAIN 狀態
        public int NTwainState()
        {
            return _twain.State;
        }
        //@讀取有裝在此電腦的掃描機(驅動程式)
        public void ReloadSourceList(ToolStripDropDownButton toolStripDropDownButton)
        {
            foreach (var src in _twain)
            {
                ToolStripMenuItem srcBtn = new ToolStripMenuItem(src.Name);
                srcBtn.Tag = src;
                srcBtn.Click += SourceMenuItem_Click;
                srcBtn.Checked = _twain.CurrentSource != null
                                && _twain.CurrentSource.Name == src.Name;
                toolStripDropDownButton.DropDownItems.Insert(0,srcBtn);
            }            
        }
        public void SourceMenuItem_Click(object sender, EventArgs e)
        {
            if (_twain.State == 4)
            {
                _twain.CurrentSource.Close();
            }
            // do nothing if source is enabled
            if (_twain.State < 4)
            {
                ToolStripMenuItem curBtn = (sender as ToolStripMenuItem);

                if (curBtn != null)
                {
                    var src = curBtn.Tag as DataSource;

                    if (src.Open() == ReturnCode.Success)
                    {
                        curBtn.Checked = true;
                        //btnStartCapture.Enabled = true;
                        LoadSourceCaps();
                    }
                    else
                    {
                        MessageBox.Show("此掃描器未開");
                    }
                }
            }
        }
        public void LoadSourceCaps()
        {
            var src = _twain.CurrentSource;
            _loadingCaps = true;
            //LoadSourceCapData loadSourceCapData = new LoadSourceCapData();
            //var test = src.SupportedCaps;

            //if (loadSourceCapData.GroupDepthEnabled = src.Capabilities.ICapPixelType.IsSupported)
            //{
            //    LoadDepth(src.Capabilities.ICapPixelType);
            //}
            //if (loadSourceCapData.GroupDPIEnabled = src.Capabilities.ICapXResolution.IsSupported && src.Capabilities.ICapYResolution.IsSupported)
            //{
            //    LoadDPI(src.Capabilities.ICapXResolution);
            //}
            LoadDPI(src.Capabilities.ICapXResolution);
            // TODO: find out if this is how duplex works or also needs the other option
            //if (groupDuplex.Enabled = src.Capabilities.CapDuplexEnabled.IsSupported)
            //{
            //    LoadDuplex(src.Capabilities.CapDuplexEnabled);
            //}
            //if (groupSize.Enabled = src.Capabilities.ICapSupportedSizes.IsSupported)
            //{
            //    LoadPaperSize(src.Capabilities.ICapSupportedSizes);
            //}
            //btnAllSettings.Enabled = src.Capabilities.CapEnableDSUIOnly.IsSupported;
            _loadingCaps = false;
        }
        public void LoadDPI(ICapWrapper<TWFix32> cap)
        {
            // only allow dpi of certain values for those source that lists everything
            var list = cap.GetValues().Where(dpi => (dpi % 50) == 0).ToList();
            comboDPI.DataSource = list;
            var cur = cap.GetCurrent();
            if (list.Contains(cur))
            {
                comboDPI.SelectedItem = cur;
            }
        }

        //private void LoadDuplex(ICapWrapper<BoolType> cap)
        //{
        //    ckDuplex.Checked = cap.GetCurrent() == BoolType.True;
        //}




        //private void LoadDepth(ICapWrapper<PixelType> cap)
        //{
        //    var list = cap.GetValues().ToList();
        //    comboDepth.DataSource = list;
        //    var cur = cap.GetCurrent();
        //    if (list.Contains(cur))
        //    {
        //        comboDepth.SelectedItem = cur;
        //    }
        //    var labelTest = cap.GetLabel();
        //    if (!string.IsNullOrEmpty(labelTest))
        //    {
        //        groupDepth.Text = labelTest;
        //    }
        //}        
        //private void LoadPaperSize(ICapWrapper<SupportedSize> cap)
        //{
        //    var list = cap.GetValues().ToList();
        //    comboSize.DataSource = list;
        //    var cur = cap.GetCurrent();
        //    if (list.Contains(cur))
        //    {
        //        comboSize.SelectedItem = cur;
        //    }
        //    var labelTest = cap.GetLabel();
        //    if (!string.IsNullOrEmpty(labelTest))
        //    {
        //        groupSize.Text = labelTest;
        //    }
        //}


        private void comboDPI_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (!_loadingCaps && _twain.State == 4)
            {
                var sel = (TWFix32)comboDPI.SelectedItem;
                _twain.CurrentSource.Capabilities.ICapXResolution.SetValue(sel);
                _twain.CurrentSource.Capabilities.ICapYResolution.SetValue(sel);
            }
        }

        public void StartScan(IntPtr Handle)
        {
            if (_twain.State == 4)
            {
                //_twain.CurrentSource.CapXferCount.Set(4);

                _stopScan = false;                
                if (_twain.CurrentSource.Capabilities.CapUIControllable.IsSupported)//.SupportedCaps.Contains(CapabilityId.CapUIControllable))
                {
                    // hide scanner ui if possible
                    if (_twain.CurrentSource.Enable(SourceEnableMode.NoUI, false, Handle) == ReturnCode.Success)
                    {
  
                    }
                }
                else
                {   // show scan ui
                    if (_twain.CurrentSource.Enable(SourceEnableMode.ShowUI, true, Handle) == ReturnCode.Success)
                    {

                    }
                }

            }
        }

        public void OpenSettings(IntPtr handle)
        {            
            _twain.CurrentSource.Enable(SourceEnableMode.ShowUIOnly, true, handle);
        }
    }
}
