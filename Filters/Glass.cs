using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class Glass : Filters
    {
        Random r = new Random();
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int n = 5;
            int resX = Clamp(x + (r.Next(2*n) - n), 0, sourseImage.Width - 1);
            int resY = Clamp(y + (r.Next(2*n) - n), 0, sourseImage.Height - 1);
            return sourseImage.GetPixel(resX, resY);
        }
    }
}
