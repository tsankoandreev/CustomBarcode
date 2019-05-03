using CustomBarcode.Objects;
using OpenCvSharp;
using System.Collections.Generic;
using System.Linq;

namespace CustomBarcode.Reader.Parsers
{
    public class TriangleParser : BaseParser
    {
        public TriangleParser(int cols, int rows)
        {
            var marker = new TriangleBarcode(cols, rows);
            if (segments == null)
                base.segments = marker.segments;
        }

        public override Mat Extract(Mat sourceImage, SegmentPoint[] points)
        {
            var dist2 = new List<Point2f>() {
                new Point(sourceImage.Width * points[0].X,sourceImage.Height * points[0].Y),
                new Point(sourceImage.Width * points[1].X,sourceImage.Height * points[1].Y),
                new Point(sourceImage.Width * points[2].X,sourceImage.Height * points[2].Y),
                new Point(sourceImage.Width * points[3].X,sourceImage.Height * points[3].Y) };

            Mat outputMat = ExtractMaskAffine(sourceImage, dist2);

            return outputMat;
        }
        public Mat ExtractMaskAffine(Mat sourceImage, List<Point2f> pts1)
        {
            Mat outputMat = new Mat(50, 50, sourceImage.Type());

            var dist = (from p in pts1 select p).Distinct().ToList(); // 3 dots
            var dstTri = new List<Point>() { new Point(0.0, 0.0), new Point(50.0, 0.0), new Point(0.0, 50.0) };

            var dstTri2f = new List<Point2f>() { dstTri[0], dstTri[1], dstTri[2] };

            Mat lambda = Cv2.GetAffineTransform(dist, dstTri2f);
            Cv2.WarpAffine(sourceImage, outputMat, lambda, outputMat.Size());

            Mat outputPieceWithBG = outputMat.EmptyClone();
            outputPieceWithBG.SetTo(Scalar.White);
            //redraw segment over mat (black)
            outputPieceWithBG.FillConvexPoly(dstTri, Scalar.Black, LineTypes.AntiAlias, 0);

            Mat resultMat = new Mat();
            Cv2.Add(outputMat, outputPieceWithBG, resultMat);

            return resultMat;
        }
    }
}
