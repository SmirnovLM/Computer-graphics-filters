using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Монохром: Серый"
    class MonochromeGrey : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(intensity, intensity, intensity);
             
        }
    }

    // Фильтр "Монохром: Красный"
    class MonochromeRed : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(intensity, 0, 0);

        }
    }

    // Фильтр "Монохром: Зеленый"
    class MonochromeGreen : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(0, intensity, 0);

        }
    }

    // Фильтр "Монохром: Синий"
    class MonochromeBlue : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(0, 0, intensity);

        }
    }

    // Фильтр "Монохром: Красно-Зеленый"
    class MonochromeRedGreen : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(intensity, Clamp(intensity - 50), 0);
        }
    }

    // Фильтр "Монохром: Красно-Голубой"
    class MonochromeRedBlue : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(intensity, 0, intensity);
        }
    }

    // Фильтр "Монохром: Зелено-Голубой"
    class MonochromeGreenBlue : Filters
    {
        protected override Color calculateNewPixelColor(Bitmap sourceImage, int x, int y)
        {
            Color sourceColor = sourceImage.GetPixel(x, y);
            int intensity = (int)(0.36 * sourceColor.R) + (int)(0.53 * sourceColor.G) + (int)(0.11 * sourceColor.B);
            return Color.FromArgb(0, intensity, intensity);
        }
    }
}
