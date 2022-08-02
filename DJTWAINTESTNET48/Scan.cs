using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DJLib;

namespace DJTWAINTESTNET48
{
    
    public partial class Scan : Form, IMessageFilter
    {
        private DJTWAIN dJTWAIN = new DJTWAIN();
        //private Graphics? m_graphics1;
        private readonly string imageName = "test";
        private readonly string imgageType = ".png";
        public Scan()
        {
            InitializeComponent();
            dJTWAIN.TwainSet(this, this.Handle);
            SetMessageFilter(true);
        }

        /// <summary>
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY), this
        /// function is only triggered when SetMessageFilter() is called with 'true'...
        /// </summary>
        /// <param name="message">Message to process</param>
        /// <returns>Result of the processing</returns>
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        public bool PreFilterMessage(ref Message message)
        {
            IntPtr a_intptrHwnd = message.HWnd;
            int a_iMsg = message.Msg;
            IntPtr a_intptrWparam = message.WParam;
            IntPtr a_intptrLparam = message.LParam;
            bool scanEnd = dJTWAIN.PreFilterMessage(a_intptrHwnd, a_iMsg, a_intptrWparam, a_intptrLparam);
            if (scanEnd)
            {
                string filePathF = @".\" + imageName + "F" + imgageType;
                string filePathR = @".\" + imageName + "R" + imgageType;
                FileStream fs = File.OpenRead(filePathF);
                pictureBox1.Image = Image.FromStream(fs);
                fs = File.OpenRead(filePathR);
                pictureBox2.Image = Image.FromStream(fs);
                fs.Close();
                return true;
            }
            return false;
        }

        /// <summary>
        /// Turn message filtering on or off, we use this to capture stuff
        /// like MSG_XFERREADY.  If it's off, then it's assumed we're getting
        /// this info through DAT_CALLBACK2...
        /// </summary>
        /// <param name="openCheck">True to turn it on</param>
        public void SetMessageFilter(bool openCheck)
        {
            if (openCheck)
            {
                Application.AddMessageFilter(this);
            }
            else
            {
                Application.RemoveMessageFilter(this);
            }
        }
        private void ButtonScanSource_Click(object sender, EventArgs e)
        {
            string selectScan; //@被選擇的掃描機
                               //string m_szProductDirectory;

            ScanSelect ScanSelect;
            DialogResult dialogresult;
            ScanSourceDataList scanSourceDataList = new ScanSourceDataList();

            dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);


            if (scanSourceDataList.ErrorMessage == "")
            {
                MessageBox.Show(scanSourceDataList.ErrorMessage);
                return;
            }

            // Ruh-roh...
            if (scanSourceDataList.LszIdentity.Count == 0)
            {                
                dJTWAIN.SuorceSelectCancel();
                dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);
            }

            // Instantiate our form...
            ScanSelect = new ScanSelect(scanSourceDataList.LszIdentity, scanSourceDataList.SzDefault)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            dialogresult = ScanSelect.ShowDialog(this);
            if (dialogresult != System.Windows.Forms.DialogResult.OK)
            {
                return;
            }

            // Get all the identities...
            // Get the selected identity...
            selectScan = ScanSelect.GetSelectedDriver();
            if (selectScan == null)
            {
                return;
            }

            dJTWAIN.ScanSource(selectScan, scanSourceDataList);

            // Update the main form title...
            this.Text = "TWAIN C# Scan (" + selectScan + ")";
        }

        private void ButtonScan_Click(object sender, EventArgs e)
        {
            dJTWAIN.StartScan(this.Handle, imageName, imgageType);
        }

        private void Scan_FormClosing(object sender, FormClosingEventArgs e)
        {
            // Make sure this thing is off...
            SetMessageFilter(false);
        }
    }
}
