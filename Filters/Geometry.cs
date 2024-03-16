using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class MovingLeft : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            if (sourseImage.Width - x < 51)
            {
                return Color.Black;
            }
            else
            {
                Color resultColor = sourseImage.GetPixel(x + 50, y);
                return resultColor;
            }
        }
    }

    class MovingRigth : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            if (x < 51)
            {
                return Color.Black;
            }
            else
            {
                Color resultColor = sourseImage.GetPixel(x - 50, y);
                return resultColor;
            }
        }
    }

    


    internal class Turn : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int corner = -30;
            int x0 = sourceImage.Width / 2;
            int y0 = sourceImage.Height / 2;
            int resX = (int)((x - x0) * Math.Sin(corner) - (y - y0) * Math.Cos(corner) + x0);
            int resY = (int)((x - x0) * Math.Cos(corner) + (y - y0) * Math.Sin(corner) + y0);
            Color resultColor;

            if (resX < sourceImage.Width && resX >= 0 && resY < sourceImage.Height && resY >= 0)
            {
                resultColor = sourceImage.GetPixel(resX, resY);
            }
            else
            {
                resultColor = Color.FromArgb(0, 0, 0);
            }
            return resultColor;
        }

    }

    internal class Waves2 : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int resX = (int)(x + 20 * Math.Sin((2 * x * Math.PI) / 60));
            Color resultColor;
            resultColor = sourceImage.GetPixel(Clamp(resX, 0, sourceImage.Width - 1),
                Clamp(y, 0, sourceImage.Height - 1));
            return resultColor;
        }
    }
    internal class Waves1 : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            int resX = (int)(x + 20 * Math.Sin((2 * y * Math.PI) / 60));
            Color resultColor;
            resultColor = sourceImage.GetPixel(Clamp(resX, 0, sourceImage.Width - 1),
                Clamp(y, 0, sourceImage.Height - 1));
            return resultColor;
        }
    }
}
