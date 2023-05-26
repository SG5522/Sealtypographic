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
            LstUrl = new ListBox();
            SuspendLayout();
            // 
            // LstUrl
            // 
            LstUrl.FormattingEnabled = true;
            LstUrl.ItemHeight = 15;
            LstUrl.Location = new Point(12, 26);
            LstUrl.Name = "LstUrl";
            LstUrl.Size = new Size(344, 274);
            LstUrl.TabIndex = 1;
            // 
            // FrmMain
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(LstUrl);
            Name = "FrmMain";
            Text = "D & J Local App";
            Load += Form1_Load;
            ResumeLayout(false);
        }

        #endregion

        private ListBox LstUrl;
    }
}