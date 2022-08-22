using System;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;
using System.Security.Permissions;

namespace DJTWAINLib
{
    public class DJTWAIN
    {
        /// <summary>
        /// Use if something really bad happens...
        /// </summary>
        //private bool blExit;

        /// <summary>
        /// If true, then show the driver's window messages while
        /// we're scanning.  Set this in the constructor...
        /// </summary>
        private bool blIndicators;
     
        private int ImageCount = 0;
        private int imageBytes = 0;

        private string saveImagePath = "";
        private string saveImageName = "";
        private string saveImagetype = "";
        
        private bool xferReadySent;
        private bool disableDsSent;

        private TWAIN twain;
        private TWAIN.TW_SETUPMEMXFER twSetupMemxfer;

        private IntPtr intPtrXfer = IntPtr.Zero;
        private IntPtr intPtrHwnd;        
        private IntPtr intPtrImage;

        private object from;


        //private ScanSourceData scanSourceData; //掃描機驅動資料

        public void TwainSet(object From , IntPtr Handle)
        {
            from = From; //來源的from
            // Open the log in our working folder, and say hi...
            TWAINWorkingGroup.Log.Open("TWAINCSScan", ".", 1);
            TWAINWorkingGroup.Log.Info("TWAINCSScan v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());

            // Init other stuff...
            blIndicators = true;
            //blExit = false;
            
            // Create our image capture object...
            try
            {
                // Init stuff...
                TWAIN.DeviceEventCallback deviceeventcallback = DeviceEventCallback;
                TWAIN.ScanCallback scancallback = ScanCallbackTrigger;
                TWAIN.RunInUiThreadDelegate runinuithreaddelegate = RunInUiThread;

                // Instantiate TWAIN, and register ourselves...
                twain = new TWAIN
                (
                    "TWAIN Working Group",
                    "TWAIN Open Source",
                    "TWAIN CS Scan App",
                    (ushort)TWAIN.TWON_PROTOCOL.MAJOR,
                    (ushort)TWAIN.TWON_PROTOCOL.MINOR,
                    ((uint)TWAIN.DG.APP2 | (uint)TWAIN.DG.CONTROL | (uint)TWAIN.DG.IMAGE),
                    TWAIN.TWCY.TAIWAN,
                    "TWAIN CS Scan App",
                    TWAIN.TWLG.CHINESE_TAIWAN,
                    2,
                    4,
                    false,
                    false,
                    deviceeventcallback,
                    scancallback,
                    runinuithreaddelegate,
                    Handle
                );
            }
            catch (Exception exception)
            {
                TWAINWorkingGroup.Log.Error("exception - " + exception.Message);
                twain = null;
                //blExit = true;
                //MessageBox.Show
                //(
                //    "Unable to start, the most likely reason is that the TWAIN\n" +
                //    "Data Source Manager is not installed on your system.\n\n" +
                //    "An internet search for 'TWAIN DSM' will locate it and once\n" +
                //    "installed, you should be able to proceed.\n\n" +
                //    "You can also try the following link:\n" +
                //    "http://sourceforge.net/projects/twain-dsm/",
                //    "Error Starting TWAIN CS Scan"
                //);
                return;
            }
        }



        /// <summary>
        /// Our callback for device events.  This is where we catch and
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
                //twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                twdeviceevent = default;
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
        /// Our scanning callback function.  We appeal directly to the supporting
        /// TWAIN object.  This way we don't have to maintain some kind of a loop
        /// inside of the application, which is the source of most problems that
        /// developers run into.
        /// 
        /// While it looks scary at first, there's really not a lot going on in
        /// here.  We do some sanity checks, we watch for certain kinds of events,
        /// we support the four methods of transferring images, and we dump out
        /// some meta-data about the transferred image.  However, because it does
        /// look scary I dropped in some region pragmas to break things up...
        /// </summary>
        /// <param name="a_blClosing">We're shutting down</param>
        /// <returns>TWAIN status</returns>
        private TWAIN.STS ScanCallbackTrigger(bool a_blClosing)
        {
            ScanCallbackEventHandler(from, new EventArgs());
            return (TWAIN.STS.SUCCESS);
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
                //SetButtons(EBUTTONSTATE.OPEN);
            }

            // Handle DAT_NULL/MSG_CLOSEDSOK...
            if (twain.IsMsgCloseDsOk() && !disableDsSent)
            {
                disableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                //SetButtons(EBUTTONSTATE.OPEN);
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
            ScanCallbackEventHandler(from, new EventArgs());

            // All done...
            return (TWAIN.STS.SUCCESS);
        }
        private void CaptureImages()
        {
            TWAIN.STS sts;
            //TWAIN.TW_IMAGEINFO twimageinfo = default(TWAIN.TW_IMAGEINFO);
            //TWAIN.TW_IMAGEMEMXFER twimagememxfer = default(TWAIN.TW_IMAGEMEMXFER);
            //TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
            //TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            TWAIN.TW_IMAGEINFO twimageinfo = default;
            TWAIN.TW_IMAGEMEMXFER twimagememxfer = default;
            TWAIN.TW_PENDINGXFERS twpendingxfers = default;
            TWAIN.TW_USERINTERFACE twuserinterface = default;

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
                    //SetButtons(EBUTTONSTATE.OPEN);
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
                //SetButtons(EBUTTONSTATE.OPEN);
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
                //SetButtons(EBUTTONSTATE.OPEN);
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
                    //SetButtons(EBUTTONSTATE.OPEN);
                    return;
                }

                //string Filename = Path.Combine(Path.GetDirectoryName(@".\"), "img" + string.Format("{0:D6}", ImageCount));
                //TWAIN.WriteImageFile(Filename + ".bmp", intPtrImage, imageBytes, out Filename);
                //掃描完存成圖檔
                if(ImageCount % 2 == 1)
                {
                    string fullNamePath = saveImagePath + @"\" + saveImageName + (ImageCount /2 + 1 ) + "F" + saveImagetype;
                    TWAIN.WriteImageFile(fullNamePath, intPtrImage, imageBytes, out fullNamePath);
                }
                else
                {
                    string fullNamePath = saveImagePath + @"\" + saveImageName + (ImageCount /2) + "R" + saveImagetype;
                    TWAIN.WriteImageFile(fullNamePath, intPtrImage, imageBytes, out fullNamePath);
                    //ImageCount = 0;
                }

                //@記憶圖片的參數初始化
                intPtrImage = IntPtr.Zero;
                imageBytes = 0;

                // End the transfer...
                twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    disableDsSent = true;
                    twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                    //SetButtons(EBUTTONSTATE.OPEN);
                    return;
                }
            }
        }
        public void SuorceSelectCancel()
        {
            Rollback(TWAIN.STATE.S2);
        }
        public void Rollback(TWAIN.STATE a_state)
        {
            //TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
            //TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            //TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);

            TWAIN.TW_PENDINGXFERS twpendingxfers = default;
            TWAIN.TW_USERINTERFACE twuserinterface = default;
            TWAIN.TW_IDENTITY twidentity = default;

            // Make sure we have something to work with...
            if (twain == null)
            {
                return;
            }

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if ((twain.GetState() == TWAIN.STATE.S7) && (a_state < TWAIN.STATE.S7))
            {
                twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);
            }

            // 6 --> 5
            if ((twain.GetState() == TWAIN.STATE.S6) && (a_state < TWAIN.STATE.S6))
            {
                twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.RESET, ref twpendingxfers);
            }

            // 5 --> 4
            if ((twain.GetState() == TWAIN.STATE.S5) && (a_state < TWAIN.STATE.S5))
            {
                twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
            }

            // 4 --> 3
            if ((twain.GetState() == TWAIN.STATE.S4) && (a_state < TWAIN.STATE.S4))
            {
                TWAIN.CsvToIdentity(ref twidentity, twain.GetDsIdentity());
                twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDS, ref twidentity);
            }

            // 3 --> 2
            if ((twain.GetState() == TWAIN.STATE.S3) && (a_state < TWAIN.STATE.S3))
            {
                twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref intPtrHwnd);
            }
        }
        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="code">the code to run</param>
        private void RunInUiThread(Action a_action)
        {
            //RunInUiThread(this, a_action);
            //RunInUiThread(from, a_action);
            a_action();
        }

        /// <summary>
        /// Our event handler for the scan callback event.  This will be
        /// called once by ScanCallbackTrigger on receipt of an event
        /// like MSG_XFERREADY, and then will be reissued on every call
        /// into ScanCallback until we're done and get back to state 4.
        ///  
        /// This helps to make sure we're always running in the context
        /// of FormMain on Windows, which is critical if we want drivers
        /// to work properly.  It also gives a way to break up the calls
        /// so the message pump is still reponsive.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ScanCallbackEventHandler(object sender, EventArgs e)
        {
            //ScanCallback((twain == null) ? true : (twain.GetState() <= TWAIN.STATE.S3));
            ScanCallback((twain == null) || (twain.GetState() <= TWAIN.STATE.S3));
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="control">the control to run in</param>
        /// <param name="code">the code to run</param>
        //public void RunInUiThread(Object a_object, Action a_action)//@思考修改 0712
        //{
        //    //Control control = (Control)a_object;
        //    //if (control.InvokeRequired)
        //    //{
        //    //    control.Invoke(new FormScan.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action });
        //    //    return;
        //    //}
        //    //RunInUiThreadDelegate runInUiThreadDelegate = new RunInUiThreadDelegate(RunInUiThread);
        //    //runInUiThreadDelegate.Invoke(a_object, a_action);
        //    a_action();
        //}

        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_object">object (really a control)</param>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Object a_object, Action a_action);

       
        /// <summary>
        /// Select list TWAIN driver...
        /// </summary>
        /// <param name="lszIdentity">所有在此機的掃描機(TWAIN)驅動</param>
        /// <param name="ErrorMessage">回傳沒有取得驅動</param>
        public void ScanSourceList(ScanSourceDataList scanSourceDataList ,IntPtr Handle)
        {
            //string szStatus;
            //List<string> lszIdentity = new List<string>();            

            TWAIN.STS sts;
            //TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);
            TWAIN.TW_IDENTITY twidentity = default;

            // Get the default driver...
            intPtrHwnd = Handle;
            //intPtrHwnd = IntPtr.Zero;
            sts = twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDSM, ref intPtrHwnd);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //MessageBox.Show("OPENDSM failed...");
                scanSourceDataList.ErrorMessage = "OPENDSM failed...";
                return;
            }

            // Get the default driver...
            sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETDEFAULT, ref twidentity);
            if (sts == TWAIN.STS.SUCCESS)
            {
                scanSourceDataList.SzDefault = TWAIN.IdentityToCsv(twidentity);
            }

            // Enumerate the drivers...
            for (sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETFIRST, ref twidentity);
                 sts != TWAIN.STS.ENDOFLIST;
                 sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETNEXT, ref twidentity))
            {
                //lszIdentity.Add(TWAIN.IdentityToCsv(twidentity));
                scanSourceDataList.LszIdentity.Add(TWAIN.IdentityToCsv(twidentity));

            }
            //scanSourceData.LszIdentity = lszIdentity;


        }

        /// <summary>
        /// open a TWAIN driver...
        /// </summary>
        /// <param name="selectScan">被選擇的掃描機</param>
        public void ScanSource(string selectScan , ScanSourceDataList scanSourceDataList)
        {
            string status;
            TWAIN.STS sts;
            TWAIN.TW_CAPABILITY twCapability;
            //TWAIN.TW_IDENTITY twIdentity = default(TWAIN.TW_IDENTITY);
            TWAIN.TW_IDENTITY twIdentity = default;

            //Make it the default, we don't care if this succeeds...
            //twidentity = default(TWAIN.TW_IDENTITY);

            TWAIN.CsvToIdentity(ref twIdentity, selectScan);
            twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twIdentity);

            // Open it...
            sts = twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDS, ref twIdentity);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //MessageBox.Show("Unable to open scanner (it is turned on and plugged in?)");
                scanSourceDataList.ErrorMessage = "Unable to open scanner (it is turned on and plugged in?)";
                //blExit = true;
                return;
            }



            // We're doing memory transfers...
            status = "";
            //twCapability = default(TWAIN.TW_CAPABILITY);
            twCapability = default;
            twain.CsvToCapability(ref twCapability, ref status, "ICAP_XFERMECH,TWON_ONEVALUE,TWTY_UINT16,TWSX_MEMORY");
            sts = twain.DatCapability(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twCapability);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //blExit = true;
                return;
            }

            // Decide whether or not to show the driver's window messages...
            status = "";
            //twCapability = default(TWAIN.TW_CAPABILITY);
            twCapability = default;
            twain.CsvToCapability(ref twCapability, ref status, "CAP_INDICATORS,TWON_ONEVALUE,TWTY_BOOL," + (blIndicators ? "TRUE" : "FALSE"));
            sts = twain.DatCapability(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twCapability);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //blExit = true;
                return;
            }

        }

        /// <summary>
        /// 掃描動作
        /// </summary>
        /// <param name="Handle"></param>
        /// <param name="savePath"></param>
        /// <param name="saveName"></param>
        /// <param name="saveType"></param>
        public void StartScan(IntPtr Handle, string savePath, string saveName, string saveType)
        {
            ImageCount = 0; //重置掃描張數
            saveImagePath = savePath;
            saveImageName = saveName;
            saveImagetype = saveType;
            string twmemRef;

            //TWAIN.STS sts;

            // Silently start scanning if we detect that customdsdata is supported,
            // otherwise bring up the driver GUI so the user can change settings...
            //if (m_formsetup.IsCustomDsDataSupported())
            //{
            //    szTwmemref = "FALSE,FALSE," + this.Handle;
            //}
            //else
            //{
            //    szTwmemref = "TRUE,FALSE," + this.Handle;
            //}

            twmemRef = "FALSE,FALSE," + Handle;
            // Send the command...
            ClearEvents();

            //TWAIN.TW_USERINTERFACE twUserInterface = default(TWAIN.TW_USERINTERFACE);
            TWAIN.TW_USERINTERFACE twUserInterface = default;
            twain.CsvToUserinterface(ref twUserInterface, twmemRef);
            twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.ENABLEDS, ref twUserInterface);
            //if (sts == TWAIN.STS.SUCCESS)
            //{
            //    SetButtons(EBUTTONSTATE.SCANNING);
            //}
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
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY)
        /// </summary>
        /// <param name="intPtrHwnd"></param>
        /// <param name="iMsg"></param>
        /// <param name="intPtrWparam"></param>
        /// <param name="intPtrLparam"></param>
        /// <returns>Result of the processing</returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(IntPtr intPtrHwnd, int iMsg, IntPtr intPtrWparam, IntPtr intPtrLparam)
        {
            if (twain != null)
            {                
                return (twain.PreFilterMessage(intPtrHwnd, iMsg, intPtrWparam, intPtrLparam));
            }
            return (true);
        }

        /// We're being closed, clean up nicely...
        public void FormClosing()   
        {

            // Get rid of the TWAIN object...
            if (twain != null)
            {                
                twain.Dispose();
                twain = null;
            }

            // Bye-bye logging...
            TWAINWorkingGroup.Log.Close();
        }

        //@依CSV的方式排成陣列
        public ScanSourceData SourceData(string a_szCsv)
        {
            string[] strings = CSV.Parse(a_szCsv);
            ScanSourceData scanSourceData = new ScanSourceData()
            {
                TwidentityId = strings[0],
                TwidentityMajorNum = strings[1],
                TwidentityMinorNum = strings[2],
                TwidentityLanguage = strings[3],
                TwidentityCountry = strings[4],
                TwidentityInfo = strings[5],
                TwidentityProtocolMajor = strings[6],
                TwidentityProtocolMinor = strings[7],
                TwidentitySupportedGroups = strings[8],
                TwidentityManufacturer = strings[9],
                TwidentityProductFamily = strings[10],
                TwidentityProductName = strings[11]
            };
            return scanSourceData;
        }
        /// <summary>
        /// Close the currently open TWAIN driver...
        /// </summary>
        public void CloseTWAINDriver()
        {            
            Rollback(TWAIN.STATE.S2);
        }
        public void Setup(IntPtr intPtr)
        {
            ClearEvents();

            TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            twain.CsvToUserinterface(ref twuserinterface, "TRUE,FALSE," + intPtr);
            twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.ENABLEDSUIONLY, ref twuserinterface);
        }
    }
}
