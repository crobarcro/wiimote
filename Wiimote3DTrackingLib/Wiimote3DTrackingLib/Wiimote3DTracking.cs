﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Emgu.CV;
using Emgu.Util;
using System.Collections;
using System.Diagnostics;
using System.Timers;
using System.IO;

namespace Wiimote3DTrackingLib
{
    using cv = Emgu.CV.CvInvoke;
    using System.Threading;

    // A delegate type for hooking up change notifications.
    public delegate void LogEventHandler(object sender, EventArgs e);

    [Serializable] public class StereoTracking
    {
        // Timer object for data logging at set intervals
        private System.Timers.Timer datalogtimer;

        // File name to save data to
        private string _logfilename;

        // A streamWriter to write to the log file
        private StreamWriter logStreamWriter;

        // An event that clients can use to be notified whenever a
        // logging event occurs
        public event LogEventHandler LogTimerTick;

        // matrix to hold the 3D locations when logging data
        private Matrix<double>[] logging3DPoints = new Matrix<double>[4];

        // an object to use as a locker
        private object _result3DLocker = new object();

        // boolean for deermining whether triangulation should be performed when 
        // logging at set intervals
        private bool DOTRIANGULATION = true;

        // boolean to determine whether to undistort point when only capturing data
        // and not performing a triangulation
        private bool DOUNDISTORTPOINTS = false;

        // boolean to determine if we should add the 3D locations to the log file
        // when logging
        private bool DOLOG3D = false;

        // boolean to determine if we should add the raw data points to the log
        // file when logging
        private bool DOLOGRAW = false;

        // boolean to determine whether we should add the undistorted points to
        // the log file when logging
        private bool DOLOGUNDISTORTED = false;

        // wiimote objects for use when logging data
        WiimoteLib.Wiimote loggingwm1, loggingwm2;

        // The size of the Wiimote camera view is fixed at 1024 x 768 pixels
        private static Size wiimoteCamSize = new System.Drawing.Size(1024, 768);
        //private static Size wiimoteCamSize = new System.Drawing.Size(1016, 760);

        // define a Y offset that takes account of the sensor bar position adjustment
        private static int yWiiIROffset = 51;

        // The expected number of IR sources
        private static readonly int NUM_IR_SRCS = 4;

        /// <summary>
        /// The maximum number of IR sources we can expect to see
        /// </summary>
        private static readonly int MAX_NUM_IR_SRCS = 4;

        /// <summary>
        /// the maximum number of calibration images (or sets of four
        /// coordinates in this case
        /// </summary>
        private static readonly int MAX_NUM_OF_CAL_IMAGES = 100;

        // The size of the calibration rectangle
        private SizeF calibRectSize = new SizeF(1, 1);

        // A (MAX_NUM_OF_CAL_IMAGES x 4) array of MCvPoint3D32f objects to hold the 
        // calibration image coordinates, there will be MAX_NUM_OF_CAL_IMAGES sets
        // of four corner coordinates stored in the array
        private Emgu.CV.Structure.MCvPoint3D32f[][] CalibObjectPoints = new Emgu.CV.Structure.MCvPoint3D32f[MAX_NUM_OF_CAL_IMAGES][];

        // We need an array of the same size to hold captured calibration images
        // from the wiimote cameras
        private System.Drawing.PointF[][] singlewmCapturedImages = new System.Drawing.PointF[MAX_NUM_OF_CAL_IMAGES][];
        private System.Drawing.PointF[][] wm1capturedImages = new System.Drawing.PointF[MAX_NUM_OF_CAL_IMAGES][];
        private System.Drawing.PointF[][] wm2capturedImages = new System.Drawing.PointF[MAX_NUM_OF_CAL_IMAGES][];

        // Delcare counters to determine how many images have been captured
        // for a given calibration.
        private int capCount = 0;
        private int stereoCapCount = 0;

        // Fundumental matrix
        Matrix<double> fundMat = new Matrix<double>(3, 3);

        //Essential matrix
        Matrix<double> essentialMat = new Matrix<double>(3, 3);

        // The rotation and perspective matrices 
        Matrix<double> R1 = new Matrix<double>(3, 3);
        Matrix<double> R2 = new Matrix<double>(3, 3);
        Matrix<double> P1 = new Matrix<double>(3, 4);
        Matrix<double> P2 = new Matrix<double>(3, 4);

        // The Q matrix will be generated during the stereo calibration
        // and used to reconstruct the 3D location of an IR point from 
        // its location in the viewer
        Matrix<double> Q = new Matrix<double>(4, 4);

        // variables for left and right image points, and their undistorted 
        // counterparts that will be used for stereo tracking
        private System.Drawing.PointF[] leftimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] rightimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] UDleftimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] UDrightimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];

        // boolean value determining if the system has been calibrated
        private bool _isStereoCalib = false;

        public StereoTracking()
        {
            calibRectSize.Width = (float)(142.0 / 1000.0);
            calibRectSize.Height = calibRectSize.Width;

            InitializeStereoTrackingImages();

            leftimagepoints.Initialize();
            rightimagepoints.Initialize();

            Initialise();
        }

        /// <summary>
        /// Constructor which takes the width of the side of the calibration 
        /// square as input. Length is in meters, if a real lengthis to be used.
        /// </summary>
        /// <param name="sideLen">The width of the calibration square sides.</param>
        public StereoTracking(float sideLen)
        {
            //sqSideLength = sWidth;
            calibRectSize.Width = sideLen;
            calibRectSize.Height = sideLen;

            Initialise();
        }

        /// <summary>
        /// Constructor which takes the length of the side of a calibration 
        /// square as input. Length is in meters, if a real length is to be used.
        /// </summary>
        /// <param name="sideLen">The width of the calibration square sides.</param>
        public StereoTracking(float Width, float Height)
        {
            //sqSideLength = sWidth;
            calibRectSize.Width = Width;
            calibRectSize.Height = Height;

            Initialise();

        }

        private void Initialise()
        {
            InitializeStereoTrackingImages();

            leftimagepoints.Initialize();
            rightimagepoints.Initialize();

            _logfilename = "data.log";

            datalogtimer = new System.Timers.Timer();
            datalogtimer.Elapsed += new ElapsedEventHandler(DataLogTimerEvent);
        }

        /// <summary>
        /// Clears the stored stereo calibration image arrays starting from a 
        /// given position by setting the X and Y values of all coordinates in 
        /// each image to zero. 
        /// </summary>
        /// <param name="startidx">Starting index from which to clear
        /// the arrays.</param>
        private void ClearStereoCalibrationImages(int startidx)
        {

            int i = startidx;

            while (i < MAX_NUM_OF_CAL_IMAGES)
            {
                for (int j = 0; j < MAX_NUM_OF_CAL_IMAGES; j++)
                {
                    wm1capturedImages[i][j].X = 0f;
                    wm1capturedImages[i][j].Y = 0f;
                    wm2capturedImages[i][j].X = 0f;
                    wm2capturedImages[i][j].Y = 0f;
                }

                i = i + 1;
            }

            if (startidx + 1 < stereoCapCount)
            {
                stereoCapCount = startidx + 1;
            }
        }

        /// <summary>
        /// Sets the height and width of a rectangular calibration object
        /// </summary>
        /// <param name="Width">Width of the object in m</param>
        /// <param name="Height">Height of the object in m</param>
        public void setCalibObjSize(float Width, float Height)
        {
            if (Width > 0 & Height > 0)
            {
                calibRectSize.Width = Width;
                calibRectSize.Height = Height;

                // Reinitialize the Image capture as the calibration 
                // object has changed
                InitializeStereoTrackingImages();

                ResetStereoCamCalibrateCapture();

            }
        }

        /// <summary>
        /// Returns the number of single camera calibration images that 
        /// have been captured.
        /// </summary>
        public int CalibrateImageCount
        {
            get { return capCount; }
        }

        /// <summary>
        /// Returns the number of stereo camera calibration image pairs 
        /// that have been captured.
        /// </summary>
        public int StereoCalibrateImageCount
        {
            get { return stereoCapCount; }
        }

        public int RemoveStereoCalibImage(int imgidx)
        {
            if (imgidx < stereoCapCount - 1)
            {

                Array.Copy(wm1capturedImages, imgidx + 1, wm1capturedImages, imgidx, wm1capturedImages.Length - imgidx - 1);

                Array.Copy(wm2capturedImages, imgidx + 1, wm2capturedImages, imgidx, wm2capturedImages.Length - imgidx - 1);

                stereoCapCount--;

                return 0;
            }
            else
            {
                return -1;
            }
        }

        /// <summary>
        /// Resets the camera calibration image capture process for both 
        /// single and stereo calibration. This is achieved by simply 
        /// setting the count of captured images to zero.
        /// </summary>
        public void Reset()
        {
            capCount = 0;
            stereoCapCount = 0;
        }

        /// <summary>
        /// Resets the single camera calibration image capture process.
        /// </summary>
        public void ResetSingleCamCalibrateCapture()
        {
            capCount = 0;
        }

        /// <summary>
        /// Resets the stereo calibration image capture process.
        /// </summary>
        public void ResetStereoCamCalibrateCapture()
        {
            stereoCapCount = 0;
        }

        /// <summary>
        /// Get a boolean informing whether a stereo calibration has 
        /// been performed.
        /// </summary>
        public bool IsStereoCalibrated
        {
            get { return _isStereoCalib; }
        }

        public void OutputCalibrationPoints()
        {

            int i = 0;

            if (capCount > 0)
            {
                Debug.WriteLine("Single camera calibration points:");

                for (i = 0; i < capCount; i++)
                {
                    Debug.WriteLine(singlewmCapturedImages[i][0].ToString() +
                        " " + singlewmCapturedImages[i][1].ToString() +
                        " " + singlewmCapturedImages[i][2].ToString() +
                        " " + singlewmCapturedImages[i][3].ToString());
                }
            }
            else
            {
                Debug.WriteLine("No single calibration images captured yet");
            }

            if (stereoCapCount > 0)
            {

                Debug.WriteLine("Stereo camera calibration points:");

                Debug.WriteLine("camera 1 calibration points:");

                Debug.WriteLine("new PointF[" + stereoCapCount.ToString() + "] {");

                for (i = 0; i < stereoCapCount; i++)
                {
                    Debug.WriteLine("new PointF[] {new PointF(" + wm1capturedImages[i][0].X.ToString() + "," + wm1capturedImages[i][0].Y.ToString() + "), new PointF(" +
                         wm1capturedImages[i][1].X.ToString() + "," + wm1capturedImages[i][1].Y.ToString() + "), new PointF(" +
                         wm1capturedImages[i][2].X.ToString() + "," + wm1capturedImages[i][2].Y.ToString() + "), new PointF(" +
                         wm1capturedImages[i][3].X.ToString() + "," + wm1capturedImages[i][3].Y.ToString() + ")" + "},");

                    //Debug.WriteLine(wm2capturedImages[i][0].ToString() +
                    //    " " + wm2capturedImages[i][1].ToString() +
                    //    " " + wm2capturedImages[i][2].ToString() +
                    //    " " + wm2capturedImages[i][3].ToString());
                }

                Debug.WriteLine("};");

                Debug.WriteLine("camera 2 calibration points:");

                Debug.WriteLine("new PointF[" + stereoCapCount.ToString() + "] {");

                for (i = 0; i < stereoCapCount; i++)
                {
                    Debug.WriteLine("new PointF[] {new PointF(" + wm2capturedImages[i][0].X.ToString() + "," + wm2capturedImages[i][0].Y.ToString() + "), new PointF(" +
                         wm2capturedImages[i][1].X.ToString() + "," + wm2capturedImages[i][1].Y.ToString() + "), new PointF(" +
                         wm2capturedImages[i][2].X.ToString() + "," + wm2capturedImages[i][2].Y.ToString() + "), new PointF(" +
                         wm2capturedImages[i][3].X.ToString() + "," + wm2capturedImages[i][3].Y.ToString() + ")" + "},");
                }

                Debug.WriteLine("};");
            }
            else
            {
                Debug.WriteLine("No stereo calibration images captured yet");
            }
        }

        /// <summary>
        /// Initializes the arrays of images and points used in the calibration.
        /// The maximum number of points that can be used is defined by the constant.
        /// MAX_NUM_OF_CAL_IMAGES.
        /// </summary>
        private void InitializeStereoTrackingImages()
        {
            // Initialize the array of calibration points (the location 
            // of the corners of the calibration rectangle in its coordinate 
            // reference frame). These points are all identical as the shape
            // of the square does not change and they are in the coordinate
            // frame of the rectangle
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
                //CalibObjectPoints[i][0].x = 0;
                //CalibObjectPoints[i][0].y = 0;
                //CalibObjectPoints[i][0].z = 0;

                //CalibObjectPoints[i][1].x = calibRectSize.Width;
                //CalibObjectPoints[i][1].y = 0;
                //CalibObjectPoints[i][1].z = 0;

                //CalibObjectPoints[i][2].x = calibRectSize.Width;
                //CalibObjectPoints[i][2].y = calibRectSize.Height;
                //CalibObjectPoints[i][2].z = 0;

                //CalibObjectPoints[i][3].x = 0;
                //CalibObjectPoints[i][3].y = calibRectSize.Height;
                //CalibObjectPoints[i][3].z = 0;

                CalibObjectPoints[i][0].x = 0;
                CalibObjectPoints[i][0].y = 0;
                CalibObjectPoints[i][0].z = 0;

                CalibObjectPoints[i][1].x = 0;
                CalibObjectPoints[i][1].y = calibRectSize.Height;
                CalibObjectPoints[i][1].z = 0;

                CalibObjectPoints[i][2].x = calibRectSize.Width;
                CalibObjectPoints[i][2].y = calibRectSize.Height;
                CalibObjectPoints[i][2].z = 0;

                CalibObjectPoints[i][3].x = calibRectSize.Width;
                CalibObjectPoints[i][3].y = 0;
                CalibObjectPoints[i][3].z = 0;
            }

            // Now initialize the array to hold the captured images 
            // from the camera
            singlewmCapturedImages.Initialize();
            wm1capturedImages.Initialize();
            wm2capturedImages.Initialize();

            ResetStereoCamCalibrateCapture();

            // Now initialize each of the sub arrays for holding the sets of 4 points
            for (int i = 0; i < wm1capturedImages.Length; i++)
            {
                singlewmCapturedImages[i] = new System.Drawing.PointF[4];
                singlewmCapturedImages[i].Initialize();
                wm1capturedImages[i] = new System.Drawing.PointF[4];
                wm1capturedImages[i].Initialize();
                wm2capturedImages[i] = new System.Drawing.PointF[4];
                wm2capturedImages[i].Initialize();
            }

        }

        /// <summary>
        /// Calibrate a single Wiimote camera using the images captured in the variable 
        /// singlewmCapturedImages, and the standard square object points.
        /// </summary>
        /// <param name="objectpoints">The locations of the object points on the calibration object. 
        /// These will always be in the reference frame of the object, in this case a 2D square so that 
        /// the 'Z' components will all be zero.</param>
        /// <param name="imagePoints">The 2D locations of the object points on the Wiimote camera image.</param>
        /// <param name="wm">A Wiimote object for the wiimote camera to be calibrated.</param>
        public void CalibrateCamera(WiimoteLib.Wiimote wm)
        {
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
            Array.Copy(singlewmCapturedImages, imagePoints, capCount);

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

        /// <summary>
        /// Calibrate a single Wiimote camera using the specified images and object points.
        /// </summary>
        /// <param name="wm">A Wiimote object for the wiimote camera to be calibrated.</param>
        /// <param name="objectpoints">The locations of the object points on the calibration object. 
        /// These will generally be in the reference frame of the object, e.g. a 2D square so that 
        /// the 'Z' components will all be zero.</param>
        /// <param name="imagePoints">The 2D locations of the object points on the Wiimote camera image.</param>
        public void CalibrateCamera(WiimoteLib.Wiimote wm, Emgu.CV.Structure.MCvPoint3D32f[][] objectPoints, System.Drawing.PointF[][] imagePoints)
        {

            Emgu.CV.CameraCalibration.CalibrateCamera(objectPoints,
                imagePoints,
                wiimoteCamSize,
                wm.WiimoteState.CameraCalibInfo.CamIntrinsic,
                Emgu.CV.CvEnum.CALIB_TYPE.DEFAULT,
                out wm.WiimoteState.CameraCalibInfo.SingleCamExtrinsic);

        }

        /// <summary>
        /// Calibrates a stereo rig comprising two wiimote cameras using a series 
        /// of images of a calibration object. In this case the images must have 
        /// been gathered previously by calling the function StereoCalibCapture
        /// repeatedly for several views of the calibration object in different  
        /// positions (ideally 15 or more). The calibration object must be an 
        /// (accurate) square of any size with the corners marked by being the 
        /// centres of four infra-red sources. To get the correct scaling of the 
        /// 3D locations, the length of a side of the square can be retrieved or set 
        /// through the SquareSideLength property, or in the appropriate 
        /// StereoTracking constructor overload. The default value is 0.142 m (142 
        /// mm) per side.
        /// </summary>
        /// <param name="wm1">Wiimotelib.Wiimote object corresponding to the left hand  
        /// wiimote camera. StereoCalibrate will fill out the Intrinsic ans stereo 
        /// extrinsic matrices in the WiimoteState.CameraCalibInfo field.</param>
        /// <param name="wm2">Wiimotelib.Wiimote object corresponding to the right hand  
        /// wiimote camera. StereoCalibrate will fill out the Intrinsic ans stereo 
        /// extrinsic matrices in the WiimoteState.CameraCalibInfo field.</param>
        public void StereoCalibrate(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {

            //Matrix<double> fundMat = new Matrix<double>(3, 3);
            //Matrix<double> essentialMat = new Matrix<double>(3, 3);

            int maxIters = 100;

            // set the stereo calibration iterative termination criteria
            // based on a maximum number of iterations and error
            Emgu.CV.Structure.MCvTermCriteria termCrit = new Emgu.CV.Structure.MCvTermCriteria(maxIters, 1e-5);

            int i = 0;

            // declare an array of 3D points to hold the locations of the points on the 
            // calibration square in its coordinate frame
            Emgu.CV.Structure.MCvPoint3D32f[][] objectPoints = new Emgu.CV.Structure.MCvPoint3D32f[stereoCapCount][];

            // declare arrays to hold the calibration image points from the wiimotes
            System.Drawing.PointF[][] wm1ImagePoints = new System.Drawing.PointF[stereoCapCount][];
            System.Drawing.PointF[][] wm2ImagePoints = new System.Drawing.PointF[stereoCapCount][];

            // Initialize the arrays of calibration and image points
            for (i = 0; i < objectPoints.Length; i++)
            {
                objectPoints[i] = new Emgu.CV.Structure.MCvPoint3D32f[4];
                wm1ImagePoints[i] = new System.Drawing.PointF[4];
                wm2ImagePoints[i] = new System.Drawing.PointF[4];

                for (int j = 0; j < 4; j++)
                {
                    objectPoints[i][j] = new Emgu.CV.Structure.MCvPoint3D32f();
                    wm1ImagePoints[i][j] = new System.Drawing.PointF();
                    wm2ImagePoints[i][j] = new System.Drawing.PointF();
                }
            }

            // copy in the correct number of images from the global
            // image storeage arrays
            Array.Copy(CalibObjectPoints, objectPoints, stereoCapCount);
            Array.Copy(wm1capturedImages, wm1ImagePoints, stereoCapCount);
            Array.Copy(wm2capturedImages, wm2ImagePoints, stereoCapCount);

            // calibrate the wiimote cameras individually
            CalibrateCamera(wm1, objectPoints, wm1ImagePoints);
            CalibrateCamera(wm2, objectPoints, wm2ImagePoints);

            // now perform a stereo calibration using the intrinsic
            // matrices generated in the previous step as the intial
            // guess for the stereo calibration
            Emgu.CV.CameraCalibration.StereoCalibrate(objectPoints,
                wm1ImagePoints,
                wm2ImagePoints,
                wm1.WiimoteState.CameraCalibInfo.CamIntrinsic,
                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic,
                wiimoteCamSize,
                Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_USE_INTRINSIC_GUESS,
                termCrit,
                out wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic,
                out fundMat,
                out essentialMat);

            // set both cameras to have the same stereo extrinsic parameters
            wm2.WiimoteState.CameraCalibInfo.StereoCamExtrinsic = wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic;
             
            Rectangle roi1 = new Rectangle();
            Rectangle roi2 = new Rectangle();

            // cvStereoRectify to get the R (rotation) and P (perspective) matrices 
            // for each camera and also the Q matrix for reprojecting the camera 
            // matrices to 3D 
            Emgu.CV.CvInvoke.cvStereoRectify(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
                wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
                wiimoteCamSize,
                wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.RotationVector.Ptr,
                wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.TranslationVector.Ptr,
                R1.Ptr,
                R2.Ptr,
                P1.Ptr,
                P2.Ptr,
                Q.Ptr,
                Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.DEFAULT,
                -1.0,
                Size.Empty,
                ref roi1,
                ref roi2);

            //Emgu.CV.CvInvoke.cvStereoRectify(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //        wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //        wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //        wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //        wiimoteCamSize,
            //        wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.RotationVector.Ptr,
            //        wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.TranslationVector.Ptr,
            //        R1.Ptr,
            //        R2.Ptr,
            //        P1.Ptr,
            //        P2.Ptr,
            //        Q.Ptr,
            //        Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.CALIB_ZERO_DISPARITY,
            //        -1.0,
            //        Size.Empty,
            //        ref roi1,
            //        ref roi2);

            // set the calibration flag to true, to indicate 
            // the rig is now calibrated
            _isStereoCalib = true;
                
        }

        public void StereoCalibrate(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2, System.Drawing.PointF[][] leftimagepoints, System.Drawing.PointF[][] rightimagepoints)
        {

            int maxIters = 100;

            Emgu.CV.Structure.MCvTermCriteria termCrit = new Emgu.CV.Structure.MCvTermCriteria(maxIters, 1e-5);

            int i = 0;

            // declare an array of 3D points to hold the locations of the points on the 
            // calibration square in its coordinate frame
            Emgu.CV.Structure.MCvPoint3D32f[][] objectPoints = new Emgu.CV.Structure.MCvPoint3D32f[leftimagepoints.Length][];

            // Initialize the arrays of calibration and image points
            for (i = 0; i < objectPoints.Length; i++)
            {
                objectPoints[i] = new Emgu.CV.Structure.MCvPoint3D32f[4];

                for (int j = 0; j < 4; j++)
                {
                    objectPoints[i][j] = new Emgu.CV.Structure.MCvPoint3D32f();
                }
            }

            Array.Copy(CalibObjectPoints, objectPoints, objectPoints.Length);

            // calibrate the wiimote cameras individually
            CalibrateCamera(wm1, objectPoints, leftimagepoints);
            CalibrateCamera(wm2, objectPoints, rightimagepoints);

            Emgu.CV.CameraCalibration.StereoCalibrate(objectPoints,
                leftimagepoints,
                rightimagepoints,
                wm1.WiimoteState.CameraCalibInfo.CamIntrinsic,
                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic,
                wiimoteCamSize,
                Emgu.CV.CvEnum.CALIB_TYPE.CV_CALIB_USE_INTRINSIC_GUESS,
                termCrit,
                out wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic,
                out fundMat,
                out essentialMat);

            wm2.WiimoteState.CameraCalibInfo.StereoCamExtrinsic = wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic;

            Rectangle roi1 = new Rectangle();
            Rectangle roi2 = new Rectangle();

            //Emgu.CV.CvInvoke.cvStereoRectify(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //                wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //                wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //                wiimoteCamSize,
            //                wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.RotationVector.Ptr,
            //                wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.TranslationVector.Ptr,
            //                R1.Ptr,
            //                R2.Ptr,
            //                P1.Ptr,
            //                P2.Ptr,
            //                Q.Ptr,
            //                Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.CALIB_ZERO_DISPARITY,
            //                -1.0,
            //                Size.Empty,
            //                ref roi1,
            //                ref roi2);

            Emgu.CV.CvInvoke.cvStereoRectify(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
                            wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
                            wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
                            wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
                            wiimoteCamSize,
                            wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.RotationVector.Ptr,
                            wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic.TranslationVector.Ptr,
                            R1.Ptr,
                            R2.Ptr,
                            P1.Ptr,
                            P2.Ptr,
                            Q.Ptr,
                            Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.DEFAULT,
                            -1.0,
                            Size.Empty,
                            ref roi1,
                            ref roi2);

            _isStereoCalib = true;

        }

        /// <summary>
        /// Get the 3D location of all points in the view of the stereo rig. This is achieved 
        /// by first undistorting the location of the points in the viewfinder using the camera
        /// intrinsic matrices, distortion coefficients, R matrices and P matrices of each camera  
        /// calculated by the StereoCalibrate() function.
        /// </summary>
        /// <param name="result3DPoints">An array of four matrices of dimensions [4, 1] to hold the
        /// X, Y and Z locations of up to four points in view and the scaling factor (although
        /// the scaling factor will already have been used to scale the points). If a 2D point is
        /// not found or the point data was invalid, all members of the respective 3D point matrix
        /// will be set to the values -9999.</param>
        /// <param name="wm1">Wiimotelib.Wiimote object corresponding to the left hand wiimote 
        /// camera. The intrinsic matrices, distortion coefficents, R and P matrices should
        /// all have been previous calculated by performing a stereo calibration.</param>
        /// <param name="wm2">Wiimotelib.Wiimote object corresponding to the right hand wiimote 
        /// camera. The intrinsic matrices, distortion coefficents, R and P matrices should
        /// all have been previous calculated by performing a stereo calibration.</param>
        public void Location3D(Matrix<double>[] result3DPoints, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {
            // capture an image of the IR points from both wiimote cameras
            StereoCapture(wm1, wm2, leftimagepoints, rightimagepoints);

            int i = 0;

            // delare a temporary variable to hold the x and y position
            // in the left imager and the disparity between the x-coordinates
            // this will be multiplied by the Q matrix to get the 3D location
            Matrix<double> XYDpoint = new Matrix<double>(4, 1);

            // undistort the points in the left had camera
            UDleftimagepoints = wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(leftimagepoints, R1, P1);

            // undistort the points in the right hand camera
            UDrightimagepoints = wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(rightimagepoints, R2, P2);

            // Get the adjustment necessary if the principle points of each camera 
            // are not equal, for instance if we have not set the STEREO_RECTIFY_TYPE
            // flag to CALIB_ZERO_DISPARITY as the cameras are pointing slightly toward 
            // each other
            Double cadjust = wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Data[0, 2] - wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Data[0, 2];

            // check for up to four points, the max number of sources currently
            for (i = 0; i < MAX_NUM_IR_SRCS; i++)
            {
                // if a point was not found, or was invalid, it will be set to have
                // coordinates outside the view or the imager. We look for this
                // in the original undistorted points and if so set the 3D point 
                // location to a predetermined value
                if (leftimagepoints[i].X <= wiimoteCamSize.Width &&
                    leftimagepoints[i].Y < wiimoteCamSize.Height &&
                    rightimagepoints[i].X <= wiimoteCamSize.Width &&
                    rightimagepoints[i].Y <= wiimoteCamSize.Height)
                {

                    // get the undistorted x-coordinate in the left camera
                    XYDpoint.Data[0, 0] = (double)(UDleftimagepoints[i].X);

                    //Debug.WriteLine("XYDpoint.Data[0, 0] = " + XYDpoint.Data[0, 0].ToString());

                    // get the undistorted y-coordinate in the left camera
                    XYDpoint.Data[1, 0] = (double)(UDleftimagepoints[i].Y);

                    // the disparity between the points is the right camera x-coordinate
                    // subtracted from the left x-coordinate
                    XYDpoint.Data[2, 0] = (double)(UDleftimagepoints[i].X - UDrightimagepoints[i].X);

                    // We adjust the disparity if required
                    //XYDpoint.Data[2, 0] = XYDpoint.Data[2, 0] + cadjust;

                    // add a one to the end of the array
                    XYDpoint.Data[3, 0] = (double)(1);

                    // Call cvPerspectiveTransform to get the 3D location
                    // CvInvoke.cvPerspectiveTransform(XYDpoint.Ptr, result3DPoints[i].Ptr, Q.Ptr);
                    MatrixMult(Q, XYDpoint, result3DPoints[i]);

                    double yOffset = yWiiIROffset / result3DPoints[i].Data[3, 0];

                    // Divide the X, Y and Z factors by the scaling factor
                    result3DPoints[i].Data[0, 0] = result3DPoints[i].Data[0, 0] / result3DPoints[i].Data[3, 0];
                    result3DPoints[i].Data[1, 0] = (result3DPoints[i].Data[1, 0] / result3DPoints[i].Data[3, 0]) - yOffset;
                    result3DPoints[i].Data[2, 0] = result3DPoints[i].Data[2, 0] / result3DPoints[i].Data[3, 0];

                    

                }
                else
                {
                    // if the 2D point is not found or contained invalid values, set 
                    // all the values in the 3D location to the value -9999
                    result3DPoints[i].Data[0, 0] = -9999;
                    result3DPoints[i].Data[1, 0] = -9999;
                    result3DPoints[i].Data[2, 0] = -9999;
                    result3DPoints[i].Data[3, 0] = -9999;
                }
                
            }

        }


        /// <summary>
        /// Multiples a [4, 1] matrix by a [4, 4] matrix (created to multiple view matrix by Q matrix
        /// as I couldn't get cvPerspectiveTransform to work, and this is what it does).
        /// </summary>
        /// <param name="mat1">A 4 x 4 matrix of doubles</param>
        /// <param name="mat2">A 4 x 1 matrix of doubles</param>
        /// <param name="outMat">A 4 x 1 matrix containing the result of the multiplication.</param>
        private void MatrixMult(Emgu.CV.Matrix<double> mat1, Emgu.CV.Matrix<double> mat2, Emgu.CV.Matrix<double> outMat)
        {
            for (int i = 0; i < 4; i++)
            {
                outMat.Data[i, 0] = 0;

                for (int j = 0; j < 4; j++)
                {
                    outMat.Data[i, 0] = outMat.Data[i, 0] + (mat1.Data[i, j] * mat2.Data[j, 0]);
                }
            }
        }

        public void Location3D_2(Matrix<double>[] result3DPoints, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {
            // capture an image of the IR points from both wiimote cameras
            StereoCapture(wm1, wm2, leftimagepoints, rightimagepoints);

            int i = 0;

            // undistort the points in the left had camera
            UDleftimagepoints = wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(leftimagepoints, R1, P1);

            // undistort the points in the right hand camera
            UDrightimagepoints = wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(rightimagepoints, R2, P2);

            System.Drawing.PointF[] tmpLeftPnt = new System.Drawing.PointF[1];
            tmpLeftPnt.Initialize();
            System.Drawing.PointF[] tmpRightPnt = new System.Drawing.PointF[1];
            tmpRightPnt.Initialize();
            Matrix<double>[] XYDpoint = new Matrix<double>[1];
            XYDpoint[0] = new Matrix<double>(4, 1);

            // check for up to four points, the max number of sources currently
            for (i = 0; i < MAX_NUM_IR_SRCS; i++)
            {
                // if a point was not found, or was invalid, it will be set to have
                // coordinates outside the view or the imager. We look for this
                // in the original undistorted points and if so set the 3D point 
                // location to a predetermined value
                if (leftimagepoints[i].X <= wiimoteCamSize.Width &&
                    leftimagepoints[i].Y < wiimoteCamSize.Height &&
                    rightimagepoints[i].X <= wiimoteCamSize.Width &&
                    rightimagepoints[i].Y <= wiimoteCamSize.Height)
                {
                    tmpLeftPnt[0] = UDleftimagepoints[i];
                    tmpRightPnt[0] = UDrightimagepoints[i]; //MatchPoint(UDleftimagepoints[i], UDrightimagepoints);

                    //cvCorrectMatches(tmpLeftPnt, tmpRightPnt);

                    triangulate(P1, P2, tmpLeftPnt, tmpRightPnt, XYDpoint);

                    XYDpoint[0].CopyTo(result3DPoints[i]);

                    //double yOffset = yWiiIROffset / result3DPoints[i].Data[3, 0];
                }
                else
                {
                    // if the 2D point is not found or contained invalid values, set 
                    // all the values in the 3D location to the value -9999
                    result3DPoints[i].Data[0, 0] = -9999;
                    result3DPoints[i].Data[1, 0] = -9999;
                    result3DPoints[i].Data[2, 0] = -9999;
                    result3DPoints[i].Data[3, 0] = -9999;
                }

            }

        }

        private System.Drawing.PointF MatchPoint(System.Drawing.PointF leftpoint, System.Drawing.PointF[] candidateMatches)
        {
            System.Drawing.PointF matchPoint = new System.Drawing.PointF();

            double distance = Math.Sqrt(Math.Pow(((double)(candidateMatches[0].X) - (double)(leftpoint.X)), 2) +
                                        Math.Pow((double)(candidateMatches[0].Y) - (double)(leftpoint.Y), 2));

            matchPoint = candidateMatches[0];

            for (int i = 1; i < candidateMatches.Length; i++)
            {
                double nextdistance = Math.Sqrt(Math.Pow((double)(candidateMatches[i].X) - (double)(leftpoint.X), 2) +
                                                Math.Pow((double)(candidateMatches[i].Y) - (double)(leftpoint.Y), 2));

                if (nextdistance < distance)
                    matchPoint = candidateMatches[i];

                distance = nextdistance;
            }

            return matchPoint;
        }

        private void cvCorrectMatches(System.Drawing.PointF[] points1, System.Drawing.PointF[] points2)
        {

            //// Make sure F uses double precision
            Matrix<double> F = fundMat;
            //cvConvert(F_, F);

            //// Make sure points1 uses double precision
            //points1 = cvCreateMat(points1_->rows,points1_->cols,CV_64FC2);
            //cvConvert(points1_, points1);

            //// Make sure points2 uses double precision
            //points2 = cvCreateMat(points2_->rows,points2_->cols,CV_64FC2);
            //cvConvert(points2_, points2);

            Matrix<double> tmp33 = new Matrix<double>(3, 3);
            Matrix<double> tmp31 = new Matrix<double>(3, 1);
            Matrix<double> tmp31_2 = new Matrix<double>(3, 1);
            Matrix<double> T1i = new Matrix<double>(3, 3);
            Matrix<double> T2i = new Matrix<double>(3, 3);
            Matrix<double> R1 = new Matrix<double>(3, 3);
            Matrix<double> R2 = new Matrix<double>(3, 3);
            Matrix<double> TFT = new Matrix<double>(3, 3);
            Matrix<double> TFTt = new Matrix<double>(3, 3);
            Matrix<double> RTFTR = new Matrix<double>(3, 3);

            Matrix<double> U = new Matrix<double>(3, 3);
            Matrix<double> S = new Matrix<double>(3, 3);
            Matrix<double> V = new Matrix<double>(3, 3);
            Matrix<double> e1 = new Matrix<double>(3, 1);
            Matrix<double> e2 = new Matrix<double>(3, 1);

            double x1, y1, x2, y2;
            double scale;
            double f1, f2, a, b, c, d;
            double t_min, s_val, t, s;

            Matrix<double> polynomial = new Matrix<double>(1, 7);

            Matrix<double> result = new Matrix<double>(1, 6);

            for (int p = 0; p < points1.Length; ++p)
            {
                // Replace F by T2-t * F * T1-t
                x1 = points1[p].X;
                y1 = points1[p].Y;
                x2 = points2[p].X;
                y2 = points2[p].Y;

                T1i.SetZero();
                T1i.Data[0, 1] = 1;
                T1i.Data[1, 1] = 1;
                T1i.Data[2, 2] = 1;
                T1i.Data[0, 2] = x1;
                T1i.Data[1, 2] = y1;

                T2i.SetZero();
                T2i.Data[0, 0] = 1;
                T2i.Data[1, 1] = 1;
                T2i.Data[2, 2] = 1;
                T2i.Data[0, 2] = x2;
                T2i.Data[1, 2] = y2;

                //cvGEMM(T2i,F,1,0,0,tmp33,CV_GEMM_A_T)
                cv.cvGEMM(T2i.Ptr, F.Ptr, 1, (IntPtr)null, 0, tmp33.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_A_T);

                TFT.SetZero();
                //cvGEMM(tmp33,T1i,1,0,0,TFT);
                cv.cvGEMM(tmp33.Ptr, T1i.Ptr, 1, (IntPtr)null, 0, TFT.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

                // Compute the right epipole e1 from F * e1 = 0
                U.SetZero();
                S.SetZero();
                V.SetZero();

                cv.cvSVD(TFT.Ptr, S.Ptr, U.Ptr, V.Ptr, Emgu.CV.CvEnum.SVD_TYPE.CV_SVD_DEFAULT);

                scale = Math.Sqrt(V.Data[0, 2] * V.Data[0, 2] + V.Data[1, 2] * V.Data[1, 2]);

                e1.Data[0, 0] = V.Data[0, 2] / scale;
                e1.Data[1, 0] = V.Data[1, 2] / scale;
                e1.Data[2, 0] = V.Data[2, 2] / scale;

                if (e1.Data[2, 0] < 0)
                {
                    e1.Data[0, 0] = -e1.Data[0, 0];
                    e1.Data[1, 0] = -e1.Data[1, 0];
                    e1.Data[2, 0] = -e1.Data[2, 0];
                }

                // Compute the left epipole e2 from e2' * F = 0  =>  F' * e2 = 0
                TFTt.SetZero();
                cv.cvTranspose(TFT.Ptr, TFTt.Ptr);

                U.SetZero();
                S.SetZero();
                V.SetZero();

                cv.cvSVD(TFTt.Ptr, S.Ptr, U.Ptr, V.Ptr, Emgu.CV.CvEnum.SVD_TYPE.CV_SVD_DEFAULT);

                e2.SetZero();

                scale = Math.Sqrt(V.Data[0, 2] * V.Data[0, 2] + V.Data[1, 2] * V.Data[1, 2]);

                e2.Data[0, 0] = V.Data[0, 2] / scale;
                e2.Data[1, 0] = V.Data[1, 2] / scale;
                e2.Data[2, 0] = V.Data[2, 2] / scale;

                if (e2.Data[2, 0] < 0)
                {
                    e2.Data[0, 0] = -e1.Data[0, 0];
                    e2.Data[1, 0] = -e1.Data[1, 0];
                    e2.Data[2, 0] = -e1.Data[2, 0];
                }

                // Replace F by R2 * F * R1'
                R1.SetZero();
                R1.Data[0, 0] = e1.Data[0, 0];
                R1.Data[0, 1] = e1.Data[1, 0];
                R1.Data[1, 0] = -e1.Data[1, 0];
                R1.Data[1, 1] = e1.Data[0, 0];
                R1.Data[2, 2] = 1;

                R2.SetZero();
                R2.Data[0, 0] = e2.Data[0, 0];
                R2.Data[0, 1] = e2.Data[1, 0];
                R2.Data[1, 0] = -e2.Data[1, 0];
                R2.Data[1, 1] = e2.Data[0, 0];
                R2.Data[2, 2] = 1;

                cv.cvGEMM(R2.Ptr, TFT.Ptr, 1, (IntPtr)null, 0, tmp33.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

                cv.cvGEMM(tmp33.Ptr, R1.Ptr, 1, (IntPtr)null, 0, RTFTR.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

                // Set f1 = e1(3), f2 = e2(3), a = F22, b = F23, c = F32, d = F33
                f1 = e1.Data[2, 0];
                f2 = e2.Data[2, 0];
                a = RTFTR.Data[1, 1];
                b = RTFTR.Data[1, 2];
                c = RTFTR.Data[2, 1];
                d = RTFTR.Data[2, 2];

                // Form the polynomial g(t) = k6*tâ¶ + k5*tâµ + k4*tâ´ + k3*tÂ³ + k2*tÂ² + k1*t + k0
                // from f1, f2, a, b, c and d
                polynomial.Data[0, 6] = +b * c * c * f1 * f1 * f1 * f1 * a - a * a * d * f1 * f1 * f1 * f1 * c;
                polynomial.Data[0, 5] = +f2 * f2 * f2 * f2 * c * c * c * c + 2 * a * a * f2 * f2 * c * c - a * a * d * d * f1 * f1 * f1 * f1 + b * b * c * c * f1 * f1 * f1 * f1 + a * a * a * a;
                polynomial.Data[0, 4] = +4 * a * a * a * b + 2 * b * c * c * f1 * f1 * a + 4 * f2 * f2 * f2 * f2 * c * c * c * d + 4 * a * b * f2 * f2 * c * c + 4 * a * a * f2 * f2 * c * d - 2 * a * a * d * f1 * f1 * c - a * d * d * f1 * f1 * f1 * f1 * b + b * b * c * f1 * f1 * f1 * f1 * d;
                polynomial.Data[0, 3] = +6 * a * a * b * b + 6 * f2 * f2 * f2 * f2 * c * c * d * d + 2 * b * b * f2 * f2 * c * c + 2 * a * a * f2 * f2 * d * d - 2 * a * a * d * d * f1 * f1 + 2 * b * b * c * c * f1 * f1 + 8 * a * b * f2 * f2 * c * d;
                polynomial.Data[0, 2] = +4 * a * b * b * b + 4 * b * b * f2 * f2 * c * d + 4 * f2 * f2 * f2 * f2 * c * d * d * d - a * a * d * c + b * c * c * a + 4 * a * b * f2 * f2 * d * d - 2 * a * d * d * f1 * f1 * b + 2 * b * b * c * f1 * f1 * d;
                polynomial.Data[0, 1] = +f2 * f2 * f2 * f2 * d * d * d * d + b * b * b * b + 2 * b * b * f2 * f2 * d * d - a * a * d * d + b * b * c * c;
                polynomial.Data[0, 0] = -a * d * d * b + b * b * c * d;

                // Solve g(t) for t to get 6 roots
                result.SetZero();
                cv.cvSolvePoly(polynomial.Ptr, result.Ptr, 100, 20);

                // Evaluate the cost function s(t) at the real part of the 6 roots
                t_min = System.Double.MaxValue;

                s_val = (1 / (f1 * f1)) + ((c * c) / (a * a + f2 * f2 * c * c));

                for (int ti = 0; ti < 6; ++ti)
                {

                    //t = result->data.db[2*ti];
                    t = result.Data[0, ti];

                    s = (t * t) / (1 + f1 * f1 * t * t) + ((c * t + d) * (c * t + d)) / ((a * t + b) * (a * t + b) + f2 * f2 * (c * t + d) * (c * t + d));

                    if (s < s_val)
                    {
                        s_val = s;
                        t_min = t;
                    }

                }

                // find the optimal x1 and y1 as the points on l1 and l2 closest to the origin
                tmp31.Data[0, 0] = t_min * t_min * f1;
                tmp31.Data[0, 1] = t_min;
                tmp31.Data[0, 2] = t_min * t_min * f1 * f1 + 1;
                tmp31.Data[0, 0] /= tmp31.Data[0, 2];
                tmp31.Data[0, 1] /= tmp31.Data[0, 2];
                tmp31.Data[0, 2] /= tmp31.Data[0, 2];

                cv.cvGEMM(T1i.Ptr, R1.Ptr, 1, (IntPtr)null, 0, tmp33.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

                cv.cvGEMM(tmp33.Ptr, tmp31.Ptr, 1, (IntPtr)null, 0, tmp31_2.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

                x1 = tmp31_2.Data[0, 0];
                y1 = tmp31_2.Data[0, 1];

                tmp31.Data[0, 0] = f2 * Math.Pow(c * t_min + d, 2);
                tmp31.Data[0, 1] = -(a * t_min + b) * (c * t_min + d);
                tmp31.Data[0, 2] = f2 * f2 * Math.Pow(c * t_min + d, 2) + Math.Pow(a * t_min + b, 2);
                tmp31.Data[0, 0] /= tmp31.Data[0, 2];
                tmp31.Data[0, 1] /= tmp31.Data[0, 2];
                tmp31.Data[0, 2] /= tmp31.Data[0, 2];

                cv.cvGEMM(T2i.Ptr, R2.Ptr, 1, (IntPtr)null, 0, tmp33.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_B_T);

                cv.cvGEMM(tmp33.Ptr, tmp31.Ptr, 1, (IntPtr)null, 0, tmp31_2.Ptr, Emgu.CV.CvEnum.GEMM_TYPE.CV_GEMM_DEFAULT);

                x2 = tmp31.Data[0, 0];
                y2 = tmp31.Data[0, 1];

                // Return the points in the matrix format that the user wants
                points1[p].X = (float)x1;
                points1[p].Y = (float)y1;
                points2[p].X = (float)x2;
                points2[p].Y = (float)y2;
            }

            //if( new_points1 )
            //    cvConvert( points1, new_points1 );
            //if( new_points2 )
            //    cvConvert( points2, new_points2 );
        }

        private void triangulate(Matrix<double> projMatr1, Matrix<double> projMatr2, System.Drawing.PointF[] projPoints1, System.Drawing.PointF[] projPoints2, Matrix<double>[] points4D)
        {

            Matrix<double> matrA = new Matrix<double>(6, 4);
            //double[] matrA_dat = new double[24];
            //matrA = cvMat(6,4,CV_64F,matrA_dat);

            //CvMat matrU;
            Matrix<double> matrW = new Matrix<double>(6, 4);
            Matrix<double> matrV = new Matrix<double>(4, 4);
            //double matrU_dat[9*9];
            //double matrW_dat[6*4];
            //double matrV_dat[4*4];

            //matrU = cvMat(6,6,CV_64F,matrU_dat);
            //matrW = cvMat(6,4,CV_64F,matrW_dat);
            //matrV = cvMat(4,4,CV_64F,matrV_dat);

            System.Drawing.PointF[][] projPoints = new PointF[2][];
            Matrix<double>[] projMatrs = new Matrix<double>[2];

            projPoints[0] = projPoints1;
            projPoints[1] = projPoints2;

            projMatrs[0] = projMatr1;
            projMatrs[1] = projMatr2;

            int i, j;

            for (i = 0; i < projPoints1.Length; i++)/* For each point */
            {
                /* Fill matrix for current point */
                for (j = 0; j < 2; j++)/* For each view */
                {
                    double x, y;

                    x = projPoints[j][i].X;
                    y = projPoints[j][i].Y;

                    for (int k = 0; k < 4; k++)
                    {
                        matrA.Data[j * 3 + 0, k] = x * projMatrs[j].Data[2, k] - projMatrs[j].Data[0, k];
                        matrA.Data[j * 3 + 1, k] = y * projMatrs[j].Data[2, k] - projMatrs[j].Data[1, k];
                        matrA.Data[j * 3 + 2, k] = x * projMatrs[j].Data[1, k] - y * projMatrs[j].Data[0, k];
                    }
                }
                /* Solve system for current point */
                {

                    //cvSVD(&matrA, &matrW, 0, &matrV, CV_SVD_V_T);
                    Emgu.CV.CvInvoke.cvSVD(matrA.Ptr, matrW.Ptr, (IntPtr)null, matrV.Ptr, Emgu.CV.CvEnum.SVD_TYPE.CV_SVD_V_T);

                    /* Copy computed point */
                    points4D[i].Data[0, 0] = matrV.Data[3, 0];/* X */
                    points4D[i].Data[1, 0] = matrV.Data[3, 1];/* Y */
                    points4D[i].Data[2, 0] = matrV.Data[3, 2];/* Z */
                    points4D[i].Data[3, 0] = matrV.Data[3, 3];/* W */
                }
            }

            /* Points was reconstructed. Try to reproject points */
            /* We can compute reprojection error if need */
            //{
            //    int i;
            
            Matrix<double>[] point3D = new Matrix<double>[points4D.Length];
            //double[] point3D_dat = new double[4];
            //Matrix<double> point3D = new Matrix<double>(4,1);

            Matrix<double> point2D = new Matrix<double>(3, 1);
            //double point2D_dat[3];
            //point2D = cvMat(3,1,CV_64F,point2D_dat);

            for (i = 0; i < projPoints1.Length; i++)
            {

                double W = points4D[i].Data[3, 0];

                point3D[i] = new Matrix<double>(4, 1);
                points4D[i].Data[0, 0] = points4D[i].Data[0, 0] / W;
                points4D[i].Data[1, 0] = points4D[i].Data[1, 0] / W;
                points4D[i].Data[2, 0] = points4D[i].Data[2, 0] / W;
                points4D[i].Data[3, 0] = 1;

                /* !!! Project this point for each camera */
                for (int currCamera = 0; currCamera < 2; currCamera++)
                {
                    point2D = projMatrs[currCamera] * points4D[i];

                    //cvMatMul(projMatrs[currCamera], &point3D, &point2D);

                    float x, y;
                    float xr, yr, wr;
                    x = projPoints[currCamera][i].X;
                    y = projPoints[currCamera][i].Y;

                    wr = (float)point2D.Data[2, 0];
                    xr = (float)(point2D.Data[0, 0] / wr);
                    yr = (float)(point2D.Data[1, 0] / wr);
                    
                    float deltaX, deltaY;
                    deltaX = (float)Math.Abs(x - xr);
                    deltaY = (float)Math.Abs(y - yr);
                }
                //
            }

            //return point3D;
        }

        /// <summary>
        /// Captures an image of the calibration square for use in calibration 
        /// of a single wiimote camera.
        /// </summary>
        /// <param name="wm">Wiimote to be calibrated</param>
        /// <returns>0 if successful, -1 if the maximum number of calibration
        /// images have already been captured (in which case the image will not 
        /// be captured, or -2 if four coordinates could not be captured for any 
        /// reason</returns>
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

            if (!(CalibCaptureOneWM(wm, coords) == 0))
            {
                return -2;
            }

            if (capCount < MAX_NUM_OF_CAL_IMAGES)
            {
                for (i = 0; i < coords.Length; i++)
                {
                    spointf[i] = coords[i].ToPointf();
                }

                this.singlewmCapturedImages[capCount] = spointf;

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

        /// <summary>
        /// Captures and stores images from two wiimotes for the purposes of stereo
        /// calibration.
        /// </summary>
        /// <param name="wm1">Wiimote 1, the left camera</param>
        /// <param name="wm2">Wiimote 2, the right camera</param>
        /// <returns>integer, -1 if the smae number of points cannot be seen in both wiimotes,
        /// -2 if image capture from Wiimote 1 was unsuccessful, -3 if image capture from
        /// Wiimote 2 was unsuccessful, -4 if the maximum allowed number of calibration
        /// images has been exceeded, or 0 if an image was successfully captured from
        /// both Wiimotes.</returns>
        public int StereoCalibCapture(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {
            int i = 0;

            int irpoints1 = wm1.WiimoteState.IRPoints();
            int irpoints2 = wm2.WiimoteState.IRPoints();

            if (irpoints1 != irpoints2)
            {
                return -1;
            }

            Wiimote3DTrackingLib.coord[] coords1 = new Wiimote3DTrackingLib.coord[irpoints1];
            Wiimote3DTrackingLib.coord[] coords2 = new Wiimote3DTrackingLib.coord[irpoints2];

            PointF[] spointf1 = new PointF[irpoints1];
            PointF[] spointf2 = new PointF[irpoints2];

            for (i = 0; i < coords1.Length; i++)
            {
                coords1[i] = new coord();
                spointf1[i] = new PointF();
                coords2[i] = new coord();
                spointf2[i] = new PointF();
            }

            if (!(CalibCaptureOneWM(wm1, coords1) == 0))
            {
                return -2;
            }

            if (!(CalibCaptureOneWM(wm2, coords2) == 0))
            {
                return -3;
            }

            // Wii puts origin in bottom left, Y up; toolbox in top left, Y down.  Convert by subtracting y from 768.		
            for (i = 0; i < NUM_IR_SRCS; i++)
            {
                coords1[i].y = wiimoteCamSize.Height - coords1[i].y;
                coords2[i].y = wiimoteCamSize.Height - coords2[i].y;
                //Debug.WriteLine("coords1[" + i.ToString() + "] = " + coords1[i].ToString() + "  coords2[" + i.ToString() + "] = " + coords2[i].ToString());
            }

            if (stereoCapCount < MAX_NUM_OF_CAL_IMAGES)
            {
                for (i = 0; i < coords1.Length; i++)
                {
                    spointf1[i] = coords1[i].ToPointf();
                    spointf2[i] = coords2[i].ToPointf();
                }

                this.wm1capturedImages[stereoCapCount] = spointf1;
                this.wm2capturedImages[stereoCapCount] = spointf2;

                // Increment the count of the number of 
                // calibration images captured
                stereoCapCount++;
            }
            else if (stereoCapCount >= MAX_NUM_OF_CAL_IMAGES)
            {
                return -4;
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
                    CalibCaptureOneWM(wmarray[i], coords[i]);
                }
            }

            //return coords;

        }

        public int CalibCaptureOneWM(WiimoteLib.Wiimote wm, coord[] coords)
        {
            // Captures the IR sensor output from the wiimotes and prints 
            // the output in a form readable by MATLAB, i.e. creates MATLAB
            // commands of the type X = [1, 2, 3]; that can make up a m-file 
            // to be run later. This is either written to a file, or to  
            // stdout depending on the contents of the global file variable

            //coord[] coords = new coord[NUM_IR_SRCS];

            if (!(GetCalibCoords(wm, coords) == 0))
            {
                return -1;
            }

            // Wii puts origin in bottom left, Y up; toolbox in top left, Y down.  Convert by subtracting y from 768.		
            for (int i = 0; i < NUM_IR_SRCS; i++)
            {
                coords[i].y = wiimoteCamSize.Height - coords[i].y;
                //Debug.WriteLine("coords[" + i.ToString() + "] = " + coords[i].ToString());
            }

            Array.Sort(coords, 0, 4, coord.compare_x());
            Array.Sort(coords, 0, 2, coord.compare_y());
            Array.Sort(coords, 2, 2, coord.compare_y_inv());

            

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
        int GetCalibCoords(WiimoteLib.Wiimote wm, coord[] coords)
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

                    if (c.x > wiimoteCamSize.Width || c.x < 0 || c.y > wiimoteCamSize.Height || c.y < 0)
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

        /// <summary>
        /// Captures the images from both wiimotes
        /// </summary>
        /// <param name="wm1">Wiimote object corresponding to left hand wiimote</param>
        /// <param name="wm2">Wiimote object corresponding to right hand wiimote</param>
        /// <param name="points1">Array of PointF objects to hold the image points from
        /// Wiimote 1.</param>
        /// <param name="points2">Array of PointF objects to hold the image points from
        /// Wiimote 2.</param>
        /// <returns>0 on success, or -1 otherwise</returns>
        public int StereoCapture(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2, PointF[] points1, PointF[] points2)
        {

            int i = 0;

            coord[] coords1 = new coord[MAX_NUM_IR_SRCS];
            coord[] coords2 = new coord[MAX_NUM_IR_SRCS];

            // initialise the coordinates
            for (i = 0; i < MAX_NUM_IR_SRCS; i++)
            {
                coords1[i] = new coord();
                coords2[i] = new coord();
            }

            if (CaptureOneWM(wm1, coords1) == 0 && CaptureOneWM(wm2, coords2)==0)
            {
                for (i = 0; i < MAX_NUM_IR_SRCS; i++)
                {
                    points1[i].X = (float)(coords1[i].x);
                    points1[i].Y = (float)(coords1[i].y);

                    points2[i].X = (float)(coords2[i].x);
                    points2[i].Y = (float)(coords2[i].y);
                }

                return 0;
            }

            return -1;

        }


        public int CaptureOneWM(WiimoteLib.Wiimote wm, coord[] coords)
        {
            // Captures the IR sensor output from the wiimotes

            //coord[] coords = new coord[NUM_IR_SRCS];
            if (coords.Length == MAX_NUM_IR_SRCS)
            {

                if (!(GetCoords(wm, coords) == 0))
                {
                    return -1;
                }

                // Wii puts origin in bottom left, Y up; toolbox in top left, Y down.  
                // Convert by subtracting y from 768.		
                //for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                //{
                //    coords[i].y = wiimoteCamSize.Height - coords[i].y;
                //    //Debug.WriteLine("coords[" + i.ToString() + "] = " + coords[i].ToString());
                //}

                // sort the coordinates in x and y 
                Array.Sort(coords, 0, MAX_NUM_IR_SRCS, coord.compare_xy());

                return 0;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Retrieves the last set of IR camera image coordinates reported by 
        /// a wiimote.
        /// </summary>
        /// <param name="wm">Wimmote, the object corresponding to the wiimote
        /// we wish to extract the coordinates from</param>
        /// <param name="coords">An array of coord objects in which to store the 
        /// coordinates reported by each IR sensor</param>
        /// <returns>integer, 0 if coordinates were successfully retreived, or
        /// -2 if the IR sensor data could not be extracted. </returns>
        int GetCoords(WiimoteLib.Wiimote wm, coord[] coords)
        {

            int i = 0;
            coord c = new coord();

            // Check if the wiimote is connected and 4 IR sources can be found
            if (wm.WiimoteState.ConnectionState == WiimoteLib.ConnectionState.Connected)
            {
                // get the coordinates of each IR source
                for (i = 0; i < MAX_NUM_IR_SRCS; i++)
                {
                    if (wm.WiimoteState.IRState.IRSensors[i].Found)
                    {
                        c.x = wm.WiimoteState.IRState.IRSensors[i].RawPosition.X;

                        c.y = wm.WiimoteState.IRState.IRSensors[i].RawPosition.Y;

                        c.pt = i + 1;

                        if (c.x > wiimoteCamSize.Width | c.x < 0 | c.y > wiimoteCamSize.Height | c.y < 0)
                        {
                            // the IR data is invalid for this source, put the 
                            // coordinate outside the view
                            coords[i].x = 4*wiimoteCamSize.Width + 1 + i;
                            coords[i].y = 4*wiimoteCamSize.Height + 1 + i;
                            coords[i].pt = c.pt;
                        }
                        else
                        {
                            // store the captured coordinate in the array
                            coords[i].x = c.x;
                            coords[i].y = c.y;
                            coords[i].pt = c.pt;
                        }
                    }
                    else
                    {
                        coords[i].x = 4 * wiimoteCamSize.Width + 1 + i;
                        coords[i].y = 4 * wiimoteCamSize.Height + 1 + i;
                        coords[i].pt = i+1;
                    }
                }
            }
            else
            {
                // return -2 as we could not get info from controller, it 
                // may be disconnected
                return -2;
            }

            return 0;

        }

        /// <summary>
        /// Retrieve the set of stereo calibration images at a given index.
        /// </summary>
        /// <param name="imageidx">The zero-based index of the desired pair 
        /// of images</param>
        /// <returns>An array of two PointF arrays of size 4, each of which
        /// contains the corresponding points from the calibration image.</returns>
        public System.Drawing.PointF[][] GetStereoCalibImage(int imageidx)
        {

            System.Drawing.PointF[][] imagepair = new System.Drawing.PointF[2][];

            if (imageidx < 0 | imageidx > stereoCapCount)
            {
                return imagepair;
            }
            else
            {

                imagepair[0] = wm1capturedImages[imageidx];

                imagepair[1] = wm2capturedImages[imageidx];

                return imagepair;

            }

        }

        /// <summary>
        /// Saves the captured stereo calibration images to disk if any have 
        /// been stored.
        /// </summary>
        /// <param name="filename">string containing the file path in which 
        /// to store the saved images. These are saved as text with the 
        /// format XA1,YA1,XA2,YA2,XA3,YA3,XA4,YA4,XB1,YB1,XB2,YB2,XB3,YB3,XB4,YB4
        /// where A refers to Wiimote 1 (the left hand camera) and B refers to
        /// Wiimote 2 (the right hand camera) and the numbers refer to the coordinate
        /// number in the image in each case.</param>
        /// <returns>integer, 0 if successful, or otherwise -1.</returns>
        public int SaveStereoCalibrationImages(string filename)
        {
            if (stereoCapCount > 0)
            {
                int i, j;

                StreamWriter calibStreamWriter = new StreamWriter(filename);

                for (i = 0; i < stereoCapCount; i++)
                {
                    for (j = 0; j < MAX_NUM_IR_SRCS; j++)
                    {
                        if (j > 0)
                        {
                            calibStreamWriter.Write(",");
                        }

                        calibStreamWriter.Write("{0},{1}", wm1capturedImages[i][j].X, wm1capturedImages[i][j].Y);
                    }

                    for (j = 0; j < MAX_NUM_IR_SRCS; j++)
                    {
                        calibStreamWriter.Write(",");

                        calibStreamWriter.Write("{0},{1}", wm2capturedImages[i][j].X, wm2capturedImages[i][j].Y);
                    }

                    calibStreamWriter.Write("\n");
                }

                calibStreamWriter.Close();

                return 0;
            }
            else
            {
                return -1;
            }

        }

        /// <summary>
        /// Loads a set of stereo calibration images from a file.
        /// </summary>
        /// <param name="filename">The filename containing the image points. This
        /// file must be in the format as created by SaveStereoCalibrationImages.</param>
        /// <returns>0 if successful.</returns>
        public int LoadStereoCalibrationImages(string filename)
        {
            StreamReader calibStreamReader = new StreamReader(filename);

            string textline = "";

            string[] values = new string[16];

            int i = 0;

            while (!calibStreamReader.EndOfStream | i > MAX_NUM_OF_CAL_IMAGES - 1)
            {
                textline = calibStreamReader.ReadLine();

                values = textline.Split(',');

                for (int j = 0; j < MAX_NUM_IR_SRCS; j++)
                {
                    wm1capturedImages[i][j].X = float.Parse(values[2 * j]);

                    wm1capturedImages[i][j].Y = float.Parse(values[2 * j + 1]);

                    wm2capturedImages[i][j].X = float.Parse(values[2 * j + 8]);

                    wm2capturedImages[i][j].Y = float.Parse(values[2 * j + 9]);
                }

                i++;

                stereoCapCount = i;

            }

            calibStreamReader.Close();

            ClearStereoCalibrationImages(stereoCapCount - 1);

            return 0;
        }


        /// <summary>
        /// Begins logging data from a pair of Wiimotes.
        /// </summary>
        /// <param name="interval">Interval in milliseconds between logging events.</param>
        /// <param name="wm1">Wimmote 1</param>
        /// <param name="wm2">Wiimote 2</param>
        private void StartLogging(double interval, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {
            // get the wiimotes we're using for logging
            loggingwm1 = wm1;

            loggingwm2 = wm2;

            // start the StreamWriter so we're ready to go on the first tick
            PrepareDataFile();

            // initialize the 3D logging point matrix
            for (int i = 0; i < 4; i++)
            {
                logging3DPoints[i] = new Matrix<double>(4, 1);
            }

            // set the timer interval and start it running
            datalogtimer.Interval = interval;

            datalogtimer.Enabled = true;
        }

        public void StartLogging(double interval, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2, bool dolog3D, bool dolograwdata, bool dologundistorted)
        {

            if (dolog3D)
            {
                DOTRIANGULATION = true;
            }
            else
            {
                DOTRIANGULATION = false;
            }

            DOLOG3D = dolog3D;

            DOLOGRAW = dolograwdata;

            DOLOGUNDISTORTED = dologundistorted;

            if (DOLOGUNDISTORTED)
            {
                DOUNDISTORTPOINTS = true;
            }

            StartLogging(interval, wm1, wm2);

        }

        public void StartLogging(double interval, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2, bool dolog3D, bool dolograwdata, bool dologundistorted, string filename)
        {

            _logfilename = filename;

            StartLogging(interval, wm1, wm2, dolog3D, dolograwdata, dologundistorted);

        }

        public void StopLogging()
        {

            datalogtimer.Enabled = false;

            logStreamWriter.Close();

        }

        private void PrepareDataFile()
        {
            logStreamWriter = new StreamWriter(_logfilename);
        }

        public Matrix<double>[] Get3DPoints()
        {
            return logging3DPoints;
        }

        // Specify what you want to happen when the Elapsed event is 
        // raised.
        private void DataLogTimerEvent(object source, ElapsedEventArgs e)
        {
            //Console.WriteLine("The Elapsed event was raised at {0}", e.SignalTime);

            if (!(DOLOGRAW || DOLOGUNDISTORTED || DOLOG3D))
            {
                StopLogging();

                return;
            }

            long datetime = DateTime.Now.ToFileTime();

            // write the time in a suitible format
            //logStreamWriter.Write(DateTime.Now.ToString("o"));
            logStreamWriter.Write(datetime.ToString());

            if (DOTRIANGULATION)
            {
                if (this.IsStereoCalibrated)
                {

                    lock (_result3DLocker)
                    {

                        // Get the 3D location, this also puts the coordinates in leftimagepoints
                        // and rightimagepoints and UDleftimagepoints and UDrightimagepoints
                        Location3D_2(logging3DPoints, loggingwm1, loggingwm2);

                        logStreamWriter.Write(DateTime.Now.ToString("o"));

                        if (DOLOGRAW)
                        {
                            for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                            {
                                // logStreamWriter.Write(",");


                                logStreamWriter.Write(",{0},{1},{2},{3}", leftimagepoints[i].X, leftimagepoints[i].Y, rightimagepoints[i].X, rightimagepoints[i].Y);


                            }
                        }

                        if (DOLOGUNDISTORTED)
                        {
                            for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                            {
                                // logStreamWriter.Write(",");

                                logStreamWriter.Write(",{0},{1},{2},{3}", UDleftimagepoints[i].X, UDleftimagepoints[i].Y, rightimagepoints[i].X, rightimagepoints[i].Y);
                            }
                        }

                        if (DOLOG3D)
                        {
                            for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                            {
                                logStreamWriter.Write(",{0},{1},{2},{3}", logging3DPoints[i].Data[0, 0], logging3DPoints[i].Data[1, 0], logging3DPoints[i].Data[2, 0], logging3DPoints[i].Data[3, 0]);
                            }
                        }

                        logStreamWriter.Write("\n");
                    }

                    // Raise the log timer tick event to let all concerned know that the 
                    // logging event has occured and some 3D points are available
                    LogTimerTick(this, EventArgs.Empty);
                }
                else
                {
                    StopLogging();

                    return;
                }
            }
            else
            {

                // capture an image of the IR points from both wiimote cameras
                StereoCapture(loggingwm1, loggingwm2, leftimagepoints, rightimagepoints);

                if (DOUNDISTORTPOINTS)
                {

                    if (loggingwm1.WiimoteState.CameraCalibInfo.IsCalibrated & loggingwm2.WiimoteState.CameraCalibInfo.IsCalibrated)
                    {
                        // undistort the points before logging
                        UDleftimagepoints = loggingwm1.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(leftimagepoints, R1, P1);

                        // undistort the points in the right hand camera
                        UDrightimagepoints = loggingwm2.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(rightimagepoints, R2, P2);

                        for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                        {
                            // logStreamWriter.Write(",");

                            logStreamWriter.Write(",{0},{1},{2},{3}", UDleftimagepoints[i].X, UDleftimagepoints[i].Y, rightimagepoints[i].X, rightimagepoints[i].Y);
                        }

                        logStreamWriter.Write("\n");
                    }
                    else
                    {
                        StopLogging();

                        return;
                    }
                }
                else
                {
                    // log the raw data from the wiimotes
                    if (DOLOGRAW)
                    {

                        for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                        {
                            logStreamWriter.Write(",{0},{1},{2},{3}", leftimagepoints[i].X, leftimagepoints[i].Y, rightimagepoints[i].X, rightimagepoints[i].Y);
                        }

                        logStreamWriter.Write("\n");
                    }
                    else
                    {
                        StopLogging();

                        return;
                    }

                }
            }

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

        // Nested class to do ascending sort on both x and y coordinates.
        private class compare_xyHelper : IComparer
        {
            int IComparer.Compare(object a, object b)
            {
                coord c1 = (coord)a;
                coord c2 = (coord)b;

                if (c1._x > c2._x)
                    return 1;
                else if (c1._x < c2._x)
                    return -1;
                else
                {
                    if (c1._y > c2._y)
                        return 1;
                    else if (c1._y < c2._y)
                        return -1;
                    else
                        return 0;
                }
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

        // Method to return IComparer object for sort helper.
        public static IComparer compare_xy()
        {
            return (IComparer)new compare_xyHelper();
        }


    }

}
