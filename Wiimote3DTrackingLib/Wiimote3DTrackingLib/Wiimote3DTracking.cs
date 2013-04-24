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
        private static readonly int NUM_IR_SRCS = 4;

        // The maximum number of IR sources we can expect to see
        private static readonly int MAX_NUM_IR_SRCS = 4;

        /// <summary>
        /// the maximum number of calibration images (or sets of four
        /// coordinates in this case
        /// </summary>
        private static readonly int MAX_NUM_OF_CAL_IMAGES = 100;

        // The length of a side of the calibration square
        private float sqSideLength;

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

        // The rotation and perspective matrices for the 
        Matrix<double> R1 = new Matrix<double>(3, 3);
        Matrix<double> R2 = new Matrix<double>(3, 3);
        Matrix<double> P1 = new Matrix<double>(3, 4);
        Matrix<double> P2 = new Matrix<double>(3, 4);

        // The Q matrix will be generated during the stereo calibration
        // and used to reconstruct the 3D location of an IR point from 
        // its location in the viewer
        Matrix<double> Q = new Matrix<double>(4, 4);

        Matrix<float> lmapx = new Matrix<float>(wiimoteCamSize.Height, wiimoteCamSize.Width);
        Matrix<float> rmapx = new Matrix<float>(wiimoteCamSize.Height, wiimoteCamSize.Width);
        Matrix<float> lmapy = new Matrix<float>(wiimoteCamSize.Height, wiimoteCamSize.Width);
        Matrix<float> rmapy = new Matrix<float>(wiimoteCamSize.Height, wiimoteCamSize.Width);


        // variables for left and right image points that will be used for 
        // stereo tracking
        private System.Drawing.PointF[] leftimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] rightimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] UDleftimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];
        private System.Drawing.PointF[] UDrightimagepoints = new System.Drawing.PointF[MAX_NUM_IR_SRCS];

        // boolean value determining if the system has been calibrated
        private bool _isStereoCalib = false;

        // Some test points for testing stereo calibration 
        public PointF[][] tp1 = new PointF[17][] {
            new PointF[] {new PointF(171,81), new PointF(174,440), new PointF(516,468), new PointF(542,105)},
            new PointF[] {new PointF(59,135), new PointF(71,464), new PointF(385,507), new PointF(409,171)},
            new PointF[] {new PointF(276,184), new PointF(318,570), new PointF(625,471), new PointF(616,124)},
            new PointF[] {new PointF(293,61), new PointF(300,460), new PointF(631,415), new PointF(655,55)},
            new PointF[] {new PointF(267,53), new PointF(206,479), new PointF(540,480), new PointF(603,120)},
            new PointF[] {new PointF(119,75), new PointF(99,471), new PointF(400,572), new PointF(458,115)},
            new PointF[] {new PointF(211,81), new PointF(81,450), new PointF(488,544), new PointF(587,161)},
            new PointF[] {new PointF(153,137), new PointF(74,499), new PointF(446,608), new PointF(590,275)},
            new PointF[] {new PointF(219,123), new PointF(192,566), new PointF(571,551), new PointF(610,161)},
            new PointF[] {new PointF(226,207), new PointF(197,644), new PointF(576,614), new PointF(623,228)},
            new PointF[] {new PointF(119,176), new PointF(76,632), new PointF(511,666), new PointF(575,235)},
            new PointF[] {new PointF(103,132), new PointF(53,416), new PointF(338,433), new PointF(410,175)},
            new PointF[] {new PointF(269,140), new PointF(315,454), new PointF(597,399), new PointF(567,101)},
            new PointF[] {new PointF(164,328), new PointF(160,662), new PointF(479,650), new PointF(491,332)},
            new PointF[] {new PointF(362,133), new PointF(411,479), new PointF(619,420), new PointF(577,127)},
            new PointF[] {new PointF(158,222), new PointF(217,560), new PointF(533,496), new PointF(482,176)},
            new PointF[] {new PointF(151,225), new PointF(173,562), new PointF(493,532), new PointF(475,212)}};

        public PointF[][] tp2 = new PointF[17][] {
            new PointF[] {new PointF(466,72), new PointF(443,441), new PointF(795,477), new PointF(852,108)},
            new PointF[] {new PointF(338,129), new PointF(326,463), new PointF(649,513), new PointF(702,174)},
            new PointF[] {new PointF(612,185), new PointF(615,577), new PointF(900,481), new PointF(920,127)},
            new PointF[] {new PointF(631,60), new PointF(597,465), new PointF(911,425), new PointF(966,58)},
            new PointF[] {new PointF(612,49), new PointF(525,482), new PointF(823,490), new PointF(905,123)},
            new PointF[] {new PointF(439,69), new PointF(391,471), new PointF(744,582), new PointF(835,116)},
            new PointF[] {new PointF(518,78), new PointF(402,451), new PointF(828,555), new PointF(906,167)},
            new PointF[] {new PointF(513,134), new PointF(375,498), new PointF(744,617), new PointF(941,282)},
            new PointF[] {new PointF(573,121), new PointF(518,573), new PointF(872,565), new PointF(933,167)},
            new PointF[] {new PointF(585,208), new PointF(522,651), new PointF(878,629), new PointF(952,234)},
            new PointF[] {new PointF(479,172), new PointF(409,636), new PointF(840,680), new PointF(925,240)},
            new PointF[] {new PointF(370,128), new PointF(288,415), new PointF(560,437), new PointF(656,177)},
            new PointF[] {new PointF(534,138), new PointF(559,458), new PointF(834,407), new PointF(821,104)},
            new PointF[] {new PointF(428,326), new PointF(412,666), new PointF(727,659), new PointF(745,336)},
            new PointF[] {new PointF(642,134), new PointF(679,486), new PointF(857,428), new PointF(822,130)},
            new PointF[] {new PointF(426,220), new PointF(475,562), new PointF(788,505), new PointF(744,178)},
            new PointF[] {new PointF(415,223), new PointF(429,563), new PointF(745,539), new PointF(731,215)}};

        public StereoTracking()
        {
            sqSideLength = (float)(142.0 / 1000.0);

            InitializeStereoTrackingImages();

            leftimagepoints.Initialize();
            rightimagepoints.Initialize();
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

            leftimagepoints.Initialize();
            rightimagepoints.Initialize();
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
        /// Returns the number of single camera calibration images that 
        /// have been captured.
        /// </summary>
        public float CalibrateImageCount
        {
            get { return capCount; }
        }

        /// <summary>
        /// Returns the number of stereo camera calibration image pairs 
        /// that have been captured.
        /// </summary>
        public float StereoCalibrateImageCount
        {
            get { return stereoCapCount; }
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

                CalibObjectPoints[i][1].x = 0;
                CalibObjectPoints[i][1].y = sqSideLength;
                CalibObjectPoints[i][1].z = 0;

                CalibObjectPoints[i][2].x = sqSideLength;
                CalibObjectPoints[i][2].y = sqSideLength;
                CalibObjectPoints[i][2].z = 0;

                CalibObjectPoints[i][3].x = sqSideLength;
                CalibObjectPoints[i][3].y = 0;
                CalibObjectPoints[i][3].z = 0;
            }

            // Now initialize the array to hold the captured images 
            // from the camera
            singlewmCapturedImages.Initialize();
            wm1capturedImages.Initialize();
            wm2capturedImages.Initialize();

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


        public void StereoCalibrate(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {

            Matrix<double> fundMat = new Matrix<double>(3, 3);
            Matrix<double> essentialMat = new Matrix<double>(3, 3);

            //Matrix<double> Q = new Matrix<double>(4, 4);

            int maxIters = 100;

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

            Array.Copy(CalibObjectPoints, objectPoints, stereoCapCount);
            Array.Copy(wm1capturedImages, wm1ImagePoints, stereoCapCount);
            Array.Copy(wm2capturedImages, wm2ImagePoints, stereoCapCount);

            // calibrate the wiimote cameras individually
            CalibrateCamera(wm1, objectPoints, wm1ImagePoints);
            CalibrateCamera(wm2, objectPoints, wm2ImagePoints);

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

            wm2.WiimoteState.CameraCalibInfo.StereoCamExtrinsic = wm1.WiimoteState.CameraCalibInfo.StereoCamExtrinsic;
             
            Rectangle roi1 = new Rectangle();
            Rectangle roi2 = new Rectangle();

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
                Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.CALIB_ZERO_DISPARITY,
                -1.0,
                Size.Empty,
                ref roi1,
                ref roi2);

            
            //Emgu.CV.CvInvoke.cvInitUndistortRectifyMap(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //    wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //    R1.Ptr,
            //    P1.Ptr,
            //    lmapx.Ptr,
            //    lmapy.Ptr);

            //Emgu.CV.CvInvoke.cvInitUndistortRectifyMap(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //    wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //    R2.Ptr,
            //    P2.Ptr,
            //    rmapx.Ptr,
            //    rmapy.Ptr);


            _isStereoCalib = true;
                
        }

        public void StereoCalibrate(WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2, System.Drawing.PointF[][] leftimagepoints, System.Drawing.PointF[][] rightimagepoints)
        {

            Matrix<double> fundMat = new Matrix<double>(3, 3);
            Matrix<double> essentialMat = new Matrix<double>(3, 3);

            //Matrix<double> Q = new Matrix<double>(4, 4);

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
                            Emgu.CV.CvEnum.STEREO_RECTIFY_TYPE.CALIB_ZERO_DISPARITY,
                            -1.0,
                            Size.Empty,
                            ref roi1,
                            ref roi2);

            //Emgu.CV.CvInvoke.cvInitUndistortRectifyMap(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //    wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //    R1.Ptr,
            //    P1.Ptr,
            //    lmapx.Ptr,
            //    lmapy.Ptr);

            //Emgu.CV.CvInvoke.cvInitUndistortRectifyMap(wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.IntrinsicMatrix.Ptr,
            //    wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.DistortionCoeffs.Ptr,
            //    R2.Ptr,
            //    P2.Ptr,
            //    rmapx.Ptr,
            //    rmapy.Ptr);

            _isStereoCalib = true;

        }

        /// <summary>
        /// Get the 3D location of all points in the view of the stereo rig
        /// </summary>
        /// <param name="result3DPoints"></param>
        /// <param name="wm1"></param>
        /// <param name="wm2"></param>
        public void Location3D(Matrix<double>[] result3DPoints, WiimoteLib.Wiimote wm1, WiimoteLib.Wiimote wm2)
        {

            //PointF[] leftimagepoints = new PointF[MAX_NUM_IR_SRCS];
            //PointF[] rightimagepoints = new PointF[MAX_NUM_IR_SRCS];

            StereoCapture(wm1, wm2, leftimagepoints, rightimagepoints);

            int i = 0;

            //Emgu.CV.CvArray<float> XYDpoint;

            Matrix<double> XYDpoint = new Matrix<double>(4, 1);

            UDleftimagepoints = wm1.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(leftimagepoints, R1, P1);

            UDrightimagepoints = wm2.WiimoteState.CameraCalibInfo.CamIntrinsic.Undistort(rightimagepoints, R2, P2);

            Emgu.CV.Structure.MCvScalar fillval = new Emgu.CV.Structure.MCvScalar(0);

            for (i = 0; i < MAX_NUM_IR_SRCS; i++)
            {
                //result3DPoints[i] = new Matrix<float>(1, 3);

                if (leftimagepoints[i].X <= wiimoteCamSize.Width &&
                    leftimagepoints[i].Y < wiimoteCamSize.Height &&
                    rightimagepoints[i].X <= wiimoteCamSize.Width &&
                    rightimagepoints[i].Y <= wiimoteCamSize.Height)
                {

                    // get the undistorted x-coordinate in the left camera
                    XYDpoint.Data[0, 0] = (double)(UDleftimagepoints[i].X);

                    //Debug.WriteLine("XYDpoint.Data[0, 0] = " + XYDpoint.Data[0, 0].ToString());

                    // get the undistorted y-coordinate in the right camera
                    XYDpoint.Data[1, 0] = (double)(UDleftimagepoints[i].Y);

                    //Debug.WriteLine("XYDpoint.Data[0, 1] = " + XYDpoint.Data[1, 0].ToString());

                    // the disparity between the points is the right camera x-coordinate
                    // subtracted from the left x-coordinate
                    XYDpoint.Data[2, 0] = (double)(UDleftimagepoints[i].X - UDrightimagepoints[i].X);

                    //Debug.WriteLine("XYDpoint.Data[0, 2] = " + XYDpoint.Data[2, 0].ToString());

                    XYDpoint.Data[3, 0] = (double)(1);

                    // Call cvPerspectiveTransform to get the 3D location
                    // CvInvoke.cvPerspectiveTransform(XYDpoint.Ptr, result3DPoints[i].Ptr, Q.Ptr);
                    MatrixMult(Q, XYDpoint, result3DPoints[i]);

                    // Divide the X, Y and Z factors by the scaling factor
                    result3DPoints[i].Data[0, 0] = result3DPoints[i].Data[0, 0] / result3DPoints[i].Data[3, 0];
                    result3DPoints[i].Data[1, 0] = result3DPoints[i].Data[1, 0] / result3DPoints[i].Data[3, 0];
                    result3DPoints[i].Data[2, 0] = result3DPoints[i].Data[2, 0] / result3DPoints[i].Data[3, 0];

                }
                else
                {
                    result3DPoints[i].Data[0, 0] = -1;
                    result3DPoints[i].Data[1, 0] = -1;
                    result3DPoints[i].Data[2, 0] = -1;
                }
                
            }

        }

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
                //coords1[i].y = 768 - coords1[i].y;
                //coords2[i].y = 768 - coords2[i].y;
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
                coords[i].y = 768 - coords[i].y;
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

                // Wii puts origin in bottom left, Y up; toolbox in top left, Y down.  Convert by subtracting y from 768.		
                for (int i = 0; i < MAX_NUM_IR_SRCS; i++)
                {
                    coords[i].y = 768 - coords[i].y;
                    //Debug.WriteLine("coords[" + i.ToString() + "] = " + coords[i].ToString());
                }

                // sort the coordinates in x and y 
                Array.Sort(coords, 0, MAX_NUM_IR_SRCS, coord.compare_xy());

                return 0;
            }
            else
            {
                return -1;
            }

        }

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

                        if (c.x > 1024 || c.x < 0 || c.y > 768 || c.y < 0)
                        {
                            // the IR data is invalid for this source, put the 
                            // coordinate outside the view
                            coords[i].x = wiimoteCamSize.Width + 1 + i;
                            coords[i].y = wiimoteCamSize.Height + 1 + i;
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
                        coords[i].x = wiimoteCamSize.Width + 1 + i;
                        coords[i].y = wiimoteCamSize.Height + 1 + i;
                        coords[i].pt = i+1;
                    }
                }
            }
            else
            {
                // return 1 as we could not get info from controller, it may be disconnected
                return -2;
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
