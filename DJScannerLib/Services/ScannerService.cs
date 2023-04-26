using DJLib;
using DJScannerLib.Configs;
using DJScannerLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;

namespace DJScannerLib.Services
{
    public class ScannerService : IScannerService
    {
        private readonly ILogger<ScannerService> logger;

        // 從外部傳入Form的資訊供twain使用
        private IntPtr intPtrHwnd;

        // 掃描影像數量
        private int ImageCount = 0;
        private int imageBytes = 0;

        // interface to TWAIN
        private TWAIN twain;
        private TWAIN.TW_IDENTITY twIdentity = default;
        private TWAIN.TW_SETUPMEMXFER twSetupMemxfer;
        private bool xferReadySent;
        private bool disableDsSent;
        private IntPtr intPtrXfer = IntPtr.Zero;
        private IntPtr intPtrImage = IntPtr.Zero;
        // Setup information...
        private TWAINDriver defaultDriver;

        int cnt = 0;

        public ScannerService(ILogger<ScannerService> logger, IOptions<FormOptions> formOptions)
        {
            intPtrHwnd = formOptions.Value.Handle;
            this.logger = logger;

            //Log.Open("TWAINCSScan", ".", 1);
            //Log.Info("TWAINCSScan v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());

            try
            {
                //// Init stuff...
                TWAIN.DeviceEventCallback deviceeventcallback = DeviceEventCallback;
                TWAIN.ScanCallback scancallback = ScanCallbackTrigger;
                TWAIN.RunInUiThreadDelegate runinuithreaddelegate = RunInUiThread;
                // Init stuff...
                // Instantiate TWAIN, and register ourselves...
                twain = new TWAIN
                (
                    "DJ TWAIN RD Group",
                    "DJ TWAIN Open Source",
                    "DJ TWAIN Scan App",
                    (ushort)TWAIN.TWON_PROTOCOL.MAJOR,
                    (ushort)TWAIN.TWON_PROTOCOL.MINOR,
                    ((uint)TWAIN.DG.APP2 | (uint)TWAIN.DG.CONTROL | (uint)TWAIN.DG.IMAGE),
                    TWAIN.TWCY.TAIWAN,
                    "DJ TWAIN Scan App",
                    TWAIN.TWLG.CHINESE_TAIWAN,
                    2,
                    4,
                    false,
                    false,
                    deviceeventcallback,
                    scancallback,
                    runinuithreaddelegate,
                    (IntPtr)null
                );
            }
            catch (Exception exception)
            {
                this.logger.LogError("exception - " + exception.Message, exception);
                twain = null;
            }
        }

        /// <inheritdoc/>
        public TWAIN GetTWAIN()
        {
            return twain;
        }

        /// <inheritdoc/>
        public DefaultDriverResult GetDefaultDriver()
        {
            DefaultDriverResult result = new();

            if(defaultDriver == null)
            {
                if (twain != null)
                {
                    // Get the default driver
                    TWAIN.STS sts = twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDSM, ref intPtrHwnd);
                    if (sts == TWAIN.STS.SUCCESS)
                    {
                        sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETDEFAULT, ref twIdentity);
                        if (sts == TWAIN.STS.SUCCESS)
                        {
                            result.Success = true;
                            defaultDriver = new()
                            {
                                Identity = TWAIN.IdentityToCsv(twIdentity)
                            };
                            result.Default = defaultDriver;
                        }
                        else
                        {
                            result.ErrorMessage = "Get Default Driver failed.";
                        }
                        twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref intPtrHwnd);
                    }
                    else
                    {
                        result.ErrorMessage = "OPENDSM failed.";
                    }
                }
                else
                {
                    result.ErrorMessage = "TWAIN Initial Error.";
                }
            }
            else
            {
                result.Success = true;
                result.Default = defaultDriver;
            }

            logger.LogInformation("GetDefaultDriver {@Result}", result);
            return result;
        }

        /// <inheritdoc/>
        public GetDriversResult GetAllDrivers()
        {
            GetDriversResult result = new();

            if(twain != null)
            {
                TWAIN.STS sts = twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDSM, ref intPtrHwnd);
                if (sts == TWAIN.STS.SUCCESS)
                {
                    // Enumerate the drivers...列舉驅動程式
                    for (sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETFIRST, ref twIdentity);
                        sts != TWAIN.STS.ENDOFLIST;
                        sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETNEXT, ref twIdentity))
                    {
                        result.Drivers.Add(new TWAINDriver
                        {
                            Identity = TWAIN.IdentityToCsv(twIdentity)
                        });
                    }

                    if (result.Drivers.Count == 0)
                    {
                        result.ErrorMessage = "There are no TWAIN drivers installed on this system...";
                    }
                    else
                    {
                        result.Success = true;
                        result.DriverNames = new List<string>();

                        // Populate our driver list...
                        foreach (TWAINDriver driver in result.Drivers)
                        {
                            result.DriverNames.Add(driver.DriverName);
                        }
                    }
                    twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref intPtrHwnd);
                }
                else
                {
                    result.ErrorMessage = "OPENDSM failed.";
                }
            }
            else
            {
                result.ErrorMessage = "TWAIN Initial Error.";
            }

            logger.LogInformation("GetAllDrivers {@Result}", result);
            return result;
        }

        /// <inheritdoc/>
        public bool SetDriver(string driver)
        {
            bool result = false;

            GetDriversResult getDriversResult = GetAllDrivers();

            if (twain != null)
            {
                TWAIN.STS sts = twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDSM, ref intPtrHwnd);
                TWAINDriver setDriver = new();
                if (sts == TWAIN.STS.SUCCESS)
                {
                    foreach (TWAINDriver twainDriver in getDriversResult.Drivers)
                    {
                        if (twainDriver.Identity.Contains(driver))
                        {
                            setDriver = twainDriver;
                            //driver = driverName;
                            break;
                        }
                    }
                    twIdentity = default;
                    TWAIN.CsvToIdentity(ref twIdentity, setDriver.DriverName);
                    twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twIdentity);

                    // Open it...
                    sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDS, ref twIdentity);

                    if (sts == TWAIN.STS.SUCCESS)
                    {
                        defaultDriver = setDriver;
                        result = true;
                    }
                    twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref intPtrHwnd);
                }
            }
            return result;
        }

        /// <inheritdoc/>
        public void Scan()
        {
            string szTwmemref;

            // Silently start scanning if we detect that customdsdata is supported,
            // otherwise bring up the driver GUI so the user can change settings...
            szTwmemref = "FALSE,FALSE," + intPtrHwnd;
            // Send the command...
            ClearEvents();
            TWAIN.TW_USERINTERFACE twuserinterface = default;
            twain.CsvToUserinterface(ref twuserinterface, szTwmemref);
            twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.ENABLEDS, ref twuserinterface);
        }

        /// <summary>
        /// Clear our event list, and reset our event...
        /// </summary>
        public void ClearEvents()
        {
            xferReadySent = false;
            disableDsSent = false;
        }

        /// <summary>
        /// 對設備事件的callback.  This is where we catch and
        /// report that a device event has been detected.  Obviously,
        /// we're not doing much with it.  A real application would
        /// probably take some kind of action...
        /// </summary>
        /// <returns>TWAIN status</returns>
        private TWAIN.STS DeviceEventCallback()
        {
            TWAIN.STS sts;
            TWAIN.TW_DEVICEEVENT twdeviceevent;

            // Drain the event queue...
            while (true)
            {
                // Try to get an event...
                twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                sts = twain.DatDeviceevent(TWAIN.DG.CONTROL, TWAIN.MSG.GET, ref twdeviceevent);
                if (sts != TWAIN.STS.SUCCESS)
                {
                    break;
                }
            }
            // Return a status, in case we ever need it for anything...
            return (TWAIN.STS.SUCCESS);
        }

        /// <summary>
        /// 掃描的callback.  直接調用支持的TWAIN object
        /// This way we don't have to maintain some kind of a loop
        /// inside of the application, which is the source of most problems that
        /// developers run into.
        /// While it looks scary at first, there's really not a lot going on in
        /// here.  We do some sanity checks, we watch for certain kinds of events,
        /// we support the four methods of transferring images, and we dump out
        /// some meta-data about the transferred image.  However, because it does
        /// look scary I dropped in some region pragmas to break things up...
        /// </summary>
        /// <param name="a_blClosing">We're shutting down</param>
        /// <returns>TWAIN status</returns>
        public TWAIN.STS ScanCallbackTrigger(bool a_blClosing)
        {
            //BeginInvoke(new MethodInvoker(delegate { scannerService.ScanCallbackEventHandler(this, new EventArgs()); }));
            ScanCallbackEventHandler(this, new EventArgs());
            return (TWAIN.STS.SUCCESS);
        }
        
        /// <summary>
        /// Our event handler for the scan callback event.  This will be
        /// called once by ScanCallbackTrigger on receipt of an event
        /// like MSG_XFERREADY, and then will be reissued on every call
        /// into ScanCallback until we're done and get back to state 4.
        /// This helps to make sure we're always running in the context
        /// of FormMain on Windows, which is critical if we want drivers
        /// to work properly.  It also gives a way to break up the calls
        /// so the message pump is still reponsive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void ScanCallbackEventHandler(object sender, EventArgs e)
        {
            ScanCallback((twain == null) || (twain.GetState() <= TWAIN.STATE.S3));
        }

        private TWAIN.STS ScanCallback(bool a_blClosing)
        {
            TWAIN.STS sts;

            // Scoot...
            if (twain == null)
            {
                return (TWAIN.STS.FAILURE);
            }

            // We're superfluous...
            if (twain.GetState() <= TWAIN.STATE.S4)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // We're leaving...
            if (a_blClosing)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // Do this in the right thread, we'll usually be in the
            // right spot, save maybe on the first call...
            //if (this.InvokeRequired)
            //{
            //    return
            //    (
            //        (TWAIN.STS)Invoke
            //        (
            //            (Func<TWAIN.STS>)delegate
            //            {
            //                return (ScanCallback(a_blClosing));
            //            }
            //        )
            //    );
            //}
            // Handle DAT_NULL/MSG_XFERREADY...
            if (twain.IsMsgXferReady() && !xferReadySent)
            {
                xferReadySent = true;

                // Get the amount of memory needed...
                //twSetupMemxfer = default(TWAIN.TW_SETUPMEMXFER);
                twSetupMemxfer = default;
                sts = twain.DatSetupmemxfer(TWAIN.DG.CONTROL, TWAIN.MSG.GET, ref twSetupMemxfer);
                if ((sts != TWAIN.STS.SUCCESS) || (twSetupMemxfer.Preferred == 0))
                {
                    xferReadySent = false;
                    if (!disableDsSent)
                    {
                        disableDsSent = true;
                        Rollback(TWAIN.STATE.S4);
                    }
                }

                // Allocate the transfer memory (with a little extra to protect ourselves)...
                intPtrXfer = Marshal.AllocHGlobal((int)twSetupMemxfer.Preferred + 65536);
                if (intPtrXfer == IntPtr.Zero)
                {
                    disableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                }
            }

            // Handle DAT_NULL/MSG_CLOSEDSREQ...
            if (twain.IsMsgCloseDsReq() && !disableDsSent)
            {
                disableDsSent = true;
                Rollback(TWAIN.STATE.S4);
            }

            // Handle DAT_NULL/MSG_CLOSEDSOK...
            if (twain.IsMsgCloseDsOk() && !disableDsSent)
            {
                disableDsSent = true;
                Rollback(TWAIN.STATE.S4);
            }

            // This is where the state machine transfers and optionally
            // saves the images to disk (it also displays them).  It'll go back
            // and forth between states 6 and 7 until an error occurs, or until
            // we run out of images...
            if (xferReadySent && !disableDsSent)
            {
                CaptureImages();
            }

            // Trigger the next event, this is where things all chain together.
            // We need begininvoke to prevent blockking, so that we don't get
            // backed up into a messy kind of recursion.  We need DoEvents,
            // because if things really start moving fast it's really hard for
            // application events, like button clicks to break through...
            //Application.DoEvents();
            //BeginInvoke(new MethodInvoker(delegate { ScanCallbackEventHandler(this, new EventArgs()); }));
            //ScanCallbackEventHandler(this, new EventArgs());
            ScanCallbackEventHandler(this, new EventArgs());

            // All done...
            return (TWAIN.STS.SUCCESS);
        }
        private void CaptureImages()
        {
            TWAIN.STS sts;
            TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
            TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            //TWAIN.TW_IMAGEINFO twimageinfo = default;
            //TWAIN.TW_IMAGEMEMXFER twimagememxfer = default;
            //TWAIN.TW_PENDINGXFERS twpendingxfers = default;
            //TWAIN.TW_USERINTERFACE twuserinterface = default;

            // Dispatch on the state...
            switch (twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    disableDsSent = true;
                    twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                    return;

                // Memory transfers...
                case TWAIN.STATE.S6:
                case TWAIN.STATE.S7:
                    TWAIN.CsvToImagememxfer(ref twimagememxfer, "0,0,0,0,0,0,0," + ((int)TWAIN.TWMF.APPOWNS | (int)TWAIN.TWMF.POINTER) + "," + twSetupMemxfer.Preferred + "," + intPtrXfer);
                    sts = twain.DatImagememxfer(TWAIN.DG.IMAGE, TWAIN.MSG.GET, ref twimagememxfer);
                    break;
            }

            // Handle problems...
            if ((sts != TWAIN.STS.SUCCESS) && (sts != TWAIN.STS.XFERDONE))
            {
                disableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Allocate or grow the image memory...
            if (intPtrImage == IntPtr.Zero)
            {
                intPtrImage = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
            }
            else
            {
                intPtrImage = Marshal.ReAllocHGlobal(intPtrImage, (IntPtr)(imageBytes + twimagememxfer.BytesWritten));
            }

            // Ruh-roh...
            if (intPtrImage == IntPtr.Zero)
            {
                disableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                return;
            }

            // Copy into the buffer, and bump up our byte tally...
            TWAIN.MemCpy(intPtrImage + imageBytes, intPtrXfer, (int)twimagememxfer.BytesWritten);
            imageBytes += (int)twimagememxfer.BytesWritten;

            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == TWAIN.STS.XFERDONE)
            {
                // Bump up our image counter, this always grows for the
                // life of the entire session...
                ImageCount += 1;

                // Get the image info...
                //sts = twain.DatImageinfo(TWAIN.DG.IMAGE, TWAIN.MSG.GET, ref twimageinfo);
                twain.DatImageinfo(TWAIN.DG.IMAGE, TWAIN.MSG.GET, ref twimageinfo);
                // Add the appropriate header...

                // Bitonal uncompressed...
                if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffBitonalUncompressed tiffbitonaluncompressed;
                    tiffbitonaluncompressed = new TWAIN.TiffBitonalUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)imageBytes);
                    intPtrImage = Marshal.ReAllocHGlobal(intPtrImage, (IntPtr)(Marshal.SizeOf(tiffbitonaluncompressed) + imageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)intPtrImage + (UInt64)Marshal.SizeOf(tiffbitonaluncompressed)), intPtrImage, imageBytes);
                    Marshal.StructureToPtr(tiffbitonaluncompressed, intPtrImage, true);
                    imageBytes += (int)Marshal.SizeOf(tiffbitonaluncompressed);
                }

                // Bitonal GROUP4...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.GROUP4))
                {
                    TWAIN.TiffBitonalG4 tiffbitonalg4;
                    tiffbitonalg4 = new TWAIN.TiffBitonalG4((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)imageBytes);
                    intPtrImage = Marshal.ReAllocHGlobal(intPtrImage, (IntPtr)(Marshal.SizeOf(tiffbitonalg4) + imageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)intPtrImage + (UInt64)Marshal.SizeOf(tiffbitonalg4)), intPtrImage, imageBytes);
                    Marshal.StructureToPtr(tiffbitonalg4, intPtrImage, true);
                    imageBytes += (int)Marshal.SizeOf(tiffbitonalg4);
                }

                // Gray uncompressed...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffGrayscaleUncompressed tiffgrayscaleuncompressed;
                    tiffgrayscaleuncompressed = new TWAIN.TiffGrayscaleUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)imageBytes);
                    intPtrImage = Marshal.ReAllocHGlobal(intPtrImage, (IntPtr)(Marshal.SizeOf(tiffgrayscaleuncompressed) + imageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)intPtrImage + (UInt64)Marshal.SizeOf(tiffgrayscaleuncompressed)), intPtrImage, imageBytes);
                    Marshal.StructureToPtr(tiffgrayscaleuncompressed, intPtrImage, true);
                    imageBytes += (int)Marshal.SizeOf(tiffgrayscaleuncompressed);
                }

                // Gray JPEG...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.JPEG))
                {
                    // No work to be done, we'll output JPEG...
                }

                // RGB uncompressed...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.RGB) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffColorUncompressed tiffcoloruncompressed;
                    tiffcoloruncompressed = new TWAIN.TiffColorUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)imageBytes);
                    intPtrImage = Marshal.ReAllocHGlobal(intPtrImage, (IntPtr)(Marshal.SizeOf(tiffcoloruncompressed) + imageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)intPtrImage + (UInt64)Marshal.SizeOf(tiffcoloruncompressed)), intPtrImage, imageBytes);
                    Marshal.StructureToPtr(tiffcoloruncompressed, intPtrImage, true);
                    imageBytes += (int)Marshal.SizeOf(tiffcoloruncompressed);
                }

                // RGB JPEG...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.RGB) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.JPEG))
                {
                    // No work to be done, we'll output JPEG...
                }

                // Oh well...
                else
                {
                    TWAINWorkingGroup.Log.Error("unsupported format <" + twimageinfo.PixelType + "," + twimageinfo.Compression + ">");
                    disableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    return;
                }
                cnt++;
                //@轉成byte值存到scanImageDatas
                byte[] abImage = new byte[imageBytes];
                Marshal.Copy(intPtrImage, abImage, 0, imageBytes);
                SixLabors.ImageSharp.Image img = SixLabors.ImageSharp.Image.Load(abImage, out IImageFormat format);
                img.SaveAsJpeg($"F:\\{cnt}.jpg");
                string base64 = ImageSharpUtil.ImageToBase64(img, format);

                //@記憶圖片的參數初始化
                Marshal.FreeHGlobal(intPtrImage);
                intPtrImage = IntPtr.Zero;
                imageBytes = 0;
                // Turn the byte array into a stream...
                MemoryStream memorystream = new MemoryStream(abImage);

                // End the transfer...
                twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    disableDsSent = true;
                    twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                    return;
                }
            }
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="code">the code to run</param>
        private void RunInUiThread(Action a_action)
        {
            a_action();
        }

        /// <inheritdoc/>
        public void Rollback(TWAIN.STATE twainState)
        {
            TWAIN.TW_PENDINGXFERS twpendingxfers = default;
            TWAIN.TW_USERINTERFACE twuserinterface = default;
            TWAIN.TW_IDENTITY twidentity = default;

            // Make sure we have something to work with...
            if (twain != null)
            {
                // Walk the states, we don't care about the status returns.  Basically,
                // these need to work, or we're guaranteed to hang...

                // 7 --> 6
                if ((twain.GetState() == TWAIN.STATE.S7) && (twainState < TWAIN.STATE.S7))
                {
                    twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);
                }

                // 6 --> 5
                if ((twain.GetState() == TWAIN.STATE.S6) && (twainState < TWAIN.STATE.S6))
                {
                    twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.RESET, ref twpendingxfers);
                }

                // 5 --> 4
                if ((twain.GetState() == TWAIN.STATE.S5) && (twainState < TWAIN.STATE.S5))
                {
                    twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                }

                // 4 --> 3
                if ((twain.GetState() == TWAIN.STATE.S4) && (twainState < TWAIN.STATE.S4))
                {
                    TWAIN.CsvToIdentity(ref twidentity, twain.GetDsIdentity());
                    twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDS, ref twidentity);
                }

                // 3 --> 2
                if ((twain.GetState() == TWAIN.STATE.S3) && (twainState < TWAIN.STATE.S3))
                {
                    twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref intPtrHwnd);
                }
            }
        }
    }
}