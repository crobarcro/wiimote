﻿using System;
using System.Diagnostics;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Emgu.CV;
using Emgu.Util;
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

            // Create a cube object, thix will be the plot area for the
            // location plot
            //mainCube = new Math3D.Cube((int)(plot3DPictureBox.Height), (int)(plot3DPictureBox.Height), (int)(2 * plot3DPictureBox.Height));

            //Math3D.PlotPoint[] plotPoints = new Math3D.PlotPoint[2];
            //plotPoints[0] = new Math3D.PlotPoint(0.1f, 0.1f, 0.1f);
            //plotPoints[1] = new Math3D.PlotPoint(0.5f, 0.5f, 0.5f);

            mainPlot = new Math3D.Plot3D((int)(plot3DPictureBox.Height), (int)(plot3DPictureBox.Height), (int)(2 * plot3DPictureBox.Height), 4);
            //mainPlot.PlotPoints[0].SetPosition(0.1, 0.1, 0.1);
            //mainPlot.PlotPoints[1].SetPosition(0.5, 0.5, 0.5);
            //mainPlot.PlotPoints[2].SetPosition(0.9, 0.9, 0.9);

            drawPlot();

            //Render();
        }

        Math3D.Plot3D mainPlot;
        Point drawOrigin;
        double cubeWidth;
        double cubeHeight;
        double cubeDepth;

        private bool do3Dplot = false;

        protected override void OnFormClosing(FormClosingEventArgs e)
        {
            base.OnFormClosing(e);

            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;

                this.Hide();
            }
            else
            {
                return;
            }

        }

        private void TrackingForm_Load(object sender, EventArgs e)
        {
            timer1.Start();
        }

        public void SetTrackingPointPos(double x, double y, double z, int ID)
        {
            x = x + 0.5;
            y = y + 0.5;

            z = z * cubeWidth / cubeDepth; 

            mainPlot.PlotPoints[ID].SetPosition(x, y, z);
        }

        private void drawPlot()
        {

            //Debug.WriteLine("PictureBox Width & Height: " + plot3DPictureBox.Width.ToString() + "  " + plot3DPictureBox.Height.ToString());

            double xzdiagonal = plot3DPictureBox.Width * 0.95;
            double xydiagonal = plot3DPictureBox.Height * 0.95;
            
            cubeWidth = xydiagonal / Math.Sqrt(2);
            cubeHeight = xydiagonal / Math.Sqrt(2);

            // determine the depth, depending on which side is the hypotenuse
            if (xzdiagonal > cubeWidth)
                cubeDepth = Math.Sqrt(Math.Pow(xzdiagonal,2.0) - Math.Pow(cubeWidth,2.0));
            else
                cubeDepth = Math.Sqrt(Math.Pow(cubeWidth, 2.0) - Math.Pow(xzdiagonal, 2.0));

            //Debug.WriteLine("Cube Width, Height & Depth: " + cubeWidth.ToString() + "  " + cubeHeight.ToString() + "  " + cubeDepth.ToString());

            // determine the origin of the drawing in the picureBox
            drawOrigin = new Point(plot3DPictureBox.Width / 2, plot3DPictureBox.Height / 2);

            //mainCube.ResizeCube((int)(cubeHeight), (int)(cubeWidth), (int)(cubeDepth), drawOrigin);
            mainPlot.ResizePlot((int)(cubeHeight), (int)(cubeWidth), (int)(cubeDepth), drawOrigin);

        }

        private void Render()
        {
            drawPlot();

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
            drawPlot();
            //drawOrigin = new Point(plot3DPictureBox.Width / 2, plot3DPictureBox.Height / 2);

            //mainCube.ResizeCube((int)(cubeWidth), (int)(cubeHeight), (int)(cubeDepth), drawOrigin);
            mainPlot.ResizePlot((int)(cubeWidth), (int)(cubeHeight), (int)(cubeDepth), drawOrigin);

            //Render();
        }

        private void AxisTrackBar_Scroll(object sender, EventArgs e)
        {
            // refresh the view when we change the rotation in any axis
            this.Refresh();
        }

        private void TrackingForm_Paint(object sender, PaintEventArgs e)
        {
            //Render();
        }

        //protected override void OnClosing(CancelEventArgs e)
        //{
        //    e.Cancel = true;
        //    this.Hide();
        //}

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

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (do3Dplot)
                Render();
        }

        private void do3DPlotCheckBox_CheckedChanged(object sender, EventArgs e)
        {
            if (do3DPlotCheckBox.Checked)
            {
                do3Dplot = true;
            }
            else
            {
                do3Dplot = false;
            }
        }

        public void Display3DPointText(Matrix<double>[] result3DPoints)
        {

            if (!(result3DPoints[0].Data[0, 0] == -9999 && result3DPoints[0].Data[1, 0] == -9999 && result3DPoints[0].Data[2, 0] == -9999))
                {
                    this.XposLabel1.Text = "X: " + result3DPoints[0].Data[0, 0].ToString("f4");
                    this.YposLabel1.Text = "Y: " + result3DPoints[0].Data[1, 0].ToString("f4");
                    this.ZposLabel1.Text = "Z: " + result3DPoints[0].Data[2, 0].ToString("f4");
                }
                else
                {
                    this.XposLabel1.Text = "X: No IR";
                    this.YposLabel1.Text = "Y: No IR";
                    this.ZposLabel1.Text = "Z: No IR";
                }

                if (!(result3DPoints[1].Data[0, 0] == -9999 && result3DPoints[1].Data[1, 0] == -9999 && result3DPoints[1].Data[2, 0] == -9999))
                {
                    this.XposLabel2.Text = "X: " + result3DPoints[1].Data[0, 0].ToString("f4");
                    this.YposLabel2.Text = "Y: " + result3DPoints[1].Data[1, 0].ToString("f4");
                    this.ZposLabel2.Text = "Z: " + result3DPoints[1].Data[2, 0].ToString("f4");
                }
                else
                {
                    this.XposLabel2.Text = "X: No IR";
                    this.YposLabel2.Text = "Y: No IR";
                    this.ZposLabel2.Text = "Z: No IR";
                }

                if (!(result3DPoints[2].Data[0, 0] == -9999 && result3DPoints[2].Data[1, 0] == -9999 && result3DPoints[2].Data[2, 0] == -9999))
                {
                    this.XposLabel3.Text = "X: " + result3DPoints[2].Data[0, 0].ToString("f4");
                    this.YposLabel3.Text = "Y: " + result3DPoints[2].Data[1, 0].ToString("f4");
                    this.ZposLabel3.Text = "Z: " + result3DPoints[2].Data[2, 0].ToString("f4");
                }
                else
                {
                    this.XposLabel3.Text = "X: No IR";
                    this.YposLabel3.Text = "Y: No IR";
                    this.ZposLabel3.Text = "Z: No IR";
                }

                if (!(result3DPoints[3].Data[0, 0] == -9999 && result3DPoints[3].Data[1, 0] == -9999 && result3DPoints[3].Data[2, 0] == -9999))
                {
                    this.XposLabel4.Text = "X: " + result3DPoints[3].Data[0, 0].ToString("f4");
                    this.YposLabel4.Text = "Y: " + result3DPoints[3].Data[1, 0].ToString("f4");
                    this.ZposLabel4.Text = "Z: " + result3DPoints[3].Data[2, 0].ToString("f4");
                }
                else
                {
                    this.XposLabel4.Text = "X: No IR";
                    this.YposLabel4.Text = "Y: No IR";
                    this.ZposLabel4.Text = "Z: No IR";
                }

                for (int j = 0; j < result3DPoints.Length; j++)
                {
                    this.SetTrackingPointPos(result3DPoints[j].Data[0, 0],
                        result3DPoints[j].Data[1, 0], -result3DPoints[j].Data[2, 0], j);
                }
        }
    }
}
