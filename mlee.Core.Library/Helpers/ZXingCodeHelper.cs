using System;
using System.IO;
using ZXing;
using ZXing.Common;

namespace mlee.Core.Library.Helpers
{
    public class ZXingCodeHelper
    {
        public static byte[] GeneratePng(string content, BarcodeFormat barcodeformat, int width, int height, int margin)
        {
            var qrWriter = new ZXing.BarcodeWriterPixelData
            {
                Format = barcodeformat,
                Options = new EncodingOptions { Height = height, Width = width, Margin = margin }
            };
            var pixelData = qrWriter.Write(content);

            // creating a bitmap from the raw pixel data; if only black and white colors are used it makes no difference
            // that the pixel data ist BGRA oriented and the bitmap is initialized with RGB
            // the System.Drawing.Bitmap class is provided by the CoreCompat.System.Drawing package
            using var bitmap = new System.Drawing.Bitmap(pixelData.Width, pixelData.Height, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            using var ms = new MemoryStream();
            // lock the data area for fast access
            var bitmapData = bitmap.LockBits(new System.Drawing.Rectangle(0, 0, pixelData.Width, pixelData.Height),
               System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppRgb);
            try
            {
                // we assume that the row stride of the bitmap is aligned to 4 byte multiplied by the width of the image
                System.Runtime.InteropServices.Marshal.Copy(pixelData.Pixels, 0, bitmapData.Scan0,
                   pixelData.Pixels.Length);
            }
            finally
            {
                bitmap.UnlockBits(bitmapData);
            }
            // save to stream as PNG
            bitmap.Save(ms, System.Drawing.Imaging.ImageFormat.Png);

            return ms.ToArray();
        }
    }
}
