using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Сепия"
    class SepiaFilter: Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.299 * sourceColor.R) + (int)(0.586 * sourceColor.G) + (int)(0.113 * sourceColor.B);

            int k = 15; // Коэффициент Сепирования
            return Color.FromArgb(Clamp(intensity + 2 * k), 
                                  Clamp((int)(intensity + 0.5 * k)), 
                                  Clamp(intensity - 1 * k));
        }
    }
}
