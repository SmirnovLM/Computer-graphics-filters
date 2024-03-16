using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Линейное Растяжение
    class LinearStretching : Filters
    {
        int min = 255, max = 0;
        float range;
        protected void FindLimits(Bitmap sourceImage, BackgroundWorker worker)
        {
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (2 * sourceImage.Width) * 100)); 
                if (worker.CancellationPending) return;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    int intensity = (int)(0.36 * sourceImage.GetPixel(i, j).R + 
                                          0.53 * sourceImage.GetPixel(i, j).G + 
                                          0.11 * sourceImage.GetPixel(i, j).B);
                    if (intensity < min) 
                        min = intensity;
                    if (intensity > max) 
                        max = intensity;
                }
            }
            range = 255 / (max - min);
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            FindLimits(sourceImage, worker);

            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (2 * resultImage.Width) * 100 + 50)); 
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            return Color.FromArgb(Clamp((int)((sourseImage.GetPixel(x, y).R - min) * range)), 
                                  Clamp((int)((sourseImage.GetPixel(x, y).G - min) * range)), 
                                  Clamp((int)((sourseImage.GetPixel(x, y).B - min) * range)));
        }
    }
}
