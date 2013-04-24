//=============================================
// Downloaded From                            |
// Visual C# Kicks - http://www.vcskicks.com/ |
//=============================================
using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Windows.Forms;

namespace LocationPlot3D
{
    class Math3D
    {
        const double PIOVER180 = Math.PI / 180.0;

        public class Vector3D
        {
            public double x;
            public double y;
            public double z;

            public Vector3D()
            {
                x = 0;
                y = 0;
                z = 0;
            }

            public Vector3D(int _x, int _y, int _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }

            public Vector3D(double _x, double _y, double _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }

            public Vector3D(float _x, float _y, float _z)
            {
                x = (double)_x;
                y = (double)_y;
                z = (double)_z;
            }

            public void SetXYZ(int _x, int _y, int _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }

            public void SetXYZ(float _x, float _y, float _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }

            public void SetXYZ(double _x, double _y, double _z)
            {
                x = _x;
                y = _y;
                z = _z;
            }

            public override string ToString()
            {
                return "(" + x.ToString() + ", " + y.ToString() + ", " + z.ToString() + ")";
            }

            public Vector3D Unit()
            {
                Vector3D unitVec = new Vector3D();

                double magnitude = this.Magnitude();

                unitVec.x = x / magnitude;
                unitVec.y = y / magnitude;
                unitVec.z = z / magnitude;

                return unitVec;
            }

            public double Magnitude()
            {
                return Math.Sqrt(Math.Pow(x, 2.0) + Math.Pow(y, 2.0) + Math.Pow(z, 2.0));
            }

            public static Vector3D operator +(Vector3D v1, Vector3D v2)
            {
                return
                (
                   new Vector3D
                   (
                      v1.x + v2.x,
                      v1.y + v2.y,
                      v1.z + v2.z
                   )
                );
            }

            public static Vector3D operator -(Vector3D v1, Vector3D v2)
            {
                return
                (
                   new Vector3D
                   (
                       v1.x - v2.x,
                       v1.y - v2.y,
                       v1.z - v2.z
                   )
                );
            }

            public static Vector3D operator -(Vector3D v1)
            {
                return
                (
                   new Vector3D
                   (
                      -v1.x,
                      -v1.y,
                      -v1.z
                   )
                );
            }

            public static Vector3D operator +(Vector3D v1)
            {
                return
                (
                   new Vector3D
                   (
                      +v1.x,
                      +v1.y,
                      +v1.z
                   )
                );
            }

            public static bool operator <(Vector3D v1, Vector3D v2)
            {
                return v1.Magnitude() < v2.Magnitude();
            }

            public static bool operator <=(Vector3D v1, Vector3D v2)
            {
                return v1.Magnitude() <= v2.Magnitude();
            }

            public static bool operator >(Vector3D v1, Vector3D v2)
            {
                return v1.Magnitude() > v2.Magnitude();
            }

            public static bool operator >=(Vector3D v1, Vector3D v2)
            {
                return v1.Magnitude() >= v2.Magnitude();
            }

            public static bool operator ==(Vector3D v1, Vector3D v2)
            {
                return
                (
                   (v1.x == v2.x) &&
                   (v1.y == v2.y) &&
                   (v1.z == v2.z)
                );
            }

            public static bool operator !=(Vector3D v1, Vector3D v2)
            {
                return !(v1 == v2);
            }

            public override int GetHashCode()
            {
                return
                (
                   (int)((x + y + z) % Int32.MaxValue)
                );
            }

            public override bool Equals(object other)
            {
                // Check object other is a Vector3 object

                if (other is Vector3D)
                {
                    // Convert object to Vector3

                    Vector3D otherVector = (Vector3D)other;

                    // Check for equality

                    return otherVector == this;
                }
                else
                {
                    return false;
                }
            }

            public bool Equals(Vector3D other)
            {
                return other == this;
            }

            public static Vector3D operator /(Vector3D v1, double s2)
            {
                return
                (
                   new Vector3D
                   (
                      v1.x / s2,
                      v1.y / s2,
                      v1.z / s2
                   )
                );
            }

            public static Vector3D operator *(Vector3D v1, double s2)
            {
                return
                (
                   new Vector3D
                   (
                      v1.x * s2,
                      v1.y * s2,
                      v1.z * s2
                   )
                );
            }

            public static Vector3D operator *(double s1, Vector3D v2)
            {
                return v2 * s1;
            }

            public static Vector3D CrossProduct(Vector3D v1, Vector3D v2)
            {
                return
                (
                   new Vector3D
                   (
                      v1.y * v2.z - v1.z * v2.y,
                      v1.z * v2.x - v1.x * v2.z,
                      v1.x * v2.y - v1.y * v2.x
                   )
                );
            }

            public Vector3D CrossProduct(Vector3D other)
            {
                return CrossProduct(this, other);
            }

            public static double DotProduct(Vector3D v1, Vector3D v2)
            {
                return
                (
                   v1.x * v2.x +
                   v1.y * v2.y +
                   v1.z * v2.z
                );
            }

            public double DotProduct(Vector3D other)
            {
                return DotProduct(this, other);
            }

            public static double Distance(Vector3D v1, Vector3D v2)
            {
                return
                (
                   Math.Sqrt
                   (
                       (v1.x - v2.x) * (v1.x - v2.x) +
                       (v1.y - v2.y) * (v1.y - v2.y) +
                       (v1.z - v2.z) * (v1.z - v2.z)
                   )
                );
            }

            public double Distance(Vector3D other)
            {
                return Distance(this, other);
            }
        }

        internal class Camera
        {
            public Vector3D position = new Vector3D();

            public float zoom = (float)Screen.PrimaryScreen.Bounds.Width / 1.5f;
        }

        public class Projection
        {
            /// <summary>
            /// Converts a 3D location to it's 2D projection and shifts  
            /// the projection inage centre to a new drawing origin.
            /// </summary>
            /// <param name="vec">Vector3D object containing the 3D location 
            /// of the point to be projected.</param>
            /// <param name="drawOrigin">Point object containing the drawing 
            /// origin to which the projection origin will be shfted.</param>
            /// <returns>A PointF object containing the shifted 2D projection  
            /// of the 3D point provided in vec.</returns>
            public static PointF Get2D(Vector3D vec, Vector3D origin3D, Point drawOrigin)
            {
                // Convert each point to it's 2D projected image position
                PointF point2D = Get2D(vec, origin3D);

                // return the point after shifting the original origin (at (0,0)) 
                // to the desired origin in drawOrigin
                return new PointF(point2D.X + drawOrigin.X, point2D.Y + drawOrigin.Y);
            }

            /// <summary>
            /// Projects a 3D location to a 2D plane. 
            /// </summary>
            /// <param name="vec">Vector3D object containing the 3D location 
            /// of the point to be projected.</param>
            /// <returns>A PointF object containing the 2D projection of the 
            /// 3D point provided in vec.</returns>
            public static PointF Get2D(Vector3D vec, Vector3D origin3D)
            {
                PointF returnPoint = new PointF();

                //float zoom = (float)Screen.PrimaryScreen.Bounds.Width / 1.5f;
                Camera tempCam = new Camera();

                tempCam.position.x = origin3D.x;
                tempCam.position.y = origin3D.y;
                tempCam.position.z = (origin3D.x * tempCam.zoom) / origin3D.x;

                double zValue = -vec.z - tempCam.position.z;

                returnPoint.X = (float)((tempCam.position.x - vec.x) / zValue * tempCam.zoom);
                returnPoint.Y = (float)((tempCam.position.y - vec.y) / zValue * tempCam.zoom);

                return returnPoint;
            }
        }

        public class Plot3D
        {
            public Cube plotArea;
            public PlotPoint[] _plotPoints;
            public Vector3D plotOrigin;
            private int _width = 0;
            private int _height = 0;
            private int _depth = 0;

            float xRotation = 0.0f;
            float yRotation = 0.0f;
            float zRotation = 0.0f;

            public Plot3D(int Width, int Height, int Depth, int numPlotPoints)
            {
                _width = Width;
                _height = Height;
                _depth = Depth;

                // Declare a cube to make a plotting area
                plotArea  = new Cube(_width, _height, _depth);

                // determine the origin of the plot object
                plotOrigin = new Math3D.Vector3D(_width / 2, _height / 2, _depth / 2);

                // declare an array of plot points to be drawn 
                _plotPoints = new PlotPoint[numPlotPoints];

                for (int i = 0; i < _plotPoints.Length; i++)
                {
                    _plotPoints[i] = new PlotPoint(0, 0, 0, plotOrigin);
                }

                InitialisePlotPoints();
            }

            public Plot3D(int Width, int Height, int Depth, PlotPoint[] plotPoints)
            {
                _width = Width;
                _height = Height;
                _depth = Depth;

                // Declare a cube to make a plotting area
                plotArea = new Cube(_width, _height, _depth);

                // determine the origin of the plot object
                plotOrigin = new Math3D.Vector3D(_width / 2, _height / 2, _depth / 2);

                // declare an array of plot points to be drawn 
                _plotPoints = plotPoints;

                InitialisePlotPoints();
            }

            private void InitialisePlotPoints()
            {
                for (int i = 0; i < _plotPoints.Length; i++)
                {
                    _plotPoints[i].SetOrigin = plotOrigin;
                }
            }

            public Bitmap DrawPlot(Point drawOrigin)
            {
                Rectangle bounds = plotArea.getDrawingBounds();
                bounds.Width += drawOrigin.X;
                bounds.Height += drawOrigin.Y;

                Bitmap finalBmp = new Bitmap(bounds.Width, bounds.Height);
                Graphics g = Graphics.FromImage(finalBmp);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                // Do the drawing
                plotArea.DrawCube(drawOrigin, g, finalBmp);

                for (int i = 0; i < _plotPoints.Length; i++)
                {
                    _plotPoints[i].DrawPlotPoint(drawOrigin, g, finalBmp);
                }

                g.Dispose();

                return finalBmp;
            }

            public void ResizePlot(int Width, int Height, int Depth, Point Origin)
            {
                float scale = 0;

                scale = Math.Max(Width / _width, Depth / _depth);

                //for (int i = 0; i < _plotPoints.Length; i++)
                //{
                   
                //}

                plotArea.ResizeCube(Width, Height, Depth, Origin);

            }

            public float RotateX
            {
                get { return xRotation; }
                set
                {
                    //rotate the difference between this rotation and last rotation
                    //RotateX(value - xRotation);
                    plotArea.RotateX = value;
                    
                    //rotate the plot points
                    for (int i=0;i <_plotPoints.Length;i++)
                    {
                        _plotPoints[i].RotateX = value;
                    }

                    xRotation = value;
                }
            }

            public float RotateY
            {
                get { return yRotation; }
                set
                {
                    plotArea.RotateY = value;

                    //rotate the plot points
                    for (int i = 0; i < _plotPoints.Length; i++)
                    {
                        _plotPoints[i].RotateY = value;
                    }

                    yRotation = value;
                }
            }

            public float RotateZ
            {
                get { return zRotation; }
                set
                {
                    plotArea.RotateZ = value;

                    //rotate the plot points
                    for (int i = 0; i < _plotPoints.Length; i++)
                    {
                        _plotPoints[i].RotateZ = value;
                    }

                    zRotation = value;
                }
            }

        }

        public class Cube
        {
            //Cube face, has four points, 3D and 2D
            internal class Face : IComparable<Face>
            {
                public enum Side
                {
                    Front,
                    Back,
                    Left,
                    Right,
                    Top,
                    Bottom
                }

                public PointF[] Corners2D;
                public Vector3D[] Corners3D;
                public Vector3D Center;
                public Side CubeSide;

                public Face()
                {
                }

                public int CompareTo(Face otherFace)
                {
                    return (int)(this.Center.z - otherFace.Center.z); //In order of which is closest to the screen
                }
            }

            public int width = 0;
            public int height = 0;
            public int depth = 0;

            float xRotation = 0.0f;
            float yRotation = 0.0f;
            float zRotation = 0.0f;

            bool drawCubeEdges = true;
            bool fillFront;
            bool fillBack;
            bool fillLeft;
            bool fillRight;
            bool fillTop;
            bool fillBottom;

            Vector3D cubeOrigin;

            Face[] faces;

            public float RotateX
            {
                get { return xRotation; }
                set
                {
                    //rotate the difference between this rotation and last rotation
                    RotateCubeX(value - xRotation);
                    xRotation = value;
                }
            }

            public float RotateY
            {
                get { return yRotation; }
                set
                {
                    RotateCubeY(value - yRotation);
                    yRotation = value;
                }
            }

            public float RotateZ
            {
                get { return zRotation; }
                set
                {
                    RotateCubeZ(value - zRotation);
                    zRotation = value;
                }
            }

            public bool DrawWires
            {
                get { return drawCubeEdges; }
                set { drawCubeEdges = value; }
            }

            public bool FillFront
            {
                get { return fillFront; }
                set { fillFront = value; }
            }

            public bool FillBack
            {
                get { return fillBack; }
                set { fillBack = value; }
            }

            public bool FillLeft
            {
                get { return fillLeft; }
                set { fillLeft = value; }
            }

            public bool FillRight
            {
                get { return fillRight; }
                set { fillRight = value; }
            }

            public bool FillTop
            {
                get { return fillTop; }
                set { fillTop = value; }
            }

            public bool FillBottom
            {
                get { return fillBottom; }
                set { fillBottom = value; }
            }

            #region Initializers

            public Cube(int side)
            {
                width = side;
                height = side;
                depth = side;
                cubeOrigin = new Math3D.Vector3D(width / 2, height / 2, depth / 2);
                InitializeCube();
            }

            public Cube(int side, Vector3D origin)
            {
                width = side;
                height = side;
                depth = side;
                cubeOrigin = origin;

                InitializeCube();
            }

            public Cube(int Width, int Height, int Depth)
            {
                width = Width;
                height = Height;
                depth = Depth;
                cubeOrigin = new Math3D.Vector3D(width / 2, height / 2, depth / 2);

                InitializeCube();
            }

            public Cube(int Width, int Height, int Depth, Vector3D origin)
            {
                width = Width;
                height = Height;
                depth = Depth;
                cubeOrigin = origin;

                InitializeCube();
            }

            public void ResizeCube(int Width, int Height, int Depth, Point origin)
            {
                width = Width;
                height = Height;
                depth = Depth;
                cubeOrigin = new Math3D.Vector3D(width / 2, height / 2, depth / 2);

                //Front Face --------------------------------------------
                faces[0].Corners3D[0].SetXYZ(0, 0, 0);
                faces[0].Corners3D[1].SetXYZ(0, height, 0);
                faces[0].Corners3D[2].SetXYZ(width, height, 0);
                faces[0].Corners3D[3].SetXYZ(width, 0, 0);
                faces[0].Center.SetXYZ(width / 2, height / 2, 0);
                // -------------------------------------------------------

                //Back Face --------------------------------------------
                faces[1].Corners3D[0].SetXYZ(0, 0, depth);
                faces[1].Corners3D[1].SetXYZ(0, height, depth);
                faces[1].Corners3D[2].SetXYZ(width, height, depth);
                faces[1].Corners3D[3].SetXYZ(width, 0, depth);
                faces[1].Center.SetXYZ(width / 2, height / 2, depth);
                // -------------------------------------------------------

                //Left Face --------------------------------------------
                faces[2].Corners3D[0].SetXYZ(0, 0, 0);
                faces[2].Corners3D[1].SetXYZ(0, 0, depth);
                faces[2].Corners3D[2].SetXYZ(0, height, depth);
                faces[2].Corners3D[3].SetXYZ(0, height, 0);
                faces[2].Center.SetXYZ(0, height / 2, depth / 2);
                // -------------------------------------------------------

                //Right Face --------------------------------------------
                faces[3].Corners3D[0].SetXYZ(width, 0, 0);
                faces[3].Corners3D[1].SetXYZ(width, 0, depth);
                faces[3].Corners3D[2].SetXYZ(width, height, depth);
                faces[3].Corners3D[3].SetXYZ(width, height, 0);
                faces[3].Center.SetXYZ(width, height / 2, depth / 2);
                // -------------------------------------------------------

                //Top Face --------------------------------------------
                faces[4].Corners3D[0].SetXYZ(0, 0, 0);
                faces[4].Corners3D[1].SetXYZ(0, 0, depth);
                faces[4].Corners3D[2].SetXYZ(width, 0, depth);
                faces[4].Corners3D[3].SetXYZ(width, 0, 0);
                faces[4].Center.SetXYZ(width / 2, 0, depth / 2);
                // -------------------------------------------------------

                //Bottom Face --------------------------------------------
                faces[5].Corners3D[0].SetXYZ(0, height, 0);
                faces[5].Corners3D[1].SetXYZ(0, height, depth);
                faces[5].Corners3D[2].SetXYZ(width, height, depth);
                faces[5].Corners3D[3].SetXYZ(width, height, 0);
                faces[5].Center.SetXYZ(width / 2, height, depth / 2);
                // -------------------------------------------------------

                RotateCubeX(xRotation);
                RotateCubeY(yRotation);
                RotateCubeZ(zRotation);

                Update2DPoints(origin);
            }

            #endregion

            private void InitializeCube()
            {
                //Fill in the cube

                faces = new Face[6]; //cube has 6 faces

                //Front Face --------------------------------------------
                faces[0] = new Face();
                faces[0].CubeSide = Face.Side.Front;
                faces[0].Corners3D = new Vector3D[4];
                faces[0].Corners3D[0] = new Vector3D(0, 0, 0);
                faces[0].Corners3D[1] = new Vector3D(0, height, 0);
                faces[0].Corners3D[2] = new Vector3D(width, height, 0);
                faces[0].Corners3D[3] = new Vector3D(width, 0, 0);
                faces[0].Center = new Vector3D(width / 2, height / 2, 0);
                // -------------------------------------------------------

                //Back Face --------------------------------------------
                faces[1] = new Face();
                faces[1].CubeSide = Face.Side.Back;
                faces[1].Corners3D = new Vector3D[4];
                faces[1].Corners3D[0] = new Vector3D(0, 0, depth);
                faces[1].Corners3D[1] = new Vector3D(0, height, depth);
                faces[1].Corners3D[2] = new Vector3D(width, height, depth);
                faces[1].Corners3D[3] = new Vector3D(width, 0, depth);
                faces[1].Center = new Vector3D(width / 2, height / 2, depth);
                // -------------------------------------------------------

                //Left Face --------------------------------------------
                faces[2] = new Face();
                faces[2].CubeSide = Face.Side.Left;
                faces[2].Corners3D = new Vector3D[4];
                faces[2].Corners3D[0] = new Vector3D(0, 0, 0);
                faces[2].Corners3D[1] = new Vector3D(0, 0, depth);
                faces[2].Corners3D[2] = new Vector3D(0, height, depth);
                faces[2].Corners3D[3] = new Vector3D(0, height, 0);
                faces[2].Center = new Vector3D(0, height / 2, depth / 2);
                // -------------------------------------------------------

                //Right Face --------------------------------------------
                faces[3] = new Face();
                faces[3].CubeSide = Face.Side.Right;
                faces[3].Corners3D = new Vector3D[4];
                faces[3].Corners3D[0] = new Vector3D(width, 0, 0);
                faces[3].Corners3D[1] = new Vector3D(width, 0, depth);
                faces[3].Corners3D[2] = new Vector3D(width, height, depth);
                faces[3].Corners3D[3] = new Vector3D(width, height, 0);
                faces[3].Center = new Vector3D(width, height / 2, depth / 2);
                // -------------------------------------------------------

                //Top Face --------------------------------------------
                faces[4] = new Face();
                faces[4].CubeSide = Face.Side.Top;
                faces[4].Corners3D = new Vector3D[4];
                faces[4].Corners3D[0] = new Vector3D(0, 0, 0);
                faces[4].Corners3D[1] = new Vector3D(0, 0, depth);
                faces[4].Corners3D[2] = new Vector3D(width, 0, depth);
                faces[4].Corners3D[3] = new Vector3D(width, 0, 0);
                faces[4].Center = new Vector3D(width / 2, 0, depth / 2);
                // -------------------------------------------------------

                //Bottom Face --------------------------------------------
                faces[5] = new Face();
                faces[5].CubeSide = Face.Side.Bottom;
                faces[5].Corners3D = new Vector3D[4];
                faces[5].Corners3D[0] = new Vector3D(0, height, 0);
                faces[5].Corners3D[1] = new Vector3D(0, height, depth);
                faces[5].Corners3D[2] = new Vector3D(width, height, depth);
                faces[5].Corners3D[3] = new Vector3D(width, height, 0);
                faces[5].Center = new Vector3D(width / 2, height, depth / 2);
                // -------------------------------------------------------
            }

            //Calculates the 2D points of each face
            private void Update2DPoints(Point drawOrigin)
            {
                //Update the 2D points of all the faces
                for (int i = 0; i < faces.Length; i++)
                {
                    Update2DPoints(drawOrigin, i);
                }
            }

            /// <summary>
            /// Calculates the projected coordinates of the 3D points in a cube face.
            /// </summary>
            /// <param name="drawOrigin">Point object containing the drawing 
            /// origin to which the 2D projection origin will be shfted.</param>
            /// <param name="faceIndex">Integer index denoting which face of the 
            /// cube is being updated.</param>
            private void Update2DPoints(Point drawOrigin, int faceIndex)
            {
                // A face has four corners
                PointF[] point2D = new PointF[4];

                //float zoom = (float)Screen.PrimaryScreen.Bounds.Width / 1.5f;
                Point tmpOrigin = new Point(0, 0);

                //Convert the 3D Points of the four corners to 2D projections
                Math3D.Vector3D vec;

                for (int i = 0; i < point2D.Length; i++)
                {
                    vec = faces[faceIndex].Corners3D[i];
                    point2D[i] = Projection.Get2D(vec, cubeOrigin, drawOrigin);
                }

                // Update the face points
                faces[faceIndex].Corners2D = point2D;
            }

            // Rotating methods, have to translate the cube to the rotation point 
            // (center), rotate, and translate back to avoid gimbal lock

            private void RotateCubeX(float deltaX)
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    //Apply rotation
                    //------Rotate points
                    Vector3D point0 = new Vector3D(0, 0, 0);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, cubeOrigin, point0); //Move corner to origin
                    faces[i].Corners3D = Math3D.RotateX(faces[i].Corners3D, deltaX);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, point0, cubeOrigin); //Move back

                    //-------Rotate center
                    faces[i].Center = Math3D.Translate(faces[i].Center, cubeOrigin, point0);
                    faces[i].Center = Math3D.RotateX(faces[i].Center, deltaX);
                    faces[i].Center = Math3D.Translate(faces[i].Center, point0, cubeOrigin);
                }
            }

            private void RotateCubeY(float deltaY)
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    //Apply rotation
                    //------Rotate points
                    Vector3D point0 = new Vector3D(0, 0, 0);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, cubeOrigin, point0); //Move corner to origin
                    faces[i].Corners3D = Math3D.RotateY(faces[i].Corners3D, deltaY);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, point0, cubeOrigin); //Move back

                    //-------Rotate center
                    faces[i].Center = Math3D.Translate(faces[i].Center, cubeOrigin, point0);
                    faces[i].Center = Math3D.RotateY(faces[i].Center, deltaY);
                    faces[i].Center = Math3D.Translate(faces[i].Center, point0, cubeOrigin);
                }
            }

            private void RotateCubeZ(float deltaZ)
            {
                for (int i = 0; i < faces.Length; i++)
                {
                    //Apply rotation
                    //------Rotate points
                    Vector3D point0 = new Vector3D(0, 0, 0);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, cubeOrigin, point0); //Move corner to origin
                    faces[i].Corners3D = Math3D.RotateZ(faces[i].Corners3D, deltaZ);
                    faces[i].Corners3D = Math3D.Translate(faces[i].Corners3D, point0, cubeOrigin); //Move back

                    //-------Rotate center
                    faces[i].Center = Math3D.Translate(faces[i].Center, cubeOrigin, point0);
                    faces[i].Center = Math3D.RotateZ(faces[i].Center, deltaZ);
                    faces[i].Center = Math3D.Translate(faces[i].Center, point0, cubeOrigin);
                }
            }

            public Bitmap DrawCube(Point drawOrigin)
            {
                //Get the corresponding 2D
                Update2DPoints(drawOrigin);

                //Get the bounds of the final bitmap
                Rectangle bounds = getDrawingBounds();
                bounds.Width += drawOrigin.X;
                bounds.Height += drawOrigin.Y;

                Bitmap finalBmp = new Bitmap(bounds.Width, bounds.Height);
                Graphics g = Graphics.FromImage(finalBmp);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                Array.Sort(faces); //sort faces from closest to farthest
                //message();
                for (int i = faces.Length - 1; i >= 0; i--) //draw faces from back to front
                {
                    switch (faces[i].CubeSide)
                    {
                        case Face.Side.Front:
                            if (fillFront)
                                g.FillPolygon(Brushes.Gray, GetFrontFace());
                            break;
                        case Face.Side.Back:
                            if (fillBack)
                                g.FillPolygon(Brushes.DarkGray, GetBackFace());
                            break;
                        case Face.Side.Left:
                            if (fillLeft)
                                g.FillPolygon(Brushes.Gray, GetLeftFace());
                            break;
                        case Face.Side.Right:
                            if (fillRight)
                                g.FillPolygon(Brushes.DarkGray, GetRightFace());
                            break;
                        case Face.Side.Top:
                            if (fillTop)
                                g.FillPolygon(Brushes.Gray, GetTopFace());
                            break;
                        case Face.Side.Bottom:
                            if (fillBottom)
                                g.FillPolygon(Brushes.DarkGray, GetBottomFace());
                            break;
                        default:
                            break;
                    }

                    if (drawCubeEdges)
                    {
                        g.DrawLine(Pens.Black, faces[i].Corners2D[0], faces[i].Corners2D[1]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[1], faces[i].Corners2D[2]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[2], faces[i].Corners2D[3]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[3], faces[i].Corners2D[0]);
                    }
                }

                g.Dispose();

                return finalBmp;
            }

            public void DrawCube(Point drawOrigin, Graphics g, Bitmap plotBmp)
            {
                //Get the corresponding 2D
                Update2DPoints(drawOrigin);

                g.SmoothingMode = SmoothingMode.AntiAlias;

                Array.Sort(faces); //sort faces from closest to farthest
                //message();
                for (int i = faces.Length - 1; i >= 0; i--) //draw faces from back to front
                {
                    switch (faces[i].CubeSide)
                    {
                        case Face.Side.Front:
                            if (fillFront)
                                g.FillPolygon(Brushes.Gray, GetFrontFace());
                            break;
                        case Face.Side.Back:
                            if (fillBack)
                                g.FillPolygon(Brushes.DarkGray, GetBackFace());
                            break;
                        case Face.Side.Left:
                            if (fillLeft)
                                g.FillPolygon(Brushes.Gray, GetLeftFace());
                            break;
                        case Face.Side.Right:
                            if (fillRight)
                                g.FillPolygon(Brushes.DarkGray, GetRightFace());
                            break;
                        case Face.Side.Top:
                            if (fillTop)
                                g.FillPolygon(Brushes.Gray, GetTopFace());
                            break;
                        case Face.Side.Bottom:
                            if (fillBottom)
                                g.FillPolygon(Brushes.DarkGray, GetBottomFace());
                            break;
                        default:
                            break;
                    }

                    if (drawCubeEdges)
                    {
                        g.DrawLine(Pens.Black, faces[i].Corners2D[0], faces[i].Corners2D[1]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[1], faces[i].Corners2D[2]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[2], faces[i].Corners2D[3]);
                        g.DrawLine(Pens.Black, faces[i].Corners2D[3], faces[i].Corners2D[0]);
                    }
                }

            }

            public PointF[] GetFrontFace()
            {
                //Returns the four points corresponding to the front face
                //Get the corresponding 2D
                return getFace(Face.Side.Front).Corners2D;
            }

            public PointF[] GetBackFace()
            {
                return getFace(Face.Side.Back).Corners2D;
            }

            public PointF[] GetRightFace()
            {
                return getFace(Face.Side.Right).Corners2D;
            }

            public PointF[] GetLeftFace()
            {
                return getFace(Face.Side.Left).Corners2D;
            }

            public PointF[] GetTopFace()
            {
                return getFace(Face.Side.Top).Corners2D;
            }

            public PointF[] GetBottomFace()
            {
                return getFace(Face.Side.Bottom).Corners2D;
            }

            private Face getFace(Face.Side side)
            {
                //Find the correct side
                //Since faces are sorted in order of closest to farthest
                //They won't always be in the same index
                for (int i = 0; i < faces.Length; i++)
                {
                    if (faces[i].CubeSide == side)
                        return faces[i];
                }

                return null; //not found
            }

            public Rectangle getDrawingBounds()
            {
                //Find the farthest most points to calculate the size of the returning bitmap
                float left = float.MaxValue;
                float right = float.MinValue;
                float top = float.MaxValue;
                float bottom = float.MinValue;

                for (int i = 0; i < faces.Length; i++)
                {
                    for (int j = 0; j < faces[i].Corners2D.Length; j++)
                    {
                        if (faces[i].Corners2D[j].X < left)
                            left = faces[i].Corners2D[j].X;
                        if (faces[i].Corners2D[j].X > right)
                            right = faces[i].Corners2D[j].X;
                        if (faces[i].Corners2D[j].Y < top)
                            top = faces[i].Corners2D[j].Y;
                        if (faces[i].Corners2D[j].Y > bottom)
                            bottom = faces[i].Corners2D[j].Y;
                    }
                }

                return new Rectangle(0, 0, (int)Math.Round(right - left), (int)Math.Round(bottom - top));
            }
        }

        public static Vector3D RotateX(Vector3D point3D, float degrees)
        {
            //[ a  b  c ] [ x ]   [ x*a + y*b + z*c ]
            //[ d  e  f ] [ y ] = [ x*d + y*e + z*f ]
            //[ g  h  i ] [ z ]   [ x*g + y*h + z*i ]

            //[ 1    0        0   ]
            //[ 0   cos(x)  sin(x)]
            //[ 0   -sin(x) cos(x)]

            double cDegrees = degrees * PIOVER180;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double y = (point3D.y * cosDegrees) + (point3D.z * sinDegrees);
            double z = (point3D.y * -sinDegrees) + (point3D.z * cosDegrees);

            return new Vector3D(point3D.x, y, z);
        }

        public static Vector3D RotateY(Vector3D point3D, float degrees)
        {
            //[ cos(x)   0    sin(x)]
            //[   0      1      0   ]
            //[-sin(x)   0    cos(x)]

            double cDegrees = degrees * PIOVER180;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double x = (point3D.x * cosDegrees) + (point3D.z * sinDegrees);
            double z = (point3D.x * -sinDegrees) + (point3D.z * cosDegrees);

            return new Vector3D(x, point3D.y, z);
        }

        public static Vector3D RotateZ(Vector3D point3D, float degrees)
        {
            //[ cos(x)  sin(x) 0]
            //[ -sin(x) cos(x) 0]
            //[    0     0     1]

            double cDegrees = degrees * PIOVER180;
            double cosDegrees = Math.Cos(cDegrees);
            double sinDegrees = Math.Sin(cDegrees);

            double x = (point3D.x * cosDegrees) + (point3D.y * sinDegrees);
            double y = (point3D.x * -sinDegrees) + (point3D.y * cosDegrees);

            return new Vector3D(x, y, point3D.z);
        }

        public static Vector3D Translate(Vector3D points3D, Vector3D oldOrigin, Vector3D newOrigin)
        {
            Vector3D difference = new Vector3D(newOrigin.x - oldOrigin.x, newOrigin.y - oldOrigin.y, newOrigin.z - oldOrigin.z);
            points3D.x += difference.x;
            points3D.y += difference.y;
            points3D.z += difference.z;
            return points3D;
        }

        public static Vector3D[] RotateX(Vector3D[] points3D, float degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateX((Vector3D)points3D[i], degrees);
            }
            return points3D;
        }

        public static Vector3D[] RotateY(Vector3D[] points3D, float degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateY((Vector3D)points3D[i], degrees);
            }
            return points3D;
        }

        public static Vector3D[] RotateZ(Vector3D[] points3D, float degrees)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = RotateZ((Vector3D)points3D[i], degrees);
            }
            return points3D;
        }

        public static Vector3D[] Translate(Vector3D[] points3D, Vector3D oldOrigin, Vector3D newOrigin)
        {
            for (int i = 0; i < points3D.Length; i++)
            {
                points3D[i] = Translate(points3D[i], oldOrigin, newOrigin);
            }
            return points3D;
        }

        // Plotting point object
        public class PlotPoint
        {
            bool drawPoint = true;

            float xRotation = 0.0f;
            float yRotation = 0.0f;
            float zRotation = 0.0f;

            public Vector3D centre;
            public Vector3D plotOrigin;

            private line3D[] plotPointLines = new line3D[3];

            private int pointSize = 5;

            public PlotPoint(int x, int y, int z)
            {
                centre = new Vector3D(x, y, z);

                plotOrigin = new Vector3D();

                InitializePoint();
            }

            public PlotPoint(int x, int y, int z, Vector3D origin)
            {
                centre = new Vector3D(x, y, z);

                plotOrigin = origin;

                InitializePoint();
            }

            public PlotPoint(Vector3D point)
            {
                centre = point;

                plotOrigin = new Vector3D();

                InitializePoint();
            }

            public PlotPoint(Vector3D point, Vector3D origin)
            {
                centre = point;

                plotOrigin = origin;

                InitializePoint();
            }

            private void InitializePoint()
            {

                plotPointLines[0] = new line3D(centre, pointSize, new Vector3D(1.0, 0.0, 0.0), plotOrigin);

                plotPointLines[1] = new line3D(centre, pointSize, new Vector3D(0.0, 1.0, 0.0), plotOrigin);

                plotPointLines[2] = new line3D(centre, pointSize, new Vector3D(0.0, 0.0, 1.0), plotOrigin);

            }

            public Vector3D SetOrigin
            {
                get { return plotOrigin; }

                set
                {
                    plotOrigin = value;

                    for (int i = 0; i < plotPointLines.Length; i++)
                    {
                        plotPointLines[i].lineOrigin = plotOrigin;
                    }
                }

            }

            public float RotateX
            {
                get { return xRotation; }
                set
                {
                    //rotate the plot point lines
                    for (int i=0;i <plotPointLines.Length;i++)
                    {
                        plotPointLines[i].RotateX = value;
                    }

                    xRotation = value;
                }
            }

            public float RotateY
            {
                get { return yRotation; }
                set
                {
                    //rotate the plot point lines
                    for (int i=0;i <plotPointLines.Length;i++)
                    {
                        plotPointLines[i].RotateY = value;
                    }

                    yRotation = value;
                }
            }

            public float RotateZ
            {
                get { return zRotation; }
                set
                {
                    //rotate the plot point lines
                    for (int i=0;i <plotPointLines.Length;i++)
                    {
                        plotPointLines[i].RotateZ = value;
                    }

                    zRotation = value;
                }
            }

            public void DrawPlotPoint(Point drawOrigin, Graphics g, Bitmap plotBmp)
            {
                //Get the corresponding 2D projection points
                if (drawPoint)
                {
                    for (int i = 0; i < plotPointLines.Length; i++)
                    {
                        plotPointLines[i].DrawLine(drawOrigin, g, plotBmp);
                    }
                }
            }

            public void ShowPoint()
            {
                drawPoint = true;
            }

            public void HidePoint()
            {
                drawPoint = false;
            }

        }

        public class line3D
        {
            private Vector3D _endA;
            private Vector3D _endB;
            private PointF _endAproj2D;
            private PointF _endBproj2D;
            private Point _endAproj2Ddraw;
            private Point _endBproj2Ddraw;
            private Vector3D _centre = new Vector3D();
            private double _length = 0;

            float xRotation = 0.0f;
            float yRotation = 0.0f;
            float zRotation = 0.0f;

            public Vector3D lineOrigin = new Vector3D();

            /// <summary>
            /// Constructor for the line object.
            /// </summary>
            /// <param name="centre">The centre point of the line.</param>
            /// <param name="length">The lenght of the line.</param>
            /// <param name="direction">A vector pointing in the desired line direction.</param>
            public line3D(Vector3D centre, double length, Vector3D direction)
            {
                length = length / 2;

                _endB = centre + (direction.Unit() * length);
                _endA = centre + (direction.Unit() * -length);

                UpdateLine();
            }

            /// <summary>
            /// Constructor for the line object.
            /// </summary>
            /// <param name="start">A Vector3D object denoting the starting point of the line.</param>
            /// <param name="end">A Vector3D object denoting the end point of the line.</param>
            public line3D(Vector3D start, Vector3D end)
            {
                _endA = start;
                _endB = end;

                UpdateLine();
            }

            public line3D(Vector3D start, Vector3D end, Vector3D origin)
            {
                _endA = start;
                _endB = end;

                lineOrigin = origin;

                UpdateLine();
            }

            public line3D(Vector3D centre, double length, Vector3D direction, Vector3D origin)
            {
                length = length / 2;

                _endB = centre + (direction.Unit() * length);
                _endA = centre + (direction.Unit() * -length);

                lineOrigin = origin;

                UpdateLine();
            }

            public Vector3D Start
            {
                get { return _endA; }
            }

            public Vector3D End
            {
                get { return _endB; }
            }


            public float RotateX
            {
                get { return xRotation; }
                set
                {
                    //rotate the difference between this rotation and last rotation
                    RotateLineX(value - xRotation);
                    xRotation = value;
                }
            }

            public float RotateY
            {
                get { return yRotation; }
                set
                {
                    RotateLineY(value - yRotation);
                    yRotation = value;
                }
            }

            public float RotateZ
            {
                get { return zRotation; }
                set
                {
                    RotateLineZ(value - zRotation);
                    zRotation = value;
                }
            }

            public void DrawLine(Point drawOrigin, Graphics g, Bitmap plotBmp)
            {
                //Get the corresponding 2D projection points
                this.Update2DPoints(drawOrigin);

                _endAproj2Ddraw.X = (int)_endAproj2D.X;
                _endAproj2Ddraw.Y = (int)_endAproj2D.Y;

                _endBproj2Ddraw.X = (int)_endBproj2D.X;
                _endBproj2Ddraw.Y = (int)_endBproj2D.Y;

                g.DrawLine(Pens.Black, _endAproj2Ddraw, _endBproj2Ddraw);
                     
            }

            private void Update2DPoints(Point drawOrigin)
            {
                
                float zoom = (float)Screen.PrimaryScreen.Bounds.Width / 1.5f;
                Point tmpOrigin = new Point(0, 0);

                //Convert the 3D Points of the line ends to 2D projections
                _endAproj2D = Projection.Get2D(_endA, lineOrigin, drawOrigin);
                _endBproj2D = Projection.Get2D(_endB, lineOrigin, drawOrigin);
                
            }

            private void UpdateLine()
            {
                _centre.x = (_endB.x - _endA.x) / 2;
                _centre.y = (_endB.y - _endA.y) / 2;
                _centre.z = (_endB.z - _endA.z) / 2;

                _length = Vector3D.Distance(_endA, _endB);
            }

            private void RotateLineX(float deltaX)
            {
                //Apply rotation
                Vector3D point0 = new Vector3D(0, 0, 0);
                _endA = Math3D.Translate(_endA, lineOrigin, point0); //Move point to origin
                _endA = Math3D.RotateX(_endA, deltaX);
                _endA = Math3D.Translate(_endA, point0, lineOrigin); //Move back

                _endB = Math3D.Translate(_endB, lineOrigin, point0); //Move corner to origin
                _endB = Math3D.RotateX(_endB, deltaX);
                _endB = Math3D.Translate(_endB, point0, lineOrigin); //Move back

                UpdateLine();
            }

            private void RotateLineY(float deltaY)
            {
                //Apply rotation
                Vector3D point0 = new Vector3D(0, 0, 0);
                _endA = Math3D.Translate(_endA, lineOrigin, point0); //Move corner to origin
                _endA = Math3D.RotateY(_endA, deltaY);
                _endA = Math3D.Translate(_endA, point0, lineOrigin); //Move back

                _endB = Math3D.Translate(_endB, lineOrigin, point0); //Move corner to origin
                _endB = Math3D.RotateY(_endB, deltaY);
                _endB = Math3D.Translate(_endB, point0, lineOrigin); //Move back

                UpdateLine();
            }

            private void RotateLineZ(float deltaZ)
            {
                //Apply rotation
                Vector3D point0 = new Vector3D(0, 0, 0);
                _endA = Math3D.Translate(_endA, lineOrigin, point0); //Move corner to origin
                _endA = Math3D.RotateZ(_endA, deltaZ);
                _endA = Math3D.Translate(_endA, point0, lineOrigin); //Move back

                _endB = Math3D.Translate(_endB, lineOrigin, point0); //Move corner to origin
                _endB = Math3D.RotateZ(_endB, deltaZ);
                _endB = Math3D.Translate(_endB, point0, lineOrigin); //Move back

                UpdateLine();
            }
        }
    }
}
