namespace DJLocalApp
{
    partial class FrmMain
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            TlpMain = new TableLayoutPanel();
            TlpAPI = new TableLayoutPanel();
            LstUrl = new ListBox();
            LblUrl = new Label();
            TlpScanner = new TableLayoutPanel();
            BtnCurrentDriver = new Button();
            BtnSetDriver = new Button();
            TvwShow = new TreeView();
            BtnAllDrivers = new Button();
            TlpMain.SuspendLayout();
            TlpAPI.SuspendLayout();
            TlpScanner.SuspendLayout();
            SuspendLayout();
            // 
            // TlpMain
            // 
            TlpMain.ColumnCount = 3;
            TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 33F));
            TlpMain.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 34F));
            TlpMain.Controls.Add(TlpAPI, 0, 0);
            TlpMain.Controls.Add(TlpScanner, 1, 0);
            TlpMain.Controls.Add(TvwShow, 2, 0);
            TlpMain.Dock = DockStyle.Fill;
            TlpMain.Location = new Point(0, 0);
            TlpMain.Name = "TlpMain";
            TlpMain.RowCount = 1;
            TlpMain.RowStyles.Add(new RowStyle(SizeType.Percent, 100F));
            TlpMain.Size = new Size(800, 450);
            TlpMain.TabIndex = 3;
            // 
            // TlpAPI
            // 
            TlpAPI.ColumnCount = 1;
            TlpAPI.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 50F));
            TlpAPI.Controls.Add(LstUrl, 0, 1);
            TlpAPI.Controls.Add(LblUrl, 0, 0);
            TlpAPI.Dock = DockStyle.Fill;
            TlpAPI.Location = new Point(3, 3);
            TlpAPI.Name = "TlpAPI";
            TlpAPI.RowCount = 2;
            TlpAPI.RowStyles.Add(new RowStyle(SizeType.Percent, 5.18018F));
            TlpAPI.RowStyles.Add(new RowStyle(SizeType.Percent, 94.81982F));
            TlpAPI.Size = new Size(258, 444);
            TlpAPI.TabIndex = 0;
            // 
            // LstUrl
            // 
            LstUrl.FormattingEnabled = true;
            LstUrl.ItemHeight = 15;
            LstUrl.Location = new Point(3, 26);
            LstUrl.Name = "LstUrl";
            LstUrl.Size = new Size(252, 169);
            LstUrl.TabIndex = 4;
            // 
            // LblUrl
            // 
            LblUrl.AutoSize = true;
            LblUrl.Dock = DockStyle.Fill;
            LblUrl.Location = new Point(3, 0);
            LblUrl.Name = "LblUrl";
            LblUrl.Size = new Size(252, 23);
            LblUrl.TabIndex = 3;
            LblUrl.Text = "API Urls:";
            LblUrl.TextAlign = ContentAlignment.MiddleLeft;
            // 
            // TlpScanner
            // 
            TlpScanner.ColumnCount = 1;
            TlpScanner.ColumnStyles.Add(new ColumnStyle(SizeType.Percent, 100F));
            TlpScanner.Controls.Add(BtnCurrentDriver, 0, 0);
            TlpScanner.Controls.Add(BtnSetDriver, 0, 2);
            TlpScanner.Controls.Add(BtnAllDrivers, 0, 1);
            TlpScanner.Dock = DockStyle.Fill;
            TlpScanner.Location = new Point(267, 3);
            TlpScanner.Name = "TlpScanner";
            TlpScanner.RowCount = 8;
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.RowStyles.Add(new RowStyle(SizeType.Percent, 12.5F));
            TlpScanner.Size = new Size(258, 444);
            TlpScanner.TabIndex = 1;
            // 
            // BtnCurrentDriver
            // 
            BtnCurrentDriver.Dock = DockStyle.Fill;
            BtnCurrentDriver.Location = new Point(3, 3);
            BtnCurrentDriver.Name = "BtnCurrentDriver";
            BtnCurrentDriver.Size = new Size(252, 49);
            BtnCurrentDriver.TabIndex = 0;
            BtnCurrentDriver.Text = "取得目前掃描器";
            BtnCurrentDriver.UseVisualStyleBackColor = true;
            BtnCurrentDriver.Click += BtnCurrentDriver_Click;
            // 
            // BtnSetDriver
            // 
            BtnSetDriver.Dock = DockStyle.Fill;
            BtnSetDriver.Location = new Point(3, 113);
            BtnSetDriver.Name = "BtnSetDriver";
            BtnSetDriver.Size = new Size(252, 49);
            BtnSetDriver.TabIndex = 1;
            BtnSetDriver.Text = "BtnSetDriver";
            BtnSetDriver.UseVisualStyleBackColor = true;
            BtnSetDriver.Click += BtnSetDriver_Click;
            // 
            // TvwShow
            // 
            TvwShow.Dock = DockStyle.Fill;
            TvwShow.Location = new Point(531, 3);
            TvwShow.Name = "TvwShow";
            TvwShow.Size = new Size(266, 444);
            TvwShow.TabIndex = 2;
            // 
            // BtnAllDrivers
            // 
            BtnAllDrivers.Dock = DockStyle.Fill;
            BtnAllDrivers.Location = new Point(3, 58);
            BtnAllDrivers.Name = "BtnAllDrivers";
            BtnAllDrivers.Size = new Size(252, 49);
            BtnAllDrivers.TabIndex = 2;
            BtnAllDrivers.Text = "取得所有掃描器";
            BtnAllDrivers.UseVisualStyleBackColor = true;
            BtnAllDrivers.Click += BtnAllDrivers_Click;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(TlpMain);
            Name = "FrmMain";
            Text = "D & J Local App";
            Load += Form1_Load;
            TlpMain.ResumeLayout(false);
            TlpAPI.ResumeLayout(false);
            TlpAPI.PerformLayout();
            TlpScanner.ResumeLayout(false);
            ResumeLayout(false);
        }

        #endregion
        private TableLayoutPanel TlpMain;
        private TableLayoutPanel TlpAPI;
        private ListBox LstUrl;
        private Label LblUrl;
        private TableLayoutPanel TlpScanner;
        private Button BtnCurrentDriver;
        private TreeView TvwShow;
        private Button BtnSetDriver;
        private Button BtnAllDrivers;
    }
}