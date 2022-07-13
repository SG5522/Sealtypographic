using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DJTAWINLibNET45;

namespace DJTWAINTESTNET6
{
    public partial class NTWAIN : Form
    {
        private DJTWAIN45 dJTWAIN45 = new();        
        public NTwainData nTwainData = new NTwainData();
        public LoadSource loadSource = new LoadSource();
        
        public NTWAIN()
        {
            InitializeComponent();            
            Text = dJTWAIN45.NTwainBitCheck();            
        }
        protected override void OnHandleCreated(EventArgs e)
        {
            base.OnHandleCreated(e);            
            dJTWAIN45.SetupTwain(pictureBox1);

        }

        private void ScanSourceComboBox_DropDown(object sender, EventArgs e)
        {
            //ReloadSourceList(sender,e);
        }
        private void btnSources_DropDownOpened(object sender, EventArgs e)
        {
            if (btnSources.DropDownItems.Count == 2)
            {
                if (dJTWAIN45.NTwainState() >= 3)
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
                ToolStripMenuItem? srcBtn = btn as ToolStripMenuItem;
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




        //private void LoadSourceCaps()
        //{
        //    var src = nTwainData.NTWAIN.CurrentSource;
        //    _loadingCaps = true;

        //    //var test = src.SupportedCaps;

        //    if (groupDepth.Enabled = src.Capabilities.ICapPixelType.IsSupported)
        //    {
        //        LoadDepth(src.Capabilities.ICapPixelType);
        //    }
        //    if (groupDPI.Enabled = src.Capabilities.ICapXResolution.IsSupported && src.Capabilities.ICapYResolution.IsSupported)
        //    {
        //        LoadDPI(src.Capabilities.ICapXResolution);
        //    }
        //    // TODO: find out if this is how duplex works or also needs the other option
        //    if (groupDuplex.Enabled = src.Capabilities.CapDuplexEnabled.IsSupported)
        //    {
        //        LoadDuplex(src.Capabilities.CapDuplexEnabled);
        //    }
        //    if (groupSize.Enabled = src.Capabilities.ICapSupportedSizes.IsSupported)
        //    {
        //        LoadPaperSize(src.Capabilities.ICapSupportedSizes);
        //    }
        //    btnAllSettings.Enabled = src.Capabilities.CapEnableDSUIOnly.IsSupported;
        //    _loadingCaps = false;
        //}
    }
}
