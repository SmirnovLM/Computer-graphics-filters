using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Светящиеся Края
    class LuminousEdze : Filters
    {
        protected float[,] kernelX = new float[,] { { -1, -2, -1 }, { 0, 0, 0 }, { 1, 2, 1 } }; // Gorizont
        protected float[,] kernelY = new float[,] { { -1, 0, 1 }, { -2, 0, 2 }, { -1, 0, 1 } }; // Vertical

        // Медианный Фильтр
        private void Swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }
        private void Sort(int[] arr)
        {
            for (int i = 0; i < arr.Length; i++)
            {
                for (int j = 0; j < arr.Length; j++)
                {
                    if (arr[i] > arr[j])
                        Swap(ref arr[i], ref arr[j]);
                }
            }
        }
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int i, int j)
        {
            int q = 0;
            int[] colorsR = new int[9], colorsG = new int[9], colorsB = new int[9];

            for (int l = -1; l <= 1; l++)
                for (int k = -1; k <= 1; k++)
                {
                    int idX = Clamp(i + k, 0, sourseImage.Width - 1);
                    int idY = Clamp(j + l, 0, sourseImage.Height - 1);

                    colorsR[q] = sourseImage.GetPixel(idX, idY).R;
                    colorsG[q] = sourseImage.GetPixel(idX, idY).G;
                    colorsB[q] = sourseImage.GetPixel(idX, idY).B;
                    q++;
                }
            Sort(colorsR);
            Sort(colorsB);
            Sort(colorsG);
            return Color.FromArgb(colorsR[9 / 2], colorsG[9 / 2], colorsB[9 / 2]);
        }

        // Выделение Границ
        protected Color calculateNewPixelColorSobel(Bitmap sourseImage, int x, int y)
        {
            float resultRY = 0, resultGY = 0, resultBY = 0;
            int radiusX1 = kernelY.GetLength(0) / 2;
            int radiusY1 = kernelY.GetLength(1) / 2;

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

            return Color.FromArgb(Clamp((int)resultR), Clamp((int)resultG), Clamp((int)resultB));
        }

        // Фильтр Максимума
        protected Color calculateNewPixelColorMax(Bitmap sourse, int x, int y)
        {
            int n = 3, 
                radiusX = n / 2, 
                radiusY = n / 2, 
                resultR = 0, 
                resultG = 0, 
                resultB = 0;

            for (int i = 0; i < n; i++)
            {
                for (int j = 0; j < n; j++)
                {
                    int idX = Clamp(x + j - radiusY, 0, sourse.Width - 1);
                    int idY = Clamp(y + i - radiusX, 0, sourse.Height - 1);

                    if (sourse.GetPixel(idX, idY).R >= resultR) 
                        resultR = sourse.GetPixel(idX, idY).R;
                    if (sourse.GetPixel(idX, idY).G >= resultG) 
                        resultG = sourse.GetPixel(idX, idY).G;
                    if (sourse.GetPixel(idX, idY).B >= resultB) 
                        resultB = sourse.GetPixel(idX, idY).B;
                }
            }
            return Color.FromArgb(resultR, resultG, resultB);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage1 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (3 * resultImage1.Width) * 100)); 
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage1.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }

            Bitmap resultImage2 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (3 * resultImage1.Width) * 100) + 33); 
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage2.SetPixel(i, j, calculateNewPixelColorSobel(resultImage1, i, j));
                }
            }

            Bitmap resultImage3 = new Bitmap(sourceImage.Width, sourceImage.Height);
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (3 * resultImage1.Width) * 100) + 66); 
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage3.SetPixel(i, j, calculateNewPixelColorMax(resultImage2, i, j));
                }
            }
            return resultImage3;
        }
    }
}
