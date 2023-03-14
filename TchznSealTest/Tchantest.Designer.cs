namespace TchznSealTest
{
    partial class Tchantest
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
            buttonOpenimage = new Button();
            button1 = new Button();
            SuspendLayout();
            // 
            // buttonOpenimage
            // 
            buttonOpenimage.Location = new Point(1054, 526);
            buttonOpenimage.Margin = new Padding(6, 6, 6, 6);
            buttonOpenimage.Name = "buttonOpenimage";
            buttonOpenimage.Size = new Size(194, 72);
            buttonOpenimage.TabIndex = 0;
            buttonOpenimage.Text = "Open Image";
            buttonOpenimage.UseVisualStyleBackColor = true;
            buttonOpenimage.Click += buttonOpenimage_Click;
            // 
            // button1
            // 
            button1.Location = new Point(332, 526);
            button1.Margin = new Padding(6);
            button1.Name = "button1";
            button1.Size = new Size(194, 72);
            button1.TabIndex = 1;
            button1.Text = "Open Image";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // Tchantest
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1406, 726);
            Controls.Add(button1);
            Controls.Add(buttonOpenimage);
            Margin = new Padding(6, 6, 6, 6);
            Name = "Tchantest";
            Text = "Form1";
            ResumeLayout(false);
        }

        #endregion

        private Button buttonOpenimage;
        private Button button1;
    }
}