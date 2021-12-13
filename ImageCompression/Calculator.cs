using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ImageCompression
{
    public static class Calculator
    {
        public static Bitmap ToGrayByEqualWeights(Image image)
        {
            var bitmap = new Bitmap(image);

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    var Y = (byte)((pixel.R + pixel.G + pixel.B) / 3);
                    bitmap.SetPixel(i, j, Color.FromArgb(Y, Y, Y));
                }
            }

            return bitmap;
        }

        public static Bitmap ToGrayByCCIR(Image image)
        {
            var bitmap = new Bitmap(image);

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    var Y = (double)77/256 * pixel.R + (double)150/256 * pixel.G + (double)29/256 * pixel.B;
                    bitmap.SetPixel(i, j, Color.FromArgb((int)Math.Round(Y), (int)Math.Round(Y), (int)Math.Round(Y)));
                }
            }

            return bitmap;
        }

        public static Bitmap ToYCbCr(Image image)
        {
            var bitmap = new Bitmap(image);

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);

                    var doubleY = (double)77/256 * pixel.R + (double)150/256 * pixel.G + (double)29/256 * pixel.B;
                    var Y = Math.Round(doubleY) < 0 ? 0 : Math.Round(doubleY) > 255 ? 255 : Math.Round(doubleY);

                    var doubleCb = (double)144/256 * (pixel.B - Y) + 128;
                    var doubleCr = (double)183/256 * (pixel.R - Y) + 128;

                    var Cb = Math.Round(doubleCb) < 0 ? 0 : Math.Round(doubleCb) > 255 ? 255 : Math.Round(doubleCb);
                    var Cr = Math.Round(doubleCr) < 0 ? 0 : Math.Round(doubleCr) > 255 ? 255 : Math.Round(doubleCr);

                    bitmap.SetPixel(i, j, Color.FromArgb((int)Y, (int)Cb, (int)Cr));
                }
            }

            return bitmap;
        }

        public static Bitmap ToRGB(Image image)
        {
            var bitmap = new Bitmap(image);

            for (var i = 0; i < bitmap.Width; i++)
            {
                for (var j = 0; j < bitmap.Height; j++)
                {
                    var pixel = bitmap.GetPixel(i, j);
                    var doubleR = pixel.R + (double)256/183 * (pixel.B - 128);
                    var doubleG = pixel.R - (double)5329/15481 * (pixel.G - 128) - (double)11103/15481 * (pixel.B - 128);
                    var doubleB = pixel.R + (double)256/144 * (pixel.G - 128);

                    var R = Math.Round(doubleR) < 0 ? 0 : Math.Round(doubleR) > 255 ? 255 : Math.Round(doubleR);
                    var G = Math.Round(doubleG) < 0 ? 0 : Math.Round(doubleG) > 255 ? 255 : Math.Round(doubleG);
                    var B = Math.Round(doubleB) < 0 ? 0 : Math.Round(doubleB) > 255 ? 255 : Math.Round(doubleB);

                    bitmap.SetPixel(i, j, Color.FromArgb((int)R, (int)G, (int)B));
                }
            }

            return bitmap;
        }

        public static string CountPSNR(Image firstImage, Image secondImage)
        {
            try
            {
                var first = new Bitmap(firstImage);
                var second = new Bitmap(secondImage);

                double numerator = (double)3 * 255 * 255 * firstImage.Width * firstImage.Width;
                double denominator = 0;

                for(var i = 0; i < first.Width; i++)
                {
                    for(var j = 0; j < first.Height; j++)
                    {
                        denominator += (first.GetPixel(i, j).R - second.GetPixel(i, j).R) * 
                            (first.GetPixel(i, j).R - second.GetPixel(i, j).R);
                        denominator += (first.GetPixel(i, j).G - second.GetPixel(i, j).G) *
                            (first.GetPixel(i, j).G - second.GetPixel(i, j).G);
                        denominator += (first.GetPixel(i, j).B - second.GetPixel(i, j).B) *
                            (first.GetPixel(i, j).B - second.GetPixel(i, j).B);
                    }
                }

                if (denominator == 0) return "infinite";

                var PSNR = 10 * Math.Log10(numerator / denominator);

                return PSNR.ToString("0.##");
            }
            catch(Exception e)
            {
                throw e;
            }
        }

    }
}
