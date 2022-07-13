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

namespace DJTWAINTESTNET48
{
    
    public partial class Form1 : Form
    {        
        
        public Form1()
        {
            InitializeComponent();
        }


        private void btnSources_DropDownOpened(object sender, EventArgs e)
        {
            if (btnSources.DropDownItems.Count == 2)
            {
                //ReloadSourceList();
            }
        }
        private void reloadSourcesListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (var btn in btnSources.DropDownItems)
            {
                ToolStripMenuItem srcBtn = btn as ToolStripMenuItem;
                if (srcBtn != null)
                {
                    srcBtn.Checked = false;
                }
            }
            //dJTWAIN45.ReloadSourceList(btnSources);
        }
        private void OpenScanButton_Click(object sender, EventArgs e)
        {
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
        }
    }
}
