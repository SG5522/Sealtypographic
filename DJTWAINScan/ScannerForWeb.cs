using Microsoft.Win32;

namespace DJTWAINScan
{
    
    public partial class ScannerForWeb : Form
    {
        private RegistryKey registry = Registry.CurrentUser.OpenSubKey(@"Software\Microsoft\Windows\CurrentVersion\Run", true);
        public ScannerForWeb()
        {
            InitializeComponent();
        }
        private void ScannerForWeb_Load(object sender, EventArgs e)
        {
            registry.SetValue("DJTWAINScan App", Application.ExecutablePath.ToString());
        }

        private void Timer1_Tick(object sender, EventArgs e)
        {
            timer1.Start();
            progressBar1.Increment(1);
            if (progressBar1.Value == 100)
            {
                timer1.Stop();
                Scan scan = new();
                scan.Show();
                this.Hide();
            }
        }
    }
}
