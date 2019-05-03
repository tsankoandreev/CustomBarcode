using System;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using CustomBarcode.Objects;
using System.Numerics;
using System.Collections.Generic;

namespace CustomBarcode.Tests
{
    [TestClass]
    public class CircleSegmentTest
    {
        public const int NUMBER_OF_SECTORS = 4;
        public const int NUMBER_OF_SEGMENTS_IN_SECTOR = 2;

        //private ISegmentGenerator generator;

        [TestMethod]
        public void TestCirkleWith8Segments()
        {
            BaseSegment[] testSegments = new BaseSegment[8] {
                new BaseSegment() {//0
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.5 , 0.83333332836628),
                new SegmentPoint(0.16666667163372 , 0.5),
                new SegmentPoint(0 , 0.5),

                new SegmentPoint(0.5 , 1),
            }
                },
           new BaseSegment() {//1
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.16666667163372 , 0.5),
                new SegmentPoint(0.5 , 0.16666667163372),
                new SegmentPoint(0.5 , 0),
                new SegmentPoint(0 , 0.5),
            }
           },
            new BaseSegment() {//2
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.5 , 0.16666667163372),
                new SegmentPoint(0.83333332836628 , 0.5),
                new SegmentPoint(1 , 0.5),
                new SegmentPoint(0.5 , 0),
            }
            },
            new BaseSegment() {//3
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.83333332836628 , 0.5),
                new SegmentPoint(0.5 , 0.83333332836628),
                new SegmentPoint(0.5 , 1),
                new SegmentPoint(1 , 0.5),
            }
            },
            new BaseSegment() {//4
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.5 , 0.666666656732559),
                new SegmentPoint(0.333333343267441 , 0.5),
                new SegmentPoint(0.16666667163372 , 0.5),
                new SegmentPoint(0.5 , 0.83333332836628)
            }
            },
            new BaseSegment() {//5
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.333333343267441 , 0.5),
                new SegmentPoint(0.5 , 0.333333343267441),
                new SegmentPoint(0.5 , 0.16666667163372),
                new SegmentPoint(0.16666667163372 , 0.5)
            }
            },
            new BaseSegment() {//6
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.5 , 0.333333343267441),
                new SegmentPoint(0.666666656732559 , 0.5),
                new SegmentPoint(0.83333332836628 , 0.5),
                new SegmentPoint(0.5 , 0.16666667163372)
            }
            },
            new BaseSegment() {//7
            Points = new SegmentPoint[4] {
                new SegmentPoint(0.666666656732559 , 0.5),
                new SegmentPoint(0.5 , 0.666666656732559),
                new SegmentPoint(0.5 , 0.83333332836628),
                new SegmentPoint(0.83333332836628 , 0.5)
            }
            }
            };
            
            CircleBarcode m = new CircleBarcode(NUMBER_OF_SECTORS, NUMBER_OF_SEGMENTS_IN_SECTOR);
            var segments = m.segments.ToArray();

            Assert.AreEqual(segments.Length, testSegments.Length, "Tested collection length is different.");

            for (int i = 0; i < segments.Length; i++)
            {
                Assert.AreEqual(segments[i].Points.Length, testSegments[i].Points.Length, $"points in segment {segments[i]} are different length");

                for (int j = 0; j < segments[i].Points.Length; j++)
                {
                    Assert.AreEqual(segments[i].Points[j].X, testSegments[i].Points[j].X, 0.0000001, $"Point{j} in segment differs.");
                    Assert.AreEqual(segments[i].Points[j].Y, testSegments[i].Points[j].Y, 0.0000001, $"Point{j} in segment differs.");
                }
            }
        }
        [TestMethod]
        public void TestGenerateAndRead()
        {
            //string pattern = getRandomPattern(11, 11);
            string pattern = "0011111110000101010010011001001010110011001001001110011101101100110010000110000011011000001100000000000110000100010111000";
            
            string patternDec = BinToDec(pattern);
            Console.WriteLine(pattern);
            var marker = new CircleBarcode(11, 11);
            var svg = CustomBarcode.Generator.SVGManager.DrawSVGPoligonsfromSegments(marker.segments.ToArray(), pattern);

            CustomBarcode.Generator.SVGManager.SaveSVGtoPNG(svg);

            var mat = Reader.Helpers.ImageHelper.GetImageFromFile("bitmap.png");

            //mat = Reader.Helpers.ImageHelper.GetSquareFromImage(mat);
            //mat.SaveImage("after-sqare.jpg");
            Reader.Helpers.ImageHelper.SetImageOrientation(ref mat);
            mat.SaveImage("preloadedImageAFTER.png");

            Reader.Helpers.ImageHelper.CropSquareWithoutFrame(ref mat);
            mat.SaveImage("preloadedImageWITHOUTFRAME.png");

            var parser = Reader.Parsers.ParserFactory.GetParser(marker);
            var result = parser.Parse(mat);
            
            Assert.AreEqual(pattern, result.Data);
        }

        private string getRandomPattern(int cols, int rows)
        {
            Random binaryrand = new Random();
            List<int> l = new List<int>();

            while (l.Find(x => x == 1) != 1)
            {
                for (int i = 0; i < (cols * rows); i++)
                {
                    l.Add(binaryrand.Next(0, 2));
                }
            }

            string s = string.Join("", l);
            return s;
        }
        public string BinToDec(string value)
        {
            BigInteger res = 0;

            foreach (char c in value)
            {
                res <<= 1;
                res += c == '1' ? 1 : 0;
            }

            return res.ToString();
        }
    }
}
