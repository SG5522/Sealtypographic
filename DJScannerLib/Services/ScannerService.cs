using DJScannerLib.Models;
using Microsoft.Extensions.Logging;
using Microsoft.VisualBasic;
using NTwain;
using NTwain.Data;
using SixLabors.ImageSharp;
using SixLabors.ImageSharp.Formats;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Reflection;

namespace DJScannerLib.Services
{
    ///<inheritdoc />
    public class ScannerService : IScannerService
    {
        private readonly ILogger<ScannerService> logger;
        private readonly IScanCallbackService scanCallbackService;

        // interface to TWAIN
        private TwainSession twainSession;

        private IList<string> dataSourceNames = new List<string>();

        private IList<DataSource> dataSources = new List<DataSource>();

        private ReturnCode returnCode = ReturnCode.Failure;

        private bool canCapture = false;
        private bool stopScan = false;
        private IList<string> base64Strings = new List<string>();

        /// <summary>
        /// Initializes a new instance of the <see cref="ScannerService"/> class.
        /// </summary>
        /// <param name="logger">The logger.</param>
        /// <param name="scanCallbackService"></param>
        public ScannerService(ILogger<ScannerService> logger, IScanCallbackService scanCallbackService)
        {
            this.logger = logger;
            this.scanCallbackService = scanCallbackService;
            SetupTwain();
        }

        ///<inheritdoc />
        public void RefreshDrivers()
        {
            ReloadSourceList();
        }

        ///<inheritdoc />
        public AllDriversResult GetAllDrivers()
        {
            AllDriversResult result = new()
            {
                Success = true,
                ErrorMessage = "",
                DriverNames = dataSourceNames
            };

            return result;
        }

        ///<inheritdoc />
        public CurrentDriverResult GetCurrentDriver()
        {
            CurrentDriverResult result = new()
            {
                Success = (returnCode == ReturnCode.Success),
                ErrorMessage = (returnCode == ReturnCode.Success)? "" : "No Current Scanner!",
                Default = twainSession.CurrentSource != null ? twainSession.CurrentSource.Name : ""
            };

            return result;
        }

        ///<inheritdoc />
        public bool SetDriver(string driverName)
        {
            bool result = false;
            if (!string.IsNullOrWhiteSpace(driverName) && twainSession.State <= 4)
            {
                if (twainSession.State == 4)
                {
                    twainSession.CurrentSource.Close();
                    canCapture = false;
                }

                DataSource setDataSource = twainSession.FirstOrDefault(ds => string.Equals(ds.Name, driverName));
                if (setDataSource != null)
                {
                    if(!setDataSource.IsOpen)
                    {
                        returnCode = setDataSource.Open();
                    }
                    else
                    {
                        returnCode = ReturnCode.Success;
                    }
                    
                    if (returnCode == ReturnCode.Success)
                    {
                        canCapture = true;
                        result = true;
                    }
                    else
                    {
                        canCapture = false;
                    }
                }
            }

            return result;
        }

        ///<inheritdoc />
        public void Scan()
        {
            if (canCapture && twainSession.State == 4)
            {
                stopScan = false;

                if (twainSession.CurrentSource.Enable(SourceEnableMode.NoUI, false, IntPtr.Zero) == ReturnCode.Success)
                {
                    canCapture = false;
                }
            }
        }

        ///<inheritdoc />
        public void StopScan()
        {
            stopScan = true;
        }

        ///<inheritdoc />
        public void Setup()
        {
            twainSession.CurrentSource?.Enable(SourceEnableMode.ShowUIOnly, true, IntPtr.Zero);  
        }

        /// <summary>
        /// 初始化、設定TWAIN
        /// </summary>
        private void SetupTwain()
        {
            twainSession = new TwainSession(TWIdentity.CreateFromAssembly(DataGroups.Image, Assembly.GetEntryAssembly()));
            twainSession.StateChanged += (s, e) =>
            {
                logger.LogInformation("State changed to " + twainSession.State + " on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            twainSession.TransferError += (s, e) =>
            {
                logger.LogError("Got xfer error on thread " + Thread.CurrentThread.ManagedThreadId);
            };
            twainSession.DataTransferred += (s, e) =>
            {
                logger.LogInformation("Transferred data event on thread " + Thread.CurrentThread.ManagedThreadId);
                // example on getting ext image info
                //IEnumerable<TWInfo> twainInfos = e.GetExtImageInfo(ExtendedImageInfo.Camera).Where(it => it.ReturnCode == ReturnCode.Success);
                //foreach (TWInfo twainInfo in twainInfos)
                //{
                //    IList<object> values = twainInfo.ReadValues();
                //    logger.LogInformation(string.Format("{0} = {1}", twainInfo.InfoID, values.FirstOrDefault()));
                //    break;
                //}

                // handle image data
                Image image = null;
                if (e.NativeData != IntPtr.Zero)
                {
                    Stream stream = e.GetNativeImageStream();
                    if (stream != null)
                    {
                        image = Image.Load(stream);
                    }
                }

                if(image != null)
                {
                    MemoryStream stream2 = new();
                    image.SaveAsJpeg(stream2);
                    string result = "data:image/jpeg;base64," + Convert.ToBase64String(stream2.ToArray());
                    //scanCallbackService.SendAsync(result, false);
                    base64Strings.Add(result);
                }
            };
            twainSession.SourceDisabled += (s, e) =>
            {
                logger.LogInformation("Source disabled event on thread " + Thread.CurrentThread.ManagedThreadId);
                canCapture = true;
                stopScan = false;
                if (base64Strings.Count > 0)
                {
                    scanCallbackService.SendAsync(string.Join("@", base64Strings), false);
                    base64Strings = new List<string>();
                }
            };
            twainSession.TransferReady += (s, e) =>
            {
                logger.LogInformation("Transferr ready event on thread " + Thread.CurrentThread.ManagedThreadId);
                e.CancelAll = stopScan;
            };

            // either set sync context and don't worry about threads during events,
            // or don't and use control.invoke during the events yourself
            logger.LogInformation("Setup thread = " + Thread.CurrentThread.ManagedThreadId);
            twainSession.SynchronizationContext = SynchronizationContext.Current;
            if (twainSession.State < 3)
            {
                // use this for internal msg loop
                if (twainSession.Open() == ReturnCode.Success)
                {
                    ReloadSourceList();
                }
                // use this to hook into current app loop
                //_twain.Open(new WindowsFormsMessageLoopHook(this.Handle));
            }
        }

        private void ReloadSourceList()
        {
            dataSourceNames = new List<string>();
            dataSources = new List<DataSource>();

            if (twainSession.State >= 3)
            {
                foreach (DataSource dataSource in twainSession)
                {
                    dataSourceNames.Add(dataSource.Name);
                    dataSources.Add(dataSource);
                }

                if(twainSession.DefaultSource != null)
                {
                    if(!twainSession.DefaultSource.IsOpen) 
                    {
                        returnCode = twainSession.DefaultSource.Open();
                    }
                    else
                    {
                        returnCode = ReturnCode.Success;
                    }

                    canCapture = (returnCode == ReturnCode.Success);
                }
            }
        }
    }
}