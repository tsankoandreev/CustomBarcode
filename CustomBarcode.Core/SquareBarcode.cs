using CustomBarcode.Common;
using CustomBarcode.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomBarcode
{
    public class SquareBarcode : BaseBarcode
    {
        public SquareBarcode(int countX, int countY) : base(countX, countY)
        {
            this.SegmentCount = (countX * countY);
            this.Generate<BaseSegment>();
        }

        protected override void Calculate<BaseSegment>(int index, BaseSegment segment)
        {
            var width = 1.0f;
            var height = 1.0f;
            segment.Height = height / (CountY);
            segment.Width = width / (CountX);

            SegmentPoint[] lastPoints = new SegmentPoint[4];
            if (index != 0 && index % CountX != 0)
                lastPoints = segments.FirstOrDefault(x => x.Index == index - 1).Points;

            int Row = (int)(segment.Index / (CountX));
            int Col = (int)(segment.Index / (CountY));

            if (lastPoints[1] == null)
                lastPoints[1] = new SegmentPoint(0, segment.Height * Row);
            if (lastPoints[2] == null)
                lastPoints[2] = new SegmentPoint(lastPoints[1].X, lastPoints[1].Y + segment.Height);

            var startPoint2 = new SegmentPoint(lastPoints[1].X + segment.Width, lastPoints[1].Y);
            var startPoint3 = new SegmentPoint(lastPoints[2].X + segment.Width, lastPoints[2].Y);

            segment.Points = new SegmentPoint[4];
            segment.Points[0] = (lastPoints[1]);
            segment.Points[1] = (startPoint2);
            segment.Points[2] = (startPoint3);
            segment.Points[3] = (lastPoints[2]);
        }

        public override BarcodeShapesEnum GetShape()
        {
            return BarcodeShapesEnum.Square;
        }
    }
}
