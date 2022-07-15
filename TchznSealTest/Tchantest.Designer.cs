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
            this.buttonOpenimage = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // buttonOpenimage
            // 
            this.buttonOpenimage.Location = new System.Drawing.Point(527, 263);
            this.buttonOpenimage.Name = "buttonOpenimage";
            this.buttonOpenimage.Size = new System.Drawing.Size(97, 36);
            this.buttonOpenimage.TabIndex = 0;
            this.buttonOpenimage.Text = "Open Image";
            this.buttonOpenimage.UseVisualStyleBackColor = true;
            this.buttonOpenimage.Click += new System.EventHandler(this.buttonOpenimage_Click);
            // 
            // Tchantest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(703, 363);
            this.Controls.Add(this.buttonOpenimage);
            this.Name = "Tchantest";
            this.Text = "Form1";
            this.ResumeLayout(false);

        }

        #endregion

        private Button buttonOpenimage;
    }
}