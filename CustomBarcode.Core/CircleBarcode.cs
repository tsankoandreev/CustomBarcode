using CustomBarcode.Common;
using CustomBarcode.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomBarcode
{
    public class CircleBarcode : BaseBarcode
    {
        public CircleBarcode(int countX, int countY) : base(countX, countY)
        {
            this.SegmentCount = (countX * countY);
            this.Generate<BaseSegment>();
        }

        protected override void Calculate<BaseSegment>(int index, BaseSegment segment)
        {
            SegmentPoint[] lastPoints = new SegmentPoint[4];
            if(index != 0 && index % CountX != 0)
                lastPoints = segments.FirstOrDefault(x => x.Index == index - 1).Points;
            
            segment.Height = (CONSTS.COMPARING_MARKER_DIAMETER / 2f) / (CountY + 1);

            float AngleGrad = 360f / CountX;
            int Row = (int)(segment.Index / (CountX));
            


            if (lastPoints[1] == null)
                lastPoints[1] = new SegmentPoint(CONSTS.COMPARING_MARKER_DIAMETER / 2.0, CONSTS.COMPARING_MARKER_DIAMETER - segment.Height - segment.Height * Row);
            if (lastPoints[2] == null)
                lastPoints[2] = new SegmentPoint(CONSTS.COMPARING_MARKER_DIAMETER / 2.0, CONSTS.COMPARING_MARKER_DIAMETER - segment.Height * Row);

            segment.Points = new SegmentPoint[4];

            var startPoint2 = new SegmentPoint(0, 0);
            var startPoint3 = new SegmentPoint(0, 0);
            var startPoint1 = lastPoints[1];
            var startPoint4 = lastPoints[2];


            startPoint2.rotatePoint(startPoint1, new SegmentPoint(CONSTS.COMPARING_MARKER_DIAMETER / 2, CONSTS.COMPARING_MARKER_DIAMETER / 2), AngleGrad / (180 / Math.PI));
            startPoint3.rotatePoint(startPoint4, new SegmentPoint(CONSTS.COMPARING_MARKER_DIAMETER / 2, CONSTS.COMPARING_MARKER_DIAMETER / 2), AngleGrad / (180 / Math.PI));

            segment.Points[0] = (lastPoints[1]);
            segment.Points[1] = (startPoint2);
            segment.Points[2] = (startPoint3);
            segment.Points[3] = (lastPoints[2]);
        }

        public override BarcodeShapesEnum GetShape()
        {
            return BarcodeShapesEnum.Circle;
        }
    }
}
