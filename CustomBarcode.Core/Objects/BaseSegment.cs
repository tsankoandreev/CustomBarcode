using CustomBarcode.Common;
using System;

namespace CustomBarcode.Objects
{
    public class BaseSegment
    {
        public int Index { get; set; }

        public float Height { get; set; }
        public float Width { get; set; }

        
        public SegmentPoint[] Points { get; set; }
        public BaseSegment() {}
    }
}
