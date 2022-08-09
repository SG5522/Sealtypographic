取章排版系統

=====================================================

注意事項

(1). Spire.Pdf.dll .NET Standard 2.0 是使用官網下載的dll

> 因為Nuget上沒有此版本只有.NET 6。
> 
> 另外也保留.NET6的環境 專案名稱DJSpireNET6



(2). 目前天創元件(使用版本3.0)因為無法多人運作以及64位元下執行

需要對天創提出以下需求:

1.需要32位元與64位元版本

2.關於函式
>  
>         public static extern int seal_build
>             (out long milSys, int color, string fileinput, string xmloutput,
>             int dpi, string libprefix, int libindex, int left, int top, int width, int height, int binarize,
>             int rotate, string path, double calx = 1.0, double caly = 1.0);
>
> 這支我們了解他是指定圖檔並進行印鑑分離，
>
> 但是使用的過程中我們發現，
> 
> 他必須要透這個這支函式從指定圖檔轉成bmp檔後，
> 
> 之後在對此bmp進行印鑑分離，
> 
> 這樣會產生問題，
> 
> 如果使用其他程式呼叫這個DLL，
> 
> 會無法掌握他的bmp生成路徑，
> 
> 造成抓不到圖檔路徑，
> 
> 得強制將該dll放在跟程式一樣的路徑，
> 
> 希望這支函式產生的bmp檔能讓我們指定位置，或是放在系統temp資料夾。

 3.分離產生的圖檔要可以存放在指定路徑

 4.由於第2項與第3項的操作

> 都是存成圖檔(而且不能指定存檔位置)，
>  我們目前要做到網頁處理，
> 
>  一定會需要多人使用，如果依照現在做法，
> 
>  會有圖檔互相覆蓋的問題，
> 
>  目前我這邊想到的是存到memory讓多人使用，
> 
> 除了這點還有其他做法嗎?




 5.可以指定特定範圍進行分離

 6.產生的分離印鑑要有圖像旋轉功能(圖像自動轉正)。

 7.天創元件內的config問題

> 天創元件內的config資料夾在使用libTC_sealinterface.dll這支前必須得先給他正確的程式路徑，
> 
> 而且config一定得放在該資料夾的前一個目錄。
> 
> 例: dll 都放在 myapp\include  那我就得放在 myapp\config
> 
> 這樣才不會得不到usb key，這段能否改成相對路徑，或是有其他方法能指定?
> 
> 以下是我使用該dll指定路徑的方式
> 
> [DllImport("kernel32.dll", CharSet = CharSet.Auto, SetLastError = true)]
>
>         static extern bool SetDllDirectory(string lpPathName);
> 
> 宣告程式路徑 (所有的天創dll都放到指定目錄下的\include)
> 
> SetDllDirectory(Directory.GetCurrentDirectory() + @"\include");
> 
> 指定使用的dll元件
> 
> private const string SealDLL = @"libTC_sealinterface.dll";
>
> 使用的函式
> 
> [DllImport(SealDLL, CallingConvention = CallingConvention.Cdecl)]
>
>         public static extern int seal_build
>             (out long milSys, int color, string fileinput, string xmloutput,
>             int dpi, string libprefix, int libindex, int left, int top, int width, int height, int binarize,
>             int rotate, string path, double calx = 1.0, double caly = 1.0);
