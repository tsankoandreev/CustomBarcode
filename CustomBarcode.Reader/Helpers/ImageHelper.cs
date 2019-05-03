using CustomBarcode.Common;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarcode.Reader.Helpers
{
    public static class ImageHelper
    {
        public static OpenCvSharp.Mat GetImageFromFile(string filename)
        {
            var mat = new Mat(filename, ImreadModes.Unchanged);
            mat.SaveImage("preloadedImage.png");
            
            return mat;

            //var fileData = System.IO.File.ReadAllBytes("generatedMarker.jpg");
            //UnityEngine.Texture2D tex = new UnityEngine.Texture2D(CONSTS.COMPARING_MARKER_DIAMETER + (int)CONSTS.ANCOR_SEGMENT_RADIUS * 2, CONSTS.COMPARING_MARKER_DIAMETER + (int)CONSTS.ANCOR_SEGMENT_RADIUS * 2);
            //tex.LoadImage(fileData);
            //
            //OpenCvSharp.Mat img = new OpenCvSharp.Mat(tex.height, tex.width, OpenCvSharp.MatType.CV_8UC3);
            //var pixels = tex.GetPixels(0);
            //var remapped = new byte[pixels.Length * 3];
            //int idx = 0;
            //foreach (var p in pixels)
            //{
            //    remapped[idx++] = (byte)Math.Floor(255 * p.b);
            //    remapped[idx++] = (byte)Math.Floor(255 * p.g);
            //    remapped[idx++] = (byte)Math.Floor(255 * p.r);
            //}
            //img.SetArray(0, 0, remapped);
            //img = img.Flip(OpenCvSharp.FlipMode.X);
            //img.SaveImage("loadedImage.jpg");
            //
            ////img = ImageHelper.CropCircleWithoutCountour(img);
            //return img;
        }

        /// <summary>
        ///1. Read the image
        ///2. Find the contours
        ///3. Select the one with maximum area.
        ///4. Find the corner points.
        ///5. Warp the image to a square
        /// </summary>
        /// <param name="img"></param>
        /// <returns></returns>
        public static Mat GetSquareFromImage(Mat img)
        {
            var gray = img.CvtColor(ColorConversionCodes.BGR2GRAY);
            gray = gray.GaussianBlur(new Size(9, 9), 2, 2);
            gray.ConvertTo(gray, MatType.CV_8UC1);

            double largest_area = 0;
            int largest_contour_index = 0;
            Rect bounding_rect = new Rect();

            List<Point> maxCurve = new List<Point>();

            Mat cannie = gray.Canny(50, 200);
            cannie.SaveImage("canny.png");

            Point[][] contours;
            HierarchyIndex[] hIndx;
            cannie.FindContours(out contours, out hIndx, RetrievalModes.Tree, ContourApproximationModes.ApproxSimple); // Find the contours in the image

            for (int i = 0; i < contours.Count(); i++) // iterate through each contour.
            {
                double area = Cv2.ContourArea(contours[i]);  //  Find the area of contour

                if (area > largest_area)
                {
                    largest_area = area;
                    largest_contour_index = i;               //Store the index of largest contour
                    bounding_rect = Cv2.BoundingRect(contours[i]); // Find the bounding rectangle for biggest contour
                }
            }

            if (bounding_rect.Width < img.Width * 0.5 || bounding_rect.Height < img.Height * 0.5)
            {
                Console.WriteLine("Size of square is too small.. ");
                return null;
            }

            maxCurve = Cv2.ApproxPolyDP(contours[largest_contour_index], contours[largest_contour_index].Length * 0.05, true).ToList();//4 corner points
            if (maxCurve.Count == 4)
            {
                Mat mc = img.Clone();
                DrawHintPoints(mc, maxCurve);
                mc.SaveImage("maxCurve.png");
            }

            Mat outputMat = new Mat(CONSTS.DEFAULT_MARKER_DIAMETER, CONSTS.DEFAULT_MARKER_DIAMETER, img.Type());

            List<Point2f> srcPoints = new List<Point2f>() { maxCurve[0], maxCurve[1], maxCurve[2], maxCurve[3] };

            var destPoints = new List<Point2f>() {
                new Point(0.0, 0.0),
                new Point(CONSTS.DEFAULT_MARKER_DIAMETER, 0.0),
                new Point(CONSTS.DEFAULT_MARKER_DIAMETER, CONSTS.DEFAULT_MARKER_DIAMETER) ,
                new Point(0.0, CONSTS.DEFAULT_MARKER_DIAMETER),};

            Mat lambda = Cv2.GetPerspectiveTransform(srcPoints, destPoints);
            Cv2.WarpPerspective(img, outputMat, lambda, outputMat.Size());
            outputMat.SaveImage("warped.png");

            Cv2.DrawContours(img, contours, largest_contour_index, new Scalar(0, 255, 0), 1); // samo za test (risuvam kuntura)
            img.SaveImage("contours.png");

            return outputMat;
        }
        public static bool CropSquareWithoutFrame(ref Mat sourceImage)
        {
            try
            {
                Mat croppedImage = new Mat(sourceImage.Width - CONSTS.FRAME_THICKNESS * 2, sourceImage.Height - CONSTS.FRAME_THICKNESS * 2, MatType.CV_8UC3);
                Rect rect = new Rect(CONSTS.FRAME_THICKNESS, CONSTS.FRAME_THICKNESS, croppedImage.Width, croppedImage.Height);

                croppedImage = sourceImage.SubMat(rect);
                croppedImage.SaveImage("croppedImage.png");

                sourceImage = croppedImage;
            }
            catch (Exception)
            {
                Console.WriteLine("Could not crop image.");
                return false;
            }
            return true;
        }

        public static bool SetImageOrientation(ref Mat img)
        {
            int MinDist = 250;
            int CanieTresh = 100;
            int DownSample = 1;

            //img.SaveImage("grayBEFOREConverted.png");
            var gray = img.CvtColor(ColorConversionCodes.BGR2GRAY);
            gray.SaveImage("grayConverted.png");
            //gray = gray.GaussianBlur(new Size(9, 9), 2, 2);
            //gray.SaveImage("grayBlurr.png");
            //gray.ConvertTo(gray, MatType.CV_8UC1);
            //gray.SaveImage("gray8uc1.png");
            
            var orientCircles = gray.HoughCircles(HoughMethods.Gradient, DownSample, MinDist, CanieTresh, 20, 0, CONSTS.FRAME_THICKNESS * 2);
            if (orientCircles.Length < 3)
                return false;

            Mat print4Test = gray.Clone();
            foreach (var c in orientCircles)
            {
                print4Test.Circle((int)c.Center.X, (int)c.Center.Y, (int)c.Radius, Scalar.Red, 2);
            }
            print4Test.SaveImage("gray.png");

            Point[] orientationPoints = new Point[4]{
                    new Point(img.Width - img.Width / 4, 0),//top right
                    new Point(img.Width - img.Width / 4, img.Height - img.Height / 4),//bottom right
                    new Point(0, img.Height - img.Height / 4),//bottom left
                    new Point(0, 0),//top left
                    };


            bool[] pointInPos = new bool[4];
            for (int i = 0; i < orientationPoints.Length; i++)
            {
                Mat piece = gray.SubMat(new Rect(orientationPoints[i], new Size(img.Width / 4, img.Height / 4)));
                var c = piece.HoughCircles(HoughMethods.Gradient, DownSample, MinDist, CanieTresh, 20, 0, CONSTS.FRAME_THICKNESS * 2);
                piece.SaveImage(string.Format("rightCornerPiece{0}.png", i));

                Console.WriteLine("piece: " + i + "  found circles: " + c.Length);
                pointInPos[i] = c.Length == 0;//true if empty
            }

            if (pointInPos.Where(x => x == true).Count() > 1) // atleast 3 orientation circles
                return false;

            for (int i = 0; i < 4; i++)
            {
                img.SaveImage(string.Format("img{0}.png", i));

                if (pointInPos[i] == true)
                    return true;
                img = img.Transpose().Flip(FlipMode.X);//rotate ccw
            }

            return false;
        }
        internal static void DrawHintPoints(Mat sourceImage, List<Point> pts)
        {
            sourceImage.Circle(pts[0], 15, Scalar.Yellow, 3, LineTypes.AntiAlias, 0);//yellow
            sourceImage.Circle(pts[1], 15, Scalar.Green, 3, LineTypes.AntiAlias, 0);//green
            sourceImage.Circle(pts[2], 15, Scalar.Red, 3, LineTypes.AntiAlias, 0);//red
            sourceImage.Circle(pts[3], 15, Scalar.Blue, 3, LineTypes.AntiAlias, 0);//blue
        }
    }
}
