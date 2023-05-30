namespace LibTest
{
    partial class EncryptionTest
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
            button1 = new Button();
            button2 = new Button();
            label1 = new Label();
            label2 = new Label();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(332, 526);
            button1.Margin = new Padding(6);
            button1.Name = "button1";
            button1.Size = new Size(194, 72);
            button1.TabIndex = 1;
            button1.Text = "壓縮";
            button1.UseVisualStyleBackColor = true;
            button1.Click += Button1_Click;
            // 
            // button2
            // 
            button2.Location = new Point(586, 526);
            button2.Margin = new Padding(6);
            button2.Name = "button2";
            button2.Size = new Size(194, 72);
            button2.TabIndex = 2;
            button2.Text = "解壓縮";
            button2.UseVisualStyleBackColor = true;
            button2.Click += Button2_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Location = new Point(344, 164);
            label1.Name = "label1";
            label1.Size = new Size(81, 30);
            label1.TabIndex = 3;
            label1.Text = "label1";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Location = new Point(344, 255);
            label2.Name = "label2";
            label2.Size = new Size(81, 30);
            label2.TabIndex = 4;
            label2.Text = "label2";
            // 
            // Tchantest
            // 
            AutoScaleDimensions = new SizeF(14F, 30F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(1406, 726);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(button2);
            Controls.Add(button1);
            Margin = new Padding(6);
            Name = "Tchantest";
            Text = "Form1";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion
        private Button button1;
        private Button button2;
        private Label label1;
        private Label label2;
    }
}