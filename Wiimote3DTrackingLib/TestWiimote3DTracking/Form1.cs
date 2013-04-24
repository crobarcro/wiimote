using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
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

        private Double drawscale = 0.4;
        private Double drawx = new Double();
        private  Double drawy = new Double();
        private Bitmap b1;
        private Bitmap b2;
        private Graphics g1;
        private Graphics g2;

        private Guid _wm1ID;
        private Guid _wm2ID;

        private bool _dotracking = false;
        private bool _dologging = false;
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

            drawx = drawscale * 1024;
            drawy = drawscale * 768;

            b1 = new Bitmap((int)drawx, (int)drawy, PixelFormat.Format24bppRgb);
            b2 = new Bitmap((int)drawx, (int)drawy, PixelFormat.Format24bppRgb);
            g1 = Graphics.FromImage(b1);
            g2 = Graphics.FromImage(b2);

            wm1 = new Wiimote();

            wm2 = new Wiimote();

            Size tempsize = new Size();

            tempsize = this.wm1IRPictureBox.Size;
            tempsize.Width = (int)drawx;
            tempsize.Height = (int)drawy;

            this.wm1IRPictureBox.Size = tempsize;
            this.wm2IRPictureBox.Size = tempsize;

            this.wm2IRPictureBox.Location = new System.Drawing.Point(this.wm1IRPictureBox.Location.X, this.wm1IRPictureBox.Location.Y + (int)(21 * this.wm1IRPictureBox.Size.Height / 20));

            wm1IRLabel1.Text = "No IR";
            wm1IRLabel2.Text = "No IR";
            wm1IRLabel3.Text = "No IR";
            wm1IRLabel4.Text = "No IR";

            wm2IRLabel1.Text = "No IR";
            wm2IRLabel2.Text = "No IR";
            wm2IRLabel3.Text = "No IR";
            wm2IRLabel4.Text = "No IR";

            wiitrack = new Wiimote3DTrackingLib.StereoTracking();

            wm1.WiimoteChanged += wm_WiimoteChanged;
            wm1.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            wm2.WiimoteChanged += wm_WiimoteChanged;
            wm2.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            for (int i = 0; i < 4; i++)
            {
                result3DPoints[i] = new Matrix<double>(4, 1);
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

            wiitrack.StereoCalibrate(wm1, wm2, wiitrack.tp1, wiitrack.tp2);
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
            if (!_dologging)
            {

                // if we're not using the Wiimote3DTracking objects logging code

                WiimoteState ws = args.WiimoteState;

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

        private void UpdateIR(Graphics g, IRSensor irSensor, Label lblRaw, Color color, float penwidth)
        {

            if (irSensor.Found)
            {
                //lblNorm.Text = irSensor.Position.ToString() + ", " + irSensor.Size;
                lblRaw.Text = irSensor.RawPosition.ToString();
                g.DrawEllipse(new Pen(color, penwidth), (int)(drawscale * irSensor.RawPosition.X), (int)(drawscale * (768 - irSensor.RawPosition.Y)),
                             (int)((irSensor.Size + 1) / drawscale), (int)((irSensor.Size + 1) / drawscale));
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

        }




    }
}
