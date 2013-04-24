namespace TestWiimote3DTracking
{
    partial class Form1
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
            this.capturebutton = new System.Windows.Forms.Button();
            this.IRpointslabel = new System.Windows.Forms.Label();
            this.countsrcsbutton = new System.Windows.Forms.Button();
            this.wm1IRPictureBox = new System.Windows.Forms.PictureBox();
            this.wm1IRLabel1 = new System.Windows.Forms.Label();
            this.wm1IRLabel2 = new System.Windows.Forms.Label();
            this.wm1IRLabel3 = new System.Windows.Forms.Label();
            this.wm1IRLabel4 = new System.Windows.Forms.Label();
            this.captureOneCountLabel = new System.Windows.Forms.Label();
            this.CalibrateWM1Button = new System.Windows.Forms.Button();
            this.ResetCalibButton = new System.Windows.Forms.Button();
            this.wm2IRPictureBox = new System.Windows.Forms.PictureBox();
            this.StereoConnectButton = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).BeginInit();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(12, 12);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // capturebutton
            // 
            this.capturebutton.Location = new System.Drawing.Point(12, 93);
            this.capturebutton.Name = "capturebutton";
            this.capturebutton.Size = new System.Drawing.Size(75, 23);
            this.capturebutton.TabIndex = 1;
            this.capturebutton.Text = "Capture";
            this.capturebutton.UseVisualStyleBackColor = true;
            this.capturebutton.Click += new System.EventHandler(this.capturebutton_Click);
            // 
            // IRpointslabel
            // 
            this.IRpointslabel.AutoSize = true;
            this.IRpointslabel.Location = new System.Drawing.Point(94, 51);
            this.IRpointslabel.Name = "IRpointslabel";
            this.IRpointslabel.Size = new System.Drawing.Size(66, 13);
            this.IRpointslabel.TabIndex = 2;
            this.IRpointslabel.Text = "IR Sources: ";
            // 
            // countsrcsbutton
            // 
            this.countsrcsbutton.Location = new System.Drawing.Point(13, 46);
            this.countsrcsbutton.Name = "countsrcsbutton";
            this.countsrcsbutton.Size = new System.Drawing.Size(75, 23);
            this.countsrcsbutton.TabIndex = 3;
            this.countsrcsbutton.Text = "No. Sources";
            this.countsrcsbutton.UseVisualStyleBackColor = true;
            this.countsrcsbutton.Click += new System.EventHandler(this.countsrcsbutton_Click);
            // 
            // wm1IRPictureBox
            // 
            this.wm1IRPictureBox.Location = new System.Drawing.Point(352, 12);
            this.wm1IRPictureBox.Name = "wm1IRPictureBox";
            this.wm1IRPictureBox.Size = new System.Drawing.Size(512, 384);
            this.wm1IRPictureBox.TabIndex = 4;
            this.wm1IRPictureBox.TabStop = false;
            // 
            // wm1IRLabel1
            // 
            this.wm1IRLabel1.AutoSize = true;
            this.wm1IRLabel1.Location = new System.Drawing.Point(265, 17);
            this.wm1IRLabel1.Name = "wm1IRLabel1";
            this.wm1IRLabel1.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel1.TabIndex = 5;
            this.wm1IRLabel1.Text = "label1";
            // 
            // wm1IRLabel2
            // 
            this.wm1IRLabel2.AutoSize = true;
            this.wm1IRLabel2.Location = new System.Drawing.Point(265, 45);
            this.wm1IRLabel2.Name = "wm1IRLabel2";
            this.wm1IRLabel2.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel2.TabIndex = 6;
            this.wm1IRLabel2.Text = "label2";
            // 
            // wm1IRLabel3
            // 
            this.wm1IRLabel3.AutoSize = true;
            this.wm1IRLabel3.Location = new System.Drawing.Point(265, 73);
            this.wm1IRLabel3.Name = "wm1IRLabel3";
            this.wm1IRLabel3.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel3.TabIndex = 7;
            this.wm1IRLabel3.Text = "label3";
            // 
            // wm1IRLabel4
            // 
            this.wm1IRLabel4.AutoSize = true;
            this.wm1IRLabel4.Location = new System.Drawing.Point(265, 101);
            this.wm1IRLabel4.Name = "wm1IRLabel4";
            this.wm1IRLabel4.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel4.TabIndex = 8;
            this.wm1IRLabel4.Text = "label4";
            // 
            // captureOneCountLabel
            // 
            this.captureOneCountLabel.AutoSize = true;
            this.captureOneCountLabel.Location = new System.Drawing.Point(94, 100);
            this.captureOneCountLabel.Name = "captureOneCountLabel";
            this.captureOneCountLabel.Size = new System.Drawing.Size(56, 13);
            this.captureOneCountLabel.TabIndex = 9;
            this.captureOneCountLabel.Text = "Captured: ";
            // 
            // CalibrateWM1Button
            // 
            this.CalibrateWM1Button.Location = new System.Drawing.Point(13, 123);
            this.CalibrateWM1Button.Name = "CalibrateWM1Button";
            this.CalibrateWM1Button.Size = new System.Drawing.Size(75, 23);
            this.CalibrateWM1Button.TabIndex = 10;
            this.CalibrateWM1Button.Text = "Calibrate 1";
            this.CalibrateWM1Button.UseVisualStyleBackColor = true;
            this.CalibrateWM1Button.Click += new System.EventHandler(this.CalibrateWM1Button_Click);
            // 
            // ResetCalibButton
            // 
            this.ResetCalibButton.Location = new System.Drawing.Point(12, 153);
            this.ResetCalibButton.Name = "ResetCalibButton";
            this.ResetCalibButton.Size = new System.Drawing.Size(75, 23);
            this.ResetCalibButton.TabIndex = 11;
            this.ResetCalibButton.Text = "Reset Calib";
            this.ResetCalibButton.UseVisualStyleBackColor = true;
            this.ResetCalibButton.Click += new System.EventHandler(this.ResetCalibButton_Click);
            // 
            // wm2IRPictureBox
            // 
            this.wm2IRPictureBox.Location = new System.Drawing.Point(352, 402);
            this.wm2IRPictureBox.Name = "wm2IRPictureBox";
            this.wm2IRPictureBox.Size = new System.Drawing.Size(512, 384);
            this.wm2IRPictureBox.TabIndex = 12;
            this.wm2IRPictureBox.TabStop = false;
            // 
            // StereoConnectButton
            // 
            this.StereoConnectButton.Location = new System.Drawing.Point(12, 224);
            this.StereoConnectButton.Name = "StereoConnectButton";
            this.StereoConnectButton.Size = new System.Drawing.Size(75, 23);
            this.StereoConnectButton.TabIndex = 13;
            this.StereoConnectButton.Text = "Connect 2";
            this.StereoConnectButton.UseVisualStyleBackColor = true;
            this.StereoConnectButton.Click += new System.EventHandler(this.StereoConnectButton_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 796);
            this.Controls.Add(this.StereoConnectButton);
            this.Controls.Add(this.wm2IRPictureBox);
            this.Controls.Add(this.ResetCalibButton);
            this.Controls.Add(this.CalibrateWM1Button);
            this.Controls.Add(this.captureOneCountLabel);
            this.Controls.Add(this.wm1IRLabel4);
            this.Controls.Add(this.wm1IRLabel3);
            this.Controls.Add(this.wm1IRLabel2);
            this.Controls.Add(this.wm1IRLabel1);
            this.Controls.Add(this.wm1IRPictureBox);
            this.Controls.Add(this.countsrcsbutton);
            this.Controls.Add(this.IRpointslabel);
            this.Controls.Add(this.capturebutton);
            this.Controls.Add(this.button1);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button capturebutton;
        private System.Windows.Forms.Label IRpointslabel;
        private System.Windows.Forms.Button countsrcsbutton;
        private System.Windows.Forms.PictureBox wm1IRPictureBox;
        private System.Windows.Forms.Label wm1IRLabel1;
        private System.Windows.Forms.Label wm1IRLabel2;
        private System.Windows.Forms.Label wm1IRLabel3;
        private System.Windows.Forms.Label wm1IRLabel4;
        private System.Windows.Forms.Label captureOneCountLabel;
        private System.Windows.Forms.Button CalibrateWM1Button;
        private System.Windows.Forms.Button ResetCalibButton;
        private System.Windows.Forms.PictureBox wm2IRPictureBox;
        private System.Windows.Forms.Button StereoConnectButton;
    }
}

