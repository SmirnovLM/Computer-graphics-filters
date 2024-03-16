using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Тиснение"
    class EbossingFilter : MatrixFilters
    {
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            const int size = 3;
            kernel = new float[size, size] { {0 ,1 ,0}, {1, 0, -1}, {0, -1, 0} };
            float resultR = 0, resultG = 0, resultB = 0;
            int radiusX = kernel.GetLength(0) / 2;  // 1
            int radiusY = kernel.GetLength(1) / 2;  // 1 

            for (int l = -radiusY; l <= radiusY; l++) 
            {
                for (int k = -radiusX; k <= radiusX; k++) 
                {
                    int idX = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourseImage.Height - 1);

                    Color neighborColor = sourseImage.GetPixel(idX, idY);
                    Color resultColor = Color.FromArgb(
                                               (int)(0.36 * neighborColor.R) +
                                               (int)(0.53 * neighborColor.G) +
                                               (int)(0.11 * neighborColor.B),

                                               (int)(0.36 * neighborColor.R) +
                                               (int)(0.53 * neighborColor.G) +
                                               (int)(0.11 * neighborColor.B),

                                               (int)(0.36 * neighborColor.R) +
                                               (int)(0.53 * neighborColor.G) +
                                               (int)(0.11 * neighborColor.B)
                                               );

                    resultR += resultColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += resultColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += resultColor.B * kernel[k + radiusX, l + radiusY];
                }
            }

            return Color.FromArgb(
                Clamp(((int)resultR + 255) / 2),
                Clamp(((int)resultG + 255) / 2),
                Clamp(((int)resultB + 255) / 2)
                );
        }
    }
}
