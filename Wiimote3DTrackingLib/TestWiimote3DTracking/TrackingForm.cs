using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using LocationPlot3D;

namespace TestWiimote3DTracking
{
    public partial class TrackingForm : Form
    {
        [System.Runtime.InteropServices.DllImportAttribute("gdi32.dll")]
        private static extern bool BitBlt(IntPtr hdcDest, int nXDest, int nYDest, int nWidth, int nHeight, IntPtr hdcSrc, int nXSrc, int nYSrc, System.Int32 dwRop);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern IntPtr GetDC(IntPtr hwnd);

        [System.Runtime.InteropServices.DllImportAttribute("user32.dll")]
        public static extern IntPtr ReleaseDC(IntPtr hwnd, IntPtr hdc);

        public TrackingForm()
        {
            InitializeComponent();
        }

        Math3D.Cube mainCube;
        Math3D.Plot3D mainPlot;
        Point drawOrigin;
        double cubeWidth;
        double cubeHeight;
        double cubeDepth;

        private void TrackingForm_Load(object sender, EventArgs e)
        {

            // Create a cube object, thix will be the plot area for the
            // location plot
            //mainCube = new Math3D.Cube((int)(plot3DPictureBox.Height), (int)(plot3DPictureBox.Height), (int)(2 * plot3DPictureBox.Height));
            
            Math3D.PlotPoint[] plotPoints = new Math3D.PlotPoint[2];
            plotPoints[0] = new Math3D.PlotPoint(50, 50, 50);
            plotPoints[1] = new Math3D.PlotPoint(50, 100, 50);

            mainPlot = new Math3D.Plot3D((int)(plot3DPictureBox.Height), (int)(plot3DPictureBox.Height), (int)(2 * plot3DPictureBox.Height), plotPoints);

            drawCube();

            Render();

        }

        private void drawCube()
        {

            Debug.WriteLine("PictureBox Width & Height: " + plot3DPictureBox.Width.ToString() + "  " + plot3DPictureBox.Height.ToString());

            double xzdiagonal = plot3DPictureBox.Width * 0.95;
            double xydiagonal = plot3DPictureBox.Height * 0.95;
            
            cubeWidth = xydiagonal / Math.Sqrt(2);
            cubeHeight = xydiagonal / Math.Sqrt(2);

            // determine the depth, depending on which side is the hypotenuse
            if (xzdiagonal > cubeWidth)
                cubeDepth = Math.Sqrt(Math.Pow(xzdiagonal,2.0) - Math.Pow(cubeWidth,2.0));
            else
                cubeDepth = Math.Sqrt(Math.Pow(cubeWidth, 2.0) - Math.Pow(xzdiagonal, 2.0));

            Debug.WriteLine("Cube Width, Height & Depth: " + cubeWidth.ToString() + "  " + cubeHeight.ToString() + "  " + cubeDepth.ToString());

            // determine the origin of the drawing in the picureBox
            drawOrigin = new Point(plot3DPictureBox.Width / 2, plot3DPictureBox.Height / 2);

            //mainCube.ResizeCube((int)(cubeHeight), (int)(cubeWidth), (int)(cubeDepth), drawOrigin);
            mainPlot.ResizePlot((int)(cubeHeight), (int)(cubeWidth), (int)(cubeDepth), drawOrigin);

        }

        private void Render()
        {
            drawCube();

            // get the values of the track bar rotation values
            //mainCube.RotateX = (float)rXTrackBar.Value;
            //mainCube.RotateY = (float)rYTrackBar.Value;
            //mainCube.RotateZ = (float)rZTrackBar.Value;

            mainPlot.RotateX = (float)rXTrackBar.Value;
            mainPlot.RotateY = (float)rYTrackBar.Value;
            mainPlot.RotateZ = (float)rZTrackBar.Value;

            // update the cube drawing
            //plot3DPictureBox.Image = mainCube.DrawCube(drawOrigin);
            plot3DPictureBox.Image = mainPlot.DrawPlot(drawOrigin);
        }

        private void TrackingForm_Resize(object sender, EventArgs e)
        {
            // keep the image in the middle when we resize the form
            drawCube();
            //drawOrigin = new Point(plot3DPictureBox.Width / 2, plot3DPictureBox.Height / 2);

            //mainCube.ResizeCube((int)(cubeWidth), (int)(cubeHeight), (int)(cubeDepth), drawOrigin);
            mainPlot.ResizePlot((int)(cubeWidth), (int)(cubeHeight), (int)(cubeDepth), drawOrigin);

            Render();
        }

        private void AxisTrackBar_Scroll(object sender, EventArgs e)
        {
            // refresh the view when we change the rotation in any axis
            this.Refresh();
        }

        private void TrackingForm_Paint(object sender, PaintEventArgs e)
        {
            Render();
        }

        protected override void OnClosing(CancelEventArgs e)
        {
            e.Cancel = true;
            this.Hide();
        }

        private void send2MatCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (send2MatCheckBox.Checked == true)
            {
                //System.Array pr = new double[4];
                //pr.SetValue(11, 0);
                //pr.SetValue(12, 1);
                //pr.SetValue(13, 2);
                //pr.SetValue(14, 3);

                //System.Array pi = new double[4];
                //pi.SetValue(1, 0);
                //pi.SetValue(2, 1);
                //pi.SetValue(3, 2);
                //pi.SetValue(4, 3);

                //matlab.PutFullMatrix("a", "base", pr, pi);

            }
            else
            {

            }
        }
    }
}
