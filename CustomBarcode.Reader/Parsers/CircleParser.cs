using CustomBarcode.Reader.Helpers;
using OpenCvSharp;
namespace CustomBarcode.Reader.Parsers
{
    class CircleParser : BaseParser
    {
        public CircleParser(int cols, int rows)
        {
            var marker = new CircleBarcode(cols, rows);
            if (segments == null)
                base.segments = marker.segments;
        }
        public override bool EvalSegment(Mat comparePiece)
        {
            float compareDiff = (COLS > 10 || ROWS > 10) ? 0.92f : 0.6f;//increase precision if lots of segments, 
            return HistogramHelper.IsImageWhite(comparePiece, compareDiff);
        }
    }
}
