using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DJTWAINLib;

namespace DJTWAINTESTNET6
{
    public partial class Scan : Form
    {
        private DJTWAIN dJTWAIN = new();
        private Graphics? m_graphics1;

        public Scan()
        {
            InitializeComponent();
            dJTWAIN.TwainSet(this,this.Handle);
        }


        /// <summary>
        /// We use this to run code in the context of the caller's UI thread...
        /// </summary>
        /// <param name="a_object">object (really a control)</param>
        /// <param name="a_action">code to run</param>
        public delegate void RunInUiThreadDelegate(Object a_object, Action a_action);


        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="code">the code to run</param>
        private void RunInUiThread(Action a_action)
        {
            RunInUiThread(this, a_action);
        }


        /// <summary>
        /// TWAIN needs help, if we want it to run stuff in our main
        /// UI thread...
        /// </summary>
        /// <param name="control">the control to run in</param>
        /// <param name="code">the code to run</param>
        public void RunInUiThread(Object a_object, Action a_action)
        {
            Control control = (Control)a_object;
            if (control.InvokeRequired)
            {
                control.Invoke(new Scan.RunInUiThreadDelegate(RunInUiThread), new object[] { a_object, a_action });
                return;
            }
            a_action();
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
                Application.AddMessageFilter((IMessageFilter)this);
            }
            else
            {
                Application.RemoveMessageFilter((IMessageFilter)this);
            }
        }

        private void btnScansource_Click(object sender, EventArgs e)
        {
            bool m_blExit;
            string szIdentity;
            string szDefault = "";
            List<string> lszIdentity = new();
            ScanSelect ScanSelect;
            DialogResult dialogresult;
            dJTWAIN.ScanSource(lszIdentity, this.Handle);

            
            // Ruh-roh...
            if (lszIdentity.Count == 0)
            {
                MessageBox.Show("There are no TWAIN drivers installed on this system...");
                return;
            }

            // Instantiate our form...
            ScanSelect = new ScanSelect(lszIdentity, szDefault)
            {
                StartPosition = FormStartPosition.CenterParent
            };
            dialogresult = ScanSelect.ShowDialog(this);
            if (dialogresult != System.Windows.Forms.DialogResult.OK)
            {
                m_blExit = true;
                return;
            }
            
            //// Get all the identities...
            //szIdentity = ScanSelect.GetSelectedDriver();
            //if (szIdentity == null)
            //{
            //    m_blExit = true;
            //    return;
            //}

            //// Get the selected identity...
            //m_blExit = true;
            //foreach (string sz in lszIdentity)
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
        }
    }
}
