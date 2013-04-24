using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace TestWiimote3DTracking
{
    public partial class TrackingForm : Form
    {
        public TrackingForm()
        {
            InitializeComponent();
        }

        private void TrackingForm_Load(object sender, EventArgs e)
        {

        }

        string XposLabel1Text
        {
            get {return XposLabel1.Text;}

            set {XposLabel1.Text = value;}
        }

        string YposLabel1Text
        {
            get { return YposLabel1.Text; }

            set { YposLabel1.Text = value; }
        }

        string ZposLabel1Text
        {
            get { return ZposLabel1.Text; }

            set { ZposLabel1.Text = value; }
        }


        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }
    }
}
