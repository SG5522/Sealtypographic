namespace LibTest
{
    partial class SpireTest
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
            BtnStart = new Button();
            BtnOpenFile = new Button();
            BtnToImageBas64 = new Button();
            SuspendLayout();
            // 
            // BtnStart
            // 
            BtnStart.Location = new Point(981, 565);
            BtnStart.Name = "BtnStart";
            BtnStart.Size = new Size(150, 46);
            BtnStart.TabIndex = 0;
            BtnStart.Text = "Start";
            BtnStart.UseVisualStyleBackColor = true;
            BtnStart.Click += BtnStart_Click;
            // 
            // BtnOpenFile
            // 
            BtnOpenFile.Location = new Point(339, 346);
            BtnOpenFile.Name = "BtnOpenFile";
            BtnOpenFile.Size = new Size(150, 46);
            BtnOpenFile.TabIndex = 1;
            BtnOpenFile.Text = "Open File";
            BtnOpenFile.UseVisualStyleBackColor = true;
            BtnOpenFile.Click += BtnOpenFile_Click;
            // 
            // BtnToImageBas64
            // 
            BtnToImageBas64.Location = new Point(641, 565);
            BtnToImageBas64.Name = "BtnToImageBas64";
            BtnToImageBas64.Size = new Size(202, 46);
            BtnToImageBas64.TabIndex = 2;
            BtnToImageBas64.Text = "ToImageBas64";
            BtnToImageBas64.UseVisualStyleBackColor = true;
            BtnToImageBas64.Click += BtnToImageBas64_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1290, 755);
            Controls.Add(BtnToImageBas64);
            Controls.Add(BtnOpenFile);
            Controls.Add(BtnStart);
            Name = "Form1";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button BtnStart;
        private Button BtnOpenFile;
        private Button BtnToImageBas64;
    }
}