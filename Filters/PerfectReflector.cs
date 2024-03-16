using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Идеальный Отражатель"
    class PerfectReflector : Filters
    {
        int Rmax = 0, Gmax = 0, Bmax = 0;
        private void FindMaxs(Bitmap sourseImage, BackgroundWorker worker)
        {
            for (int i = 0; i < sourseImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (2 * sourseImage.Width) * 100));
                if (worker.CancellationPending)
                    return;

                for (int j = 0; j < sourseImage.Height; j++)
                {
                    if (sourseImage.GetPixel(i,j).R >= Rmax)
                        Rmax = sourseImage.GetPixel(i,j).R;
                    if (sourseImage.GetPixel(i, j).G >= Gmax)
                        Gmax = sourseImage.GetPixel(i, j).G;
                    if (sourseImage.GetPixel(i, j).B >= Bmax)
                        Bmax = sourseImage.GetPixel(i, j).B;
                }
            }
        }

        public override Bitmap processImage(Bitmap sourseImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourseImage.Width, sourseImage.Height);
            FindMaxs(sourseImage, worker);
            
            for (int i = 0; i < sourseImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (2 * resultImage.Width) * 100 + 50));
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourseImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourseImage, i, j));
                }
            } 
            return resultImage;
        }

        

        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            return Color.FromArgb(Clamp((int)(sourseImage.GetPixel(x, y).R * 255 / Rmax)),
                                  Clamp((int)(sourseImage.GetPixel(x, y).G * 255 / Gmax)),
                                  Clamp((int)(sourseImage.GetPixel(x, y).B * 255 / Bmax)));
        }
    }
}
