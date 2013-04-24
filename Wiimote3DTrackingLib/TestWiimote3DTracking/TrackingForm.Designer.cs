namespace TestWiimote3DTracking
{
    partial class TrackingForm
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
            this.components = new System.ComponentModel.Container();
            this.XposLabel1 = new System.Windows.Forms.Label();
            this.YposLabel1 = new System.Windows.Forms.Label();
            this.ZposLabel1 = new System.Windows.Forms.Label();
            this.ZposLabel2 = new System.Windows.Forms.Label();
            this.YposLabel2 = new System.Windows.Forms.Label();
            this.XposLabel2 = new System.Windows.Forms.Label();
            this.ZposLabel4 = new System.Windows.Forms.Label();
            this.YposLabel4 = new System.Windows.Forms.Label();
            this.XposLabel4 = new System.Windows.Forms.Label();
            this.ZposLabel3 = new System.Windows.Forms.Label();
            this.YposLabel3 = new System.Windows.Forms.Label();
            this.XposLabel3 = new System.Windows.Forms.Label();
            this.trackingTabControl = new System.Windows.Forms.TabControl();
            this.textTrackingTabPage = new System.Windows.Forms.TabPage();
            this.plotTrackingTabPage = new System.Windows.Forms.TabPage();
            this.rZTrackBar = new System.Windows.Forms.TrackBar();
            this.rXTrackBar = new System.Windows.Forms.TrackBar();
            this.rYTrackBar = new System.Windows.Forms.TrackBar();
            this.plot3DPictureBox = new System.Windows.Forms.PictureBox();
            this.optionsTabPage = new System.Windows.Forms.TabPage();
            this.send2MatCheckBox = new System.Windows.Forms.CheckBox();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.do3DPlotCheckBox = new System.Windows.Forms.CheckBox();
            this.trackingTabControl.SuspendLayout();
            this.textTrackingTabPage.SuspendLayout();
            this.plotTrackingTabPage.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rZTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rXTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.rYTrackBar)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.plot3DPictureBox)).BeginInit();
            this.optionsTabPage.SuspendLayout();
            this.SuspendLayout();
            // 
            // XposLabel1
            // 
            this.XposLabel1.AutoSize = true;
            this.XposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel1.Location = new System.Drawing.Point(6, 3);
            this.XposLabel1.Name = "XposLabel1";
            this.XposLabel1.Size = new System.Drawing.Size(115, 76);
            this.XposLabel1.TabIndex = 0;
            this.XposLabel1.Text = "X: ";
            // 
            // YposLabel1
            // 
            this.YposLabel1.AutoSize = true;
            this.YposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel1.Location = new System.Drawing.Point(3, 79);
            this.YposLabel1.Name = "YposLabel1";
            this.YposLabel1.Size = new System.Drawing.Size(115, 76);
            this.YposLabel1.TabIndex = 1;
            this.YposLabel1.Text = "Y: ";
            // 
            // ZposLabel1
            // 
            this.ZposLabel1.AutoSize = true;
            this.ZposLabel1.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel1.Location = new System.Drawing.Point(3, 149);
            this.ZposLabel1.Name = "ZposLabel1";
            this.ZposLabel1.Size = new System.Drawing.Size(111, 76);
            this.ZposLabel1.TabIndex = 2;
            this.ZposLabel1.Text = "Z: ";
            // 
            // ZposLabel2
            // 
            this.ZposLabel2.AutoSize = true;
            this.ZposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel2.Location = new System.Drawing.Point(338, 149);
            this.ZposLabel2.Name = "ZposLabel2";
            this.ZposLabel2.Size = new System.Drawing.Size(111, 76);
            this.ZposLabel2.TabIndex = 5;
            this.ZposLabel2.Text = "Z: ";
            // 
            // YposLabel2
            // 
            this.YposLabel2.AutoSize = true;
            this.YposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel2.Location = new System.Drawing.Point(338, 80);
            this.YposLabel2.Name = "YposLabel2";
            this.YposLabel2.Size = new System.Drawing.Size(115, 76);
            this.YposLabel2.TabIndex = 4;
            this.YposLabel2.Text = "Y: ";
            // 
            // XposLabel2
            // 
            this.XposLabel2.AutoSize = true;
            this.XposLabel2.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel2.Location = new System.Drawing.Point(338, 4);
            this.XposLabel2.Name = "XposLabel2";
            this.XposLabel2.Size = new System.Drawing.Size(115, 76);
            this.XposLabel2.TabIndex = 3;
            this.XposLabel2.Text = "X: ";
            // 
            // ZposLabel4
            // 
            this.ZposLabel4.AutoSize = true;
            this.ZposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel4.Location = new System.Drawing.Point(338, 385);
            this.ZposLabel4.Name = "ZposLabel4";
            this.ZposLabel4.Size = new System.Drawing.Size(111, 76);
            this.ZposLabel4.TabIndex = 11;
            this.ZposLabel4.Text = "Z: ";
            // 
            // YposLabel4
            // 
            this.YposLabel4.AutoSize = true;
            this.YposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel4.Location = new System.Drawing.Point(338, 309);
            this.YposLabel4.Name = "YposLabel4";
            this.YposLabel4.Size = new System.Drawing.Size(115, 76);
            this.YposLabel4.TabIndex = 10;
            this.YposLabel4.Text = "Y: ";
            // 
            // XposLabel4
            // 
            this.XposLabel4.AutoSize = true;
            this.XposLabel4.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel4.Location = new System.Drawing.Point(339, 233);
            this.XposLabel4.Name = "XposLabel4";
            this.XposLabel4.Size = new System.Drawing.Size(115, 76);
            this.XposLabel4.TabIndex = 9;
            this.XposLabel4.Text = "X: ";
            // 
            // ZposLabel3
            // 
            this.ZposLabel3.AutoSize = true;
            this.ZposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ZposLabel3.Location = new System.Drawing.Point(6, 385);
            this.ZposLabel3.Name = "ZposLabel3";
            this.ZposLabel3.Size = new System.Drawing.Size(111, 76);
            this.ZposLabel3.TabIndex = 8;
            this.ZposLabel3.Text = "Z: ";
            // 
            // YposLabel3
            // 
            this.YposLabel3.AutoSize = true;
            this.YposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.YposLabel3.Location = new System.Drawing.Point(6, 309);
            this.YposLabel3.Name = "YposLabel3";
            this.YposLabel3.Size = new System.Drawing.Size(115, 76);
            this.YposLabel3.TabIndex = 7;
            this.YposLabel3.Text = "Y: ";
            // 
            // XposLabel3
            // 
            this.XposLabel3.AutoSize = true;
            this.XposLabel3.Font = new System.Drawing.Font("Microsoft Sans Serif", 50F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.XposLabel3.Location = new System.Drawing.Point(7, 233);
            this.XposLabel3.Name = "XposLabel3";
            this.XposLabel3.Size = new System.Drawing.Size(115, 76);
            this.XposLabel3.TabIndex = 6;
            this.XposLabel3.Text = "X: ";
            // 
            // trackingTabControl
            // 
            this.trackingTabControl.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.trackingTabControl.Controls.Add(this.textTrackingTabPage);
            this.trackingTabControl.Controls.Add(this.plotTrackingTabPage);
            this.trackingTabControl.Controls.Add(this.optionsTabPage);
            this.trackingTabControl.Location = new System.Drawing.Point(12, 12);
            this.trackingTabControl.Name = "trackingTabControl";
            this.trackingTabControl.SelectedIndex = 0;
            this.trackingTabControl.Size = new System.Drawing.Size(749, 509);
            this.trackingTabControl.TabIndex = 12;
            // 
            // textTrackingTabPage
            // 
            this.textTrackingTabPage.Controls.Add(this.XposLabel1);
            this.textTrackingTabPage.Controls.Add(this.ZposLabel4);
            this.textTrackingTabPage.Controls.Add(this.YposLabel1);
            this.textTrackingTabPage.Controls.Add(this.YposLabel4);
            this.textTrackingTabPage.Controls.Add(this.ZposLabel1);
            this.textTrackingTabPage.Controls.Add(this.XposLabel4);
            this.textTrackingTabPage.Controls.Add(this.XposLabel2);
            this.textTrackingTabPage.Controls.Add(this.ZposLabel3);
            this.textTrackingTabPage.Controls.Add(this.YposLabel2);
            this.textTrackingTabPage.Controls.Add(this.YposLabel3);
            this.textTrackingTabPage.Controls.Add(this.XposLabel3);
            this.textTrackingTabPage.Controls.Add(this.ZposLabel2);
            this.textTrackingTabPage.Location = new System.Drawing.Point(4, 22);
            this.textTrackingTabPage.Name = "textTrackingTabPage";
            this.textTrackingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.textTrackingTabPage.Size = new System.Drawing.Size(741, 483);
            this.textTrackingTabPage.TabIndex = 0;
            this.textTrackingTabPage.Text = "Text";
            this.textTrackingTabPage.UseVisualStyleBackColor = true;
            // 
            // plotTrackingTabPage
            // 
            this.plotTrackingTabPage.Controls.Add(this.rZTrackBar);
            this.plotTrackingTabPage.Controls.Add(this.rXTrackBar);
            this.plotTrackingTabPage.Controls.Add(this.rYTrackBar);
            this.plotTrackingTabPage.Controls.Add(this.plot3DPictureBox);
            this.plotTrackingTabPage.Location = new System.Drawing.Point(4, 22);
            this.plotTrackingTabPage.Name = "plotTrackingTabPage";
            this.plotTrackingTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.plotTrackingTabPage.Size = new System.Drawing.Size(741, 483);
            this.plotTrackingTabPage.TabIndex = 1;
            this.plotTrackingTabPage.Text = "3D Plot";
            this.plotTrackingTabPage.UseVisualStyleBackColor = true;
            // 
            // rZTrackBar
            // 
            this.rZTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rZTrackBar.Location = new System.Drawing.Point(690, 7);
            this.rZTrackBar.Maximum = 360;
            this.rZTrackBar.Minimum = -360;
            this.rZTrackBar.Name = "rZTrackBar";
            this.rZTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.rZTrackBar.Size = new System.Drawing.Size(45, 421);
            this.rZTrackBar.TabIndex = 3;
            this.rZTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rZTrackBar.Scroll += new System.EventHandler(this.AxisTrackBar_Scroll);
            // 
            // rXTrackBar
            // 
            this.rXTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)));
            this.rXTrackBar.Location = new System.Drawing.Point(6, 7);
            this.rXTrackBar.Maximum = 360;
            this.rXTrackBar.Minimum = -360;
            this.rXTrackBar.Name = "rXTrackBar";
            this.rXTrackBar.Orientation = System.Windows.Forms.Orientation.Vertical;
            this.rXTrackBar.Size = new System.Drawing.Size(45, 422);
            this.rXTrackBar.TabIndex = 2;
            this.rXTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rXTrackBar.Scroll += new System.EventHandler(this.AxisTrackBar_Scroll);
            // 
            // rYTrackBar
            // 
            this.rYTrackBar.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.rYTrackBar.Location = new System.Drawing.Point(57, 435);
            this.rYTrackBar.Maximum = 360;
            this.rYTrackBar.Minimum = -360;
            this.rYTrackBar.Name = "rYTrackBar";
            this.rYTrackBar.Size = new System.Drawing.Size(627, 45);
            this.rYTrackBar.TabIndex = 1;
            this.rYTrackBar.TickStyle = System.Windows.Forms.TickStyle.None;
            this.rYTrackBar.Scroll += new System.EventHandler(this.AxisTrackBar_Scroll);
            // 
            // plot3DPictureBox
            // 
            this.plot3DPictureBox.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.plot3DPictureBox.Location = new System.Drawing.Point(57, 6);
            this.plot3DPictureBox.Name = "plot3DPictureBox";
            this.plot3DPictureBox.Size = new System.Drawing.Size(627, 422);
            this.plot3DPictureBox.TabIndex = 0;
            this.plot3DPictureBox.TabStop = false;
            // 
            // optionsTabPage
            // 
            this.optionsTabPage.Controls.Add(this.do3DPlotCheckBox);
            this.optionsTabPage.Controls.Add(this.send2MatCheckBox);
            this.optionsTabPage.Location = new System.Drawing.Point(4, 22);
            this.optionsTabPage.Name = "optionsTabPage";
            this.optionsTabPage.Padding = new System.Windows.Forms.Padding(3);
            this.optionsTabPage.Size = new System.Drawing.Size(741, 483);
            this.optionsTabPage.TabIndex = 2;
            this.optionsTabPage.Text = "Options";
            this.optionsTabPage.UseVisualStyleBackColor = true;
            // 
            // send2MatCheckBox
            // 
            this.send2MatCheckBox.AutoSize = true;
            this.send2MatCheckBox.Location = new System.Drawing.Point(20, 27);
            this.send2MatCheckBox.Name = "send2MatCheckBox";
            this.send2MatCheckBox.Size = new System.Drawing.Size(184, 17);
            this.send2MatCheckBox.TabIndex = 0;
            this.send2MatCheckBox.Text = "Send Tracking Data To MATLAB";
            this.send2MatCheckBox.UseVisualStyleBackColor = true;
            this.send2MatCheckBox.CheckedChanged += new System.EventHandler(this.send2MatCheckBox_CheckedChanged);
            // 
            // timer1
            // 
            this.timer1.Interval = 200;
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // do3DPlotCheckBox
            // 
            this.do3DPlotCheckBox.AutoSize = true;
            this.do3DPlotCheckBox.Location = new System.Drawing.Point(20, 51);
            this.do3DPlotCheckBox.Name = "do3DPlotCheckBox";
            this.do3DPlotCheckBox.Size = new System.Drawing.Size(78, 17);
            this.do3DPlotCheckBox.TabIndex = 1;
            this.do3DPlotCheckBox.Text = "Do 3D Plot";
            this.do3DPlotCheckBox.UseVisualStyleBackColor = true;
            this.do3DPlotCheckBox.CheckedChanged += new System.EventHandler(this.do3DPlotCheckBox_CheckedChanged);
            // 
            // TrackingForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(773, 533);
            this.Controls.Add(this.trackingTabControl);
            this.Name = "TrackingForm";
            this.Text = "TrackingForm";
            this.Load += new System.EventHandler(this.TrackingForm_Load);
            this.Paint += new System.Windows.Forms.PaintEventHandler(this.TrackingForm_Paint);
            this.Resize += new System.EventHandler(this.TrackingForm_Resize);
            this.trackingTabControl.ResumeLayout(false);
            this.textTrackingTabPage.ResumeLayout(false);
            this.textTrackingTabPage.PerformLayout();
            this.plotTrackingTabPage.ResumeLayout(false);
            this.plotTrackingTabPage.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.rZTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rXTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.rYTrackBar)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.plot3DPictureBox)).EndInit();
            this.optionsTabPage.ResumeLayout(false);
            this.optionsTabPage.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        public System.Windows.Forms.Label ZposLabel2;
        public System.Windows.Forms.Label YposLabel2;
        public System.Windows.Forms.Label XposLabel2;
        public System.Windows.Forms.Label ZposLabel4;
        public System.Windows.Forms.Label YposLabel4;
        public System.Windows.Forms.Label XposLabel4;
        public System.Windows.Forms.Label ZposLabel3;
        public System.Windows.Forms.Label YposLabel3;
        public System.Windows.Forms.Label XposLabel3;
        public System.Windows.Forms.Label XposLabel1;
        public System.Windows.Forms.Label YposLabel1;
        public System.Windows.Forms.Label ZposLabel1;
        private System.Windows.Forms.TabControl trackingTabControl;
        private System.Windows.Forms.TabPage textTrackingTabPage;
        private System.Windows.Forms.TabPage plotTrackingTabPage;
        private System.Windows.Forms.TabPage optionsTabPage;
        private System.Windows.Forms.CheckBox send2MatCheckBox;
        private System.Windows.Forms.TrackBar rZTrackBar;
        private System.Windows.Forms.TrackBar rXTrackBar;
        private System.Windows.Forms.TrackBar rYTrackBar;
        private System.Windows.Forms.PictureBox plot3DPictureBox;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.CheckBox do3DPlotCheckBox;


    }
}