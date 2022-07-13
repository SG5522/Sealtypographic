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
using DJTAWINLibNET45;

namespace DJTWAINTESTNET48
{
    
    public partial class Form1 : Form
    {        
        private readonly DJTWAIN45 dJTWAIN45 = new DJTWAIN45();
        public NTwainData nTwainData = new NTwainData();
        public LoadSource loadSource = new LoadSource();
        
        public Form1()
        {
            InitializeComponent();
            Text = dJTWAIN45.NTwainBitCheck();
            dJTWAIN45.ImageCodecCheck();
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);
            //dJTWAIN45.NTWAIN2();
            dJTWAIN45.SetupTwain(pictureBox1);
            dJTWAIN45.comboxinit(comboDPI);
            //this.BeginInvoke(new Action(() =>
            //{
            //    foreach (Image image in images)
            //    {
            //        pictureBox1.Image = image;
            //    }
            //}));

        }

        private void btnSources_DropDownOpened(object sender, EventArgs e)
        {
            if (btnSources.DropDownItems.Count == 2)
            {
                //ReloadSourceList();
                if(dJTWAIN45.NTwainState() >= 3)
                {
                    while (btnSources.DropDownItems.IndexOf(sepSourceList) > 0)
                    {
                        var first = btnSources.DropDownItems[0];
                        //first.Click -= SourceMenuItem_Click;
                        btnSources.DropDownItems.Remove(first);

                    }
                    dJTWAIN45.ReloadSourceList(btnSources);   
                }
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
            dJTWAIN45.OpenSettings(Handle);
        }

        private void btnStartScan_Click(object sender, EventArgs e)
        {
            dJTWAIN45.StartScan(this.Handle);
        }
    }
}
