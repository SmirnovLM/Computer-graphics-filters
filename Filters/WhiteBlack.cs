using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Черно-Белый Фильтр
    class WhiteBlack : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourсeImage, int x, int y)
        {
            Color sourсeColor = sourсeImage.GetPixel(x, y);
            return (sourсeColor.R + sourсeColor.G + sourсeColor.B) < (3 * 255 / 2) ? 
                Color.FromArgb(0, 0, 0) : Color.FromArgb(255, 255, 255); 
        }
    }
}
