namespace DJTWAINTESTNET6
{
    partial class scantest
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
            this.components = new System.ComponentModel.Container();
            this.twain32 = new Saraff.Twain.Twain32(this.components);
            this.SelcetButton = new System.Windows.Forms.Button();
            this.ScanButton = new System.Windows.Forms.Button();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // twain32
            // 
            this.twain32.AppProductName = "Saraff.Twain.NET";
            this.twain32.Country = Saraff.Twain.TwCountry.TAIWAN;
            this.twain32.IsTwain2Enable = true;
            this.twain32.Language = Saraff.Twain.TwLanguage.CHINESE_TAIWAN;
            this.twain32.Parent = this;
            this.twain32.AcquireCompleted += new System.EventHandler(this.Twain32_AcquireCompleted);
            this.twain32.AcquireError += new System.EventHandler<Saraff.Twain.Twain32.AcquireErrorEventArgs>(this.twain32_AcquireError);
            this.twain32.TwainStateChanged += new System.EventHandler<Saraff.Twain.Twain32.TwainStateEventArgs>(this.Twain32_TwainStateChanged);
            // 
            // SelcetButton
            // 
            this.SelcetButton.Location = new System.Drawing.Point(442, 337);
            this.SelcetButton.Name = "SelcetButton";
            this.SelcetButton.Size = new System.Drawing.Size(75, 23);
            this.SelcetButton.TabIndex = 0;
            this.SelcetButton.Text = "Select DS";
            this.SelcetButton.UseVisualStyleBackColor = true;
            this.SelcetButton.Click += new System.EventHandler(this.SelcetButton_Click);
            // 
            // ScanButton
            // 
            this.ScanButton.Location = new System.Drawing.Point(559, 337);
            this.ScanButton.Name = "ScanButton";
            this.ScanButton.Size = new System.Drawing.Size(75, 23);
            this.ScanButton.TabIndex = 1;
            this.ScanButton.Text = "scan";
            this.ScanButton.UseVisualStyleBackColor = true;
            this.ScanButton.Click += new System.EventHandler(this.ScanButton_Click);
            // 
            // pictureBox1
            // 
            this.pictureBox1.Location = new System.Drawing.Point(57, 12);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(596, 301);
            this.pictureBox1.TabIndex = 2;
            this.pictureBox1.TabStop = false;
            // 
            // scantest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(701, 391);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.ScanButton);
            this.Controls.Add(this.SelcetButton);
            this.Name = "scantest";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private Saraff.Twain.Twain32 twain32;
        private Button SelcetButton;
        private Button ScanButton;
        private PictureBox pictureBox1;
    }
}