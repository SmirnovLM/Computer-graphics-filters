using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    abstract class MorphologyFilters : Filters
    {
        protected abstract bool calcCord(int a, Color b); //Эта функция сравнивает интенсивность цвета (переводит его в чб) с неким значением
        protected bool[,] calcKer(int c, int n)
        {
            int h = n / 2;
            bool[,] ker = new bool[n, n];
            switch (c)
            {
                case 1:
                    for (int i = 0; i < n; i++) // Поле в виде Креста
                    {
                        ker[i, h] = ker[h, i] = true;
                    }
                    break;
                case 2:
                    for (int i = 0; i < n; i++) // Поле в виде Ромба
                    {
                        for (int j = h; j > Math.Abs(h - i) - 1; j--)
                        {
                            ker[i, j] = ker[i, n - 1 - j] = true;
                        }
                    }
                    break;
                default:
                    for (int i = 0; i < n; i++) // Поле в виде Прямоугольника
                    {
                        for (int j = 0; j < n; j++)
                        {
                            ker[i, j] = true;
                        }
                    }
                    break;
            }
            return ker;
        }
        protected bool[,] ker = null;
        protected int res = 0; 
        public MorphologyFilters() { }
        public MorphologyFilters(bool[,] kern) 
        {
            ker = kern;
        }

        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            int radX = ker.GetLength(0) / 2;
            int radY = ker.GetLength(1) / 2;
            int resC = res; //Обнуление переменной
            int resX = x;
            int resY = y;
            for (int i = 0; i < ker.GetLength(1); i++)
            {
                for (int j = 0; j < ker.GetLength(0); j++)
                {
                    //Подсчёт нового значения пикселя на основе всех цветов, входящих в поле (определяется ядром) вокруг него.
                    int idx = Clamp(x + j - radY, 0, sourseImage.Width - 1);
                    int idy = Clamp(y + i - radX, 0, sourseImage.Height - 1);
                    Color neigCol = sourseImage.GetPixel(idx, idy);
                    //Принцип работы в том, чтобы запомнить наиболее подходящий пиксель в поле и взять его
                    if (ker[j, i] && calcCord(resC, neigCol))
                    {
                        resC = (int)(0.36 * neigCol.R + 0.53 * neigCol.G + 0.11 * neigCol.B);
                        resX = idx;
                        resY = idy;
                    }
                }
            }
            return sourseImage.GetPixel(resX, resY);
        }
    }


    class ErosDiam : MorphologyFilters
    { 
        protected override bool calcCord(int a, Color b)
        {
            return a < (int)(0.36 * b.R + 0.53 * b.G + 0.11 * b.B); 
        }
        public ErosDiam(int c = 2)
        {
            res = 0; 
            int n = 5;
            ker = calcKer(c, n);
        }
    }
    class DilatDiam : MorphologyFilters
    {
        protected override bool calcCord(int a, Color b)
        {
            return a > (int)(0.36 * b.R + 0.53 * b.G + 0.11 * b.B);
        }
        public DilatDiam(int c = 2)
        {
            res = 255;
            int n = 5;
            ker = calcKer(c, n);
        }
    }
    
    class DiskDiam : MorphologyFilters
    {
        protected override bool calcCord(int a, Color b)
        {
            return a < (int)(0.36 * b.R + 0.53 * b.G + 0.11 * b.B);
        }
        protected bool calcCord_(int a, Color b)
        {
            return a > (int)(0.36 * b.R + 0.53 * b.G + 0.11 * b.B);
        }
        protected bool[,] ker_;
        public DiskDiam(int c = 2)
        {
            int n = 5;
            int n_ = n + 2;
            res = 0;
            ker = calcKer(c, n);
            ker_ = calcKer(c, n_);
        }
        protected Color calcNewColor_(Bitmap sourse, int x, int y) //А вот и копипаст
        {
            int radX = ker.GetLength(0) / 2;
            int radY = ker.GetLength(1) / 2;
            int resC = 255 - res;
            int resX = x;
            int resY = y;
            for (int i = 0; i < ker.GetLength(1); i++)
            {
                for (int j = 0; j < ker.GetLength(0); j++)
                {
                    int idx = Clamp(x + j - radY, 0, sourse.Width - 1);
                    int idy = Clamp(y + i - radX, 0, sourse.Height - 1);
                    Color neigCol = sourse.GetPixel(idx, idy);
                    if (ker[j, i] && calcCord_(resC, neigCol))
                    {
                        resC = (int)(0.36 * neigCol.R + 0.53 * neigCol.G + 0.11 * neigCol.B);
                        resX = idx;
                        resY = idy;
                    }
                }
            }
            return sourse.GetPixel(resX, resY);
        }
        public override Bitmap processImage(Bitmap source, BackgroundWorker worker)
        //Такой финт ушами возможен только если вы поставите virtual возле соответствующей операции у исходного класса Filters
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / result.Width * 100) / 2); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result.SetPixel(i, j, calculateNewPixelColor(source, i, j));
                }
            }
            Bitmap Rresult = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / result.Width * 100) / 2 + 50); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    Rresult.SetPixel(i, j, calcNewColor_(result, i, j));
                }
            }
            return Rresult;
        }
    }

    //Следующий фильтр просто использует операции в другом порядке
    class ClosDiam : DiskDiam
    {
        public ClosDiam(int c = 2)
        {
            int n = 7;
            int n_ = n - 2;
            res = 0;
            ker = calcKer(c, n);
            ker_ = calcKer(c, n_);
        }
        public override Bitmap processImage(Bitmap source, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / result.Width * 100) / 2); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result.SetPixel(i, j, calcNewColor_(source, i, j));
                }
            }
            Bitmap Rresult = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / result.Width * 100) / 2 + 50); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    Rresult.SetPixel(i, j, calculateNewPixelColor(result, i, j));
                }
            }
            return Rresult;
        }
    }
    //Градиент выдаёт разницу между результатами работы эрозии и делатации. Наследование от размыкания применяется для удобства и сокращения копипаста
    class GradDiam : DiskDiam
    {
        public GradDiam(int c = 2)
        {

            int n = 3;
            res = 0;
            ker = calcKer(c, n);
        }
        //Вспомогательная фукция, выдающая разницу яркости двух пикселей (только для монохрома)
        protected Color EndColor(Color f, Color s)
        {
            int resR = Math.Abs(f.R - s.R);
            int resG = Math.Abs(f.G - s.G);
            int resB = Math.Abs(f.B - s.B);
            return Color.FromArgb(resR, resG, resB);
        }
        public override Bitmap processImage(Bitmap source, BackgroundWorker worker)
        {
            Bitmap result1 = new Bitmap(source.Width, source.Height); //Считаем эрозию
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result1.SetPixel(i, j, calculateNewPixelColor(source, i, j));
                }
            }
            Bitmap result2 = new Bitmap(source.Width, source.Height); //Считаем делатацию
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 33); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result2.SetPixel(i, j, calcNewColor_(source, i, j));
                }
            }
            Bitmap EndResult = new Bitmap(source.Width, source.Height); //Вычитаем
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 66); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    EndResult.SetPixel(i, j, EndColor(result1.GetPixel(i, j), result2.GetPixel(i, j)));
                }
            }
            return EndResult;
        }
    }
    //Фильтр "цилиндр" или "верхушка шляпы" похожа на градиент, но вычитает исходник из размыкания
    class TopHatDiam : DiskDiam
    {
        public TopHatDiam(int c = 2)
        {

            int n = 3;
            int n_ = n + 2;
            res = 0;
            ker = calcKer(c, n);
            ker_ = calcKer(c, n_);
        }
        protected Color EndColor(Color f, Color s)
        {
            int resR = Math.Abs(f.R - s.R);
            int resG = Math.Abs(f.G - s.G);
            int resB = Math.Abs(f.B - s.B);
            return Color.FromArgb(resR, resG, resB);
        }
        public override Bitmap processImage(Bitmap source, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)  //размыкание
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result.SetPixel(i, j, calculateNewPixelColor(source, i, j));
                }
            }
            Bitmap result1 = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 33); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result1.SetPixel(i, j, calcNewColor_(result, i, j));
                }
            }
            Bitmap EndResult = new Bitmap(source.Width, source.Height); //Вычитаем
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 66); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    EndResult.SetPixel(i, j, EndColor(result1.GetPixel(i, j), source.GetPixel(i, j)));
                }
            }
            return EndResult;
        }
    }
    //Фильтр "Чёрная шляпа" или "дно шляпы" похож на цилиндр, но вычитание происходит из замыкания
    class BlackHatDiam : ClosDiam
    {
        public BlackHatDiam(int c = 2)
        {

            int n = 5;
            int n_ = n - 2;
            res = 0;
            ker = calcKer(c, n);
            ker_ = calcKer(c, n_);
        }
        protected Color EndColor(Color f, Color s)
        {
            int resR = Math.Abs(f.R - s.R);
            int resG = Math.Abs(f.G - s.G);
            int resB = Math.Abs(f.B - s.B);
            return Color.FromArgb(resR, resG, resB);
        }
        public override Bitmap processImage(Bitmap source, BackgroundWorker worker)
        {
            Bitmap result = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)  //замыкание
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result.SetPixel(i, j, calcNewColor_(source, i, j));
                }
            }
            Bitmap result1 = new Bitmap(source.Width, source.Height);
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 33); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    result1.SetPixel(i, j, calculateNewPixelColor(result, i, j));
                }
            }
            Bitmap EndResult = new Bitmap(source.Width, source.Height); //Вычитаем
            for (int i = 0; i < source.Width; i++)
            {
                try { worker.ReportProgress((int)((float)i / source.Width * 100) / 3 + 66); }
                catch (System.InvalidOperationException) { }
                if (worker.CancellationPending) return null;
                for (int j = 0; j < source.Height; j++)
                {
                    EndResult.SetPixel(i, j, EndColor(result1.GetPixel(i, j), source.GetPixel(i, j)));
                }
            }
            return EndResult;
        }
    }
}




