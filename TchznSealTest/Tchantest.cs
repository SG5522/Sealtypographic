using TchznSeal;

namespace TchznSealTest
{
    public partial class Tchantest : Form
    {
        private readonly AutoSealSplit autoSealSplit = new();
        public Tchantest()
        {
            InitializeComponent();
        }

        private void buttonOpenimage_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog();
            dialog.Multiselect = false;//該值確定是否可以選擇多個檔案
            dialog.Title = "請選擇資料夾";
            dialog.Filter = "所有檔案(*.*)|*.*";
            if (dialog.ShowDialog() == System.Windows.Forms.DialogResult.OK)
            {
                string filepath = dialog.FileName;
                autoSealSplit.SplitSeal(filepath, @"C:\temp\", "Test", "R");
            }
        }
    }
}
