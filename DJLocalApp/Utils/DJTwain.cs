using NTwain.Data;
using NTwain;
using System.Reflection;

namespace DJLocalApp.Utils
{
    /// <summary>
    /// 
    /// </summary>
    public class DJTwain
    {
        public TwainSession twainSession { get; set; }

        private bool stopScan;

        /// <summary>
        /// 
        /// </summary>
        public void SetupTwain()
        {
            twainSession = new TwainSession(TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly()));
            twainSession.StateChanged += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("State changed to " + twainSession.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            twainSession.TransferError += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Got xfer error on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            twainSession.DataTransferred += (s, e) =>
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
                    //this.BeginInvoke(new Action(() =>
                    //{
                    //    if (pictureBox1.Image != null)
                    //    {
                    //        pictureBox1.Image.Dispose();
                    //        pictureBox1.Image = null;
                    //    }
                    //    pictureBox1.Image = img;
                    //}));
                }
            };
            twainSession.SourceDisabled += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Source disabled event on thread " + Thread.CurrentThread.ManagedThreadId);
                //this.BeginInvoke(new Action(() =>
                //{
                //    btnStopScan.Enabled = false;
                //    btnStartCapture.Enabled = true;
                //    panelOptions.Enabled = true;
                //    LoadSourceCaps();
                //}));
            };
            twainSession.TransferReady += (s, e) =>
            {
                PlatformInfo.Current.Log.Info("Transferr ready event on thread " + Thread.CurrentThread.ManagedThreadId);
                e.CancelAll = stopScan;
            };

            // either set sync context and don't worry about threads during events,
            // or don't and use control.invoke during the events yourself
            PlatformInfo.Current.Log.Info("Setup thread = " + Thread.CurrentThread.ManagedThreadId);
            twainSession.SynchronizationContext = SynchronizationContext.Current;
            //if (twainSession.State < 3)
            //{
                // use this for internal msg loop
                //twainSession.Open();
                // use this to hook into current app loop
                //_twain.Open(new WindowsFormsMessageLoopHook(this.Handle));
            //}
        }
    }
}
