using Microsoft.Win32;
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
            var height = _img.Height;
            var width = _img.Width;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    _img.SetPixel(i, j, GetMainColor(_img.GetPixel(i, j)).Result);
                }
            }

            return Imaging.CreateBitmapSourceFromHBitmap(_img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public async Task ToMainColorsAsync()
        {
            for (var i = 0; i < _img.Width; i++)
            {
                for (var j = 0; j < _img.Height; j++)
                {
                    _img.SetPixel(i, j,await GetMainColor(_img.GetPixel(i, j)));
                }
            }
        }
        public BitmapSource GetResult()
        {
            return Imaging.CreateBitmapSourceFromHBitmap(_img.GetHbitmap(), IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
        }

        public async Task<Color> GetMainColor(Color from)
        {
            if (from.R > from.B && from.R > from.G)
                return await Task.FromResult(ColorTranslator.FromHtml("#FF0000"));

            if (from.G > from.B && from.G > from.B)
                return await Task.FromResult(ColorTranslator.FromHtml("#00FF00"));

            return await Task.FromResult(ColorTranslator.FromHtml("#0000FF"));
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
