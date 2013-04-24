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
            this.Log3DCheckBox = new System.Windows.Forms.CheckBox();
            this.LogUDCheckBox = new System.Windows.Forms.CheckBox();
            this.LogRawCheckBox = new System.Windows.Forms.CheckBox();
            this.StopLoggingButton = new System.Windows.Forms.Button();
            this.StartLoggingButton = new System.Windows.Forms.Button();
            this.yCalibTextBox = new System.Windows.Forms.TextBox();
            this.xCalibTextBox = new System.Windows.Forms.TextBox();
            this.calibObjSizeButton = new System.Windows.Forms.Button();
            this.showTrackingFormButton = new System.Windows.Forms.Button();
            this.StopTrackingButton = new System.Windows.Forms.Button();
            this.StartTrackingButton = new System.Windows.Forms.Button();
            this.button2 = new System.Windows.Forms.Button();
            this.CalibrateStereoButton = new System.Windows.Forms.Button();
            this.captureTwoCountabel = new System.Windows.Forms.Label();
            this.ResetDualCalibButton = new System.Windows.Forms.Button();
            this.dualcapturebutton = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.wm1IRSourceslabel = new System.Windows.Forms.Label();
            this.wm2IRSourceslabel = new System.Windows.Forms.Label();
            this.PrintPointsButton = new System.Windows.Forms.Button();
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPage1 = new System.Windows.Forms.TabPage();
            this.tabPage2 = new System.Windows.Forms.TabPage();
            this.XposLabel1 = new System.Windows.Forms.Label();
            this.ZposLabel4 = new System.Windows.Forms.Label();
            this.YposLabel1 = new System.Windows.Forms.Label();
            this.YposLabel4 = new System.Windows.Forms.Label();
            this.ZposLabel1 = new System.Windows.Forms.Label();
            this.XposLabel4 = new System.Windows.Forms.Label();
            this.XposLabel2 = new System.Windows.Forms.Label();
            this.ZposLabel3 = new System.Windows.Forms.Label();
            this.YposLabel2 = new System.Windows.Forms.Label();
            this.YposLabel3 = new System.Windows.Forms.Label();
            this.XposLabel3 = new System.Windows.Forms.Label();
            this.ZposLabel2 = new System.Windows.Forms.Label();
            this.ReviewButton = new System.Windows.Forms.Button();
            this.StopReviewButton = new System.Windows.Forms.Button();
            this.PrevCalImageButton = new System.Windows.Forms.Button();
            this.NextCalImageButton = new System.Windows.Forms.Button();
            this.DeleteImageButton = new System.Windows.Forms.Button();
            this.CalibReviewLabel = new System.Windows.Forms.Label();
            this.SaveStereoCalibImagesButton = new System.Windows.Forms.Button();
            this.LoadStereoCalibImagesButton = new System.Windows.Forms.Button();
            this.FrequencyLabel = new System.Windows.Forms.Label();
            this.FreqInputTextBox = new System.Windows.Forms.TextBox();
            this.EnlargeViewsButton = new System.Windows.Forms.Button();
            this.ReduceViewsButton = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.ReduceWM1SensitivityButton = new System.Windows.Forms.Button();
            this.IncreaseWM1SensitivityButton = new System.Windows.Forms.Button();
            this.ReduceWM2SensitivityButton = new System.Windows.Forms.Button();
            this.IncreaseWM2SensitivityButton = new System.Windows.Forms.Button();
            this.label2 = new System.Windows.Forms.Label();
            this.SaveSystemButton = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.tabControl1.SuspendLayout();
            this.tabPage1.SuspendLayout();
            this.tabPage2.SuspendLayout();
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
            this.IRpointslabel.Location = new System.Drawing.Point(81, 115);
            this.IRpointslabel.Name = "IRpointslabel";
            this.IRpointslabel.Size = new System.Drawing.Size(66, 13);
            this.IRpointslabel.TabIndex = 2;
            this.IRpointslabel.Text = "IR Sources: ";
            // 
            // countsrcsbutton
            // 
            this.countsrcsbutton.Location = new System.Drawing.Point(6, 110);
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
            this.wm1IRPictureBox.Location = new System.Drawing.Point(460, 4);
            this.wm1IRPictureBox.Name = "wm1IRPictureBox";
            this.wm1IRPictureBox.Size = new System.Drawing.Size(385, 237);
            this.wm1IRPictureBox.TabIndex = 4;
            this.wm1IRPictureBox.TabStop = false;
            // 
            // wm1IRLabel1
            // 
            this.wm1IRLabel1.AutoSize = true;
            this.wm1IRLabel1.Location = new System.Drawing.Point(291, 23);
            this.wm1IRLabel1.Name = "wm1IRLabel1";
            this.wm1IRLabel1.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel1.TabIndex = 5;
            this.wm1IRLabel1.Text = "label1";
            // 
            // wm1IRLabel2
            // 
            this.wm1IRLabel2.AutoSize = true;
            this.wm1IRLabel2.Location = new System.Drawing.Point(291, 44);
            this.wm1IRLabel2.Name = "wm1IRLabel2";
            this.wm1IRLabel2.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel2.TabIndex = 6;
            this.wm1IRLabel2.Text = "label2";
            // 
            // wm1IRLabel3
            // 
            this.wm1IRLabel3.AutoSize = true;
            this.wm1IRLabel3.Location = new System.Drawing.Point(291, 65);
            this.wm1IRLabel3.Name = "wm1IRLabel3";
            this.wm1IRLabel3.Size = new System.Drawing.Size(35, 13);
            this.wm1IRLabel3.TabIndex = 7;
            this.wm1IRLabel3.Text = "label3";
            // 
            // wm1IRLabel4
            // 
            this.wm1IRLabel4.AutoSize = true;
            this.wm1IRLabel4.Location = new System.Drawing.Point(291, 86);
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
            this.CalibrateWM1Button.Location = new System.Drawing.Point(130, 81);
            this.CalibrateWM1Button.Name = "CalibrateWM1Button";
            this.CalibrateWM1Button.Size = new System.Drawing.Size(125, 23);
            this.CalibrateWM1Button.TabIndex = 10;
            this.CalibrateWM1Button.Text = "Calibrate Single Wiimote";
            this.CalibrateWM1Button.UseVisualStyleBackColor = true;
            this.CalibrateWM1Button.Click += new System.EventHandler(this.CalibrateWM1Button_Click);
            // 
            // ResetCalibButton
            // 
            this.ResetCalibButton.Location = new System.Drawing.Point(6, 81);
            this.ResetCalibButton.Name = "ResetCalibButton";
            this.ResetCalibButton.Size = new System.Drawing.Size(122, 23);
            this.ResetCalibButton.TabIndex = 11;
            this.ResetCalibButton.Text = "Reset Image Capture";
            this.ResetCalibButton.UseVisualStyleBackColor = true;
            this.ResetCalibButton.Click += new System.EventHandler(this.ResetCalibButton_Click);
            // 
            // wm2IRPictureBox
            // 
            this.wm2IRPictureBox.Anchor = System.Windows.Forms.AnchorStyles.Left;
            this.wm2IRPictureBox.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.wm2IRPictureBox.Location = new System.Drawing.Point(460, 247);
            this.wm2IRPictureBox.Name = "wm2IRPictureBox";
            this.wm2IRPictureBox.Size = new System.Drawing.Size(385, 262);
            this.wm2IRPictureBox.TabIndex = 12;
            this.wm2IRPictureBox.TabStop = false;
            // 
            // StereoConnectButton
            // 
            this.StereoConnectButton.Location = new System.Drawing.Point(65, 21);
            this.StereoConnectButton.Name = "StereoConnectButton";
            this.StereoConnectButton.Size = new System.Drawing.Size(111, 23);
            this.StereoConnectButton.TabIndex = 13;
            this.StereoConnectButton.Text = "Connect 2";
            this.StereoConnectButton.UseVisualStyleBackColor = true;
            this.StereoConnectButton.Click += new System.EventHandler(this.StereoConnectButton_Click);
            // 
            // DisconnectAllButton
            // 
            this.DisconnectAllButton.Location = new System.Drawing.Point(3, 156);
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
            this.wm2IRLabel4.Location = new System.Drawing.Point(369, 86);
            this.wm2IRLabel4.Name = "wm2IRLabel4";
            this.wm2IRLabel4.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel4.TabIndex = 18;
            this.wm2IRLabel4.Text = "label4";
            // 
            // wm2IRLabel3
            // 
            this.wm2IRLabel3.AutoSize = true;
            this.wm2IRLabel3.Location = new System.Drawing.Point(369, 65);
            this.wm2IRLabel3.Name = "wm2IRLabel3";
            this.wm2IRLabel3.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel3.TabIndex = 17;
            this.wm2IRLabel3.Text = "label3";
            // 
            // wm2IRLabel2
            // 
            this.wm2IRLabel2.AutoSize = true;
            this.wm2IRLabel2.Location = new System.Drawing.Point(369, 44);
            this.wm2IRLabel2.Name = "wm2IRLabel2";
            this.wm2IRLabel2.Size = new System.Drawing.Size(35, 13);
            this.wm2IRLabel2.TabIndex = 16;
            this.wm2IRLabel2.Text = "label2";
            // 
            // wm2IRLabel1
            // 
            this.wm2IRLabel1.AutoSize = true;
            this.wm2IRLabel1.Location = new System.Drawing.Point(369, 23);
            this.wm2IRLabel1.Name = "wm2IRLabel1";
            this.wm2IRLabel1.Size = new System.Drawing.Size(65, 13);
            this.wm2IRLabel1.TabIndex = 15;
            this.wm2IRLabel1.Text = "{X = XXX.X}";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.FreqInputTextBox);
            this.groupBox1.Controls.Add(this.FrequencyLabel);
            this.groupBox1.Controls.Add(this.LoadStereoCalibImagesButton);
            this.groupBox1.Controls.Add(this.SaveStereoCalibImagesButton);
            this.groupBox1.Controls.Add(this.Log3DCheckBox);
            this.groupBox1.Controls.Add(this.LogUDCheckBox);
            this.groupBox1.Controls.Add(this.LogRawCheckBox);
            this.groupBox1.Controls.Add(this.StopLoggingButton);
            this.groupBox1.Controls.Add(this.StartLoggingButton);
            this.groupBox1.Controls.Add(this.yCalibTextBox);
            this.groupBox1.Controls.Add(this.xCalibTextBox);
            this.groupBox1.Controls.Add(this.calibObjSizeButton);
            this.groupBox1.Controls.Add(this.showTrackingFormButton);
            this.groupBox1.Controls.Add(this.StopTrackingButton);
            this.groupBox1.Controls.Add(this.StartTrackingButton);
            this.groupBox1.Controls.Add(this.button2);
            this.groupBox1.Controls.Add(this.CalibrateStereoButton);
            this.groupBox1.Controls.Add(this.captureTwoCountabel);
            this.groupBox1.Controls.Add(this.ResetDualCalibButton);
            this.groupBox1.Controls.Add(this.dualcapturebutton);
            this.groupBox1.Controls.Add(this.StereoConnectButton);
            this.groupBox1.Location = new System.Drawing.Point(6, 185);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(261, 407);
            this.groupBox1.TabIndex = 19;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Dual Wiimote Controls";
            // 
            // Log3DCheckBox
            // 
            this.Log3DCheckBox.AutoSize = true;
            this.Log3DCheckBox.Location = new System.Drawing.Point(173, 221);
            this.Log3DCheckBox.Name = "Log3DCheckBox";
            this.Log3DCheckBox.Size = new System.Drawing.Size(61, 17);
            this.Log3DCheckBox.TabIndex = 29;
            this.Log3DCheckBox.Text = "Log 3D";
            this.Log3DCheckBox.UseVisualStyleBackColor = true;
            // 
            // LogUDCheckBox
            // 
            this.LogUDCheckBox.AutoSize = true;
            this.LogUDCheckBox.Location = new System.Drawing.Point(103, 221);
            this.LogUDCheckBox.Name = "LogUDCheckBox";
            this.LogUDCheckBox.Size = new System.Drawing.Size(63, 17);
            this.LogUDCheckBox.TabIndex = 28;
            this.LogUDCheckBox.Text = "Log UD";
            this.LogUDCheckBox.UseVisualStyleBackColor = true;
            // 
            // LogRawCheckBox
            // 
            this.LogRawCheckBox.AutoSize = true;
            this.LogRawCheckBox.Location = new System.Drawing.Point(27, 221);
            this.LogRawCheckBox.Name = "LogRawCheckBox";
            this.LogRawCheckBox.Size = new System.Drawing.Size(69, 17);
            this.LogRawCheckBox.TabIndex = 27;
            this.LogRawCheckBox.Text = "Log Raw";
            this.LogRawCheckBox.UseVisualStyleBackColor = true;
            // 
            // StopLoggingButton
            // 
            this.StopLoggingButton.Enabled = false;
            this.StopLoggingButton.Location = new System.Drawing.Point(7, 269);
            this.StopLoggingButton.Name = "StopLoggingButton";
            this.StopLoggingButton.Size = new System.Drawing.Size(91, 23);
            this.StopLoggingButton.TabIndex = 26;
            this.StopLoggingButton.Text = "Stop Logging";
            this.StopLoggingButton.UseVisualStyleBackColor = true;
            this.StopLoggingButton.Click += new System.EventHandler(this.StopLoggingButton_Click);
            // 
            // StartLoggingButton
            // 
            this.StartLoggingButton.Location = new System.Drawing.Point(6, 244);
            this.StartLoggingButton.Name = "StartLoggingButton";
            this.StartLoggingButton.Size = new System.Drawing.Size(91, 23);
            this.StartLoggingButton.TabIndex = 25;
            this.StartLoggingButton.Text = "Start Logging";
            this.StartLoggingButton.UseVisualStyleBackColor = true;
            this.StartLoggingButton.Click += new System.EventHandler(this.StartLoggingButton_Click);
            // 
            // yCalibTextBox
            // 
            this.yCalibTextBox.Location = new System.Drawing.Point(182, 52);
            this.yCalibTextBox.Name = "yCalibTextBox";
            this.yCalibTextBox.Size = new System.Drawing.Size(52, 20);
            this.yCalibTextBox.TabIndex = 24;
            // 
            // xCalibTextBox
            // 
            this.xCalibTextBox.Location = new System.Drawing.Point(124, 52);
            this.xCalibTextBox.Name = "xCalibTextBox";
            this.xCalibTextBox.Size = new System.Drawing.Size(52, 20);
            this.xCalibTextBox.TabIndex = 23;
            // 
            // calibObjSizeButton
            // 
            this.calibObjSizeButton.Location = new System.Drawing.Point(7, 50);
            this.calibObjSizeButton.Name = "calibObjSizeButton";
            this.calibObjSizeButton.Size = new System.Drawing.Size(111, 23);
            this.calibObjSizeButton.TabIndex = 22;
            this.calibObjSizeButton.Text = "Set Object Size";
            this.calibObjSizeButton.UseVisualStyleBackColor = true;
            this.calibObjSizeButton.Click += new System.EventHandler(this.calibObjSizeButton_Click);
            // 
            // showTrackingFormButton
            // 
            this.showTrackingFormButton.Enabled = false;
            this.showTrackingFormButton.Location = new System.Drawing.Point(124, 377);
            this.showTrackingFormButton.Name = "showTrackingFormButton";
            this.showTrackingFormButton.Size = new System.Drawing.Size(131, 23);
            this.showTrackingFormButton.TabIndex = 21;
            this.showTrackingFormButton.Text = "Show Tracking Form";
            this.showTrackingFormButton.UseVisualStyleBackColor = true;
            this.showTrackingFormButton.Click += new System.EventHandler(this.showTrackingFormButton_Click);
            // 
            // StopTrackingButton
            // 
            this.StopTrackingButton.Enabled = false;
            this.StopTrackingButton.Location = new System.Drawing.Point(151, 348);
            this.StopTrackingButton.Name = "StopTrackingButton";
            this.StopTrackingButton.Size = new System.Drawing.Size(104, 23);
            this.StopTrackingButton.TabIndex = 20;
            this.StopTrackingButton.Text = "Stop Tracking";
            this.StopTrackingButton.UseVisualStyleBackColor = true;
            this.StopTrackingButton.Click += new System.EventHandler(this.StopTrackingButton_Click);
            // 
            // StartTrackingButton
            // 
            this.StartTrackingButton.Enabled = false;
            this.StartTrackingButton.Location = new System.Drawing.Point(51, 348);
            this.StartTrackingButton.Name = "StartTrackingButton";
            this.StartTrackingButton.Size = new System.Drawing.Size(96, 23);
            this.StartTrackingButton.TabIndex = 19;
            this.StartTrackingButton.Text = "Begin Tracking";
            this.StartTrackingButton.UseVisualStyleBackColor = true;
            this.StartTrackingButton.Click += new System.EventHandler(this.StartTrackingButton_Click);
            // 
            // button2
            // 
            this.button2.Enabled = false;
            this.button2.Location = new System.Drawing.Point(130, 319);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(125, 23);
            this.button2.TabIndex = 18;
            this.button2.Text = "Test Stereo Calibration";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.button2_Click);
            // 
            // CalibrateStereoButton
            // 
            this.CalibrateStereoButton.Location = new System.Drawing.Point(30, 166);
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
            this.captureTwoCountabel.Location = new System.Drawing.Point(121, 84);
            this.captureTwoCountabel.Name = "captureTwoCountabel";
            this.captureTwoCountabel.Size = new System.Drawing.Size(56, 13);
            this.captureTwoCountabel.TabIndex = 16;
            this.captureTwoCountabel.Text = "Captured: ";
            // 
            // ResetDualCalibButton
            // 
            this.ResetDualCalibButton.Location = new System.Drawing.Point(7, 137);
            this.ResetDualCalibButton.Name = "ResetDualCalibButton";
            this.ResetDualCalibButton.Size = new System.Drawing.Size(140, 23);
            this.ResetDualCalibButton.TabIndex = 15;
            this.ResetDualCalibButton.Text = "Reset Dual Image Capture";
            this.ResetDualCalibButton.UseVisualStyleBackColor = true;
            this.ResetDualCalibButton.Click += new System.EventHandler(this.ResetDualCalibButton_Click);
            // 
            // dualcapturebutton
            // 
            this.dualcapturebutton.Location = new System.Drawing.Point(7, 79);
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
            this.groupBox2.Location = new System.Drawing.Point(6, 6);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(261, 144);
            this.groupBox2.TabIndex = 20;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Single Wiimote Controls";
            // 
            // wm1IRSourceslabel
            // 
            this.wm1IRSourceslabel.AutoSize = true;
            this.wm1IRSourceslabel.Location = new System.Drawing.Point(291, 107);
            this.wm1IRSourceslabel.Name = "wm1IRSourceslabel";
            this.wm1IRSourceslabel.Size = new System.Drawing.Size(35, 13);
            this.wm1IRSourceslabel.TabIndex = 21;
            this.wm1IRSourceslabel.Text = "label1";
            // 
            // wm2IRSourceslabel
            // 
            this.wm2IRSourceslabel.AutoSize = true;
            this.wm2IRSourceslabel.Location = new System.Drawing.Point(369, 107);
            this.wm2IRSourceslabel.Name = "wm2IRSourceslabel";
            this.wm2IRSourceslabel.Size = new System.Drawing.Size(35, 13);
            this.wm2IRSourceslabel.TabIndex = 22;
            this.wm2IRSourceslabel.Text = "label1";
            // 
            // PrintPointsButton
            // 
            this.PrintPointsButton.Location = new System.Drawing.Point(186, 156);
            this.PrintPointsButton.Name = "PrintPointsButton";
            this.PrintPointsButton.Size = new System.Drawing.Size(75, 23);
            this.PrintPointsButton.TabIndex = 18;
            this.PrintPointsButton.Text = "Print Points";
            this.PrintPointsButton.UseVisualStyleBackColor = true;
            this.PrintPointsButton.Click += new System.EventHandler(this.PrintPointsButton_Click);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPage1);
            this.tabControl1.Controls.Add(this.tabPage2);
            this.tabControl1.Location = new System.Drawing.Point(3, 1);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(281, 630);
            this.tabControl1.TabIndex = 23;
            // 
            // tabPage1
            // 
            this.tabPage1.Controls.Add(this.groupBox2);
            this.tabPage1.Controls.Add(this.PrintPointsButton);
            this.tabPage1.Controls.Add(this.groupBox1);
            this.tabPage1.Controls.Add(this.DisconnectAllButton);
            this.tabPage1.Location = new System.Drawing.Point(4, 22);
            this.tabPage1.Name = "tabPage1";
            this.tabPage1.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage1.Size = new System.Drawing.Size(273, 604);
            this.tabPage1.TabIndex = 0;
            this.tabPage1.Text = "Setup";
            this.tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            this.tabPage2.Controls.Add(this.XposLabel1);
            this.tabPage2.Controls.Add(this.ZposLabel4);
            this.tabPage2.Controls.Add(this.YposLabel1);
            this.tabPage2.Controls.Add(this.YposLabel4);
            this.tabPage2.Controls.Add(this.ZposLabel1);
            this.tabPage2.Controls.Add(this.XposLabel4);
            this.tabPage2.Controls.Add(this.XposLabel2);
            this.tabPage2.Controls.Add(this.ZposLabel3);
            this.tabPage2.Controls.Add(this.YposLabel2);
            this.tabPage2.Controls.Add(this.YposLabel3);
            this.tabPage2.Controls.Add(this.XposLabel3);
            this.tabPage2.Controls.Add(this.ZposLabel2);
            this.tabPage2.Location = new System.Drawing.Point(4, 22);
            this.tabPage2.Name = "tabPage2";
            this.tabPage2.Padding = new System.Windows.Forms.Padding(3);
            this.tabPage2.Size = new System.Drawing.Size(273, 598);
            this.tabPage2.TabIndex = 1;
            this.tabPage2.Text = "Tracking";
            this.tabPage2.UseVisualStyleBackColor = true;
            // 
            // XposLabel1
            // 
            this.XposLabel1.AutoSize = true;
            this.XposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel1.Location = new System.Drawing.Point(6, 3);
            this.XposLabel1.Name = "XposLabel1";
            this.XposLabel1.Size = new System.Drawing.Size(58, 39);
            this.XposLabel1.TabIndex = 12;
            this.XposLabel1.Text = "X: ";
            // 
            // ZposLabel4
            // 
            this.ZposLabel4.AutoSize = true;
            this.ZposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel4.Location = new System.Drawing.Point(3, 544);
            this.ZposLabel4.Name = "ZposLabel4";
            this.ZposLabel4.Size = new System.Drawing.Size(56, 39);
            this.ZposLabel4.TabIndex = 23;
            this.ZposLabel4.Text = "Z: ";
            // 
            // YposLabel1
            // 
            this.YposLabel1.AutoSize = true;
            this.YposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel1.Location = new System.Drawing.Point(3, 42);
            this.YposLabel1.Name = "YposLabel1";
            this.YposLabel1.Size = new System.Drawing.Size(57, 39);
            this.YposLabel1.TabIndex = 13;
            this.YposLabel1.Text = "Y: ";
            // 
            // YposLabel4
            // 
            this.YposLabel4.AutoSize = true;
            this.YposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel4.Location = new System.Drawing.Point(4, 505);
            this.YposLabel4.Name = "YposLabel4";
            this.YposLabel4.Size = new System.Drawing.Size(57, 39);
            this.YposLabel4.TabIndex = 22;
            this.YposLabel4.Text = "Y: ";
            // 
            // ZposLabel1
            // 
            this.ZposLabel1.AutoSize = true;
            this.ZposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel1.Location = new System.Drawing.Point(4, 81);
            this.ZposLabel1.Name = "ZposLabel1";
            this.ZposLabel1.Size = new System.Drawing.Size(56, 39);
            this.ZposLabel1.TabIndex = 14;
            this.ZposLabel1.Text = "Z: ";
            // 
            // XposLabel4
            // 
            this.XposLabel4.AutoSize = true;
            this.XposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel4.Location = new System.Drawing.Point(4, 466);
            this.XposLabel4.Name = "XposLabel4";
            this.XposLabel4.Size = new System.Drawing.Size(58, 39);
            this.XposLabel4.TabIndex = 21;
            this.XposLabel4.Text = "X: ";
            // 
            // XposLabel2
            // 
            this.XposLabel2.AutoSize = true;
            this.XposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel2.Location = new System.Drawing.Point(3, 149);
            this.XposLabel2.Name = "XposLabel2";
            this.XposLabel2.Size = new System.Drawing.Size(58, 39);
            this.XposLabel2.TabIndex = 15;
            this.XposLabel2.Text = "X: ";
            // 
            // ZposLabel3
            // 
            this.ZposLabel3.AutoSize = true;
            this.ZposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel3.Location = new System.Drawing.Point(7, 382);
            this.ZposLabel3.Name = "ZposLabel3";
            this.ZposLabel3.Size = new System.Drawing.Size(56, 39);
            this.ZposLabel3.TabIndex = 20;
            this.ZposLabel3.Text = "Z: ";
            // 
            // YposLabel2
            // 
            this.YposLabel2.AutoSize = true;
            this.YposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel2.Location = new System.Drawing.Point(3, 188);
            this.YposLabel2.Name = "YposLabel2";
            this.YposLabel2.Size = new System.Drawing.Size(57, 39);
            this.YposLabel2.TabIndex = 16;
            this.YposLabel2.Text = "Y: ";
            // 
            // YposLabel3
            // 
            this.YposLabel3.AutoSize = true;
            this.YposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel3.Location = new System.Drawing.Point(6, 343);
            this.YposLabel3.Name = "YposLabel3";
            this.YposLabel3.Size = new System.Drawing.Size(57, 39);
            this.YposLabel3.TabIndex = 19;
            this.YposLabel3.Text = "Y: ";
            // 
            // XposLabel3
            // 
            this.XposLabel3.AutoSize = true;
            this.XposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel3.Location = new System.Drawing.Point(6, 304);
            this.XposLabel3.Name = "XposLabel3";
            this.XposLabel3.Size = new System.Drawing.Size(58, 39);
            this.XposLabel3.TabIndex = 18;
            this.XposLabel3.Text = "X: ";
            // 
            // ZposLabel2
            // 
            this.ZposLabel2.AutoSize = true;
            this.ZposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel2.Location = new System.Drawing.Point(3, 232);
            this.ZposLabel2.Name = "ZposLabel2";
            this.ZposLabel2.Size = new System.Drawing.Size(56, 39);
            this.ZposLabel2.TabIndex = 17;
            this.ZposLabel2.Text = "Z: ";
            // 
            // ReviewButton
            // 
            this.ReviewButton.Location = new System.Drawing.Point(290, 162);
            this.ReviewButton.Name = "ReviewButton";
            this.ReviewButton.Size = new System.Drawing.Size(75, 23);
            this.ReviewButton.TabIndex = 0;
            this.ReviewButton.Text = "Review Cal";
            this.ReviewButton.UseVisualStyleBackColor = true;
            this.ReviewButton.Click += new System.EventHandler(this.ReviewButton_Click);
            // 
            // StopReviewButton
            // 
            this.StopReviewButton.Location = new System.Drawing.Point(371, 162);
            this.StopReviewButton.Name = "StopReviewButton";
            this.StopReviewButton.Size = new System.Drawing.Size(75, 23);
            this.StopReviewButton.TabIndex = 24;
            this.StopReviewButton.Text = "Stop Rev";
            this.StopReviewButton.UseVisualStyleBackColor = true;
            this.StopReviewButton.Click += new System.EventHandler(this.StopReviewButton_Click);
            // 
            // PrevCalImageButton
            // 
            this.PrevCalImageButton.Location = new System.Drawing.Point(290, 191);
            this.PrevCalImageButton.Name = "PrevCalImageButton";
            this.PrevCalImageButton.Size = new System.Drawing.Size(35, 23);
            this.PrevCalImageButton.TabIndex = 25;
            this.PrevCalImageButton.Text = "<-";
            this.PrevCalImageButton.UseVisualStyleBackColor = true;
            this.PrevCalImageButton.Click += new System.EventHandler(this.PrevCalImageButton_Click);
            // 
            // NextCalImageButton
            // 
            this.NextCalImageButton.Location = new System.Drawing.Point(330, 191);
            this.NextCalImageButton.Name = "NextCalImageButton";
            this.NextCalImageButton.Size = new System.Drawing.Size(35, 23);
            this.NextCalImageButton.TabIndex = 26;
            this.NextCalImageButton.Text = "->";
            this.NextCalImageButton.UseVisualStyleBackColor = true;
            this.NextCalImageButton.Click += new System.EventHandler(this.NextCalImageButton_Click);
            // 
            // DeleteImageButton
            // 
            this.DeleteImageButton.Enabled = false;
            this.DeleteImageButton.Location = new System.Drawing.Point(371, 191);
            this.DeleteImageButton.Name = "DeleteImageButton";
            this.DeleteImageButton.Size = new System.Drawing.Size(75, 23);
            this.DeleteImageButton.TabIndex = 27;
            this.DeleteImageButton.Text = "Delete Im";
            this.DeleteImageButton.UseVisualStyleBackColor = true;
            this.DeleteImageButton.Click += new System.EventHandler(this.DeleteImageButton_Click);
            // 
            // CalibReviewLabel
            // 
            this.CalibReviewLabel.AutoSize = true;
            this.CalibReviewLabel.Location = new System.Drawing.Point(287, 225);
            this.CalibReviewLabel.Name = "CalibReviewLabel";
            this.CalibReviewLabel.Size = new System.Drawing.Size(39, 13);
            this.CalibReviewLabel.TabIndex = 28;
            this.CalibReviewLabel.Text = "Image:";
            // 
            // SaveStereoCalibImagesButton
            // 
            this.SaveStereoCalibImagesButton.Enabled = false;
            this.SaveStereoCalibImagesButton.Location = new System.Drawing.Point(7, 108);
            this.SaveStereoCalibImagesButton.Name = "SaveStereoCalibImagesButton";
            this.SaveStereoCalibImagesButton.Size = new System.Drawing.Size(91, 23);
            this.SaveStereoCalibImagesButton.TabIndex = 30;
            this.SaveStereoCalibImagesButton.Text = "Save Calib Im";
            this.SaveStereoCalibImagesButton.UseVisualStyleBackColor = true;
            this.SaveStereoCalibImagesButton.Click += new System.EventHandler(this.SaveStereoCalibImagesButton_Click);
            // 
            // LoadStereoCalibImagesButton
            // 
            this.LoadStereoCalibImagesButton.Location = new System.Drawing.Point(104, 108);
            this.LoadStereoCalibImagesButton.Name = "LoadStereoCalibImagesButton";
            this.LoadStereoCalibImagesButton.Size = new System.Drawing.Size(86, 23);
            this.LoadStereoCalibImagesButton.TabIndex = 31;
            this.LoadStereoCalibImagesButton.Text = "Load Calib Im";
            this.LoadStereoCalibImagesButton.UseVisualStyleBackColor = true;
            this.LoadStereoCalibImagesButton.Click += new System.EventHandler(this.LoadStereoCalibImagesButton_Click);
            // 
            // FrequencyLabel
            // 
            this.FrequencyLabel.AutoSize = true;
            this.FrequencyLabel.Location = new System.Drawing.Point(103, 249);
            this.FrequencyLabel.Name = "FrequencyLabel";
            this.FrequencyLabel.Size = new System.Drawing.Size(55, 13);
            this.FrequencyLabel.TabIndex = 32;
            this.FrequencyLabel.Text = "Sample/s:";
            // 
            // FreqInputTextBox
            // 
            this.FreqInputTextBox.Location = new System.Drawing.Point(164, 246);
            this.FreqInputTextBox.Name = "FreqInputTextBox";
            this.FreqInputTextBox.Size = new System.Drawing.Size(39, 20);
            this.FreqInputTextBox.TabIndex = 33;
            this.FreqInputTextBox.Text = "2";
            // 
            // EnlargeViewsButton
            // 
            this.EnlargeViewsButton.Location = new System.Drawing.Point(328, 123);
            this.EnlargeViewsButton.Name = "EnlargeViewsButton";
            this.EnlargeViewsButton.Size = new System.Drawing.Size(23, 23);
            this.EnlargeViewsButton.TabIndex = 29;
            this.EnlargeViewsButton.Text = "+";
            this.EnlargeViewsButton.UseVisualStyleBackColor = true;
            this.EnlargeViewsButton.Click += new System.EventHandler(this.EnlargeViewsButton_Click);
            // 
            // ReduceViewsButton
            // 
            this.ReduceViewsButton.Location = new System.Drawing.Point(357, 123);
            this.ReduceViewsButton.Name = "ReduceViewsButton";
            this.ReduceViewsButton.Size = new System.Drawing.Size(23, 23);
            this.ReduceViewsButton.TabIndex = 30;
            this.ReduceViewsButton.Text = "-";
            this.ReduceViewsButton.UseVisualStyleBackColor = true;
            this.ReduceViewsButton.Click += new System.EventHandler(this.ReduceViewsButton_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(291, 258);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(136, 13);
            this.label1.TabIndex = 31;
            this.label1.Text = "Adjust Wiimote 1 Sensitivity";
            // 
            // ReduceWM1SensitivityButton
            // 
            this.ReduceWM1SensitivityButton.Location = new System.Drawing.Point(357, 282);
            this.ReduceWM1SensitivityButton.Name = "ReduceWM1SensitivityButton";
            this.ReduceWM1SensitivityButton.Size = new System.Drawing.Size(35, 23);
            this.ReduceWM1SensitivityButton.TabIndex = 33;
            this.ReduceWM1SensitivityButton.Text = "-";
            this.ReduceWM1SensitivityButton.UseVisualStyleBackColor = true;
            this.ReduceWM1SensitivityButton.Click += new System.EventHandler(this.ReduceWM1SensitivityButton_Click);
            // 
            // IncreaseWM1SensitivityButton
            // 
            this.IncreaseWM1SensitivityButton.Location = new System.Drawing.Point(319, 282);
            this.IncreaseWM1SensitivityButton.Name = "IncreaseWM1SensitivityButton";
            this.IncreaseWM1SensitivityButton.Size = new System.Drawing.Size(35, 23);
            this.IncreaseWM1SensitivityButton.TabIndex = 32;
            this.IncreaseWM1SensitivityButton.Text = "+";
            this.IncreaseWM1SensitivityButton.UseVisualStyleBackColor = true;
            this.IncreaseWM1SensitivityButton.Click += new System.EventHandler(this.IncreaseWM1SensitivityButton_Click);
            // 
            // ReduceWM2SensitivityButton
            // 
            this.ReduceWM2SensitivityButton.Location = new System.Drawing.Point(357, 345);
            this.ReduceWM2SensitivityButton.Name = "ReduceWM2SensitivityButton";
            this.ReduceWM2SensitivityButton.Size = new System.Drawing.Size(35, 23);
            this.ReduceWM2SensitivityButton.TabIndex = 36;
            this.ReduceWM2SensitivityButton.Text = "-";
            this.ReduceWM2SensitivityButton.UseVisualStyleBackColor = true;
            this.ReduceWM2SensitivityButton.Click += new System.EventHandler(this.ReduceWM2SensitivityButton_Click);
            // 
            // IncreaseWM2SensitivityButton
            // 
            this.IncreaseWM2SensitivityButton.Location = new System.Drawing.Point(319, 345);
            this.IncreaseWM2SensitivityButton.Name = "IncreaseWM2SensitivityButton";
            this.IncreaseWM2SensitivityButton.Size = new System.Drawing.Size(35, 23);
            this.IncreaseWM2SensitivityButton.TabIndex = 35;
            this.IncreaseWM2SensitivityButton.Text = "+";
            this.IncreaseWM2SensitivityButton.UseVisualStyleBackColor = true;
            this.IncreaseWM2SensitivityButton.Click += new System.EventHandler(this.IncreaseWM2SensitivityButton_Click);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(291, 321);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(136, 13);
            this.label2.TabIndex = 34;
            this.label2.Text = "Adjust Wiimote 2 Sensitivity";
            // 
            // SaveSystemButton
            // 
            this.SaveSystemButton.Enabled = false;
            this.SaveSystemButton.Location = new System.Drawing.Point(286, 391);
            this.SaveSystemButton.Name = "SaveSystemButton";
            this.SaveSystemButton.Size = new System.Drawing.Size(160, 23);
            this.SaveSystemButton.TabIndex = 37;
            this.SaveSystemButton.Text = "Save System";
            this.SaveSystemButton.UseVisualStyleBackColor = true;
            this.SaveSystemButton.Click += new System.EventHandler(this.SaveSystemButton_Click);
            // 
            // button3
            // 
            this.button3.Enabled = false;
            this.button3.Location = new System.Drawing.Point(286, 420);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(160, 23);
            this.button3.TabIndex = 38;
            this.button3.Text = "Load System";
            this.button3.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.AutoSizeMode = System.Windows.Forms.AutoSizeMode.GrowAndShrink;
            this.ClientSize = new System.Drawing.Size(855, 639);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.SaveSystemButton);
            this.Controls.Add(this.ReduceWM2SensitivityButton);
            this.Controls.Add(this.IncreaseWM2SensitivityButton);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.ReduceWM1SensitivityButton);
            this.Controls.Add(this.IncreaseWM1SensitivityButton);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.ReduceViewsButton);
            this.Controls.Add(this.EnlargeViewsButton);
            this.Controls.Add(this.CalibReviewLabel);
            this.Controls.Add(this.DeleteImageButton);
            this.Controls.Add(this.NextCalImageButton);
            this.Controls.Add(this.PrevCalImageButton);
            this.Controls.Add(this.StopReviewButton);
            this.Controls.Add(this.ReviewButton);
            this.Controls.Add(this.tabControl1);
            this.Controls.Add(this.wm1IRPictureBox);
            this.Controls.Add(this.wm2IRLabel1);
            this.Controls.Add(this.wm2IRLabel2);
            this.Controls.Add(this.wm2IRPictureBox);
            this.Controls.Add(this.wm2IRSourceslabel);
            this.Controls.Add(this.wm2IRLabel3);
            this.Controls.Add(this.wm1IRLabel1);
            this.Controls.Add(this.wm1IRLabel4);
            this.Controls.Add(this.wm1IRSourceslabel);
            this.Controls.Add(this.wm2IRLabel4);
            this.Controls.Add(this.wm1IRLabel2);
            this.Controls.Add(this.wm1IRLabel3);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.wm1IRPictureBox)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.wm2IRPictureBox)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.tabControl1.ResumeLayout(false);
            this.tabPage1.ResumeLayout(false);
            this.tabPage2.ResumeLayout(false);
            this.tabPage2.PerformLayout();
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
        private System.Windows.Forms.Button StartTrackingButton;
        private System.Windows.Forms.Button StopTrackingButton;
        private System.Windows.Forms.Button showTrackingFormButton;
        private System.Windows.Forms.TextBox yCalibTextBox;
        private System.Windows.Forms.TextBox xCalibTextBox;
        private System.Windows.Forms.Button calibObjSizeButton;
        private System.Windows.Forms.Button StartLoggingButton;
        private System.Windows.Forms.Button StopLoggingButton;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPage1;
        private System.Windows.Forms.TabPage tabPage2;
        private System.Windows.Forms.Label XposLabel1;
        private System.Windows.Forms.Label ZposLabel4;
        private System.Windows.Forms.Label YposLabel1;
        private System.Windows.Forms.Label YposLabel4;
        private System.Windows.Forms.Label ZposLabel1;
        private System.Windows.Forms.Label XposLabel4;
        private System.Windows.Forms.Label XposLabel2;
        private System.Windows.Forms.Label ZposLabel3;
        private System.Windows.Forms.Label YposLabel2;
        private System.Windows.Forms.Label YposLabel3;
        private System.Windows.Forms.Label XposLabel3;
        private System.Windows.Forms.Label ZposLabel2;
        private System.Windows.Forms.Button ReviewButton;
        private System.Windows.Forms.Button StopReviewButton;
        private System.Windows.Forms.Button PrevCalImageButton;
        private System.Windows.Forms.Button NextCalImageButton;
        private System.Windows.Forms.Button DeleteImageButton;
        private System.Windows.Forms.Label CalibReviewLabel;
        private System.Windows.Forms.CheckBox Log3DCheckBox;
        private System.Windows.Forms.CheckBox LogUDCheckBox;
        private System.Windows.Forms.CheckBox LogRawCheckBox;
        private System.Windows.Forms.Button SaveStereoCalibImagesButton;
        private System.Windows.Forms.Button LoadStereoCalibImagesButton;
        private System.Windows.Forms.TextBox FreqInputTextBox;
        private System.Windows.Forms.Label FrequencyLabel;
        private System.Windows.Forms.Button EnlargeViewsButton;
        private System.Windows.Forms.Button ReduceViewsButton;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button ReduceWM1SensitivityButton;
        private System.Windows.Forms.Button IncreaseWM1SensitivityButton;
        private System.Windows.Forms.Button ReduceWM2SensitivityButton;
        private System.Windows.Forms.Button IncreaseWM2SensitivityButton;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button SaveSystemButton;
        private System.Windows.Forms.Button button3;
    }
}

