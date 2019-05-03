using CustomBarcode.Common;
using CustomBarcode.Objects;
using Svg;
using System.IO;
using System.Linq;
using System.Windows;

namespace CustomBarcode.Generator
{
    public static class SVGManager
    {

        public static string DrawSVGPoligonsfromSegments(BaseSegment[] segments, string pattern)
        {
            string result = string.Empty;

            int circleThickness = 10;
            result += "<?xml version=\"1.0\" encoding=\"utf-8\"?><!DOCTYPE svg> <svg width =\"" + (CONSTS.DEFAULT_MARKER_DIAMETER + circleThickness * 2) + "\" height =\"" + (CONSTS.DEFAULT_MARKER_DIAMETER + circleThickness * 2) + "\" >";
            int center = (CONSTS.DEFAULT_MARKER_DIAMETER + circleThickness * 2) / 2;

            result += "<rect width= \"" + (CONSTS.DEFAULT_MARKER_DIAMETER + circleThickness * 2) + "\" height=\"" + (CONSTS.DEFAULT_MARKER_DIAMETER + circleThickness * 2) + "\" fill = \"none\" stroke = \"black\" stroke-width = \"" + circleThickness + "\"/>";

            result = DrawOrientationCircles(result, circleThickness);

            int idx = 0;
            foreach (var item in segments)
            {
                if (item == null || item.Points == null || item.Points[0] == null)
                    continue;
                var points = new Point[] {
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[0].X + circleThickness,CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[0].Y+circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[1].X + circleThickness,CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[1].Y+circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[2].X + circleThickness,CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[2].Y+circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[3].X + circleThickness,CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[3].Y+circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[0].X + circleThickness,CONSTS.DEFAULT_MARKER_DIAMETER * item.Points[0].Y+circleThickness),
                    };
                string pts = string.Join(" ", points.Select(x => x.X.ToString().Replace(",", ".") + " , " + x.Y.ToString().Replace(",", ".")));

                if (pattern[idx].Equals('1'))
                    result += "<polygon points =\"" + pts + "\"" + " style =\"fill:black\" />";
                else
                    result += "<polygon points =\"" + pts + "\"" + " style =\"fill:none\" />";
                idx++;
            }

            result += "</svg>";


            return result;
        }

        private static string DrawOrientationSquares(string result, int circleThickness)
        {
            var points = new Point[] {
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness       , CONSTS.DEFAULT_MARKER_DIAMETER * 0 +circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness * 2   , CONSTS.DEFAULT_MARKER_DIAMETER * 0 +circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness *2    , CONSTS.DEFAULT_MARKER_DIAMETER * 0 +circleThickness *2),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness       , CONSTS.DEFAULT_MARKER_DIAMETER * 0 +circleThickness *2),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness       , CONSTS.DEFAULT_MARKER_DIAMETER * 0 +circleThickness),
                    };
            string pts = string.Join(" ", points.Select(x => x.X.ToString().Replace(",", ".") + " , " + x.Y.ToString().Replace(",", ".")));
            result += "<polygon points =\"" + pts + "\"" + " style =\"fill:black\" />";
            
            points = new Point[] {
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * 1     ,CONSTS.DEFAULT_MARKER_DIAMETER * 1 ),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * 1 + circleThickness        ,CONSTS.DEFAULT_MARKER_DIAMETER * 1 ),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * 1 + circleThickness     ,CONSTS.DEFAULT_MARKER_DIAMETER * 1 +circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * 1     ,CONSTS.DEFAULT_MARKER_DIAMETER * 1 + circleThickness ),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER * 1     ,CONSTS.DEFAULT_MARKER_DIAMETER * 1 ),
                    };
            pts = string.Join(" ", points.Select(x => x.X.ToString().Replace(",", ".") + " , " + x.Y.ToString().Replace(",", ".")));
            result += "<polygon points =\"" + pts + "\"" + " style =\"fill:black\" />";

            points = new Point[] {
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness    , CONSTS.DEFAULT_MARKER_DIAMETER * 1 ),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness *2      , CONSTS.DEFAULT_MARKER_DIAMETER * 1),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness *2      , CONSTS.DEFAULT_MARKER_DIAMETER * 1 +circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness   , CONSTS.DEFAULT_MARKER_DIAMETER * 1 +circleThickness),
                    new Point(CONSTS.DEFAULT_MARKER_DIAMETER *0 +circleThickness    , CONSTS.DEFAULT_MARKER_DIAMETER * 1 ),
                    };
            pts = string.Join(" ", points.Select(x => x.X.ToString().Replace(",", ".") + " , " + x.Y.ToString().Replace(",", ".")));
            result += "<polygon points =\"" + pts + "\"" + " style =\"fill:black\" />";

            return result;
        }
        private static string DrawOrientationCircles(string result, int circleThickness)
        {
            
            result += "<circle cx =\"" + (circleThickness * 2 + circleThickness) + "\"" + " cy =\"" + (circleThickness * 2+ circleThickness)+  "\"" + " r =\"" + circleThickness + "\"" + " style =\"fill:black\" />";
            result += "<circle cx =\"" + (CONSTS.DEFAULT_MARKER_DIAMETER - circleThickness) + "\"" + " cy =\"" + (CONSTS.DEFAULT_MARKER_DIAMETER - circleThickness) + "\""+ " r =\"" + circleThickness + "\"" + " style =\"fill:black\" />";
            result += "<circle cx =\"" + (circleThickness * 2 + circleThickness) + "\"" + " cy =\"" + (CONSTS.DEFAULT_MARKER_DIAMETER - circleThickness) + "\""+ " r =\"" + circleThickness + "\"" + " style =\"fill:black\" />";

            return result;
        }

        public static void SaveSVGtoPNG(string svg)
        {
            System.Drawing.Bitmap bitmap;
            string fileName = "bitmap.png";
            var byteArray = System.Text.Encoding.ASCII.GetBytes(svg);
            using (var stream = new MemoryStream(byteArray))
            {
                var svgDocument = SvgDocument.Open<SvgDocument>(stream, null);
                using (bitmap = new System.Drawing.Bitmap(svgDocument.Draw()))
                {

                    bitmap.Save(fileName, System.Drawing.Imaging.ImageFormat.Png);
                }
            }

            bitmap.Dispose();
        }
    }
}
