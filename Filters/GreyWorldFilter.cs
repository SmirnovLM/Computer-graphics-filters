using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Серый Мир"
    class GreyWorldFilter: Filters
    {
        float _R = 0, _G = 0, _B = 0, Avg = 0;
        private void Finding(Bitmap sourceImage, BackgroundWorker worker)
        {
            for (int i = 0; i < sourceImage.Width; i++)
            {
                worker.ReportProgress((int)((float)i / (2 * sourceImage.Width) * 100));
                if (worker.CancellationPending) return;

                for (int j = 0; j < sourceImage.Height; j++)
                {
                    _R += sourceImage.GetPixel(i, j).R; // Color.R
                    _G += sourceImage.GetPixel(i, j).G; // Color.G
                    _B += sourceImage.GetPixel(i, j).B; // Color.B
                }
            }
            _R /= (sourceImage.Width * sourceImage.Height);
            _G /= (sourceImage.Width * sourceImage.Height);
            _B /= (sourceImage.Width * sourceImage.Height);

            Avg = (_R + _G + _B) / 3;
        }

        public override Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height);
            Finding(sourceImage, worker);
           
            for (int i = 0; i < sourceImage.Width; i++) 
            {
                worker.ReportProgress((int)((float)i / (2 * resultImage.Width) * 100 + 50));
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++) {
                    resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            return Color.FromArgb(Clamp((int)(sourceImage.GetPixel(x, y).R * Avg / _R), 0, 255),
                                  Clamp((int)(sourceImage.GetPixel(x, y).G * Avg / _G), 0, 255),
                                  Clamp((int)(sourceImage.GetPixel(x, y).B * Avg / _B), 0, 255));
        }
    }
}
