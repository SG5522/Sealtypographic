using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Runtime.InteropServices;
using TWAINWorkingGroup;
using System.IO;
using System.Security.Permissions;

namespace DJTWAINLib
{
    public class DJTWAIN
    {
        /// <summary>
        /// Use if something really bad happens...
        /// </summary>
        private bool m_blExit;
        /// <summary>
        /// If true, then show the driver's window messages while
        /// we're scanning.  Set this in the constructor...
        /// </summary>
        private bool m_blIndicators;

        private int m_iUseBitmap;        
        private int m_iImageCount = 0;
        private TWAIN m_twain;
        private bool m_blXferReadySent;
        private TWAIN.TW_SETUPMEMXFER m_twsetupmemxfer;
        private bool m_blDisableDsSent;
        private IntPtr m_intptrXfer = IntPtr.Zero;
        private IntPtr m_intptrHwnd;
        private object m_From;
        private IntPtr m_intptrImage;
        private int m_iImageBytes = 0;

        //private ScanSourceData scanSourceData; //掃描機驅動資料


        public void TwainSet(object From , IntPtr Handle)
        {
            m_From = From; //來源的from
            // Open the log in our working folder, and say hi...
            TWAINWorkingGroup.Log.Open("TWAINCSScan", ".", 1);
            TWAINWorkingGroup.Log.Info("TWAINCSScan v" + System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString());

            // Init other stuff...
            m_iUseBitmap = 0;
            m_blIndicators = true;
            m_blExit = false;

            // Create our image capture object...
            try
            {
                // Init stuff...
                TWAIN.DeviceEventCallback deviceeventcallback = DeviceEventCallback;
                TWAIN.ScanCallback scancallback = ScanCallbackTrigger;
                TWAIN.RunInUiThreadDelegate runinuithreaddelegate = RunInUiThread;

                // Instantiate TWAIN, and register ourselves...
                m_twain = new TWAIN
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
                m_twain = null;
                m_blExit = true;
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

            //// Init our picture box...
            //InitImage();

            //// Prep for TWAIN events...
            //SetMessageFilter(true);

            //// Init our buttons...
            //SetButtons(EBUTTONSTATE.CLOSED);
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
                twdeviceevent = default(TWAIN.TW_DEVICEEVENT);
                sts = m_twain.DatDeviceevent(TWAIN.DG.CONTROL, TWAIN.MSG.GET, ref twdeviceevent);
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
            //BeginInvoke(new MethodInvoker(delegate { ScanCallbackEventHandler(this, new EventArgs()); }));
            //ScanCallbackEventHandler(this, new EventArgs());
            ScanCallbackEventHandler(m_From, new EventArgs());
            return (TWAIN.STS.SUCCESS);
        }
        private TWAIN.STS ScanCallback(bool a_blClosing)
        {
            TWAIN.STS sts;

            // Scoot...
            if (m_twain == null)
            {
                return (TWAIN.STS.FAILURE);
            }

            // We're superfluous...
            if (m_twain.GetState() <= TWAIN.STATE.S4)
            {
                return (TWAIN.STS.SUCCESS);
            }

            // We're leaving...
            if (a_blClosing)
            {
                return (TWAIN.STS.SUCCESS);
            }


            // Handle DAT_NULL/MSG_XFERREADY...
            if (m_twain.IsMsgXferReady() && !m_blXferReadySent)
            {
                m_blXferReadySent = true;

                // Get the amount of memory needed...
                m_twsetupmemxfer = default(TWAIN.TW_SETUPMEMXFER);
                sts = m_twain.DatSetupmemxfer(TWAIN.DG.CONTROL, TWAIN.MSG.GET, ref m_twsetupmemxfer);
                if ((sts != TWAIN.STS.SUCCESS) || (m_twsetupmemxfer.Preferred == 0))
                {
                    m_blXferReadySent = false;
                    if (!m_blDisableDsSent)
                    {
                        m_blDisableDsSent = true;
                        Rollback(TWAIN.STATE.S4);
                    }
                }

                // Allocate the transfer memory (with a little extra to protect ourselves)...
                m_intptrXfer = Marshal.AllocHGlobal((int)m_twsetupmemxfer.Preferred + 65536);
                if (m_intptrXfer == IntPtr.Zero)
                {
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                }
            }

            // Handle DAT_NULL/MSG_CLOSEDSREQ...
            if (m_twain.IsMsgCloseDsReq() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                //SetButtons(EBUTTONSTATE.OPEN);
            }

            // Handle DAT_NULL/MSG_CLOSEDSOK...
            if (m_twain.IsMsgCloseDsOk() && !m_blDisableDsSent)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                //SetButtons(EBUTTONSTATE.OPEN);
            }

            // This is where the state machine transfers and optionally
            // saves the images to disk (it also displays them).  It'll go back
            // and forth between states 6 and 7 until an error occurs, or until
            // we run out of images...
            if (m_blXferReadySent && !m_blDisableDsSent)
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
            ScanCallbackEventHandler(m_From, new EventArgs());

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

            // Dispatch on the state...
            switch (m_twain.GetState())
            {
                // Not a good state, just scoot...
                default:
                    return;

                // We're on our way out...
                case TWAIN.STATE.S5:
                    m_blDisableDsSent = true;
                    m_twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                    //SetButtons(EBUTTONSTATE.OPEN);
                    return;

                // Memory transfers...
                case TWAIN.STATE.S6:
                case TWAIN.STATE.S7:
                    TWAIN.CsvToImagememxfer(ref twimagememxfer, "0,0,0,0,0,0,0," + ((int)TWAIN.TWMF.APPOWNS | (int)TWAIN.TWMF.POINTER) + "," + m_twsetupmemxfer.Preferred + "," + m_intptrXfer);
                    sts = m_twain.DatImagememxfer(TWAIN.DG.IMAGE, TWAIN.MSG.GET, ref twimagememxfer);
                    break;
            }

            // Handle problems...
            if ((sts != TWAIN.STS.SUCCESS) && (sts != TWAIN.STS.XFERDONE))
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                //SetButtons(EBUTTONSTATE.OPEN);
                return;
            }

            // Allocate or grow the image memory...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_intptrImage = Marshal.AllocHGlobal((int)twimagememxfer.BytesWritten);
            }
            else
            {
                m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(m_iImageBytes + twimagememxfer.BytesWritten));
            }

            // Ruh-roh...
            if (m_intptrImage == IntPtr.Zero)
            {
                m_blDisableDsSent = true;
                Rollback(TWAIN.STATE.S4);
                //SetButtons(EBUTTONSTATE.OPEN);
                return;
            }

            // Copy into the buffer, and bump up our byte tally...
            TWAIN.MemCpy(m_intptrImage + m_iImageBytes, m_intptrXfer, (int)twimagememxfer.BytesWritten);
            m_iImageBytes += (int)twimagememxfer.BytesWritten;

            // If we saw XFERDONE we can save the image, display it,
            // end the transfer, and see if we have more images...
            if (sts == TWAIN.STS.XFERDONE)
            {
                // Bump up our image counter, this always grows for the
                // life of the entire session...
                m_iImageCount += 1;

                // Get the image info...
                sts = m_twain.DatImageinfo(TWAIN.DG.IMAGE, TWAIN.MSG.GET, ref twimageinfo);

                // Add the appropriate header...

                // Bitonal uncompressed...
                if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffBitonalUncompressed tiffbitonaluncompressed;
                    tiffbitonaluncompressed = new TWAIN.TiffBitonalUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonaluncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonaluncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonaluncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonaluncompressed);
                }

                // Bitonal GROUP4...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.BW) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.GROUP4))
                {
                    TWAIN.TiffBitonalG4 tiffbitonalg4;
                    tiffbitonalg4 = new TWAIN.TiffBitonalG4((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffbitonalg4) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffbitonalg4)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffbitonalg4, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffbitonalg4);
                }

                // Gray uncompressed...
                else if (((TWAIN.TWPT)twimageinfo.PixelType == TWAIN.TWPT.GRAY) && ((TWAIN.TWCP)twimageinfo.Compression == TWAIN.TWCP.NONE))
                {
                    TWAIN.TiffGrayscaleUncompressed tiffgrayscaleuncompressed;
                    tiffgrayscaleuncompressed = new TWAIN.TiffGrayscaleUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffgrayscaleuncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffgrayscaleuncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffgrayscaleuncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffgrayscaleuncompressed);
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
                    tiffcoloruncompressed = new TWAIN.TiffColorUncompressed((uint)twimageinfo.ImageWidth, (uint)twimageinfo.ImageLength, (uint)twimageinfo.XResolution.Whole, (uint)m_iImageBytes);
                    m_intptrImage = Marshal.ReAllocHGlobal(m_intptrImage, (IntPtr)(Marshal.SizeOf(tiffcoloruncompressed) + m_iImageBytes));
                    TWAIN.MemMove((IntPtr)((UInt64)m_intptrImage + (UInt64)Marshal.SizeOf(tiffcoloruncompressed)), m_intptrImage, m_iImageBytes);
                    Marshal.StructureToPtr(tiffcoloruncompressed, m_intptrImage, true);
                    m_iImageBytes += (int)Marshal.SizeOf(tiffcoloruncompressed);
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
                    m_blDisableDsSent = true;
                    Rollback(TWAIN.STATE.S4);
                    //SetButtons(EBUTTONSTATE.OPEN);
                    return;
                }

                // Save the image to disk, if we're doing that...
                //if (!string.IsNullOrEmpty(m_formsetup.GetImageFolder()))
                //{
                //    // Create the directory, if needed...
                //    if (!Directory.Exists(m_formsetup.GetImageFolder()))
                //    {
                //        try
                //        {
                //            Directory.CreateDirectory(m_formsetup.GetImageFolder());
                //        }
                //        catch (Exception exception)
                //        {
                //            TWAINWorkingGroup.Log.Error("CreateDirectory failed - " + exception.Message);
                //        }
                //    }

                //    // Write it out...
                //    string szFilename = Path.Combine(m_formsetup.GetImageFolder(), "img" + string.Format("{0:D6}", m_iImageCount));
                //    TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);
                //}
                string szFilename = Path.Combine(Path.GetDirectoryName(@"\"), "img" + string.Format("{0:D6}", m_iImageCount));
                TWAIN.WriteImageFile(szFilename, m_intptrImage, m_iImageBytes, out szFilename);


                // Turn the image into a byte array, and free the original memory...
                //byte[] abImage = new byte[m_iImageBytes];
                //Marshal.Copy(m_intptrImage, abImage, 0, m_iImageBytes);
                //Marshal.FreeHGlobal(m_intptrImage);
                //m_intptrImage = IntPtr.Zero;
                //m_iImageBytes = 0;

                // Turn the byte array into a stream...
                //MemoryStream memorystream = new MemoryStream(abImage);
                //Image image = Image.FromStream(memorystream);
                
                //// Display the image...
                //if (m_iUseBitmap == 0)
                //{
                //    m_iUseBitmap = 1;
                //    LoadImage(ref m_pictureboxImage1, ref m_graphics1, ref m_bitmapGraphic1, bitmap);
                //}
                //else
                //{
                //    m_iUseBitmap = 0;
                //    LoadImage(ref m_pictureboxImage2, ref m_graphics2, ref m_bitmapGraphic2, bitmap);
                //}

                //// Cleanup...
                //bitmap.Dispose();
                //memorystream = null; // disposed by the bitmap
                //abImage = null;

                // End the transfer...
                m_twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);

                // Looks like we're done!
                if (twpendingxfers.Count == 0)
                {
                    m_blDisableDsSent = true;
                    m_twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
                    //SetButtons(EBUTTONSTATE.OPEN);
                    return;
                }
            }
        }
        public void Rollback(TWAIN.STATE a_state)
        {
            TWAIN.TW_PENDINGXFERS twpendingxfers = default(TWAIN.TW_PENDINGXFERS);
            TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);

            // Make sure we have something to work with...
            if (m_twain == null)
            {
                return;
            }

            // Walk the states, we don't care about the status returns.  Basically,
            // these need to work, or we're guaranteed to hang...

            // 7 --> 6
            if ((m_twain.GetState() == TWAIN.STATE.S7) && (a_state < TWAIN.STATE.S7))
            {
                m_twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.ENDXFER, ref twpendingxfers);
            }

            // 6 --> 5
            if ((m_twain.GetState() == TWAIN.STATE.S6) && (a_state < TWAIN.STATE.S6))
            {
                m_twain.DatPendingxfers(TWAIN.DG.CONTROL, TWAIN.MSG.RESET, ref twpendingxfers);
            }

            // 5 --> 4
            if ((m_twain.GetState() == TWAIN.STATE.S5) && (a_state < TWAIN.STATE.S5))
            {
                m_twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.DISABLEDS, ref twuserinterface);
            }

            // 4 --> 3
            if ((m_twain.GetState() == TWAIN.STATE.S4) && (a_state < TWAIN.STATE.S4))
            {
                TWAIN.CsvToIdentity(ref twidentity, m_twain.GetDsIdentity());
                m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDS, ref twidentity);
            }

            // 3 --> 2
            if ((m_twain.GetState() == TWAIN.STATE.S3) && (a_state < TWAIN.STATE.S3))
            {
                m_twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.CLOSEDSM, ref m_intptrHwnd);
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
            RunInUiThread(m_From, a_action);
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
            ScanCallback((m_twain == null) ? true : (m_twain.GetState() <= TWAIN.STATE.S3));
        }

        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="control">the control to run in</param>
        /// <param name="code">the code to run</param>
        static public void RunInUiThread(object a_object, Action a_action)//@思考修改 0712
        {
            //Control control = (Control)a_object;
            //if (control.InvokeRequired)
            //{
            //    control.Invoke(new FormScan.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action });
            //    return;
            //}
            //FormScan.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action }
            a_action();
        }

        /// <summary>
        /// Select list TWAIN driver...
        /// </summary>
        /// <param name="lszIdentity">所有在此機的掃描機(TWAIN)驅動</param>
        /// <param name="ErrorMessage">回傳沒有取得驅動</param>
        public void ScanSourceList(ScanSourceDataList scanSourceDataList ,IntPtr Handle)
        {                
            string szStatus;
            //List<string> lszIdentity = new List<string>();


            TWAIN.STS sts;            
            TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);

            // Get the default driver...
            m_intptrHwnd = Handle;
            sts = m_twain.DatParent(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDSM, ref m_intptrHwnd);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //MessageBox.Show("OPENDSM failed...");
                scanSourceDataList.ErrorMessage = "OPENDSM failed...";
                return;
            }

            // Get the default driver...
            sts = m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETDEFAULT, ref twidentity);
            if (sts == TWAIN.STS.SUCCESS)
            {
                scanSourceDataList.SzDefault = TWAIN.IdentityToCsv(twidentity);
            }

            // Enumerate the drivers...
            for (sts = m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETFIRST, ref twidentity);
                 sts != TWAIN.STS.ENDOFLIST;
                 sts = m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.GETNEXT, ref twidentity))
            {
                //lszIdentity.Add(TWAIN.IdentityToCsv(twidentity));
                scanSourceDataList.LszIdentity.Add(TWAIN.IdentityToCsv(twidentity));

            }
            //scanSourceData.LszIdentity = lszIdentity;


        }

        /// <summary>
        /// open a TWAIN driver...
        /// </summary>
        /// <param name="szIdentity">被選擇的掃描機</param>
        public void ScanSource(string szIdentity , ScanSourceDataList scanSourceDataList)
        {
            string szStatus;
            TWAIN.STS sts;
            TWAIN.TW_CAPABILITY twcapability;
            TWAIN.TW_IDENTITY twidentity = default(TWAIN.TW_IDENTITY);

            //Make it the default, we don't care if this succeeds...
            //twidentity = default(TWAIN.TW_IDENTITY);

            TWAIN.CsvToIdentity(ref twidentity, szIdentity);
            m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twidentity);

            // Open it...
            sts = m_twain.DatIdentity(TWAIN.DG.CONTROL, TWAIN.MSG.OPENDS, ref twidentity);
            if (sts != TWAIN.STS.SUCCESS)
            {
                //MessageBox.Show("Unable to open scanner (it is turned on and plugged in?)");
                scanSourceDataList.ErrorMessage = "Unable to open scanner (it is turned on and plugged in?)";
                m_blExit = true;
                return;
            }



            // We're doing memory transfers...
            szStatus = "";
            twcapability = default(TWAIN.TW_CAPABILITY);
            m_twain.CsvToCapability(ref twcapability, ref szStatus, "ICAP_XFERMECH,TWON_ONEVALUE,TWTY_UINT16,TWSX_MEMORY");
            sts = m_twain.DatCapability(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twcapability);
            if (sts != TWAIN.STS.SUCCESS)
            {
                m_blExit = true;
                return;
            }

            // Decide whether or not to show the driver's window messages...
            szStatus = "";
            twcapability = default(TWAIN.TW_CAPABILITY);
            m_twain.CsvToCapability(ref twcapability, ref szStatus, "CAP_INDICATORS,TWON_ONEVALUE,TWTY_BOOL," + (m_blIndicators ? "TRUE" : "FALSE"));
            sts = m_twain.DatCapability(TWAIN.DG.CONTROL, TWAIN.MSG.SET, ref twcapability);
            if (sts != TWAIN.STS.SUCCESS)
            {
                m_blExit = true;
                return;
            }

        }
        //@開始掃描
        public void StartScan(IntPtr Handle)
        {
            m_iUseBitmap = 0;
            string szTwmemref;
            TWAIN.STS sts;

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
            szTwmemref = "FALSE,FALSE," + Handle;
            // Send the command...
            ClearEvents();
            TWAIN.TW_USERINTERFACE twuserinterface = default(TWAIN.TW_USERINTERFACE);
            m_twain.CsvToUserinterface(ref twuserinterface, szTwmemref);
            sts = m_twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.ENABLEDS, ref twuserinterface);
            //if (sts == TWAIN.STS.SUCCESS)
            //{
            //    SetButtons(EBUTTONSTATE.SCANNING);
            //}
        }
        public void ClearEvents()
        {
            m_blXferReadySent = false;
            m_blDisableDsSent = false;
        }
        //[SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]

        /// We're being closed, clean up nicely...
        public void FormClosing()
        {

            // Get rid of the TWAIN object...
            if (m_twain != null)
            {
                m_twain.Dispose();
                m_twain = null;
            }

            // Bye-bye logging...
            TWAINWorkingGroup.Log.Close();
        }

        //@依CSV的方式排成陣列
        public string[] CSVFormat(string a_szCsv)
        {
            return CSV.Parse(a_szCsv);
        }
        
    }
}
