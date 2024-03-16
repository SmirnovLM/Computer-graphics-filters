using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Яркость" (Выше)
    class BrightnessHigherFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourсeImage, int x, int y)
        {
            int k = 50;
            return Color.FromArgb(Clamp(sourсeImage.GetPixel(x, y).R + k), 
                                  Clamp(sourсeImage.GetPixel(x, y).G + k), 
                                  Clamp(sourсeImage.GetPixel(x, y).B + k));
        }
    }

    // Фильтр "Яркость" (Ниже)
    class BrightnessBelowFilter : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourсeImage, int x, int y)
        {
            int k = -20;
            return Color.FromArgb(Clamp(sourсeImage.GetPixel(x, y).R + k), 
                                  Clamp(sourсeImage.GetPixel(x, y).G + k), 
                                  Clamp(sourсeImage.GetPixel(x, y).B + k));
        }
    }
}
