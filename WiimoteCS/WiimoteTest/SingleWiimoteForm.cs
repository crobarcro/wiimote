//////////////////////////////////////////////////////////////////////////////////
//	SingleWiimoteForm.cs
//	Managed Wiimote Library Tester
//	Written by Brian Peek (http://www.brianpeek.com/)
//  for MSDN's Coding4Fun (http://msdn.microsoft.com/coding4fun/)
//	Visit http://blogs.msdn.com/coding4fun/archive/2007/03/14/1879033.aspx
//  and http://www.codeplex.com/WiimoteLib
//  for more information
//////////////////////////////////////////////////////////////////////////////////

using System;
using System.Windows.Forms;
using WiimoteLib;

namespace WiimoteTest
{

	public partial class SingleWiimoteForm : Form
	{
		Wiimote wm = new Wiimote();

		public SingleWiimoteForm()
		{
			InitializeComponent();
		}

		private void Form1_Load(object sender, EventArgs e)
		{
			wm.WiimoteChanged += wm_WiimoteChanged;
            wm.WiimoteExtensionChanged += wm_WiimoteExtensionChanged;

            wiimoteInfo1.Wiimote = wm;

            wiimoteInfo1.Show();
		}

		private void wm_WiimoteChanged(object sender, WiimoteChangedEventArgs args)
		{
			wiimoteInfo1.UpdateState(args);
		}

		private void wm_WiimoteExtensionChanged(object sender, WiimoteExtensionChangedEventArgs args)
		{
			wiimoteInfo1.UpdateExtension(args);

			if(args.Inserted)
				wm.SetReportType(InputReport.IRExtensionAccel, true);
			else
				wm.SetReportType(InputReport.IRAccel, true);
		}

		private void Form1_FormClosing(object sender, FormClosingEventArgs e)
		{
			wm.Disconnect();
		}

        private void button1_Click(object sender, EventArgs e)
        {

            try
            {
                wm.Connect();
                wm.SetReportType(InputReport.IRAccel, true);
            }
            catch
            {
                MessageBox.Show("Exception thrown by Connect() method.");
            }


        }

        private void setledsbutton_Click(object sender, EventArgs e)
        {

            wm.SetLEDs(this.LED1checkBox.Checked, this.LED2checkBox.Checked, this.LED3checkBox.Checked, this.LED4checkBox.Checked);
        }

        private void LED1checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LED2checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LED3checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }

        private void LED4checkBox_CheckedChanged(object sender, EventArgs e)
        {

        }
    }
}
