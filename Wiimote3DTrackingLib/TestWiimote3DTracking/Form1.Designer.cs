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
            this.DisconnectAllButton = new System.Windows.Forms.Button();
            this.wm2IRLabel4 = new System.Windows.Forms.Label();
            this.wm2IRLabel3 = new System.Windows.Forms.Label();
            this.wm2IRLabel2 = new System.Windows.Forms.Label();
            this.wm2IRLabel1 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.CalibrateStereoButton = new System.Windows.Forms.Button();
            this.captureTwoCountabel = new System.Windows.Forms.Label();
            this.ResetDualCalibButton = new System.Windows.Forms.Button();
            this.dualcapturebutton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.wm1IRSourceslabel = new System.Windows.Forms.Label();
            this.wm2IRSourceslabel = new System.Windows.Forms.Label();
            this.PrintPointsButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(6, 23);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(137, 23);
            this.button1.TabIndex = 0;
            this.button1.Text = "Connect 1";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // capturebutton
            // 
            this.capturebutton.Location = new System.Drawing.Point(6, 52);
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
            this.IRpointslabel.Location = new System.Drawing.Point(88, 144);
            this.IRpointslabel.Name = "IRpointslabel";
            this.IRpointslabel.Size = new System.Drawing.Size(66, 13);
            this.IRpointslabel.TabIndex = 2;
            this.IRpointslabel.Text = "IR Sources: ";
            // 
            // countsrcsbutton
            // 
            this.countsrcsbutton.Location = new System.Drawing.Point(7, 139);
            this.countsrcsbutton.Name = "countsrcsbutton";
            this.countsrcsbutton.Size = new System.Drawing.Size(75, 23);
            this.countsrcsbutton.TabIndex = 3;
            this.countsrcsbutton.Text = "No. Sources";
            this.countsrcsbutton.UseVisualStyleBackColor = true;
            this.countsrcsbutton.Click += new System.EventHandler(this.countsrcsbutton_Click);
            // 
            // wm1IRPictureBox
            // 
            this.wm1IRPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wm1IRPictureBox.Location = new System.Drawing.Point(352, 12);
            this.wm1IRPictureBox.Name = "wm1IRPictureBox";
            this.wm1IRPictureBox.Size = new System.Drawing.Size(512, 384);
            this.wm1IRPictureBox.TabIndex = 4;
            this.wm1IRPictureBox.TabStop = false;
            // 
            // wm1IRLabel1
            // 
            this.wm1IRLabel1.AutoSize = true;
            this.wm1IRLabel1.Location = new System.Drawing.Point(265, 14);
            this.wm1IRLabel1.Name = "wm1IRLabel1";
            this.wm1IRLabel1.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel1.TabIndex = 5;
            this.wm1IRLabel1.Text = "label1";
            // 
            // wm1IRLabel2
            // 
            this.wm1IRLabel2.AutoSize = true;
            this.wm1IRLabel2.Location = new System.Drawing.Point(265, 42);
            this.wm1IRLabel2.Name = "wm1IRLabel2";
            this.wm1IRLabel2.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel2.TabIndex = 6;
            this.wm1IRLabel2.Text = "label2";
            // 
            // wm1IRLabel3
            // 
            this.wm1IRLabel3.AutoSize = true;
            this.wm1IRLabel3.Location = new System.Drawing.Point(265, 70);
            this.wm1IRLabel3.Name = "wm1IRLabel3";
            this.wm1IRLabel3.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel3.TabIndex = 7;
            this.wm1IRLabel3.Text = "label3";
            // 
            // wm1IRLabel4
            // 
            this.wm1IRLabel4.AutoSize = true;
            this.wm1IRLabel4.Location = new System.Drawing.Point(265, 98);
            this.wm1IRLabel4.Name = "wm1IRLabel4";
            this.wm1IRLabel4.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel4.TabIndex = 8;
            this.wm1IRLabel4.Text = "label4";
            // 
            // captureOneCountLabel
            // 
            this.captureOneCountLabel.AutoSize = true;
            this.captureOneCountLabel.Location = new System.Drawing.Point(87, 57);
            this.captureOneCountLabel.Name = "captureOneCountLabel";
            this.captureOneCountLabel.Size = new System.Drawing.Size(56, 13);
            this.captureOneCountLabel.TabIndex = 9;
            this.captureOneCountLabel.Text = "Captured: ";
            // 
            // CalibrateWM1Button
            // 
            this.CalibrateWM1Button.Location = new System.Drawing.Point(6, 110);
            this.CalibrateWM1Button.Name = "CalibrateWM1Button";
            this.CalibrateWM1Button.Size = new System.Drawing.Size(137, 23);
            this.CalibrateWM1Button.TabIndex = 10;
            this.CalibrateWM1Button.Text = "Calibrate Single Wiimote";
            this.CalibrateWM1Button.UseVisualStyleBackColor = true;
            this.CalibrateWM1Button.Click += new System.EventHandler(this.CalibrateWM1Button_Click);
            // 
            // ResetCalibButton
            // 
            this.ResetCalibButton.Location = new System.Drawing.Point(6, 81);
            this.ResetCalibButton.Name = "ResetCalibButton";
            this.ResetCalibButton.Size = new System.Drawing.Size(137, 23);
            this.ResetCalibButton.TabIndex = 11;
            this.ResetCalibButton.Text = "Reset Image Capture";
            this.ResetCalibButton.UseVisualStyleBackColor = true;
            this.ResetCalibButton.Click += new System.EventHandler(this.ResetCalibButton_Click);
            // 
            // wm2IRPictureBox
            // 
            this.wm2IRPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wm2IRPictureBox.Location = new System.Drawing.Point(352, 402);
            this.wm2IRPictureBox.Name = "wm2IRPictureBox";
            this.wm2IRPictureBox.Size = new System.Drawing.Size(512, 384);
            this.wm2IRPictureBox.TabIndex = 12;
            this.wm2IRPictureBox.TabStop = false;
            // 
            // StereoConnectButton
            // 
            this.StereoConnectButton.Location = new System.Drawing.Point(6, 19);
            this.StereoConnectButton.Name = "StereoConnectButton";
            this.StereoConnectButton.Size = new System.Drawing.Size(174, 23);
            this.StereoConnectButton.TabIndex = 13;
            this.StereoConnectButton.Text = "Connect 2";
            this.StereoConnectButton.UseVisualStyleBackColor = true;
            this.StereoConnectButton.Click += new System.EventHandler(this.StereoConnectButton_Click);
            // 
            // DisconnectAllButton
            // 
            this.DisconnectAllButton.Location = new System.Drawing.Point(13, 373);
            this.DisconnectAllButton.Name = "DisconnectAllButton";
            this.DisconnectAllButton.Size = new System.Drawing.Size(102, 23);
            this.DisconnectAllButton.TabIndex = 14;
            this.DisconnectAllButton.Text = "Disconnect All";
            this.DisconnectAllButton.UseVisualStyleBackColor = true;
            this.DisconnectAllButton.Click += new System.EventHandler(this.DisconnectAllButton_Click);
            // 
            // wm2IRLabel4
            // 
            this.wm2IRLabel4.AutoSize = true;
            this.wm2IRLabel4.Location = new System.Drawing.Point(265, 489);
            this.wm2IRLabel4.Name = "wm2IRLabel4";
            this.wm2IRLabel4.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel4.TabIndex = 18;
            this.wm2IRLabel4.Text = "label4";
            // 
            // wm2IRLabel3
            // 
            this.wm2IRLabel3.AutoSize = true;
            this.wm2IRLabel3.Location = new System.Drawing.Point(265, 460);
            this.wm2IRLabel3.Name = "wm2IRLabel3";
            this.wm2IRLabel3.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel3.TabIndex = 17;
            this.wm2IRLabel3.Text = "label3";
            // 
            // wm2IRLabel2
            // 
            this.wm2IRLabel2.AutoSize = true;
            this.wm2IRLabel2.Location = new System.Drawing.Point(265, 431);
            this.wm2IRLabel2.Name = "wm2IRLabel2";
            this.wm2IRLabel2.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel2.TabIndex = 16;
            this.wm2IRLabel2.Text = "label2";
            // 
            // wm2IRLabel1
            // 
            this.wm2IRLabel1.AutoSize = true;
            this.wm2IRLabel1.Location = new System.Drawing.Point(265, 402);
            this.wm2IRLabel1.Name = "wm2IRLabel1";
            this.wm2IRLabel1.Size = new System.Drawing.Size(65, 13);
            this.wm2IRLabel1.TabIndex = 15;
            this.wm2IRLabel1.Text = "{X = XXX.X}";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.CalibrateStereoButton);
            this.groupBox1.Controls.Add(this.captureTwoCountabel);
            this.groupBox1.Controls.Add(this.ResetDualCalibButton);
            this.groupBox1.Controls.Add(this.dualcapturebutton);
            this.groupBox1.Controls.Add(this.StereoConnectButton);
            this.groupBox1.Location = new System.Drawing.Point(13, 402);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(246, 384);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dual Wiimote Controls";
            // 
            // CalibrateStereoButton
            // 
            this.CalibrateStereoButton.Location = new System.Drawing.Point(7, 109);
            this.CalibrateStereoButton.Name = "CalibrateStereoButton";
            this.CalibrateStereoButton.Size = new System.Drawing.Size(173, 23);
            this.CalibrateStereoButton.TabIndex = 17;
            this.CalibrateStereoButton.Text = "Calibrate Stereo Rig";
            this.CalibrateStereoButton.UseVisualStyleBackColor = true;
            this.CalibrateStereoButton.Click += new System.EventHandler(this.CalibrateStereoButton_Click);
            // 
            // captureTwoCountabel
            // 
            this.captureTwoCountabel.AutoSize = true;
            this.captureTwoCountabel.Location = new System.Drawing.Point(124, 54);
            this.captureTwoCountabel.Name = "captureTwoCountabel";
            this.captureTwoCountabel.Size = new System.Drawing.Size(56, 13);
            this.captureTwoCountabel.TabIndex = 16;
            this.captureTwoCountabel.Text = "Captured: ";
            // 
            // ResetDualCalibButton
            // 
            this.ResetDualCalibButton.Location = new System.Drawing.Point(7, 79);
            this.ResetDualCalibButton.Name = "ResetDualCalibButton";
            this.ResetDualCalibButton.Size = new System.Drawing.Size(173, 23);
            this.ResetDualCalibButton.TabIndex = 15;
            this.ResetDualCalibButton.Text = "Reset Dual Image Capture";
            this.ResetDualCalibButton.UseVisualStyleBackColor = true;
            this.ResetDualCalibButton.Click += new System.EventHandler(this.ResetDualCalibButton_Click);
            // 
            // dualcapturebutton
            // 
            this.dualcapturebutton.Location = new System.Drawing.Point(7, 49);
            this.dualcapturebutton.Name = "dualcapturebutton";
            this.dualcapturebutton.Size = new System.Drawing.Size(111, 23);
            this.dualcapturebutton.TabIndex = 14;
            this.dualcapturebutton.Text = "Capture Both";
            this.dualcapturebutton.UseVisualStyleBackColor = true;
            this.dualcapturebutton.Click += new System.EventHandler(this.dualcapturebutton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.button1);
            this.groupBox2.Controls.Add(this.capturebutton);
            this.groupBox2.Controls.Add(this.CalibrateWM1Button);
            this.groupBox2.Controls.Add(this.ResetCalibButton);
            this.groupBox2.Controls.Add(this.captureOneCountLabel);
            this.groupBox2.Controls.Add(this.countsrcsbutton);
            this.groupBox2.Controls.Add(this.IRpointslabel);
            this.groupBox2.Location = new System.Drawing.Point(13, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(241, 355);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single Wiimote Controls";
            // 
            // wm1IRSourceslabel
            // 
            this.wm1IRSourceslabel.AutoSize = true;
            this.wm1IRSourceslabel.Location = new System.Drawing.Point(268, 122);
            this.wm1IRSourceslabel.Name = "wm1IRSourceslabel";
            this.wm1IRSourceslabel.Size = new System.Drawing.Size(35, 13);
            this.wm1IRSourceslabel.TabIndex = 21;
            this.wm1IRSourceslabel.Text = "label1";
            // 
            // wm2IRSourceslabel
            // 
            this.wm2IRSourceslabel.AutoSize = true;
            this.wm2IRSourceslabel.Location = new System.Drawing.Point(266, 511);
            this.wm2IRSourceslabel.Name = "wm2IRSourceslabel";
            this.wm2IRSourceslabel.Size = new System.Drawing.Size(35, 13);
            this.wm2IRSourceslabel.TabIndex = 22;
            this.wm2IRSourceslabel.Text = "label1";
            // 
            // PrintPointsButton
            // 
            this.PrintPointsButton.Location = new System.Drawing.Point(179, 373);
            this.PrintPointsButton.Name = "PrintPointsButton";
            this.PrintPointsButton.Size = new System.Drawing.Size(75, 23);
            this.PrintPointsButton.TabIndex = 18;
            this.PrintPointsButton.Text = "Print Points";
            this.PrintPointsButton.UseVisualStyleBackColor = true;
            this.PrintPointsButton.Click += new System.EventHandler(this.PrintPointsButton_Click);
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(7, 180);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(173, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Test Stereo Calibration";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(876, 778);
            this.Controls.Add(this.PrintPointsButton);
            this.Controls.Add(this.wm2IRSourceslabel);
            this.Controls.Add(this.wm1IRSourceslabel);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.wm2IRLabel4);
            this.Controls.Add(this.wm2IRLabel3);
            this.Controls.Add(this.wm2IRLabel2);
            this.Controls.Add(this.wm2IRLabel1);
            this.Controls.Add(this.DisconnectAllButton);
            this.Controls.Add(this.wm2IRPictureBox);
            this.Controls.Add(this.wm1IRLabel4);
            this.Controls.Add(this.wm1IRLabel3);
            this.Controls.Add(this.wm1IRLabel2);
            this.Controls.Add(this.wm1IRLabel1);
            this.Controls.Add(this.wm1IRPictureBox);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
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
        private System.Windows.Forms.Button DisconnectAllButton;
        private System.Windows.Forms.Label wm2IRLabel4;
        private System.Windows.Forms.Label wm2IRLabel3;
        private System.Windows.Forms.Label wm2IRLabel2;
        private System.Windows.Forms.Label wm2IRLabel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.Button dualcapturebutton;
        private System.Windows.Forms.Button CalibrateStereoButton;
        private System.Windows.Forms.Label captureTwoCountabel;
        private System.Windows.Forms.Button ResetDualCalibButton;
        private System.Windows.Forms.Label wm1IRSourceslabel;
        private System.Windows.Forms.Label wm2IRSourceslabel;
        private System.Windows.Forms.Button PrintPointsButton;
        private System.Windows.Forms.Button button2;
    }
}

