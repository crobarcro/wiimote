//////////////////////////////////////////////////////////////////////////////////
//	MultipleWiimoteForm.cs
//	Managed Wiimote Library Tester
//	Written by Brian Peek (http://www.brianpeek.com/)
//  for MSDN's Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//	Visit http://blogs.msdn.com/coding4fun/archive/2007/03/14/1879033.aspx
//  and http://www.codeplex.com/WiimoteLib
//  for more information
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using WiimoteLib;

namespace WiimoteTest
{
	public partial class WiimoteInfo : Form
	{
		private delegate void UpdateWiimoteStateDelegate(WiimoteChangedEventArgs args);
		private delegate void UpdateExtensionChangedDelegate(WiimoteExtensionChangedEventArgs args);

        private static Double drawscale = 0.5;
        private static Double drawx = drawscale * 1024;
        private static Double drawy = drawscale * 768;
		private Bitmap b = new Bitmap((int) drawx, (int) drawy, PixelFormat.Format24bppRgb);
		private Graphics g;

        //Wiimote mWiimote = new Wiimote();

		private Wiimote mWiimote;

		public WiimoteInfo()
		{
            Size tempsize = new Size();

            InitializeComponent();

            tempsize = this.pbIR.Size;
            tempsize.Width = (int) drawx;
            tempsize.Height = (int) drawy;

            this.pbIR.Size = tempsize;

            g = Graphics.FromImage(b);

            lblDevicePath.Text = "Wiimote Disconnected";

            mWiimote = new Wiimote();

            mWiimote.WiimoteChanged += wm_WiimoteChanged;
            mWiimote.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

		}

		public WiimoteInfo(Wiimote wm) : this()
		{
			mWiimote = wm;
		}

        private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
        {
            UpdateState(args);
        }

        private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
        {
            UpdateExtension(args);

            if (args.Inserted)
                mWiimote.SetReportType(InputReport.IRExtensionAccel, true);
            else
                mWiimote.SetReportType(InputReport.IRAccel, true);
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

        private void chkLED_CheckedChanged(object sender, EventArgs e)
        {
            //mWiimote.SetLEDs(chkLED1.Checked, chkLED2.Checked, chkLED3.Checked, chkLED4.Checked);
        }

        private void chkRumble_CheckedChanged(object sender, EventArgs e)
        {
            //mWiimote.SetRumble(chkRumble.Checked);
        }

		private void UpdateWiimoteChanged(WiimoteChangedEventArgs args)
		{
			WiimoteState ws = args.WiimoteState;

			clbButtons.SetItemChecked(0, ws.ButtonState.A);
			clbButtons.SetItemChecked(1, ws.ButtonState.B);
			clbButtons.SetItemChecked(2, ws.ButtonState.Minus);
			clbButtons.SetItemChecked(3, ws.ButtonState.Home);
			clbButtons.SetItemChecked(4, ws.ButtonState.Plus);
			clbButtons.SetItemChecked(5, ws.ButtonState.One);
			clbButtons.SetItemChecked(6, ws.ButtonState.Two);
			clbButtons.SetItemChecked(7, ws.ButtonState.Up);
			clbButtons.SetItemChecked(8, ws.ButtonState.Down);
			clbButtons.SetItemChecked(9, ws.ButtonState.Left);
			clbButtons.SetItemChecked(10, ws.ButtonState.Right);

			lblAccel.Text = ws.AccelState.Values.ToString();

			chkLED1.Checked = ws.LEDState.LED1;
			chkLED2.Checked = ws.LEDState.LED2;
			chkLED3.Checked = ws.LEDState.LED3;
			chkLED4.Checked = ws.LEDState.LED4;

			switch(ws.ExtensionType)
			{
				case ExtensionType.Nunchuk:
					lblChuk.Text = ws.NunchukState.AccelState.Values.ToString();
					lblChukJoy.Text = ws.NunchukState.Joystick.ToString();
					chkC.Checked = ws.NunchukState.C;
					chkZ.Checked = ws.NunchukState.Z;
					break;

				case ExtensionType.ClassicController:
					clbCCButtons.SetItemChecked(0, ws.ClassicControllerState.ButtonState.A);
					clbCCButtons.SetItemChecked(1, ws.ClassicControllerState.ButtonState.B);
					clbCCButtons.SetItemChecked(2, ws.ClassicControllerState.ButtonState.X);
					clbCCButtons.SetItemChecked(3, ws.ClassicControllerState.ButtonState.Y);
					clbCCButtons.SetItemChecked(4, ws.ClassicControllerState.ButtonState.Minus);
					clbCCButtons.SetItemChecked(5, ws.ClassicControllerState.ButtonState.Home);
					clbCCButtons.SetItemChecked(6, ws.ClassicControllerState.ButtonState.Plus);
					clbCCButtons.SetItemChecked(7, ws.ClassicControllerState.ButtonState.Up);
					clbCCButtons.SetItemChecked(8, ws.ClassicControllerState.ButtonState.Down);
					clbCCButtons.SetItemChecked(9, ws.ClassicControllerState.ButtonState.Left);
					clbCCButtons.SetItemChecked(10, ws.ClassicControllerState.ButtonState.Right);
					clbCCButtons.SetItemChecked(11, ws.ClassicControllerState.ButtonState.ZL);
					clbCCButtons.SetItemChecked(12, ws.ClassicControllerState.ButtonState.ZR);
					clbCCButtons.SetItemChecked(13, ws.ClassicControllerState.ButtonState.TriggerL);
					clbCCButtons.SetItemChecked(14, ws.ClassicControllerState.ButtonState.TriggerR);

					lblCCJoy1.Text = ws.ClassicControllerState.JoystickL.ToString();
					lblCCJoy2.Text = ws.ClassicControllerState.JoystickR.ToString();

					lblTriggerL.Text = ws.ClassicControllerState.TriggerL.ToString();
					lblTriggerR.Text = ws.ClassicControllerState.TriggerR.ToString();
					break;

				case ExtensionType.Guitar:
				    clbGuitarButtons.SetItemChecked(0, ws.GuitarState.FretButtonState.Green);
				    clbGuitarButtons.SetItemChecked(1, ws.GuitarState.FretButtonState.Red);
				    clbGuitarButtons.SetItemChecked(2, ws.GuitarState.FretButtonState.Yellow);
				    clbGuitarButtons.SetItemChecked(3, ws.GuitarState.FretButtonState.Blue);
				    clbGuitarButtons.SetItemChecked(4, ws.GuitarState.FretButtonState.Orange);
				    clbGuitarButtons.SetItemChecked(5, ws.GuitarState.ButtonState.Minus);
				    clbGuitarButtons.SetItemChecked(6, ws.GuitarState.ButtonState.Plus);
				    clbGuitarButtons.SetItemChecked(7, ws.GuitarState.ButtonState.StrumUp);
				    clbGuitarButtons.SetItemChecked(8, ws.GuitarState.ButtonState.StrumDown);

					clbTouchbar.SetItemChecked(0, ws.GuitarState.TouchbarState.Green);
					clbTouchbar.SetItemChecked(1, ws.GuitarState.TouchbarState.Red);
					clbTouchbar.SetItemChecked(2, ws.GuitarState.TouchbarState.Yellow);
					clbTouchbar.SetItemChecked(3, ws.GuitarState.TouchbarState.Blue);
					clbTouchbar.SetItemChecked(4, ws.GuitarState.TouchbarState.Orange);

					lblGuitarJoy.Text = ws.GuitarState.Joystick.ToString();
					lblGuitarWhammy.Text = ws.GuitarState.WhammyBar.ToString();
					lblGuitarType.Text = ws.GuitarState.GuitarType.ToString();
				    break;

				case ExtensionType.Drums:
					clbDrums.SetItemChecked(0, ws.DrumsState.Red);
					clbDrums.SetItemChecked(1, ws.DrumsState.Blue);
					clbDrums.SetItemChecked(2, ws.DrumsState.Green);
					clbDrums.SetItemChecked(3, ws.DrumsState.Yellow);
					clbDrums.SetItemChecked(4, ws.DrumsState.Orange);
					clbDrums.SetItemChecked(5, ws.DrumsState.Pedal);
					clbDrums.SetItemChecked(6, ws.DrumsState.Minus);
					clbDrums.SetItemChecked(7, ws.DrumsState.Plus);

					lbDrumVelocity.Items.Clear();
					lbDrumVelocity.Items.Add(ws.DrumsState.RedVelocity);
					lbDrumVelocity.Items.Add(ws.DrumsState.BlueVelocity);
					lbDrumVelocity.Items.Add(ws.DrumsState.GreenVelocity);
					lbDrumVelocity.Items.Add(ws.DrumsState.YellowVelocity);
					lbDrumVelocity.Items.Add(ws.DrumsState.OrangeVelocity);
					lbDrumVelocity.Items.Add(ws.DrumsState.PedalVelocity);

					lblDrumJoy.Text = ws.DrumsState.Joystick.ToString();
					break;

				case ExtensionType.BalanceBoard:
					if(chkLbs.Checked)
					{
						lblBBTL.Text = ws.BalanceBoardState.SensorValuesLb.TopLeft.ToString();
						lblBBTR.Text = ws.BalanceBoardState.SensorValuesLb.TopRight.ToString();
						lblBBBL.Text = ws.BalanceBoardState.SensorValuesLb.BottomLeft.ToString();
						lblBBBR.Text = ws.BalanceBoardState.SensorValuesLb.BottomRight.ToString();
						lblBBTotal.Text = ws.BalanceBoardState.WeightLb.ToString();
					}
					else
					{
						lblBBTL.Text = ws.BalanceBoardState.SensorValuesKg.TopLeft.ToString();
						lblBBTR.Text = ws.BalanceBoardState.SensorValuesKg.TopRight.ToString();
						lblBBBL.Text = ws.BalanceBoardState.SensorValuesKg.BottomLeft.ToString();
						lblBBBR.Text = ws.BalanceBoardState.SensorValuesKg.BottomRight.ToString();
						lblBBTotal.Text = ws.BalanceBoardState.WeightKg.ToString();
					}
					lblCOG.Text = ws.BalanceBoardState.CenterOfGravity.ToString();
					break;
			}

			//g.Clear(Color.Black);
            g.Clear(Color.White);

            float penwidth = 2.0F;

            UpdateIR(ws.IRState.IRSensors[0], lblIR1, lblIR1Raw, chkFound1, Color.Red, penwidth);
            UpdateIR(ws.IRState.IRSensors[1], lblIR2, lblIR2Raw, chkFound2, Color.Blue, penwidth);
            UpdateIR(ws.IRState.IRSensors[2], lblIR3, lblIR3Raw, chkFound3, Color.Black, penwidth);
            UpdateIR(ws.IRState.IRSensors[3], lblIR4, lblIR4Raw, chkFound4, Color.Purple, penwidth);

			if(ws.IRState.IRSensors[0].Found && ws.IRState.IRSensors[1].Found)
                g.DrawEllipse(new Pen(Color.Green, 1.0F), (int)(drawscale * ws.IRState.RawMidpoint.X), (int)(drawscale * ws.IRState.RawMidpoint.Y), (int)(2 / drawscale), (int)(2 / drawscale));

			pbIR.Image = b;

			pbBattery.Value = (ws.Battery > 0xc8 ? 0xc8 : (int)ws.Battery);
			lblBattery.Text = ws.Battery.ToString();
			lblDevicePath.Text = "Device Path: " + mWiimote.HIDDevicePath;
		}

        private void UpdateIR(IRSensor irSensor, Label lblNorm, Label lblRaw, CheckBox chkFound, Color color, float penwidth)
		{
			chkFound.Checked = irSensor.Found;

			if(irSensor.Found)
			{
				lblNorm.Text = irSensor.Position.ToString() + ", " + irSensor.Size;
				lblRaw.Text = irSensor.RawPosition.ToString();
				g.DrawEllipse(new Pen(color, penwidth), (int)(drawscale * irSensor.RawPosition.X), (int)(drawscale * irSensor.RawPosition.Y),
							 (int)((irSensor.Size+1)/drawscale), (int)((irSensor.Size+1)/drawscale));
			}
		}

		private void UpdateExtensionChanged(WiimoteExtensionChangedEventArgs args)
		{
			chkExtension.Text = args.ExtensionType.ToString();
			chkExtension.Checked = args.Inserted;
		}

		public Wiimote Wiimote
		{
			set { mWiimote = value; }
		}

        private void WiimoteInfo_Load(object sender, EventArgs e)
        {
            this.Show();
        }

        private void WiimoteInfo_FormClosing(object sender, FormClosingEventArgs e)
        {
            mWiimote.Disconnect();
        }

        private void connectbutton_Click(object sender, EventArgs e)
        {
            bool success = false;

            try
            {
                mWiimote.Connect();
                mWiimote.SetReportType(InputReport.IRAccel, true);
                success = true;
            }
            catch
            {
                MessageBox.Show("Exception thrown by Connect() method.");
            }

            if (success)
            {
                if (mWiimote.WiimoteState.LEDState.LED1)
                {
                    mWiimote.SetLEDs(false, true, false, false);
                }
                else
                {
                    mWiimote.SetLEDs(true, false, false, false);
                }

                this.connectbutton.Enabled = false;
                this.disconnectbutton.Enabled = true;
            }

        }

        private void disconnectbutton_Click(object sender, EventArgs e)
        {
            mWiimote.Disconnect();

            this.connectbutton.Enabled = true;
            this.disconnectbutton.Enabled = false;
            lblDevicePath.Text = "Wiimote Disconnected";
        }


	}
}
