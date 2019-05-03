using CustomBarcode.Common;
using CustomBarcode.Reader.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarcode.Reader.Parsers
{
    public static class ParserFactory
    {
        public static IParser GetParser(BaseBarcode marker)
        {
            switch (marker.GetShape())
            {
                case BarcodeShapesEnum.Circle:
                    return new CircleParser(marker.CountX,marker.CountY);
                case BarcodeShapesEnum.Square:
                    return new SquareParser(marker.CountX, marker.CountY);
                case BarcodeShapesEnum.Triangle:
                    return new TriangleParser(marker.CountX, marker.CountY);
                default:
                    return null;
            }
        }
    }
}
