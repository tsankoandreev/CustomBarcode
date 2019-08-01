# Custom Barcode project

## CustomBarcode.Core
- Core library that produces 3 different barcode shapes (circle, triangle and square)
- by provided CountX and CountY (rows, cols) generates (countX * countY) Segments each with 4 SegmentPoints (corners)


## CustomBarcode.Generator
- WPF app that uses CustomBarcode.Core and by given shape and CountX and CountY generates SVG Poligons from Segments
- app saves generated svg to png file ready for use.

## CustomBarcode.Reader
- This library uses OpenCvSharp and provides Parsers for each barcode shape.
- Parser recieves Mat(OpenCvSharp), extract segments from it and evaluates each segment histogram if it is black or white (1|0)
