using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media.Imaging;

namespace ImageLib
{
    public class ImageProcessing
    {

        private Bitmap _img;

        public ImageProcessing(BitmapSource source)
        {
            _img = ConvertToBitmap(source);
        }
        public ImageProcessing() { }

        public BitmapSource ToMainColors()
        {
            var bitmapData = _img.LockBits(new Rectangle(0, 0, _img.Width, _img.Height), ImageLockMode.ReadWrite, _img.PixelFormat);

            var bytesPerPixel = Image.GetPixelFormatSize(_img.PixelFormat) / 8;
            var byteCount = bitmapData.Stride * _img.Height;
            var pixels = new byte[byteCount];
            var ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            var heightInPixels = bitmapData.Height;
            var widthInBytes = bitmapData.Width * bytesPerPixel;

            for (var y = 0; y < heightInPixels; y++)
            {
                var currentLine = y * bitmapData.Stride;
                for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1];
                    int oldRed = pixels[currentLine + x + 2];

                    var colors = PickMainColor(oldRed, oldGreen, oldBlue);

                    pixels[currentLine + x] = (byte)colors[2];
                    pixels[currentLine + x + 1] = (byte)colors[1];
                    pixels[currentLine + x + 2] = (byte)colors[0];
                }
            }
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            _img.UnlockBits(bitmapData);
            return Imaging.CreateBitmapSourceFromHBitmap(_img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public async Task<BitmapSource> ToMainColorsAsync()
        {
            var bitmapData = _img.LockBits(new Rectangle(0, 0, _img.Width, _img.Height), ImageLockMode.ReadWrite, _img.PixelFormat);

            var bytesPerPixel = Image.GetPixelFormatSize(_img.PixelFormat) / 8;
            var byteCount = bitmapData.Stride * _img.Height;
            var pixels = new byte[byteCount];
            var ptrFirstPixel = bitmapData.Scan0;
            Marshal.Copy(ptrFirstPixel, pixels, 0, pixels.Length);
            var heightInPixels = bitmapData.Height;
            var widthInBytes = bitmapData.Width * bytesPerPixel;

            for (var y = 0; y < heightInPixels; y++)
            {
                await Task.Run(() => { 
                var currentLine = y * bitmapData.Stride;
                for (var x = 0; x < widthInBytes; x = x + bytesPerPixel)
                {
                    int oldBlue = pixels[currentLine + x];
                    int oldGreen = pixels[currentLine + x + 1]; 
                    int oldRed = pixels[currentLine + x + 2];
                    var colors = PickMainColor(oldRed, oldGreen, oldBlue);
                    pixels[currentLine + x] = (byte)colors[2];
                    pixels[currentLine + x + 1] = (byte)colors[1];
                    pixels[currentLine + x + 2] = (byte)colors[0];
                }
                });
            }
            Marshal.Copy(pixels, 0, ptrFirstPixel, pixels.Length);
            _img.UnlockBits(bitmapData);
            return Imaging.CreateBitmapSourceFromHBitmap(_img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public int[] PickMainColor(int r, int g, int b)
        {
            if(r >= g && r >= b)
                return new []{255, 0, 0};
            if (g >= r && g >= b)
                return new[] { 0, 255, 0 };
            return new[] { 0, 0, 255 };
        }

        public BitmapSource GetImage()
        {
            return Imaging.CreateBitmapSourceFromHBitmap(_img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public Bitmap ConvertToBitmap(BitmapSource bitmapSource)
        {
            var width = bitmapSource.PixelWidth;
            var height = bitmapSource.PixelHeight;
            var stride = width * ((bitmapSource.Format.BitsPerPixel + 7) / 8);
            var memoryBlockPointer = Marshal.AllocHGlobal(height * stride);
            bitmapSource.CopyPixels(new Int32Rect(0, 0, width, height), memoryBlockPointer, height * stride, stride);
            var bitmap = new Bitmap(width, height, stride, PixelFormat.Format32bppPArgb, memoryBlockPointer);
            return bitmap;
        }
    }
}
