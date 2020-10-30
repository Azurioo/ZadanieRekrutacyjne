using System;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageLib
{
    public class ImageProcessing
    {
        public Bitmap ToMainColors(Bitmap input)
        {
            var height = input.Height;
            var width = input.Width;

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    input.SetPixel(i, j, GetMainColor(input.GetPixel(i, j)));
                }
            }

            return input;
        }

        public async Task<Bitmap> ToMainColorsTask(Bitmap input)
        {
            var height = input.Height;
            var width = input.Width;

            for (var i = 0; i < height; i++)
            {
                for (var j = 0; j < width; j++)
                {
                    input.SetPixel(i, j, await Task.FromResult(GetMainColor(input.GetPixel(i, j))));
                }
            }

            return input;
        }

        public Color GetMainColor(Color from)
        {
            if (from.R == from.B && from.B == from.G)
                return from;

            if (from.R > from.B && from.R > from.G)
                return ColorTranslator.FromHtml("#FF0000");

            if (from.G > from.B && from.G > from.B)
                return ColorTranslator.FromHtml("#00FF00");

            return ColorTranslator.FromHtml("#0000FF");
        }
    }
}
