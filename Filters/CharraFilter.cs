using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class CharraFilter: MatrixFilters
    {
        // Фильтр "Щарра"
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            float[,] kernelY = { { 3, 10, 3 }, { 0, 0, 0 }, { -3, -10, -3 } }; // Вертикальное Ядро
            float resultRY = 0, resultGY = 0, resultBY = 0;
            int radiusX1 = kernelY.GetLength(0) / 2;
            int radiusY1 = kernelY.GetLength(1) / 2;

            float[,] kernelX = { { 3, 0, -3 }, { 10, 0, -10 }, { 3, 0, -3 } }; // Горизонтальное Ядро
            float resultRX = 0, resultGX = 0, resultBX = 0;
            int radiusX2 = kernelX.GetLength(0) / 2;
            int radiusY2 = kernelX.GetLength(1) / 2;

            for (int l = -radiusY1; l <= radiusY1; l++) 
            {
                for (int k = -radiusX1; k <= radiusX1; k++) 
                {
                    int idX1 = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY1 = Clamp(y + l, 0, sourseImage.Height - 1);

                    resultRY += sourseImage.GetPixel(idX1, idY1).R * kernelY[k + radiusX1, l + radiusY1];
                    resultGY += sourseImage.GetPixel(idX1, idY1).G * kernelY[k + radiusX1, l + radiusY1];
                    resultBY += sourseImage.GetPixel(idX1, idY1).B * kernelY[k + radiusX1, l + radiusY1];
                }
            }

            for (int l = -radiusY2; l <= radiusY2; l++) 
            {
                for (int k = -radiusX2; k <= radiusX2; k++) 
                {
                    int idX2 = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY2 = Clamp(y + l, 0, sourseImage.Height - 1);

                    resultRX += sourseImage.GetPixel(idX2, idY2).R * kernelX[k + radiusX2, l + radiusY2];
                    resultGX += sourseImage.GetPixel(idX2, idY2).G * kernelX[k + radiusX2, l + radiusY2];
                    resultBX += sourseImage.GetPixel(idX2, idY2).B * kernelX[k + radiusX2, l + radiusY2];
                }
            }

            float resultR = (float)Math.Sqrt(Math.Pow(resultRX, 2) + Math.Pow(resultRY, 2));
            float resultG = (float)Math.Sqrt(Math.Pow(resultGX, 2) + Math.Pow(resultGY, 2));
            float resultB = (float)Math.Sqrt(Math.Pow(resultGX, 2) + Math.Pow(resultGY, 2));

            return Color.FromArgb(
                Clamp(
                    (int)(0.36 * resultR) +
                    (int)(0.53 * resultG) +
                    (int)(0.11 * resultB)),
                Clamp(
                    (int)(0.36 * resultR) +
                    (int)(0.53 * resultG) +
                    (int)(0.11 * resultB)),
                Clamp(
                    (int)(0.36 * resultR) +
                    (int)(0.53 * resultG) +
                    (int)(0.11 * resultB)));
        }
    }
}
