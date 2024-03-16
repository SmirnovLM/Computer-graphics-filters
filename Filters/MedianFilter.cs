using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Медианный Фильтр
    class MedianFilter: Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int i, int j)
        {
            int q = 0;
            int[] colorsR = new int[9], colorsG = new int[9], colorsB = new int[9];

            for (int l = -1; l <= 1; l++)
            {
                for (int k = -1; k <= 1; k++)
                {
                    int idX = Clamp(i + k, 0, sourceImage.Width - 1);
                    int idY = Clamp(j + l, 0, sourceImage.Height - 1);

                    colorsR[q] = sourceImage.GetPixel(idX, idY).R;
                    colorsG[q] = sourceImage.GetPixel(idX, idY).G;
                    colorsB[q] = sourceImage.GetPixel(idX, idY).B;
                    q++;
                }
            }
            Sort(colorsR);
            Sort(colorsB);
            Sort(colorsG);
            return Color.FromArgb(colorsR[9 / 2], colorsG[9 / 2], colorsB[9 / 2]);
        }
        private void Swap(ref int x, ref int y)
        {
            int t = x;
            x = y;
            y = t;
        }
        private void Sort(int[] array)
        {
            for (int i = 0; i < array.Length; i++)
            {
                for (int j = 0; j < array.Length; j++)
                {
                    if (array[i] > array[j])
                        Swap(ref array[i], ref array[j]);
                }
            }
        }
    }
}

