using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomBarcode.Objects
{
    public class SegmentPoint
    {
        public double X { get; set; }
        public double Y { get; set; }

        public SegmentPoint(double v1, double v2)
        {
            this.X = v1;
            this.Y = v2;
        }

        public static SegmentPoint operator +(SegmentPoint s1, SegmentPoint S2)
        {
            return new SegmentPoint(s1.X + S2.X, s1.Y + S2.Y);
        }
        public static SegmentPoint operator -(SegmentPoint s1, SegmentPoint S2)
        {
            return new SegmentPoint(s1.X - S2.X, s1.Y - S2.Y);
        }

        private void rotate2D(SegmentPoint inPoint, double angRad)
        {
            //CW rotation
            this.X = (double)(Math.Cos(angRad) * inPoint.X - Math.Sin(angRad) * inPoint.Y);
            this.Y = (double)(Math.Sin(angRad) * inPoint.X + Math.Cos(angRad) * inPoint.Y);
                
        }
        public void rotatePoint(SegmentPoint inPoint, SegmentPoint center, double angRad)
        {
            rotate2D(inPoint - center, angRad);
            this.X = this.X + center.X;
            this.Y = this.Y + center.Y;
        }
    }
}
