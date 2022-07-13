namespace DJTWAINTESTNET6
{
    partial class NTWAIN
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(NTWAIN));
            this.OpenScanButton = new System.Windows.Forms.Button();
            this.ScanSourceComboBox = new System.Windows.Forms.ComboBox();
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.btnSources = new System.Windows.Forms.ToolStripDropDownButton();
            this.sepSourceList = new System.Windows.Forms.ToolStripSeparator();
            this.reloadSourcesListToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnStartScan = new System.Windows.Forms.Button();
            this.toolStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // OpenScanButton
            // 
            this.OpenScanButton.Location = new System.Drawing.Point(23, 286);
            this.OpenScanButton.Name = "OpenScanButton";
            this.OpenScanButton.Size = new System.Drawing.Size(75, 23);
            this.OpenScanButton.TabIndex = 0;
            this.OpenScanButton.Text = "OpenScan";
            this.OpenScanButton.UseVisualStyleBackColor = true;
            this.OpenScanButton.Click += new System.EventHandler(this.OpenScanButton_Click);
            // 
            // ScanSourceComboBox
            // 
            this.ScanSourceComboBox.FormattingEnabled = true;
            this.ScanSourceComboBox.Location = new System.Drawing.Point(47, 84);
            this.ScanSourceComboBox.Name = "ScanSourceComboBox";
            this.ScanSourceComboBox.Size = new System.Drawing.Size(121, 23);
            this.ScanSourceComboBox.TabIndex = 1;
            this.ScanSourceComboBox.DropDown += new System.EventHandler(this.ScanSourceComboBox_DropDown);
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.btnSources});
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(640, 25);
            this.toolStrip1.TabIndex = 2;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // btnSources
            // 
            this.btnSources.DisplayStyle = System.Windows.Forms.ToolStripItemDisplayStyle.Text;
            this.btnSources.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sepSourceList,
            this.reloadSourcesListToolStripMenuItem});
            this.btnSources.Image = ((System.Drawing.Image)(resources.GetObject("btnSources.Image")));
            this.btnSources.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.btnSources.Name = "btnSources";
            this.btnSources.Size = new System.Drawing.Size(99, 22);
            this.btnSources.Text = "Select &sources";
            this.btnSources.DropDownOpened += new System.EventHandler(this.btnSources_DropDownOpened);
            // 
            // sepSourceList
            // 
            this.sepSourceList.Name = "sepSourceList";
            this.sepSourceList.Size = new System.Drawing.Size(175, 6);
            // 
            // reloadSourcesListToolStripMenuItem
            // 
            this.reloadSourcesListToolStripMenuItem.Name = "reloadSourcesListToolStripMenuItem";
            this.reloadSourcesListToolStripMenuItem.Size = new System.Drawing.Size(178, 22);
            this.reloadSourcesListToolStripMenuItem.Text = "&Reload sources list";
            this.reloadSourcesListToolStripMenuItem.Click += new System.EventHandler(this.reloadSourcesListToolStripMenuItem_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(248, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(392, 362);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 3;
            this.pictureBox1.TabStop = false;
            // 
            // btnStartScan
            // 
            this.btnStartScan.Location = new System.Drawing.Point(145, 286);
            this.btnStartScan.Name = "btnStartScan";
            this.btnStartScan.Size = new System.Drawing.Size(75, 23);
            this.btnStartScan.TabIndex = 4;
            this.btnStartScan.Text = "&Start Scan";
            this.btnStartScan.UseVisualStyleBackColor = true;
            this.btnStartScan.Click += new System.EventHandler(this.btnStartScan_Click);
            // 
            // NTWAIN
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(640, 363);
            this.Controls.Add(this.btnStartScan);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.toolStrip1);
            this.Controls.Add(this.ScanSourceComboBox);
            this.Controls.Add(this.OpenScanButton);
            this.Name = "NTWAIN";
            this.Text = "NTWAIN";
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private Button OpenScanButton;
        private ComboBox ScanSourceComboBox;
        private ToolStrip toolStrip1;
        private ToolStripDropDownButton btnSources;
        private ToolStripSeparator sepSourceList;
        private ToolStripMenuItem reloadSourcesListToolStripMenuItem;
        private PictureBox pictureBox1;
        private Button btnStartScan;
    }
}