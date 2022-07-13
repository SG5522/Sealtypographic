using DJTWAINLib;
using Saraff.Twain;
using DJTAWINLibNET45;

namespace DJTWAINTESTNET6
{
    
    public partial class scantest : Form
    {
        private bool _isEnable = false;
        private readonly Twain32 Twain = new();
        private DJTWAIN45 dJTWAIN45 = new DJTWAIN45();

        public scantest()
        {
            InitializeComponent();
            //twain32.Country = TwCountry.USA;
            //dJ45.DJTWAINLibNET45set();
            //dJTWAIN45.NTwainInit();
            //dJTWAIN45.SetupTwain();
            try
            {
                Twain.Country = TwCountry.TAIWAN;
                Twain.Language = TwLanguage.CHINESE_TRADITIONAL;
                Twain.AcquireCompleted += new EventHandler(Twain32_AcquireCompleted);
                Twain.AcquireError += new EventHandler<Twain32.AcquireErrorEventArgs>(twain32_AcquireError);
                Twain.TwainStateChanged += new EventHandler<Saraff.Twain.Twain32.TwainStateEventArgs>(Twain32_TwainStateChanged);
                Twain.OpenDSM();
                //twain32.OpenDSM();
            }
            catch (Exception ex)
            {
                MessageBox.Show(string.Format("{0}\n\n{1}", ex.Message, ex.StackTrace), "SAMPLE1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void SelcetButton_Click(object sender, EventArgs e)
        {
            try
            {
                if (Environment.OSVersion.Platform == PlatformID.Unix)
                {
                    using SelectSource _dlg = new() { Twain = twain32 };
                    if (_dlg.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    {
                        twain32.SetDefaultSource(_dlg.SourceIndex);
                        twain32.SourceIndex = _dlg.SourceIndex;
                    }
                }
                else
                {
                    //twain32.CloseDataSource();
                    //twain32.SelectSource();
                    Twain.CloseDataSource();
                    Twain.SelectSource();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SAMPLE1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void ScanButton_Click(object sender, EventArgs e)
        {            
            try
            {
                //twain32.Acquire();
                Twain.Acquire();
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SAMPLE1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Twain32_AcquireCompleted(object sender, EventArgs e)
        {
            try
            {
                if (this.pictureBox1.Image != null)
                {
                    this.pictureBox1.Image.Dispose();
                }
                //if (this.twain32.ImageCount > 0)
                //{
                //    this.pictureBox1.Image = this.twain32.GetImage(0);
                //}
                if (Twain.ImageCount > 0)
                {
                    this.pictureBox1.Image = Twain.GetImage(0);
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SAMPLE1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void Twain32_TwainStateChanged(object sender, Saraff.Twain.Twain32.TwainStateEventArgs e)
        {
            try
            {
                if ((e.TwainState & Saraff.Twain.Twain32.TwainStateFlag.DSEnabled) == 0 && this._isEnable)
                {
                    this._isEnable = false;
                    // <<< scaning finished (or closed)
                }
                this._isEnable = (e.TwainState & Saraff.Twain.Twain32.TwainStateFlag.DSEnabled) != 0;
            }
            catch (Exception ex)
            {
                MessageBox.Show(ex.Message, "SAMPLE1", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void twain32_AcquireError(object sender, Saraff.Twain.Twain32.AcquireErrorEventArgs e)
        {


            Twain.AcquireError += (sender, e) =>
            {
                throw e.Exception;
            };
        }
    }
}