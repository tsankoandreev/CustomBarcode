using CustomBarcode.Objects;
using CustomBarcode.Reader.Helpers;
using CustomBarcode.Reader.Objects;
using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarcode.Reader.Parsers
{
    public abstract class BaseParser : Interfaces.IParser
    {
        public List<BaseSegment> segments { get; protected set; }
        public int COLS { get; set; }
        public int ROWS { get; set; }

        public virtual ParserResult Parse(Mat actualCircle)
        {
            ParserResult result = new ParserResult() { Data = string.Empty };
            if (segments == null)
                return null;

            actualCircle.SaveImage("ACTUAL_IMAGE.png");
            foreach (var item in segments)
            {
                //slice of image for current segment
                Mat pieceSegmentOfActualCircle = Extract(actualCircle, item.Points);
                pieceSegmentOfActualCircle.SaveImage(string.Format("pieceSegmentOfActualCircle-index[{0}].png", item.Index));

                bool isWhite = EvalSegment(pieceSegmentOfActualCircle);

                result.Data += isWhite ? "0" : "1";
            }
            actualCircle.SaveImage("ACTUAL_IMAGE_AFTER.png");

            return result;
        }

        public virtual bool EvalSegment(Mat comparePiece)
        {
            return HistogramHelper.IsImageWhite(comparePiece);
        }
        public virtual Mat Extract(Mat sourceImage, SegmentPoint[] points)
        {
            Mat outputMat = new Mat(50, 50, sourceImage.Type());

            var pts1 = new List<Point2f>() {
                new Point(sourceImage.Width * points[0].X,sourceImage.Height * points[0].Y),
                new Point(sourceImage.Width * points[1].X,sourceImage.Height * points[1].Y),
                new Point(sourceImage.Width * points[2].X,sourceImage.Height * points[2].Y),
                new Point(sourceImage.Width * points[3].X,sourceImage.Height * points[3].Y) };
            var pts2 = new List<Point2f>() { new Point(0, 0), new Point(49, 0), new Point(49, 49), new Point(0, 49) };

            Mat lambda = Cv2.GetPerspectiveTransform(pts1, pts2);
            Cv2.WarpPerspective(sourceImage, outputMat, lambda, outputMat.Size());

            return outputMat;
        }
    }
}
