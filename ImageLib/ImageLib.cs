using System;
using System.ComponentModel;
using System.Drawing;
using System.Threading.Tasks;

namespace ImageLib
{
    public class ImageProcessing
    {

        private Bitmap img;

        public ImageProcessing(Bitmap img)
        {
            this.img = img;
        }
        public ImageProcessing() { }
        public Bitmap ToMainColors()
        {
            var height = img.Height;
            var width = img.Width;

            for (var i = 0; i < width; i++)
            {
                for (var j = 0; j < height; j++)
                {
                    img.SetPixel(i, j, GetMainColor(img.GetPixel(i, j)));
                }
            }

            return img;
        }

        public void ToMainColorsAsync(object sender, DoWorkEventArgs e)
        {
            var height = img.Height;
            var width = img.Width;

            for (var i = 0; i < width; i++)
            {
                int progressPercentage = Convert.ToInt32(((double)i / width) * 100);
                for (var j = 0; j < height; j++)
                {
                    img.SetPixel(i, j, GetMainColor(img.GetPixel(i, j)));
                }
                (sender as BackgroundWorker).ReportProgress(progressPercentage);
            }

            e.Result = img;
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
