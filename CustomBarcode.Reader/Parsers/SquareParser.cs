using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarcode.Reader.Parsers
{
    public class SquareParser : BaseParser
    {
        public SquareParser(int cols, int rows)
        {
            var marker = new SquareBarcode(cols, rows);
            if (segments == null)
                base.segments = marker.segments;
        }
    }
}
