using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.Util;
using System.Collections;
using System.Diagnostics;

namespace Wiimote3DTrackingLib
{

    public class StereoTracking
    {
        
        // The size of the Wiimote camera view is fixed at 1024 x 768 pixels
        private static Size wiimoteCamSize = new System.Drawing.Size(1024, 768);

        // The expected number of IR sources
        private static int NUM_IR_SRCS = 4;

        /// <summary>
        /// the maximum number of calibration images (or sets of four
        /// coordinates in this case
        /// </summary>
        private static int MAX_NUM_OF_CAL_IMAGES = 100;

        // The length of a side of the calibration square
        private float sqSideLength;

        // A (MAX_NUM_OF_CAL_IMAGES x 4) array of MCvPoint3D32f objects to hold the 
        // calibration image coordinates, there will be MAX_NUM_OF_CAL_IMAGES sets
        // of four corner coordinates stored in the array
        private Emgu.CV.Structure.MCvPoint3D32f[][] CalibObjectPoints = new Emgu.CV.Structure.MCvPoint3D32f[MAX_NUM_OF_CAL_IMAGES][];

        // We need an array of the same size to hold captured calibration images
        // from the wiimote camera
        private System.Drawing.PointF[][] wm1capturedImages = new System.Drawing.PointF[MAX_NUM_OF_CAL_IMAGES][];

        // Delcare a counter to determine how many images have been captured
        // for a given calibration.
        private int capCount = 0;

        public StereoTracking()
        {
            sqSideLength = (float)(136.0 / 1000.0);

            InitializeStereoTrackingImages();
        }

        /// <summary>
        /// Constructor which takes the width of the side of the calibration 
        /// square as input. Length is in meters, if a real lengthis to be used.
        /// </summary>
        /// <param name="sWidth">The width of the calibration square sides.</param>
        public StereoTracking(float sWidth)
        {
            sqSideLength = sWidth;

            InitializeStereoTrackingImages();
        }

        /// <summary>
        /// Gets or sets the size of the sides of the calibration square 
        /// in metres.
        /// </summary>
        public float SquareSideLength
        {
            get { return sqSideLength; }

            set { sqSideLength = SquareSideLength; }
        }

        /// <summary>
        /// Returns the number of calibration images that have captured.
        /// </summary>
        public float CalibrateImageCount
        {
            get { return capCount; }
        }

        /// <summary>
        /// Resets the calibration image capture process.
        /// </summary>
        public void ResetCalibrateCapture()
        {
            capCount = 0;
        }

        /// <summary>
        /// Initializes the arrays of images and points used in the calibration.
        /// The maximum number of points that can be used is defined by the constant.
        /// MAX_NUM_OF_CAL_IMAGES.
        /// </summary>
        private void InitializeStereoTrackingImages()
        {
            // Initialize the array of calibration points (the location 
            // of the corners of the calibration square in its coordinate 
            // reference frame). These points are all identical as the shape
            // of the square does not change and they are in the coordinate
            // frame of the square
            CalibObjectPoints.Initialize();

            for (int i = 0; i<CalibObjectPoints.Length; i++)
            {
                // each member of CalibObjectPoints is initialised to an
                // array of 4 coordinates, one for each corner of the 
                // calibration square
                CalibObjectPoints[i] = new Emgu.CV.Structure.MCvPoint3D32f[4];
                CalibObjectPoints[i].Initialize();

                // the corners are stored moving anti-clockwise from the bottom left
                //    1  2  3  4
                // x  0  1  1  0 ;  
                // y  0  0  1  1 ; 
                // z  0  0  0  0
                CalibObjectPoints[i][0].x = 0;
                CalibObjectPoints[i][0].y = 0;
                CalibObjectPoints[i][0].z = 0;

                CalibObjectPoints[i][1].x = sqSideLength;
                CalibObjectPoints[i][1].y = 0;
                CalibObjectPoints[i][1].z = 0;

                CalibObjectPoints[i][2].x = sqSideLength;
                CalibObjectPoints[i][2].y = sqSideLength;
                CalibObjectPoints[i][2].z = 0;

                CalibObjectPoints[i][3].x = 0;
                CalibObjectPoints[i][3].y = sqSideLength;
                CalibObjectPoints[i][3].z = 0;
            }

            // Now initialize the array to hold the captured images 
            // from the camera
            wm1capturedImages.Initialize();

            // Now initialize each of the sub arrays for holding the sets of 4 points
            for (int i = 0; i < wm1capturedImages.Length; i++)
            {
                wm1capturedImages[i] = new System.Drawing.PointF[4];
                wm1capturedImages[i].Initialize();
            }

        }


        /// <summary>
        /// Calibrate a single Wiimote camera
        /// </summary>
        /// <param name="objectpoints">The locations of the object points on the calibration object. 
        /// These will always be in the reference frame of the object, in this case a 2D square so that 
        /// the 'Z' components will all be zero.</param>
        /// <param name="imagePoints">The 2D locations of the object points on the Wiimote camera image.</param>
        /// <param name="wm">A Wiimote object for the wiimote camera to be calibrated.</param>
        public void CalibrateCamera(WiimoteLib.Wiimote wm)
        {
//, System.Drawing.PointF[][] imagePoints)
            int i = 0;

            Emgu.CV.Structure.MCvPoint3D32f[][] objectPoints = new Emgu.CV.Structure.MCvPoint3D32f[capCount][];

            System.Drawing.PointF[][] imagePoints = new System.Drawing.PointF[capCount][];

            // Initialize an array of calibration object points of the same size as 
            // the number of captured images
            for (i=0; i<objectPoints.Length; i++)
            {
                objectPoints[i] = new Emgu.CV.Structure.MCvPoint3D32f[4];
                imagePoints[i] = new System.Drawing.PointF[4];

                for (int j = 0; j < 4; j++)
                {
                    objectPoints[i][j] = new Emgu.CV.Structure.MCvPoint3D32f();
                    imagePoints[i][j] = new System.Drawing.PointF();
                }
            }

            Array.Copy(CalibObjectPoints, objectPoints, capCount);
            Array.Copy(wm1capturedImages, imagePoints, capCount);

            Emgu.CV.CameraCalibration.CalibrateCamera(objectPoints,
                imagePoints,
                wiimoteCamSize,
                wm.WiimoteState.CameraCalibInfo.CamIntrinsic,
                Emgu.CV.CvEnum.CALIB_TYPE.DEFAULT,
                out wm.WiimoteState.CameraCalibInfo.SingleCamExtrinsic);

            //Emgu.CV.CameraCalibration.CalibrateCamera(objectPoints,
            //    imagePoints,
            //    wiimoteCamSize,
            //    wm.WiimoteState.CameraCalibInfo.CamIntrinsic,
            //    Emgu.CV.CvEnum.CALIB_TYPE.DEFAULT,
            //    out wm.WiimoteState.CameraCalibInfo.CamExtrinsic);

        }


        public void StereoCalibrate(Emgu.CV.Structure.MCvPoint3D32f[][] objectPoints, System.Drawing.PointF[][] leftImagePoints, System.Drawing.PointF[][] rightImagePoints, WiimoteLib.Wiimote leftwm, WiimoteLib.Wiimote rightwm)
        {

            Matrix<double> fundMat = new Matrix<double>(2, 2);
            Matrix<double> essentialMat = new Matrix<double>(3, 3);

            int maxIters = 1000;

            Emgu.CV.Structure.MCvTermCriteria termCrit = new Emgu.CV.Structure.MCvTermCriteria(maxIters);

            Emgu.CV.CameraCalibration.StereoCalibrate(objectPoints,
                leftImagePoints,
                rightImagePoints,
                leftwm.WiimoteState.CameraCalibInfo.CamIntrinsic,
                rightwm.WiimoteState.CameraCalibInfo.CamIntrinsic,
                wiimoteCamSize,
                Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_USE_INTRINSIC_GUESS,
                termCrit,
                out leftwm.WiimoteState.CameraCalibInfo.StereoCamExtrinsic,
                out fundMat,
                out essentialMat);
        }

        public int SingleCalibCapture(WiimoteLib.Wiimote wm)
        {
            int i = 0;

            int irpoints = wm.WiimoteState.IRPoints();

            Wiimote3DTrackingLib.coord[] coords = new Wiimote3DTrackingLib.coord[irpoints];

            PointF[] spointf = new PointF[irpoints];

            for (i = 0; i < coords.Length; i++)
            {
                coords[i] = new coord();
                spointf[i] = new PointF();
            }

            if (!(CaptureOneWM(wm, coords) == 0))
            {
                return -2;
            }

            if (capCount < MAX_NUM_OF_CAL_IMAGES)
            {
                for (i = 0; i < coords.Length; i++)
                {
                    spointf[i] = coords[i].ToPointf();
                }

                this.wm1capturedImages[capCount] = spointf;

                // Increment the count of the number of 
                // calibration images captured
                capCount++;
            }
            else if (capCount >= MAX_NUM_OF_CAL_IMAGES)
            {
                return -1;
            }
            
            return 0;
        }

        public void CaptureMultipleWM(WiimoteLib.Wiimote[] wmarray, coord[][] coords)
        {
            int i = 0;

            //coord[][] coords = new coord[wmarray.Length][];
            //coords.Initialize();

            //// Captures the IR sensor output from several wiimotes
            //for (i = 0; i < wmarray.Length; i++)
            //{
            //    coords[i] = new coord[NUM_IR_SRCS];
            //    coords[i].Initialize();
            //}

            if (wmarray.Length == coords.Length)
            {
                for (i = 0; i < wmarray.Length; i++)
                {
                    CaptureOneWM(wmarray[i], coords[i]);
                }
            }

            //return coords;

        }

        public int CaptureOneWM(WiimoteLib.Wiimote wm, coord[] coords)
        {
            // Captures the IR sensor output from the wiimotes and prints 
            // the output in a form readable by MATLAB, i.e. creates MATLAB
            // commands of the type X = [1, 2, 3]; that can make up a m-file 
            // to be run later. This is either written to a file, or to  
            // stdout depending on the contents of the global file variable

            //coord[] coords = new coord[NUM_IR_SRCS];

            if (!(get_coords(wm, coords) == 0))
            {
                return -1;
            }

            Array.Sort(coords, 0, 4, coord.compare_x());
            Array.Sort(coords, 0, 2, coord.compare_x());
            Array.Sort(coords, 2, 2, coord.compare_x());

            // Wii puts origin in bottom left, Y up; toolbox in top left, Y down.  Convert by subtracting y from 768.		
            for (int i = 0; i < NUM_IR_SRCS; i++)
            {
                coords[i].y = 768 - coords[i].y;
                Debug.WriteLine("coords[" + i.ToString() + "] = " + coords[i].ToString());
            }

            return 0;

        }

        /// <summary>
        /// Gets a set of coordinates representing the four corners of the 
        /// calibration square from a Wiimote. Returns 0 if successful or 
        /// 1 if four IR points cannot be seen, or the data from the sources 
        /// is invalid.
        /// </summary>
        /// <param name="wm">Wiimote object.</param>
        /// <param name="coords">Array of four coord objects for storing the coordinates.</param>
        /// <returns>Integer indicating success, 0 if successful, otherwise 1.</returns>
        int get_coords(WiimoteLib.Wiimote wm, coord[] coords)
        {

            int i = 0;
            coord c = new coord();

            // Check if the wiimote is connected and 4 IR sources can be found
            if (wm.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected & wm.WiimoteState.IRPoints() == NUM_IR_SRCS)
            {
                // get the coordinates of each IR source in the camera view
                for (i = 0; i < wm.WiimoteState.IRPoints(); i++)
                {
                    c.x = wm.WiimoteState.IRState.IRSensors[i].RawPosition.X;

                    c.y = wm.WiimoteState.IRState.IRSensors[i].RawPosition.Y;

                    if (c.x > 1024 || c.x < 0 || c.y > 768 || c.y < 0)
                    {
                        // the IR data is invalid for this source
                        return 1;
                    }

                    c.pt = i+1;

                    // store the captured coordinate in the array
                    coords[i].x = c.x;
                    coords[i].y = c.y;
                    coords[i].pt = c.pt;
                }
            }
            else
            {
                // return 1 as we could not get info from controller, it may be disconnected
                return 1;
            }

            return 0;

        }


    }

    /// <summary>
    /// A coord type to hold coordinate data with x y and number info.
    /// The coord implements the IComparer interface for sorting the 
    /// order of the coordinates found by the Wiimotes.
    /// </summary>
    public class coord : IComparable
    {

        // Beginning of nested classes.

        // Nested class to do ascending sort on x coordinate.
        private class compare_xHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                coord c1 = new coord();
                coord c2 = new coord();

                c1 = (coord)a;
                c2 = (coord)b;

                if (c1._x > c2._x)
                    return 1;

                if (c1._x < c2._x)
                    return -1;

                else
                    return 0;
            }
        }

        // Nested class to do ascending sort on y coordinate.
        private class compare_yHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                coord c1 = (coord)a;
                coord c2 = (coord)b;

                if (c1._y > c2._y)
                    return 1;

                if (c1._y < c2._y)
                    return -1;

                else
                    return 0;
            }
        }

        // Nested class to do descending sort on y coordinate.
        private class compare_y_invHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                coord c1 = (coord)a;
                coord c2 = (coord)b;

                if (c1._y < c2._y)
                    return 1;

                if (c1._y > c2._y)
                    return -1;

                else
                    return 0;
            }
        }

        // End of nested classes.

        // initialize the variables to all be -1

        /// <summary>
        /// X-coordinate
        /// </summary>
        private int _x = -1;
        /// <summary>
        /// Y-coordinate
        /// </summary>
        private int _y = -1;
        /// <summary>
        /// Coordinate number
        /// </summary>
        private int _pt = -1;

        public coord()
        {
            _x = -1;
            _y = -1;
            _pt = -1;
        }

        public int x
        {
            get { return _x; }
            set { _x = value; }
        }

        public int y
        {
            get { return _y; }
            set { _y = value; }
        }

        public int pt
        {
            get { return _pt; }
            set { _pt = value; }
        }

        public override string ToString()
		{
			return string.Format("{{X={0}, Y={1}, i={2}}}", _x, _y, _pt);
		}

        public System.Drawing.PointF ToPointf()
        {
            PointF spointf = new PointF();

            spointf.X = (float)(_x);
            spointf.Y = (float)(_y);

            return spointf;
        }

        // Implement IComparable CompareTo to provide default sort order.
        int IComparable.CompareTo(object obj)
        {
            coord c = (coord)obj;
            if (this._pt > c._pt)
            {
                return 1;
            }
            if (this._pt < c._pt)
            {
                return -1;
            }
            else
                return 0;
        }


        // Method to return IComparer object for sort helper.
        public static IComparer compare_x()
        {
            return (IComparer)new compare_xHelper();
        }

        // Method to return IComparer object for sort helper.
        public static IComparer compare_y()
        {
            return (IComparer)new compare_yHelper();
        }

        // Method to return IComparer object for sort helper.
        public static IComparer compare_y_inv()
        {
            return (IComparer)new compare_y_invHelper();
        }


    }

}
