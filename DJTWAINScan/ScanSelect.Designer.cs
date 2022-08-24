namespace DJTWAINScan
{
    partial class ScanSelect
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
            this.ListBoxSourceSelect = new System.Windows.Forms.ListBox();
            this.ButtonSelectScanSoucre = new System.Windows.Forms.Button();
            this.labelSelect = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // ListBoxSourceSelect
            // 
            this.ListBoxSourceSelect.FormattingEnabled = true;
            this.ListBoxSourceSelect.ItemHeight = 15;
            this.ListBoxSourceSelect.Location = new System.Drawing.Point(20, 57);
            this.ListBoxSourceSelect.Name = "ListBoxSourceSelect";
            this.ListBoxSourceSelect.Size = new System.Drawing.Size(197, 154);
            this.ListBoxSourceSelect.Sorted = true;
            this.ListBoxSourceSelect.TabIndex = 0;
            this.ListBoxSourceSelect.DoubleClick += new System.EventHandler(this.ListBoxSourceSelect_DoubleClick);
            // 
            // ButtonSelectScanSoucre
            // 
            this.ButtonSelectScanSoucre.Location = new System.Drawing.Point(20, 227);
            this.ButtonSelectScanSoucre.Name = "ButtonSelectScanSoucre";
            this.ButtonSelectScanSoucre.Size = new System.Drawing.Size(75, 23);
            this.ButtonSelectScanSoucre.TabIndex = 1;
            this.ButtonSelectScanSoucre.Text = "確定";
            this.ButtonSelectScanSoucre.UseVisualStyleBackColor = true;
            this.ButtonSelectScanSoucre.Click += new System.EventHandler(this.ButtonSelectScanSoucre_Click);
            // 
            // labelSelect
            // 
            this.labelSelect.AutoSize = true;
            this.labelSelect.Location = new System.Drawing.Point(20, 27);
            this.labelSelect.Name = "labelSelect";
            this.labelSelect.Size = new System.Drawing.Size(67, 15);
            this.labelSelect.TabIndex = 2;
            this.labelSelect.Text = "選擇掃描機";
            // 
            // ScanSelect
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(238, 275);
            this.Controls.Add(this.labelSelect);
            this.Controls.Add(this.ButtonSelectScanSoucre);
            this.Controls.Add(this.ListBoxSourceSelect);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "ScanSelect";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "ScanSelect";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private ListBox ListBoxSourceSelect;
        private Button ButtonSelectScanSoucre;
        private Label labelSelect;
    }
}