using DJLocalAPI;
using DJScannerLib.Services;
using ScannerLib.Services;

//@net core net5 net6 對unicode 以外文字的支援不足必須加這段
System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);
ApplicationConfiguration.Initialize();
Application.Run(new FrmDJLLocalAPI(args));