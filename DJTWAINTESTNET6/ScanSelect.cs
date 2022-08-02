using DJLib;

namespace DJTWAINTESTNET6
{
    public partial class ScanSelect : Form
    {
        private readonly DJTWAIN dJTWAIN = new();
        private string selected = ""; //回傳選擇的裝置

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="identitys">list of scanners to show</param>
        /// <param name="defaultSource">the default selection</param>
        public ScanSelect(List<string> identitys, string defaultSource)
        {
            //string[] identityArray;
            //string[] defaultArray;            
            // Init stuff...
            InitializeComponent();
            ScanSourceData defaultData = dJTWAIN.SourceData(defaultSource);
            
            // Explode the default...
            //defaultArray = dJTWAIN.CSVFormat(defaultSource);

            // Suspend updating...
            ListBoxSourceSelect.BeginUpdate();

            // Populate our driver list...
            foreach (string sz in identitys)
            {
                //identityArray = dJTWAIN.CSVFormat(sz);
                //ListBoxSourceSelect.Items.Add(identityArray[11].ToString());
                ScanSourceData identityData = dJTWAIN.SourceData(sz);
                ListBoxSourceSelect.Items.Add(identityData.TwidentityProductName);
            }

            // Select the default...
            //ListBoxSourceSelect.SelectedIndex = ListBoxSourceSelect.FindStringExact(defaultArray[11]);
            ListBoxSourceSelect.SelectedIndex = ListBoxSourceSelect.FindStringExact(defaultData.TwidentityProductName);

            // Resume updating...
            ListBoxSourceSelect.EndUpdate();
        }
        public string GetSelectedDriver()
        {
            return selected;
        }

        private void ButtonSelectScanSoucre_Click(object sender, EventArgs e)
        {
            selected = (string)ListBoxSourceSelect.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }

        private void ListBoxSourceSelect_DoubleClick(object sender, EventArgs e)
        {
            selected = (string)ListBoxSourceSelect.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }
    }
}
