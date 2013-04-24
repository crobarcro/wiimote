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

namespace TestWiimote3DTracking
{
    public partial class Form1 : Form
    {

        Wiimote wm1;
        Wiimote wm2;

        Wiimote3DTrackingLib.StereoTracking wiitrack;

        private Double drawscale = 0.5;
        private Double drawx = new Double();
        private  Double drawy = new Double();
        private Bitmap b1;
        private Bitmap b2;
        private Graphics g1;
        private Graphics g2;

        private Guid _wm1ID;
        private Guid _wm2ID;

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

            //wiitrack.StereoCalibrate(wm1, wm2, wiitrack.tp1, wiitrack.tp2);
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
            WiimoteState ws = args.WiimoteState;

            if (ws.ID == _wm1ID)
            {
                if (ws.ConnectionState == WiimoteLib.ConnectionState.Connected)
                {
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

            //pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            //lblBattery.Text = ws.Battery.ToString();
            //lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
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












    }
}
