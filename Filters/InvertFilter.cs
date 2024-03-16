using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Инверсия"
    class InvertFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.FromArgb(255 - sourceImage.GetPixel(x, y).R, 
                                  255 - sourceImage.GetPixel(x, y).G, 
                                  255 - sourceImage.GetPixel(x, y).B);
        }
    }
}
