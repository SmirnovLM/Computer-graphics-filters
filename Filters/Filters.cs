using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using System.Drawing;
using System.ComponentModel;

namespace Filters
{
    abstract class Filters
    {
        // Общая для всех фильтров часть: "Обработка Изображения"
        virtual public Bitmap processImage(Bitmap sourceImage, BackgroundWorker worker)
        {
            Bitmap resultImage = new Bitmap(sourceImage.Width, sourceImage.Height); // Результирующее изображение
            
            for (int i = 0; i < sourceImage.Width; i++)  
            {
                worker.ReportProgress((int)((float)i / resultImage.Width * 100));   // Заполнение полосы Прогресса
                if (worker.CancellationPending) return null;

                for (int j = 0; j < sourceImage.Height; j++)  
                {
                        resultImage.SetPixel(i, j, calculateNewPixelColor(sourceImage, i, j));
                }
            }
            return resultImage;
        }

        // Вычисление нового цветового значение для пикселя с координатами (х, у)
        protected abstract Color calculateNewPixelColor(Bitmap sourseImage, int x, int y);
        
        // Функция, приводящая значения к допустимому диапазону
        public int Clamp(int value, int min = 0, int max = 255)
        {
            if (value < min) return min;
            if (value > max) return max;
            return value;
        }
    }

}
