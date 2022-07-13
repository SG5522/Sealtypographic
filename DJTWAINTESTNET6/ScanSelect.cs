using System;
using System.Collections.Generic;
using System.Windows.Forms;
using DJTWAINLib;

namespace DJTWAINTESTNET6
{
    public partial class ScanSelect : Form
    {
        private readonly DJTWAIN dJTWAIN = new();
        private string m_szSelected = ""; //回傳選擇的裝置

        /// <summary>
        /// Our constructor...
        /// </summary>
        /// <param name="a_lszIdentity">list of scanners to show</param>
        /// <param name="a_szDefault">the default selection</param>
        public ScanSelect(List<string> a_lszIdentity, string a_szDefault)
        {
            string[] aszIdentity;
            string[] aszDefault;
            // Init stuff...
            InitializeComponent();


            // Explode the default...
            aszDefault = dJTWAIN.CSVFormat(a_szDefault);

            // Suspend updating...
            ListBoxSourceSelect.BeginUpdate();

            // Populate our driver list...
            foreach (string sz in a_lszIdentity)
            {
                aszIdentity = dJTWAIN.CSVFormat(sz);
                ListBoxSourceSelect.Items.Add(aszIdentity[11].ToString());
            }

            // Select the default...
            ListBoxSourceSelect.SelectedIndex = ListBoxSourceSelect.FindStringExact(aszDefault[11]);

            // Resume updating...
            ListBoxSourceSelect.EndUpdate();
        }
        public string GetSelectedDriver()
        {
            return m_szSelected;
        }

        private void ButtonSelectScanSoucre_Click(object sender, EventArgs e)
        {
            m_szSelected = (string)ListBoxSourceSelect.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }

        private void ListBoxSourceSelect_DoubleClick(object sender, EventArgs e)
        {
            m_szSelected = (string)ListBoxSourceSelect.SelectedItem;
            this.DialogResult = DialogResult.OK;
        }
    }
}
