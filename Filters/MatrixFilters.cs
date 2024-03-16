using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    /*
     Главная часть матричного фильтра - ядро;
     Ядро - это матрица коэффициентов, которая покомпонентно умножается на значение
            пикселей изображения для получения требуемого результата;
    */
    class MatrixFilters: Filters
    {
        protected float[,] kernel = null; // Двумерный массив

        // Функция, вычисляющая цвет пикселей на основе своих соседей:
        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            // Переменные х и у - это поступающие на вход координаты текущего пикселя изображения
            // Радиусы фильтра по ширине и высоте на основании матрицы
            int radiusX = kernel.GetLength(0) / 2; // 1
            int radiusY = kernel.GetLength(1) / 2; // 1

            // Цветовые компоненты результирующего цвета
            float resultR = 0; 
            float resultG = 0; 
            float resultB = 0; 

            // Перебор Окрестности Пикселя:
            /*
             В каждой из точек окрестности вычисляется цвет, умножается на значение из ябра и 
             прибавляется к результирующим компонентам цвета:
             1) Переменные х и y, как было сказано выше, – это координаты текущего пикселя. 
             2) Переменные l и k принимают значения от -radius до radius и означают положение элемента в 
                матрице фильтра (ядре), если начало отсчета поместить в центр матрицы. 
             3) В переменных idX и idY хранятся координаты пикселей-соседей пикселя (x,y), 
                с которым совмещается центр матрицы, и для которого происходит вычисления цвета 
                (т.е. это координаты окрестности пикселя (x,y), в которую входят и его собственные координаты). 
             4) Предполагаем, что ядро фильтра, поступающее на вход, уже является нормированным 
                (т.е. сумма его коэффициентов не выходит за пределы [0, 1]), 
                что нужно, чтобы не выйти за допустимые границы интенсивности изображения 
                и при этом не потерять часть информации после обрезки результата функцией Clamp
            */
            for (int l = - radiusY; l <= radiusY; l++) {
                for (int k = - radiusX; k <= radiusX; k++) {
                    int idX = Clamp(x + k, 0, sourseImage.Width - 1);
                    int idY = Clamp(y + l, 0, sourseImage.Height - 1);

                    Color neighborColor = sourseImage.GetPixel(idX, idY);

                    resultR += neighborColor.R * kernel[k + radiusX, l + radiusY];
                    resultG += neighborColor.G * kernel[k + radiusX, l + radiusY];
                    resultB += neighborColor.B * kernel[k + radiusX, l + radiusY];
                }
            }
            return Color.FromArgb(
                Clamp((int)resultR, 0, 255),
                Clamp((int)resultG, 0, 255),
                Clamp((int)resultB, 0, 255)
                );
        }
    }
}
