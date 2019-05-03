using CustomBarcode.Common;
using CustomBarcode.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomBarcode
{
    public class TriangleBarcode : BaseBarcode
    {
        public TriangleBarcode(int countX, int countY) : base(countX, countY)
        {
            if (countX != countY)
                throw new ArgumentException("Count X and Count Y must match");

            this.SegmentCount = (countX * countY);
            this.Generate<BaseSegment>();
        }

        /// <summary>
        /// overrides base Generate to build custom triangle shape 
        /// count of segments (cols) in every next row is 2 * row + 1 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        protected override void Generate<T>()
        {
            if (this.SegmentCount <= 0)
                return;
            int index = 0;
            for (int row = 0; row < CountY; row++)
            {
                for (int col = 0; col < 2 * row + 1; col++)
                {
                    var segment = new T();
                    segment.Index = index;

                    Calculate(row, col, segment);

                    this.segments.Add(segment);
                    index++;
                }
            }
        }

        protected void Calculate(int row, int col, BaseSegment segment)
        {
            segment.Height = CONSTS.COMPARING_MARKER_DIAMETER / CountY;
            segment.Width = CONSTS.COMPARING_MARKER_DIAMETER / CountX;

            SegmentPoint[] lastPoints = new SegmentPoint[4];
            
            if(col != 0)
                lastPoints = segments.FirstOrDefault(x => x.Index == segment.Index - 1).Points;
            
            int Row = row;
            int Col = col;

            segment.Points = new SegmentPoint[4];

            if (lastPoints == null || lastPoints[1] == null && lastPoints[2] == null) //nov red
            {
                segment.Points[0] = new SegmentPoint(CONSTS.COMPARING_MARKER_DIAMETER / 2 - (segment.Width / 2) * Row, segment.Height * Row);
                segment.Points[1] = segment.Points[0];
                segment.Points[2] = new SegmentPoint(segment.Points[0].X + segment.Width / 2, segment.Points[0].Y + segment.Height);
                segment.Points[3] = new SegmentPoint(segment.Points[0].X - segment.Width / 2, segment.Points[0].Y + segment.Height);
            }
            else
            {
                if (Col % 2 == 0)//vseki 4eten element (pravilen)
                { 
                    segment.Points[0] = lastPoints[1];//gore
                    segment.Points[1] = segment.Points[0];//gore
                    segment.Points[2] = new SegmentPoint(lastPoints[2].X + segment.Width, lastPoints[2].Y);//dolu
                    segment.Points[3] = lastPoints[2];//dolu
                }
                else//(oburnat nadolu)
                {
                    segment.Points[0] = lastPoints[0];//gore
                    segment.Points[1] = new SegmentPoint(lastPoints[0].X + segment.Width, lastPoints[0].Y);//gore
                    segment.Points[2] = lastPoints[2];//dolu
                    segment.Points[3] = segment.Points[2];//dolu
                }
            }
        }

        public override BarcodeShapesEnum GetShape()
        {
            return BarcodeShapesEnum.Triangle;
        }
    }
}
