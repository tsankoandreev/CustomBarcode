using CustomBarcode.Reader.Objects;
using OpenCvSharp;

namespace CustomBarcode.Reader.Interfaces
{
    public interface IParser
    {
        ParserResult Parse(Mat markerMat);
    }
}
