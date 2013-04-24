using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.IO;
using System.Runtime.Serialization;
using System.Runtime.Serialization.Formatters.Binary;
using System.Windows.Forms;
using WiimoteLib;
using Wiimote3DTrackingLib;
using Emgu.CV;
using Emgu.Util;
using LocationPlot3D;

namespace TestWiimote3DTracking
{
    public partial class Form1 : Form
    {

        Wiimote wm1;
        Wiimote wm2;

        Wiimote3DTrackingLib.StereoTracking wiitrack;

        const double MAX_DRAWSCALE = 2.0;
        const double DRAWSCALE_ENLARGE_AMOUNT = 0.05;

        private Double drawscale = 0.4;


        private Double drawx = new Double();
        private  Double drawy = new Double();
        private Bitmap b1;
        private Bitmap b2;
        private Graphics g1;
        private Graphics g2;
        private object _bitmaplocker = new object();

        private Guid _wm1ID;
        private Guid _wm2ID;

        private bool _dotracking = false;
        private bool _dologging = false;
        private bool _displayirpoints = true;

        private int calibimageviewidx = 0;

        private Matrix<double>[] result3DPoints = new Matrix<double>[4];

        // declare an object to protect the Location array while we access 
        // it using the lock statement
        static readonly object _result3DLocker = new object();


        TrackingForm TForm = new TrackingForm();
        private bool _tformloaded = false;

        #region form code

        public Form1()
        {
            InitializeComponent();


            wm1 = new Wiimote();

            wm2 = new Wiimote();

            ResizeControls();

            wm1IRLabel1.Text = "No IR";
            wm1IRLabel2.Text = "No IR";
            wm1IRLabel3.Text = "No IR";
            wm1IRLabel4.Text = "No IR";

            wm2IRLabel1.Text = "No IR";
            wm2IRLabel2.Text = "No IR";
            wm2IRLabel3.Text = "No IR";
            wm2IRLabel4.Text = "No IR";

            wiitrack = new Wiimote3DTrackingLib.StereoTracking();
            wiitrack.LogTimerTick += new LogEventHandler(wiitrack_LogTimerTick);

            wm1.WiimoteChanged += wm_WiimoteChanged;
            wm1.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            wm2.WiimoteChanged += wm_WiimoteChanged;
            wm2.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            for (int i = 0; i < 4; i++)
            {
                result3DPoints[i] = new Matrix<double>(4, 1);
            }

        }

        private void ResizeControls()
        {
            //drawx = drawscale * 1024;
            //drawy = drawscale * 768;

            drawx = drawscale * 1016;
            drawy = drawscale * 760;

            lock (_bitmaplocker)
            {

                b1 = new Bitmap((int)(Math.Ceiling(drawx)), (int)(Math.Ceiling(drawy)), PixelFormat.Format24bppRgb);
                b2 = new Bitmap((int)(Math.Ceiling(drawx)), (int)(Math.Ceiling(drawy)), PixelFormat.Format24bppRgb);
                g1 = Graphics.FromImage(b1);
                g2 = Graphics.FromImage(b2);

                if (2 * (int)(Math.Ceiling(drawy)) + 15 > 630)
                {
                    this.tabControl1.Height = 2 * (int)(Math.Ceiling(drawy)) + 15;

                    this.Height = this.tabControl1.Height + 5;
                }
                else
                {
                    this.tabControl1.Height = 630;

                    this.Height = this.tabControl1.Height + 5;
                }

                Size tempsize = new Size();

                tempsize = this.wm1IRPictureBox.Size;
                tempsize.Width = (int)(Math.Ceiling(drawx));
                tempsize.Height = (int)(Math.Ceiling(drawy));

                this.wm1IRPictureBox.Size = tempsize;
                this.wm2IRPictureBox.Size = tempsize;

                this.wm1IRPictureBox.Location = new System.Drawing.Point(this.wm1IRPictureBox.Location.X, 5);
                this.wm2IRPictureBox.Location = new System.Drawing.Point(this.wm1IRPictureBox.Location.X, this.wm1IRPictureBox.Location.Y + 5 + (int)(Math.Ceiling(drawy)));

            }

        }

        private void Form1_Load(object sender, EventArgs e)
		{
            this.Show();
		}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                wm1.Disconnect();
            }

            if (wm2.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                wm2.Disconnect();
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wm1.Connect();
                wm1.SetReportType(InputReport.IRAccel, true);
                _wm1ID = wm1.ID;
            }
            catch
            {
                MessageBox.Show("Exception thrown by Connect() method.");
                return;
            }

            if (wm1.WiimoteState.LEDState.LED1)
            {

                wm1.SetLEDs(false, true, false, false);
            }
            else
            {
                wm1.SetLEDs(true, false, false, false);
            }

        }

        private void capturebutton_Click(object sender, EventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                //Wiimote3DTrackingLib.coord[] coords = new Wiimote3DTrackingLib.coord[wm.WiimoteState.IRPoints()];

                //coords.Initialize();

                //for (int i = 0; i < coords.Length; i++)
                //    coords[i] = new coord();

                //wiitrack.CaptureOneWM(wm, coords);

                wiitrack.SingleCalibCapture(wm1);

                captureOneCountLabel.Text = "Captured " + wiitrack.CalibrateImageCount.ToString() + " Images";
            }
            else
            {
                MessageBox.Show("Wiimote not connected!");
            }
        }

        private void dualcapturebutton_Click(object sender, EventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected &
                wm2.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                wiitrack.StereoCalibCapture(wm1, wm2);

                this.captureTwoCountabel.Text = "Captured " + wiitrack.StereoCalibrateImageCount.ToString() + " Image Pairs";

                this.SaveStereoCalibImagesButton.Enabled = true;
            }
            else
            {
                MessageBox.Show("Both wiimote are not connected!");
            }


        }

        private void countsrcsbutton_Click(object sender, EventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                this.IRpointslabel.Text = "IR Sources: " + wm1.WiimoteState.IRPoints().ToString();
            }
            else
            {
                MessageBox.Show("Wiimote not connected!");
            }
        }

        private void CalibrateWM1Button_Click(object sender, EventArgs e)
        {
            if (wiitrack.CalibrateImageCount > 4)
            {
                wiitrack.CalibrateCamera(wm1);
            }
            else
            {
                MessageBox.Show("At least 5 images are required for camera calibration");
            }
        }

        private void CalibrateStereoButton_Click(object sender, EventArgs e)
        {
            if (wiitrack.StereoCalibrateImageCount > 14)
            {
                try
                {
                    wiitrack.StereoCalibrate(wm1, wm2);
                }
                catch
                {
                    MessageBox.Show("Calibration failed");
                }
            }
            else
            {
                MessageBox.Show("At least 5 images are required for camera calibration");
            }
        }

        private void ResetCalibButton_Click(object sender, EventArgs e)
        {
            wiitrack.ResetSingleCamCalibrateCapture();

            captureOneCountLabel.Text = "Captured " + wiitrack.CalibrateImageCount.ToString() + " Images";
        }

        private void ResetDualCalibButton_Click(object sender, EventArgs e)
        {
            wiitrack.ResetStereoCamCalibrateCapture();

            this.captureTwoCountabel.Text = "Captured " + wiitrack.CalibrateImageCount.ToString() + " Image Pairs";

            this.SaveStereoCalibImagesButton.Enabled = false;
        }

        private void StereoConnectButton_Click(object sender, EventArgs e)
        {
            // Get the device paths of all wiimotes in the HID device list
            StringCollection devicePaths = Wiimote.FindAllWiiMotes();

            if (devicePaths.Count >= 2)
            {
                try
                {
                    wm1.Connect(devicePaths[0]);
                }
                catch
                {
                    MessageBox.Show("Wiimote 1 could not be connected, Exception thrown by Connect() method.");
                    return;
                }

                try
                {
                    wm2.Connect(devicePaths[1]);
                }
                catch
                {
                    MessageBox.Show("Wiimote 1 could not be connected, Exception thrown by Connect() method.");
                    return;
                }

                wm1.SetReportType(InputReport.IRAccel, true);
                _wm1ID = wm1.ID;
                wm1.SetLEDs(true, false, false, false);

                wm2.SetReportType(InputReport.IRAccel, true);
                _wm2ID = wm2.ID;
                wm2.SetLEDs(false, true, false, false);

            }
            else
            {
                MessageBox.Show("Not enough wiimotes were found in the HID device list, is there 2 connected?");
            }
        }

        private void DisconnectAllButton_Click(object sender, EventArgs e)
        {
            wm1.Disconnect();
            wm2.Disconnect();
            wm1IRPictureBox.Image = null;
            wm2IRPictureBox.Image = null;

            wiitrack.Reset();
        }

        private void PrintPointsButton_Click(object sender, EventArgs e)
        {
            wiitrack.OutputCalibrationPoints();
        }


        private void button2_Click(object sender, EventArgs e)
        {
            // Test Stereo Calibration button

            //wiitrack.StereoCalibrate(wm1, wm2, wiitrack.tp1, wiitrack.tp2);
        }

        private void StartTrackingButton_Click(object sender, EventArgs e)
        {
            if (wiitrack.IsStereoCalibrated)
            {
                // check if form has already been loaded
                if (!_tformloaded)
                {
                    TForm.Show();
                    _tformloaded = true;
                }

                _dotracking = true;

            }
            else
            {
                MessageBox.Show("Stereo rig has not yet been calibrated");
            }
        }

        private void StopTrackingButton_Click(object sender, EventArgs e)
        {
            TForm.Hide();
            _tformloaded = false;
            _dotracking = false;
        }


        private void StartLoggingButton_Click(object sender, EventArgs e)
        {

            string logfile = "test_log.csv";

            SaveFileDialog openFileDialog1 = new SaveFileDialog();

            openFileDialog1.InitialDirectory = "c:\\";
            openFileDialog1.Filter = "Comma-separated values file (*.csv)|*.csv|Log files (*.log)|*.log|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = true;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                logfile = openFileDialog1.FileName;
            }

            // clear the IR display
            g1.Clear(Color.White);

            g2.Clear(Color.White);

            //if (!_tformloaded)
            //{
            //    TForm.Show();
            //    _tformloaded = true;
            //}

            //_dotracking = false;

            //_displayirpoints = false;

            wiitrack.StartLogging(1000.0 * 1.0 / (double.Parse(FreqInputTextBox.Text)), wm1, wm2, this.Log3DCheckBox.Checked, this.LogRawCheckBox.Checked, this.LogUDCheckBox.Checked, logfile);

            this.StopLoggingButton.Enabled = true;

            this.StartLoggingButton.Enabled = false;

        }

        private void StopLoggingButton_Click(object sender, EventArgs e)
        {
            wiitrack.StopLogging();

            _displayirpoints = true;

            this.StartLoggingButton.Enabled = true;

            this.StopLoggingButton.Enabled = false;
        }


        private void ReviewButton_Click(object sender, EventArgs e)
        {
            if (wiitrack.StereoCalibrateImageCount > 0)
            {
                // stop updating the real-time display of the ir points
                _displayirpoints = false;

                // reset the calration image count to zero
                calibimageviewidx = 0;

                // get the first calibration image set 
                System.Drawing.PointF[][] wmcalibpoints = wiitrack.GetStereoCalibImage(calibimageviewidx);

                // draw the two calibration images
                DisplayCalibPoints(g1, wmcalibpoints[0]);

                wm1IRPictureBox.Image = b1;

                DisplayCalibPoints(g2, wmcalibpoints[1]);

                wm2IRPictureBox.Image = b2;

                CalibReviewLabel.Text = "Image: " + (calibimageviewidx + 1).ToString();

                this.DeleteImageButton.Enabled = true;

            }
            else
            {
                MessageBox.Show("No calibration images to display");
            }

        }

        private void StopReviewButton_Click(object sender, EventArgs e)
        {
            _displayirpoints = true;

            this.DeleteImageButton.Enabled = false;
        }

        private void NextCalImageButton_Click(object sender, EventArgs e)
        {
            if (calibimageviewidx == wiitrack.StereoCalibrateImageCount - 1)
            {
                calibimageviewidx = 0;
            }
            else
            {
                calibimageviewidx = calibimageviewidx + 1;
            }

            // get the first calibration image set 
            System.Drawing.PointF[][] wm1calibpoints = wiitrack.GetStereoCalibImage(calibimageviewidx);

            // draw the two calibration images
            DisplayCalibPoints(g1, wm1calibpoints[0]);
            wm1IRPictureBox.Image = b1;

            DisplayCalibPoints(g2, wm1calibpoints[1]);
            wm2IRPictureBox.Image = b2;

            CalibReviewLabel.Text = "Image: " + (calibimageviewidx + 1).ToString();
        }

        private void PrevCalImageButton_Click(object sender, EventArgs e)
        {
            if (calibimageviewidx == 0)
            {
                calibimageviewidx = wiitrack.StereoCalibrateImageCount - 1;
            }
            else
            {
                calibimageviewidx = calibimageviewidx - 1;
            }

            // get the first calibration image set 
            System.Drawing.PointF[][] wmcalibpoints = wiitrack.GetStereoCalibImage(calibimageviewidx);

            // draw the two calibration images
            DisplayCalibPoints(g1, wmcalibpoints[0]);
            wm1IRPictureBox.Image = b1;

            DisplayCalibPoints(g2, wmcalibpoints[1]);
            wm2IRPictureBox.Image = b2;

            CalibReviewLabel.Text = "Image: " + (calibimageviewidx + 1).ToString();
        }


        private void DisplayCalibPoints(Graphics g, System.Drawing.PointF[] calibpoints)
        {

            g.Clear(Color.White);

            float penwidth = 2.0F;

            DrawCross(g, calibpoints[0], penwidth, Color.Red);

            DrawCross(g, calibpoints[1], penwidth, Color.Blue);

            DrawCross(g, calibpoints[2], penwidth, Color.DarkGoldenrod);

            DrawCross(g, calibpoints[3], penwidth, Color.DarkGreen);

        }

        private void DrawCross(Graphics g, System.Drawing.PointF position, float penwidth, Color pencolour)
        {
            System.Drawing.PointF[] linepoints = new System.Drawing.PointF[4];

            linepoints[0].X = (float)((position.X ) * drawscale) - (float)(b1.Height) * 0.05f;
            linepoints[0].Y = (float)(position.Y * drawscale);

            linepoints[1].X = (float)((position.X ) * drawscale) + (float)(b1.Height) * 0.05f;
            linepoints[1].Y = (float)(position.Y * drawscale);

            linepoints[2].X = (float)(position.X * drawscale);
            linepoints[2].Y = (float)(position.Y * drawscale) - (float)(b1.Height) * 0.05f;

            linepoints[3].X = (float)(position.X * drawscale);
            linepoints[3].Y = (float)(position.Y * drawscale) + (float)(b1.Height) * 0.05f;

            g.DrawLine(new Pen(pencolour, penwidth), linepoints[0], linepoints[1]);

            g.DrawLine(new Pen(pencolour, penwidth), linepoints[2], linepoints[3]);

        }

        private void DeleteImageButton_Click(object sender, EventArgs e)
        {
            wiitrack.RemoveStereoCalibImage(calibimageviewidx);

            calibimageviewidx--;

            // get the first calibration image set 
            System.Drawing.PointF[][] wmcalibpoints = wiitrack.GetStereoCalibImage(calibimageviewidx);

            // draw the two calibration images
            DisplayCalibPoints(g1, wmcalibpoints[0]);
            wm1IRPictureBox.Image = b1;

            DisplayCalibPoints(g2, wmcalibpoints[1]);
            wm2IRPictureBox.Image = b2;

            CalibReviewLabel.Text = "Image: " + (calibimageviewidx + 1).ToString();

            captureTwoCountabel.Text = "Captured " + wiitrack.StereoCalibrateImageCount.ToString() + " Image Pairs";
        }

        private void SaveStereoCalibImagesButton_Click(object sender, EventArgs e)
        {
            if (wiitrack.StereoCalibrateImageCount > 0)
            {
                string calibimagefile = "test_save.csv";

                SaveFileDialog openFileDialog1 = new SaveFileDialog();

                openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
                openFileDialog1.Filter = "Comma-separated values file (*.csv)|*.csv|All files (*.*)|*.*";
                openFileDialog1.FilterIndex = 1;
                openFileDialog1.RestoreDirectory = false;

                if (openFileDialog1.ShowDialog() == DialogResult.OK)
                {
                    calibimagefile = openFileDialog1.FileName;
                }

                wiitrack.SaveStereoCalibrationImages(calibimagefile);
            }
            else
            {
                MessageBox.Show("No calibration images to save!");
            }

        }

        private void LoadStereoCalibImagesButton_Click(object sender, EventArgs e)
        {

            string calibimagefile = "test_save.csv";

            OpenFileDialog openFileDialog1 = new OpenFileDialog();

            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            openFileDialog1.Filter = "Comma-separated values file (*.csv)|*.csv|All files (*.*)|*.*";
            openFileDialog1.FilterIndex = 1;
            openFileDialog1.RestoreDirectory = false;

            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                calibimagefile = openFileDialog1.FileName;
            }

            wiitrack.LoadStereoCalibrationImages(calibimagefile);

        }

        private void EnlargeViewsButton_Click(object sender, EventArgs e)
        {
            if (drawscale <= MAX_DRAWSCALE)
            {
                drawscale = drawscale + DRAWSCALE_ENLARGE_AMOUNT;

                ResizeControls();
            }
        }

        private void ReduceViewsButton_Click(object sender, EventArgs e)
        {
            if (drawscale > 0 + DRAWSCALE_ENLARGE_AMOUNT)
            {
                drawscale = drawscale - DRAWSCALE_ENLARGE_AMOUNT;

                ResizeControls();

            }
        }

        private void IncreaseWM1SensitivityButton_Click(object sender, EventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                switch (wm1.WiimoteState.IRState.Sensitivity)
                {
                    case IRSensitivity.WiiLevel1:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel2);
                        break;
                    case IRSensitivity.WiiLevel2:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel3);
                        break;
                    case IRSensitivity.WiiLevel3:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel4);
                        break;
                    case IRSensitivity.WiiLevel4:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel5);
                        break;
                    case IRSensitivity.WiiLevel5:
                        wm1.SetIRSensitivity(IRSensitivity.Maximum);
                        break;
                    default:
                        break;

                }

            }
        }

        private void ReduceWM1SensitivityButton_Click(object sender, EventArgs e)
        {
            if (wm1.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                switch (wm1.WiimoteState.IRState.Sensitivity)
                {
                    case IRSensitivity.WiiLevel1:
                        // do nothing
                        break;
                    case IRSensitivity.WiiLevel2:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel1);
                        break;
                    case IRSensitivity.WiiLevel3:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel2);
                        break;
                    case IRSensitivity.WiiLevel4:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel3);
                        break;
                    case IRSensitivity.WiiLevel5:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel4);
                        break;
                    case IRSensitivity.Maximum:
                        wm1.SetIRSensitivity(IRSensitivity.WiiLevel5);
                        break;
                    default:
                        break;
                }

            }
        }

        private void IncreaseWM2SensitivityButton_Click(object sender, EventArgs e)
        {
            if (wm2.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                switch (wm2.WiimoteState.IRState.Sensitivity)
                {
                    case IRSensitivity.WiiLevel1:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel2);
                        break;
                    case IRSensitivity.WiiLevel2:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel3);
                        break;
                    case IRSensitivity.WiiLevel3:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel4);
                        break;
                    case IRSensitivity.WiiLevel4:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel5);
                        break;
                    case IRSensitivity.WiiLevel5:
                        wm2.SetIRSensitivity(IRSensitivity.Maximum);
                        break;
                    default:
                        break;

                }

            }
        }

        private void ReduceWM2SensitivityButton_Click(object sender, EventArgs e)
        {
            if (wm2.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                switch (wm2.WiimoteState.IRState.Sensitivity)
                {
                    case IRSensitivity.WiiLevel1:
                        // do nothing
                        break;
                    case IRSensitivity.WiiLevel2:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel1);
                        break;
                    case IRSensitivity.WiiLevel3:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel2);
                        break;
                    case IRSensitivity.WiiLevel4:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel3);
                        break;
                    case IRSensitivity.WiiLevel5:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel4);
                        break;
                    case IRSensitivity.Maximum:
                        wm2.SetIRSensitivity(IRSensitivity.WiiLevel5);
                        break;
                    default:
                        break;
                }

            }
        }


        private void SaveSystemButton_Click(object sender, EventArgs e)
        {
            string filename;

            SaveFileDialog saveFileDialog1 = new SaveFileDialog();

            saveFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();
            saveFileDialog1.Filter = "Data file (*.dat)|*.dat|All files (*.*)|*.*";
            saveFileDialog1.FilterIndex = 1;
            saveFileDialog1.RestoreDirectory = false;

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                filename = saveFileDialog1.FileName;

                Stream s = File.Open(filename, FileMode.Create);

                BinaryFormatter b = new BinaryFormatter();

                b.Serialize(s, wiitrack);

                s.Close();
            }
            else
            {
                return;
            }

        }


        private void showTrackingFormButton_Click(object sender, EventArgs e)
        {
            TForm.Show();
        }


        private void calibObjSizeButton_Click(object sender, EventArgs e)
        {
            float Width;
            float Height;

            try
            {
                Width = float.Parse(xCalibTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid Width");
                return;
            }

            try
            {
                Height = float.Parse(yCalibTextBox.Text);
            }
            catch
            {
                MessageBox.Show("Invalid Height");
                return;
            }

            wiitrack.setCalibObjSize(Width, Height);

            this.captureTwoCountabel.Text = "Captured " + wiitrack.CalibrateImageCount.ToString() + " Image Pairs";

        }

        #endregion

        #region event handlers

        private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
        private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            UpdateState(args);
        }

        private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
		{
			UpdateExtension(args);

			if(args.Inserted)
				wm1.SetReportType(InputReport.IRExtensionAccel, true);
			else
				wm1.SetReportType(InputReport.IRAccel, true);
		}

        public void UpdateState(WiimoteChangedEventArgs args)
        {
            if (InvokeRequired)
            {
                if (!IsHandleCreated)
                {
                    this.CreateControl();
                }

                BeginInvoke(new UpdateWiimoteStateDelegate(UpdateWiimoteChanged), args);
            }
            else
            {
                UpdateWiimoteChanged(args);
            }
        }

        private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
        {
            

            if (_dologging)
            {
                //g1.Clear(Color.White);
            }
            else
            {

                WiimoteState ws = args.WiimoteState;

                if (_displayirpoints)
                {
                    if (ws.ID == _wm1ID)
                    {
                        if (ws.ConnectionState == WiimoteLib.ConnectionState.Connected)
                        {
                            // first draw the positions in the graphic
                            g1.Clear(Color.White);

                            float penwidth = 2.0F;

                            wm1IRSourceslabel.Text = "IR Sources: " + ws.IRPoints().ToString();

                            UpdateIR(g1, ws.IRState.IRSensors[0], wm1IRLabel1, Color.Red, penwidth);
                            UpdateIR(g1, ws.IRState.IRSensors[1], wm1IRLabel2, Color.Blue, penwidth);
                            UpdateIR(g1, ws.IRState.IRSensors[2], wm1IRLabel3, Color.Black, penwidth);
                            UpdateIR(g1, ws.IRState.IRSensors[3], wm1IRLabel4, Color.Purple, penwidth);

                            //if (ws.IRState.IRSensors[0].Found && ws.IRState.IRSensors[1].Found)
                            //    g1.DrawEllipse(new Pen(Color.Green, 1.0F), (int)(drawscale * ws.IRState.RawMidpoint.X), (int)(drawscale * ws.IRState.RawMidpoint.Y), (int)(2 / drawscale), (int)(2 / drawscale));

                            wm1IRPictureBox.Image = b1;
                        }
                        else
                        {
                            wm1IRPictureBox.Image = null;
                        }
                    }
                    else if (ws.ID == _wm2ID)
                    {
                        if (ws.ConnectionState == WiimoteLib.ConnectionState.Connected)
                        {
                            g2.Clear(Color.White);

                            float penwidth = 2.0F;

                            wm2IRSourceslabel.Text = "IR Sources: " + ws.IRPoints().ToString();

                            UpdateIR(g2, ws.IRState.IRSensors[0], wm2IRLabel1, Color.Red, penwidth);
                            UpdateIR(g2, ws.IRState.IRSensors[1], wm2IRLabel2, Color.Blue, penwidth);
                            UpdateIR(g2, ws.IRState.IRSensors[2], wm2IRLabel3, Color.Black, penwidth);
                            UpdateIR(g2, ws.IRState.IRSensors[3], wm2IRLabel4, Color.Purple, penwidth);

                            //if (ws.IRState.IRSensors[0].Found && ws.IRState.IRSensors[1].Found)
                            //    g2.DrawEllipse(new Pen(Color.Green, 1.0F), (int)(drawscale * ws.IRState.RawMidpoint.X), (int)(drawscale * (768 - ws.IRState.RawMidpoint.Y)), (int)(2 / drawscale), (int)(2 / drawscale));

                            wm2IRPictureBox.Image = b2;
                        }
                        else
                        {
                            wm2IRPictureBox.Image = null;
                        }
                    }
                }

                // now get the info from wm1 that is required for 3D tracking
                if (_dotracking)
                {
                    // get the 3D location of points in view of the cameras (if any)
                    // storing them in the result3DPoints array. Where points are not
                    // present the X, Y and Z values will all have values -9999
                    //wiitrack.Location3D(result3DPoints, wm1, wm2);

                    lock (_result3DLocker)
                    {
                        wiitrack.Location3D_2(result3DPoints, wm1, wm2);

                        //wiitrack.Location3D(result3DPoints, wm1, wm2);                    
                        
                        if (!(result3DPoints[0].Data[0, 0] == -9999 && result3DPoints[0].Data[1, 0] == -9999 && result3DPoints[0].Data[2, 0] == -9999))        
                        {
                            TForm.XposLabel1.Text = "X: " + result3DPoints[0].Data[0, 0].ToString("f4");
                            TForm.YposLabel1.Text = "Y: " + result3DPoints[0].Data[1, 0].ToString("f4"); 
                            TForm.ZposLabel1.Text = "Z: " + result3DPoints[0].Data[2, 0].ToString("f4"); 
                        }                    
                        else                    
                        {                        
                            TForm.XposLabel1.Text = "X: No IR";                        
                            TForm.YposLabel1.Text = "Y: No IR";                        
                            TForm.ZposLabel1.Text = "Z: No IR";                    
                        }
                        if (!(result3DPoints[1].Data[0, 0] == -9999 && result3DPoints[1].Data[1, 0] == -9999 && result3DPoints[1].Data[2, 0] == -9999))
                        {
                            TForm.XposLabel2.Text = "X: " + result3DPoints[1].Data[0, 0].ToString("f4");
                            TForm.YposLabel2.Text = "Y: " + result3DPoints[1].Data[1, 0].ToString("f4");
                            TForm.ZposLabel2.Text = "Z: " + result3DPoints[1].Data[2, 0].ToString("f4");
                        }
                        else
                        {
                            TForm.XposLabel2.Text = "X: No IR";
                            TForm.YposLabel2.Text = "Y: No IR";
                            TForm.ZposLabel2.Text = "Z: No IR";
                        }

                        if (!(result3DPoints[2].Data[0, 0] == -9999 && result3DPoints[2].Data[1, 0] == -9999 && result3DPoints[2].Data[2, 0] == -9999))
                        {
                            TForm.XposLabel3.Text = "X: " + result3DPoints[2].Data[0, 0].ToString("f4");
                            TForm.YposLabel3.Text = "Y: " + result3DPoints[2].Data[1, 0].ToString("f4");
                            TForm.ZposLabel3.Text = "Z: " + result3DPoints[2].Data[2, 0].ToString("f4");
                        }
                        else
                        {
                            TForm.XposLabel3.Text = "X: No IR";
                            TForm.YposLabel3.Text = "Y: No IR";
                            TForm.ZposLabel3.Text = "Z: No IR";
                        }

                        if (!(result3DPoints[3].Data[0, 0] == -9999 && result3DPoints[3].Data[1, 0] == -9999 && result3DPoints[3].Data[2, 0] == -9999))
                        {
                            TForm.XposLabel4.Text = "X: " + result3DPoints[3].Data[0, 0].ToString("f4");
                            TForm.YposLabel4.Text = "Y: " + result3DPoints[3].Data[1, 0].ToString("f4");
                            TForm.ZposLabel4.Text = "Z: " + result3DPoints[3].Data[2, 0].ToString("f4");
                        }
                        else
                        {
                            TForm.XposLabel4.Text = "X: No IR";
                            TForm.YposLabel4.Text = "Y: No IR";
                            TForm.ZposLabel4.Text = "Z: No IR";
                        }

                        for (int j = 0; j < result3DPoints.Length; j++)
                        {
                            TForm.SetTrackingPointPos(result3DPoints[j].Data[0, 0],
                                result3DPoints[j].Data[1, 0], -result3DPoints[j].Data[2, 0], j);
                        }
                    }

                }
                //pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
                //lblBattery.Text = ws.Battery.ToString();
                //lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
            }
        }

        // function to be called when a logging event occurs in the Stereotracking class
        private void wiitrack_LogTimerTick(object sender, EventArgs e)
        {
            lock (_result3DLocker)
            {
                //wiitrack.Location3D_2(result3DPoints, wm1, wm2);
                wiitrack.Get3DPoints().CopyTo(result3DPoints, 0);

                this.Display3DPointText(result3DPoints);
            }
            
        }

        delegate void SetTextCallback(string text);

        public void Display3DPointText(Matrix<double>[] result3DPoints)
        {
            //Form1.CheckForIllegalCrossThreadCalls = false;
            string Xstring;
            string Ystring;
            string Zstring;

            if (!(result3DPoints[0].Data[0, 0] == -9999 && result3DPoints[0].Data[1, 0] == -9999 && result3DPoints[0].Data[2, 0] == -9999))
            {
                Xstring = "X: " + result3DPoints[0].Data[0, 0].ToString("f4");
                Ystring = "Y: " + result3DPoints[0].Data[1, 0].ToString("f4");
                Zstring = "Z: " + result3DPoints[0].Data[2, 0].ToString("f4");
            }
            else
            {
                Xstring = "X: No IR";
                Ystring = "Y: No IR";
                Zstring = "Z: No IR";
            }

            if (this.XposLabel1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetXposLabel1Text);
                this.Invoke
                    (d, new object[] { Xstring });
            }
            else
            {
                this.XposLabel1.Text = Xstring;
            }

            if (this.YposLabel1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetYposLabel1Text);
                this.Invoke
                    (d, new object[] { Ystring });
            }
            else
            {
                this.YposLabel1.Text = Ystring;
            }

            if (this.ZposLabel1.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetZposLabel1Text);
                this.Invoke
                    (d, new object[] { Zstring });
            }
            else
            {
                this.ZposLabel1.Text = Zstring;
            }

            if (!(result3DPoints[1].Data[0, 0] == -9999 && result3DPoints[1].Data[1, 0] == -9999 && result3DPoints[1].Data[2, 0] == -9999))
            {
                Xstring = "X: " + result3DPoints[1].Data[0, 0].ToString("f4");
                Ystring = "Y: " + result3DPoints[1].Data[1, 0].ToString("f4");
                Zstring = "Z: " + result3DPoints[1].Data[2, 0].ToString("f4");
            }
            else
            {
                Xstring = "X: No IR";
                Ystring = "Y: No IR";
                Zstring = "Z: No IR";
            }

            if (this.XposLabel2.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetXposLabel2Text);
                this.Invoke
                    (d, new object[] { Xstring });
            }
            else
            {
                this.XposLabel2.Text = Xstring;
            }

            if (this.YposLabel2.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetYposLabel2Text);
                this.Invoke
                    (d, new object[] { Ystring });
            }
            else
            {
                this.YposLabel2.Text = Ystring;
            }

            if (this.ZposLabel2.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetZposLabel2Text);
                this.Invoke
                    (d, new object[] { Zstring });
            }
            else
            {
                this.ZposLabel2.Text = Zstring;
            }

            if (!(result3DPoints[2].Data[0, 0] == -9999 && result3DPoints[2].Data[1, 0] == -9999 && result3DPoints[2].Data[2, 0] == -9999))
            {
                Xstring = "X: " + result3DPoints[2].Data[0, 0].ToString("f4");
                Ystring = "Y: " + result3DPoints[2].Data[1, 0].ToString("f4");
                Zstring = "Z: " + result3DPoints[2].Data[2, 0].ToString("f4");
            }
            else
            {
                Xstring = "X: No IR";
                Ystring = "Y: No IR";
                Zstring = "Z: No IR";
            }

            if (this.XposLabel3.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetXposLabel3Text);
                this.Invoke
                    (d, new object[] { Xstring });
            }
            else
            {
                this.XposLabel3.Text = Xstring;
            }

            if (this.YposLabel3.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetYposLabel3Text);
                this.Invoke
                    (d, new object[] { Ystring });
            }
            else
            {
                this.YposLabel3.Text = Ystring;
            }

            if (this.ZposLabel3.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetZposLabel3Text);
                this.Invoke
                    (d, new object[] { Zstring });
            }
            else
            {
                this.ZposLabel3.Text = Zstring;
            }

            if (!(result3DPoints[3].Data[0, 0] == -9999 && result3DPoints[3].Data[1, 0] == -9999 && result3DPoints[3].Data[2, 0] == -9999))
            {
                Xstring = "X: " + result3DPoints[3].Data[0, 0].ToString("f4");
                Ystring = "Y: " + result3DPoints[3].Data[1, 0].ToString("f4");
                Zstring = "Z: " + result3DPoints[3].Data[2, 0].ToString("f4");
            }
            else
            {
                Xstring = "X: No IR";
                Ystring = "Y: No IR";
                Zstring = "Z: No IR";
            }

            if (this.XposLabel4.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetXposLabel4Text);
                this.Invoke
                    (d, new object[] { Xstring });
            }
            else
            {
                this.XposLabel4.Text = Xstring;
            }

            if (this.YposLabel4.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetYposLabel4Text);
                this.Invoke
                    (d, new object[] { Ystring });
            }
            else
            {
                this.YposLabel4.Text = Ystring;
            }

            if (this.ZposLabel4.InvokeRequired)
            {
                // It's on a different thread, so use Invoke.
                SetTextCallback d = new SetTextCallback(SetZposLabel4Text);
                this.Invoke
                    (d, new object[] { Zstring });
            }
            else
            {
                this.ZposLabel4.Text = Zstring;
            }

            //for (int j = 0; j < result3DPoints.Length; j++)
            //{
            //    this.SetTrackingPointPos(result3DPoints[j].Data[0, 0],
            //        result3DPoints[j].Data[1, 0], -result3DPoints[j].Data[2, 0], j);
            //}
            //Form1.CheckForIllegalCrossThreadCalls = true;
        }

        private void SetXposLabel1Text(string text)
        {
            this.XposLabel1.Text = text;
        }

        private void SetXposLabel2Text(string text)
        {
            this.XposLabel2.Text = text;
        }

        private void SetXposLabel3Text(string text)
        {
            this.XposLabel3.Text = text;
        }

        private void SetXposLabel4Text(string text)
        {
            this.XposLabel4.Text = text;
        }

        private void SetYposLabel1Text(string text)
        {
            this.YposLabel1.Text = text;
        }

        private void SetYposLabel2Text(string text)
        {
            this.YposLabel2.Text = text;
        }

        private void SetYposLabel3Text(string text)
        {
            this.YposLabel3.Text = text;
        }

        private void SetYposLabel4Text(string text)
        {
            this.YposLabel4.Text = text;
        }

        private void SetZposLabel1Text(string text)
        {
            this.ZposLabel1.Text = text;
        }

        private void SetZposLabel2Text(string text)
        {
            this.ZposLabel2.Text = text;
        }

        private void SetZposLabel3Text(string text)
        {
            this.ZposLabel3.Text = text;
        }

        private void SetZposLabel4Text(string text)
        {
            this.ZposLabel4.Text = text;
        }


    

        private void UpdateIR(Graphics g, IRSensor irSensor, Label lblRaw, Color color, float penwidth)
        {

            if (irSensor.Found)
            {
                //lblNorm.Text = irSensor.Position.ToString() + ", " + irSensor.Size;
                lblRaw.Text = irSensor.RawPosition.ToString();
                g.DrawEllipse(new Pen(color, penwidth), (int)(drawscale * irSensor.RawPosition.X), (int)(drawscale * (760 - irSensor.RawPosition.Y)),
                             (int)((irSensor.Size + 1) / drawscale), (int)((irSensor.Size + 1) / drawscale));

                if (irSensor.RawPosition.Y < 0)
                {
                    float temp = irSensor.Position.Y;
                }
            }
            else
            {
                lblRaw.Text = "No IR";
            }
        }

        public void UpdateExtension(WiimoteExtensionChangedEventArgs args)
        {
            if (InvokeRequired)
            {
                BeginInvoke(new UpdateExtensionChangedDelegate(UpdateExtensionChanged), args);
            }
            else
            {
                UpdateExtensionChanged(args);
            }
        }

        private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
        {
            // Event handler for the wiimote extension being changed
        }

        #endregion

    }
}
