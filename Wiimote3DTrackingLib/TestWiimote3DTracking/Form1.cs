using System;
using System.Collections.Generic;
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

        private static Double drawscale = 0.5;
        private static Double drawx = drawscale * 1024;
        private static Double drawy = drawscale * 768;
        private Bitmap b = new Bitmap((int)drawx, (int)drawy, PixelFormat.Format24bppRgb);
        private Graphics g;

        #region form code

        public Form1()
        {
            InitializeComponent();

            wm1 = new Wiimote();

            wm2 = new Wiimote();

            Size tempsize = new Size();

            tempsize = this.wm1IRPictureBox.Size;
            tempsize.Width = (int)drawx;
            tempsize.Height = (int)drawy;

            this.wm1IRPictureBox.Size = tempsize;
            this.wm2IRPictureBox.Size = tempsize;

            g = Graphics.FromImage(b);

            wm1IRLabel1.Text = "No IR";
            wm1IRLabel2.Text = "No IR";
            wm1IRLabel3.Text = "No IR";
            wm1IRLabel4.Text = "No IR";

            wiitrack = new Wiimote3DTrackingLib.StereoTracking();

            wm1.WiimoteChanged += wm_WiimoteChanged;
            wm1.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            wm2.WiimoteChanged += wm_WiimoteChanged;
            wm2.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

        }
		
		private void Form1_Load(object sender, EventArgs e)
		{
			wm1.WiimoteChanged += wm_WiimoteChanged;
            wm1.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            this.Show();
		}

        private void Form1_FormClosing(object sender, FormClosingEventArgs e)
        {
            wm1.Disconnect();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            try
            {
                wm1.Connect();
                wm1.SetReportType(InputReport.IRAccel, true);
            }
            catch
            {
                MessageBox.Show("Exception thrown by Connect() method.");
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

        #endregion

        #region Wiimote event handlers

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

            g.Clear(Color.White);

            float penwidth = 2.0F;

            UpdateIR(ws.IRState.IRSensors[0], wm1IRLabel1, Color.Red, penwidth);
            UpdateIR(ws.IRState.IRSensors[1], wm1IRLabel2, Color.Blue, penwidth);
            UpdateIR(ws.IRState.IRSensors[2], wm1IRLabel3, Color.Black, penwidth);
            UpdateIR(ws.IRState.IRSensors[3], wm1IRLabel4, Color.Purple, penwidth);

            if (ws.IRState.IRSensors[0].Found && ws.IRState.IRSensors[1].Found)
                g.DrawEllipse(new Pen(Color.Green, 1.0F), (int)(drawscale * ws.IRState.RawMidpoint.X), (int)(drawscale * ws.IRState.RawMidpoint.Y), (int)(2 / drawscale), (int)(2 / drawscale));

            if (ws.ID == 0 | ws.ID == 1)
            {
                wm1IRPictureBox.Image = b;
            }
            else if (ws.ID == 2)
            {
                wm2IRPictureBox.Image = b;
            }

            //pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
            //lblBattery.Text = ws.Battery.ToString();
            //lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
        }

        private void UpdateIR(IRSensor irSensor, Label lblRaw, Color color, float penwidth)
        {

            if (irSensor.Found)
            {
                //lblNorm.Text = irSensor.Position.ToString() + ", " + irSensor.Size;
                lblRaw.Text = irSensor.RawPosition.ToString();
                g.DrawEllipse(new Pen(color, penwidth), (int)(drawscale * irSensor.RawPosition.X), (int)(drawscale * irSensor.RawPosition.Y),
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

        private void ResetCalibButton_Click(object sender, EventArgs e)
        {
            wiitrack.ResetCalibrateCapture();

            captureOneCountLabel.Text = "Captured " + wiitrack.CalibrateImageCount.ToString() + " Images";
        }

        private void StereoConnectButton_Click(object sender, EventArgs e)
        {

        }

    }
}
