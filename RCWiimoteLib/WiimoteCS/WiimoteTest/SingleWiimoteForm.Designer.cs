namespace WiimoteTest
{
	partial class SingleWiimoteForm
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
            this.button1 = new System.Windows.Forms.Button();
            this.setledsbutton = new System.Windows.Forms.Button();
            this.LED1checkBox = new System.Windows.Forms.CheckBox();
            this.LED2checkBox = new System.Windows.Forms.CheckBox();
            this.LED3checkBox = new System.Windows.Forms.CheckBox();
            this.LED4checkBox = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(304, 120);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "button1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // setledsbutton
            // 
            this.setledsbutton.Location = new System.Drawing.Point(304, 176);
            this.setledsbutton.Name = "setledsbutton";
            this.setledsbutton.Size = new System.Drawing.Size(75, 23);
            this.setledsbutton.TabIndex = 1;
            this.setledsbutton.Text = "Set LEDs";
            this.setledsbutton.UseVisualStyleBackColor = true;
            this.setledsbutton.Click += new System.EventHandler(this.setledsbutton_Click);
            // 
            // LED1checkBox
            // 
            this.LED1checkBox.AutoSize = true;
            this.LED1checkBox.Location = new System.Drawing.Point(270, 219);
            this.LED1checkBox.Name = "LED1checkBox";
            this.LED1checkBox.Size = new System.Drawing.Size(56, 17);
            this.LED1checkBox.TabIndex = 2;
            this.LED1checkBox.Text = "LED 1";
            this.LED1checkBox.UseVisualStyleBackColor = true;
            this.LED1checkBox.CheckedChanged += new System.EventHandler(this.LED1checkBox_CheckedChanged);
            // 
            // LED2checkBox
            // 
            this.LED2checkBox.AutoSize = true;
            this.LED2checkBox.Location = new System.Drawing.Point(270, 243);
            this.LED2checkBox.Name = "LED2checkBox";
            this.LED2checkBox.Size = new System.Drawing.Size(56, 17);
            this.LED2checkBox.TabIndex = 3;
            this.LED2checkBox.Text = "LED 2";
            this.LED2checkBox.UseVisualStyleBackColor = true;
            this.LED2checkBox.CheckedChanged += new System.EventHandler(this.LED2checkBox_CheckedChanged);
            // 
            // LED3checkBox
            // 
            this.LED3checkBox.AutoSize = true;
            this.LED3checkBox.Location = new System.Drawing.Point(270, 267);
            this.LED3checkBox.Name = "LED3checkBox";
            this.LED3checkBox.Size = new System.Drawing.Size(56, 17);
            this.LED3checkBox.TabIndex = 4;
            this.LED3checkBox.Text = "LED 3";
            this.LED3checkBox.UseVisualStyleBackColor = true;
            this.LED3checkBox.CheckedChanged += new System.EventHandler(this.LED3checkBox_CheckedChanged);
            // 
            // LED4checkBox
            // 
            this.LED4checkBox.AutoSize = true;
            this.LED4checkBox.Location = new System.Drawing.Point(270, 291);
            this.LED4checkBox.Name = "LED4checkBox";
            this.LED4checkBox.Size = new System.Drawing.Size(56, 17);
            this.LED4checkBox.TabIndex = 5;
            this.LED4checkBox.Text = "LED 4";
            this.LED4checkBox.UseVisualStyleBackColor = true;
            this.LED4checkBox.CheckedChanged += new System.EventHandler(this.LED4checkBox_CheckedChanged);
            // 
            // SingleWiimoteForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(698, 453);
            this.Controls.Add(this.LED4checkBox);
            this.Controls.Add(this.LED3checkBox);
            this.Controls.Add(this.LED2checkBox);
            this.Controls.Add(this.LED1checkBox);
            this.Controls.Add(this.setledsbutton);
            this.Controls.Add(this.button1);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.Name = "SingleWiimoteForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Wiimote Tester";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

		}

		#endregion

		private WiimoteInfo wiimoteInfo1 = new WiimoteInfo();
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button setledsbutton;
        private System.Windows.Forms.CheckBox LED1checkBox;
        private System.Windows.Forms.CheckBox LED2checkBox;
        private System.Windows.Forms.CheckBox LED3checkBox;
        private System.Windows.Forms.CheckBox LED4checkBox;

	}
}

