using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Security.Permissions;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DJTWAINLib;

namespace DJTWAINTESTNET6
{
    public partial class Scan : Form, IMessageFilter
    {
        private DJTWAIN dJTWAIN = new();
        private Graphics? m_graphics1;
        private bool blExit;

        public Scan()
        {
            InitializeComponent();
            dJTWAIN.TwainSet(this,this.Handle);
            SetMessageFilter(true);
            blExit = false;
        }   


        /// <summary>
        /// We're being closed, clean up nicely...
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void ThisFormClosing(object sender, FormClosingEventArgs e)
        {
            // Make sure this thing is off...
            SetMessageFilter(false);
            /// We're being closed, clean up nicely...
            dJTWAIN.FormClosing();
            // This will prevent ReportImage from doing anything as we close...
            m_graphics1 = null;
        }
        [SecurityPermissionAttribute(SecurityAction.LinkDemand, Flags = SecurityPermissionFlag.UnmanagedCode)]
        /// <summary>
        /// Monitor for DG_CONTROL / DAT_NULL / MSG_* stuff (ex MSG_XFERREADY), this
        /// function is only triggered when SetMessageFilter() is called with 'true'...
        /// </summary>
        /// <param name="message">Message to process</param>
        /// <returns>Result of the processing</returns>
        public bool PreFilterMessage(ref Message message)
        {
            IntPtr a_intptrHwnd = message.HWnd;
            int a_iMsg = message.Msg;
            IntPtr a_intptrWparam = message.WParam;
            IntPtr a_intptrLparam = message.LParam;
            return dJTWAIN.PreFilterMessage(a_intptrHwnd, a_iMsg, a_intptrWparam, a_intptrLparam);
        }
        /// <summary>
        /// Turn message filtering on or off, we use this to capture stuff
        /// like MSG_XFERREADY.  If it's off, then it's assumed we're getting
        /// this info through DAT_CALLBACK2...
        /// </summary>
        /// <param name="a_blAdd">True to turn it on</param>
        public void SetMessageFilter(bool a_blAdd)
        {
            if (a_blAdd)
            {
                Application.AddMessageFilter(this);
            }
            else
            {
                Application.RemoveMessageFilter(this);
            }
        }

        private void btnScansource_Click(object sender, EventArgs e)
        {
            bool blExit;
            string selectScan; //@被選擇的掃描機
            string m_szProductDirectory;
            
            ScanSelect ScanSelect;
            DialogResult dialogresult;
            ScanSourceDataList scanSourceDataList = new();
            
            dJTWAIN.ScanSourceList(scanSourceDataList, this.Handle);


            if(scanSourceDataList.ErrorMessage =="")
            {
                MessageBox.Show(scanSourceDataList.ErrorMessage);
                return;
            }
            
            // Ruh-roh...
            if (scanSourceDataList.LszIdentity.Count == 0)
            {
                MessageBox.Show("There are no TWAIN drivers installed on this system...");
                return;
            }

            // Instantiate our form...
            ScanSelect = new ScanSelect(scanSourceDataList.LszIdentity, scanSourceDataList.SzDefault)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            dialogresult = ScanSelect.ShowDialog(this);
            if (dialogresult != System.Windows.Forms.DialogResult.OK)
            {
                blExit = true;
                return;
            }

            // Get all the identities...
            // Get the selected identity...
            selectScan = ScanSelect.GetSelectedDriver();
            if (selectScan == null)
            {
                blExit = true;
                return;
            }

            dJTWAIN.ScanSource(selectScan, scanSourceDataList);

            // Get the selected identity...
            //m_blExit = true;
            //foreach (string sz in ScanSourceData.LszIdentity)
            //{
            //    if (sz.Contains(szIdentity))
            //    {
            //        m_blExit = false;
            //        szIdentity = sz;
            //        break;
            //    }
            //}
            //if (m_blExit)
            //{
            //    return;
            //}

            // Update the main form title...
            this.Text = "TWAIN C# Scan (" + selectScan + ")";

            // Strip off unsafe chars.  Sadly, mono let's us down here...
            //m_szProductDirectory = CSV.Parse(szIdentity)[11];
            //m_szProductDirectory = dJTWAIN.CSVFormat(scanSourceDataList.SzDefault)[11];
            //foreach (char c in new char[41]
            //                { '\x00', '\x01', '\x02', '\x03', '\x04', '\x05', '\x06', '\x07',
            //                  '\x08', '\x09', '\x0A', '\x0B', '\x0C', '\x0D', '\x0E', '\x0F', '\x10', '\x11', '\x12',
            //                  '\x13', '\x14', '\x15', '\x16', '\x17', '\x18', '\x19', '\x1A', '\x1B', '\x1C', '\x1D',
            //                  '\x1E', '\x1F', '\x22', '\x3C', '\x3E', '\x7C', ':', '*', '?', '\\', '/'
            //                }
            //        )
            //{
            //    m_szProductDirectory = m_szProductDirectory.Replace(c, '_');
            //}

            //// New state...
            //SetButtons(EBUTTONSTATE.OPEN);

            //// Create the setup form...
            //m_formsetup = new FormSetup(this, ref m_twain, m_szProductDirectory);
        }

        private void ButtonScan_Click(object sender, EventArgs e)
        {
            dJTWAIN.StartScan(this.Handle);
            //sts = m_twain.DatUserinterface(TWAIN.DG.CONTROL, TWAIN.MSG.ENABLEDS, ref twuserinterface);
            //if (sts == TWAIN.STS.SUCCESS)
            //{
            //    SetButtons(EBUTTONSTATE.SCANNING);
            //}
        }
        /// <summary>
        /// Something horrible has happened and we need to abort...
        /// </summary>
        /// <returns></returns>
        public bool ExitRequested()
        {
            return (blExit);
        }
    }
}
