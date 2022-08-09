namespace DJTWAINTESTNET6
{
    partial class Scan
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
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.buttonScanSource = new System.Windows.Forms.Button();
            this.ButtonScan = new System.Windows.Forms.Button();
            this.ButtonSetup = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.scanImageListView = new System.Windows.Forms.ListView();
            this.selectScanPathButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(36, 3);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(360, 340);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 0;
            this.pictureBox1.TabStop = false;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Location = new System.Drawing.Point(35, 3);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(360, 340);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox2.TabIndex = 1;
            this.pictureBox2.TabStop = false;
            // 
            // buttonScanSource
            // 
            this.buttonScanSource.Enabled = false;
            this.buttonScanSource.Location = new System.Drawing.Point(421, 393);
            this.buttonScanSource.Name = "buttonScanSource";
            this.buttonScanSource.Size = new System.Drawing.Size(75, 23);
            this.buttonScanSource.TabIndex = 2;
            this.buttonScanSource.Text = "Open";
            this.buttonScanSource.UseVisualStyleBackColor = true;
            this.buttonScanSource.Click += new System.EventHandler(this.ButtonScanSource_Click);
            // 
            // ButtonScan
            // 
            this.ButtonScan.Enabled = false;
            this.ButtonScan.Location = new System.Drawing.Point(668, 393);
            this.ButtonScan.Name = "ButtonScan";
            this.ButtonScan.Size = new System.Drawing.Size(75, 23);
            this.ButtonScan.TabIndex = 3;
            this.ButtonScan.Text = "Scan";
            this.ButtonScan.UseVisualStyleBackColor = true;
            this.ButtonScan.Click += new System.EventHandler(this.ButtonScan_Click);
            // 
            // ButtonSetup
            // 
            this.ButtonSetup.Enabled = false;
            this.ButtonSetup.Location = new System.Drawing.Point(541, 393);
            this.ButtonSetup.Name = "ButtonSetup";
            this.ButtonSetup.Size = new System.Drawing.Size(75, 23);
            this.ButtonSetup.TabIndex = 4;
            this.ButtonSetup.Text = "Setup";
            this.ButtonSetup.UseVisualStyleBackColor = true;
            this.ButtonSetup.Click += new System.EventHandler(this.ButtonSetup_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 3);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(440, 375);
            this.tabControl1.TabIndex = 5;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.pictureBox1);
            this.tabPage1.Location = new System.Drawing.Point(4, 24);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(432, 347);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "正面";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.pictureBox2);
            this.tabPage2.Location = new System.Drawing.Point(4, 24);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(432, 347);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "背面";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // splitContainer1
            // 
            this.splitContainer1.Location = new System.Drawing.Point(3, 3);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.scanImageListView);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.tabControl1);
            this.splitContainer1.Size = new System.Drawing.Size(800, 384);
            this.splitContainer1.SplitterDistance = 350;
            this.splitContainer1.TabIndex = 6;
            // 
            // scanImageListView
            // 
            this.scanImageListView.HeaderStyle = System.Windows.Forms.ColumnHeaderStyle.None;
            this.scanImageListView.Location = new System.Drawing.Point(3, 3);
            this.scanImageListView.Name = "scanImageListView";
            this.scanImageListView.Size = new System.Drawing.Size(344, 378);
            this.scanImageListView.TabIndex = 0;
            this.scanImageListView.UseCompatibleStateImageBehavior = false;
            this.scanImageListView.View = System.Windows.Forms.View.Details;
            this.scanImageListView.SelectedIndexChanged += new System.EventHandler(this.scanImageListView_SelectedIndexChanged);
            // 
            // selectScanPathButton
            // 
            this.selectScanPathButton.Location = new System.Drawing.Point(3, 393);
            this.selectScanPathButton.Name = "selectScanPathButton";
            this.selectScanPathButton.Size = new System.Drawing.Size(120, 23);
            this.selectScanPathButton.TabIndex = 7;
            this.selectScanPathButton.Text = "選擇掃描檔案位置";
            this.selectScanPathButton.UseVisualStyleBackColor = true;
            this.selectScanPathButton.Click += new System.EventHandler(this.SelectScanPathButton_Click);
            // 
            // Scan
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(804, 450);
            this.Controls.Add(this.selectScanPathButton);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.ButtonSetup);
            this.Controls.Add(this.ButtonScan);
            this.Controls.Add(this.buttonScanSource);
            this.Name = "Scan";
            this.Text = "Scan";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ThisFormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private PictureBox pictureBox1;
        private PictureBox pictureBox2;
        private Button buttonScanSource;
        private Button ButtonScan;
        private Button ButtonSetup;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private SplitContainer splitContainer1;
        private ListView scanImageListView;
        private Button selectScanPathButton;
    }
}