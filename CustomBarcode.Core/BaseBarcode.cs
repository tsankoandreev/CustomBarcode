using CustomBarcode.Common;
using CustomBarcode.Objects;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CustomBarcode
{
    public abstract class BaseBarcode
    {
        /// <summary>
        /// horizontal count of items
        /// </summary>
        public int CountX { get; }
        /// <summary>
        /// vertical count of items
        /// </summary>
        public int CountY { get; }

        public int SegmentCount { get; set; }
        public List<BaseSegment> segments { get; set; }


        public BaseBarcode(int countX, int countY)
        {
            this.CountX = countX;
            this.CountY = countY;
            this.SegmentCount = 0;
            this.segments = new List<BaseSegment>();
            //this.Generate();
        }

        protected virtual void Generate<T>() where T : BaseSegment , new()
        {
            if (this.SegmentCount <= 0)
                return;
            for (int i = 0; i < this.SegmentCount; i++)
            {
                var segment = new T();
                segment.Index = i;
                this.Calculate(i, segment);
                this.segments.Add(segment);
            }
        }

        protected virtual void Calculate<T>(int index, T segment) where T : BaseSegment, new()
        {
            throw new NotImplementedException();
        }

        public abstract BarcodeShapesEnum GetShape();
    }
}
