using OpenCvSharp;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CustomBarcode.Reader.Helpers
{
    /// <summary>
    /// histDiff: 
    /// 0 = 100% match
    /// 1 = 0% match
    /// </summary>
    /// <param name="pieceMat"></param>
    /// <returns>bool result: if same = true</returns>
    public static class HistogramHelper
    {
        //private static Mat whiteHist = null;
        //private static int[] hdims = { 256 }; // Histogram size for each dimension
        //private static Rangef[] ranges = { new Rangef(0, 256), }; // min/max 
        public static bool IsImageWhite(Mat pieceMat, float compareDiff = 0.45f)
        {
            int count_transparent = 0;
            for (int row = 0; row < pieceMat.Rows; row++)
            {
                for (int col = 0; col < pieceMat.Cols; col++)
                {
                    Vec4b val = pieceMat.At<Vec4b>(row, col);
                    if (val[3] > 0)
                        count_transparent++;
                }
            }
            int totalIdx = pieceMat.Cols * pieceMat.Rows;
            return count_transparent / totalIdx < compareDiff;
            
           // if (whiteHist == null)
           // {
           //     Mat whitePieceToCompare = pieceMat.EmptyClone();
           //     whitePieceToCompare.SetTo(Scalar.White);
           //     whiteHist = new Mat();
           // 
           //     Cv2.CalcHist(
           //         new Mat[] { whitePieceToCompare },
           //         new int[] { 0 },
           //         null,
           //         whiteHist,
           //         1,
           //         hdims,
           //         ranges);
           // }
           // 
           // 
           // Mat hist1 = new Mat();
           // 
           // Cv2.CalcHist(
           //     new Mat[] { pieceMat },
           //     new int[] { 0 },
           //     null,
           //     hist1,
           //     1,
           //     hdims,
           //     ranges);
           // 
           // 
           // double histDiff = Cv2.CompareHist(hist1, whiteHist, HistCompMethods.Bhattacharyya);
           // //Debug.Log("hist val:" + histDiff);
           // if (Math.Round(histDiff, 2) < compareDiff) // < 0,5 white
           //     return true;
           // else
           //     return false;
            
        }
    }
}
