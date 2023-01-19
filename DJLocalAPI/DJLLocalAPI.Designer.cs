namespace DJLocalAPI
{
    partial class DJLLocalAPI
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
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tpScanner = new System.Windows.Forms.TabPage();
            this.btnSetDriver = new System.Windows.Forms.Button();
            this.btnScan = new System.Windows.Forms.Button();
            this.lblSetResult = new System.Windows.Forms.Label();
            this.btnGetDrivers = new System.Windows.Forms.Button();
            this.lbDriver = new System.Windows.Forms.ListBox();
            this.tpSeal = new System.Windows.Forms.TabPage();
            this.tabControl1.SuspendLayout();
            this.tpScanner.SuspendLayout();
            this.SuspendLayout();
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tpScanner);
            this.tabControl1.Controls.Add(this.tpSeal);
            this.tabControl1.Location = new System.Drawing.Point(37, 36);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(1286, 850);
            this.tabControl1.TabIndex = 0;
            // 
            // tpScanner
            // 
            this.tpScanner.Controls.Add(this.btnSetDriver);
            this.tpScanner.Controls.Add(this.btnScan);
            this.tpScanner.Controls.Add(this.lblSetResult);
            this.tpScanner.Controls.Add(this.btnGetDrivers);
            this.tpScanner.Controls.Add(this.lbDriver);
            this.tpScanner.Location = new System.Drawing.Point(8, 44);
            this.tpScanner.Name = "tpScanner";
            this.tpScanner.Padding = new System.Windows.Forms.Padding(3);
            this.tpScanner.Size = new System.Drawing.Size(1270, 798);
            this.tpScanner.TabIndex = 0;
            this.tpScanner.Text = "掃描";
            this.tpScanner.UseVisualStyleBackColor = true;
            // 
            // btnSetDriver
            // 
            this.btnSetDriver.Location = new System.Drawing.Point(108, 658);
            this.btnSetDriver.Name = "btnSetDriver";
            this.btnSetDriver.Size = new System.Drawing.Size(272, 58);
            this.btnSetDriver.TabIndex = 4;
            this.btnSetDriver.Text = "設定掃描器";
            this.btnSetDriver.UseVisualStyleBackColor = true;
            this.btnSetDriver.Click += new System.EventHandler(this.btnSetDriver_Click);
            // 
            // btnScan
            // 
            this.btnScan.Location = new System.Drawing.Point(495, 568);
            this.btnScan.Name = "btnScan";
            this.btnScan.Size = new System.Drawing.Size(272, 58);
            this.btnScan.TabIndex = 3;
            this.btnScan.Text = "掃描";
            this.btnScan.UseVisualStyleBackColor = true;
            this.btnScan.Click += new System.EventHandler(this.btnScan_Click);
            // 
            // lblSetResult
            // 
            this.lblSetResult.AutoSize = true;
            this.lblSetResult.Location = new System.Drawing.Point(504, 48);
            this.lblSetResult.Name = "lblSetResult";
            this.lblSetResult.Size = new System.Drawing.Size(133, 30);
            this.lblSetResult.TabIndex = 2;
            this.lblSetResult.Text = "設置狀態：";
            // 
            // btnGetDrivers
            // 
            this.btnGetDrivers.Location = new System.Drawing.Point(108, 568);
            this.btnGetDrivers.Name = "btnGetDrivers";
            this.btnGetDrivers.Size = new System.Drawing.Size(272, 58);
            this.btnGetDrivers.TabIndex = 1;
            this.btnGetDrivers.Text = "取得掃描器清單";
            this.btnGetDrivers.UseVisualStyleBackColor = true;
            this.btnGetDrivers.Click += new System.EventHandler(this.btnGetDrivers_Click);
            // 
            // lbDriver
            // 
            this.lbDriver.FormattingEnabled = true;
            this.lbDriver.ItemHeight = 30;
            this.lbDriver.Location = new System.Drawing.Point(60, 34);
            this.lbDriver.Name = "lbDriver";
            this.lbDriver.Size = new System.Drawing.Size(362, 424);
            this.lbDriver.TabIndex = 0;
            this.lbDriver.MouseDoubleClick += new System.Windows.Forms.MouseEventHandler(this.lbDriver_MouseDoubleClick);
            // 
            // tpSeal
            // 
            this.tpSeal.Location = new System.Drawing.Point(8, 44);
            this.tpSeal.Name = "tpSeal";
            this.tpSeal.Padding = new System.Windows.Forms.Padding(3);
            this.tpSeal.Size = new System.Drawing.Size(1270, 798);
            this.tpSeal.TabIndex = 1;
            this.tpSeal.Text = "印鑑";
            this.tpSeal.UseVisualStyleBackColor = true;
            // 
            // DJLLocalAPI
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(14F, 30F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1532, 944);
            this.Controls.Add(this.tabControl1);
            this.Name = "DJLLocalAPI";
            this.Text = "Form1";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.DJLLocalAPI_FormClosed);
            this.Load += new System.EventHandler(this.DJLLocalAPI_Load);
            this.tabControl1.ResumeLayout(false);
            this.tpScanner.ResumeLayout(false);
            this.tpScanner.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private TabControl tabControl1;
        private TabPage tpScanner;
        private Button btnGetDrivers;
        private ListBox lbDriver;
        private TabPage tpSeal;
        private Label lblSetResult;
        private Button btnScan;
        private Button btnSetDriver;
    }
}