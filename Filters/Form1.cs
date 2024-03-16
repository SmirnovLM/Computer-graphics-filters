using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;


namespace Filters
{
    public partial class Form1 : Form
    {
        Bitmap image;  // Растровое изображение (работающее)
        Bitmap image1; // Растровое изображение (исходное - начальное)
        public Form1()
        {
            InitializeComponent();
        }

        // Загрузка Картинки в Приложение с Компьютера:
        private void загрузкаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenFileDialog dialog = new OpenFileDialog(); // Создание диалога для открытия нужного файла

            dialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";  

            // Проверка выбрал ли пользователь файл
            if (dialog.ShowDialog() == DialogResult.OK) {
                image = new Bitmap(dialog.FileName);  // Инициализация работающей переменной выбранным изображением
                image1 = new Bitmap(dialog.FileName); // Инициализация исходной переменной выбранным изображением
            }

            pictureBox1.Image = image; // Визуализация картинки на форме приложения
            pictureBox1.Refresh();     // Обновление
        }

        // Сохранение обработанного (измененного) изображения на компьютер:
        private void сохранениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (pictureBox1.Image != null && image != image1)
            {
                SaveFileDialog saveFileDialog = new SaveFileDialog(); // Создание Диалога для сохранения изображения:
                saveFileDialog.Title = "Сохранить картинку как...";   // Заглавие Диалогового Окна

                saveFileDialog.OverwritePrompt = true; // Предупреждение об указывании имени уже существующего файла:
                saveFileDialog.CheckPathExists = true; // Предупреждение об указывании несуществующего пути:

                saveFileDialog.Filter = "Image files | *.png; *.jpg; *.bmp | All Files (*.*) | *.*";
                saveFileDialog.ShowHelp = true;

                if (saveFileDialog.ShowDialog() == DialogResult.OK) {
                    try {
                        image.Save(saveFileDialog.FileName, System.Drawing.Imaging.ImageFormat.Jpeg);
                    }
                    catch {
                        MessageBox.Show("Невозможно сохранить изображение", "Ошибка",
                        MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }

            }
        }

        // Исходник
        private void исходникToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            image = image1;
            pictureBox1.Image = image;
            pictureBox1.Refresh();
        }




        // Выполнение кода одного из фильтров:
        private void backgroundWorker1_DoWork(object sender, DoWorkEventArgs e)
        {
            Bitmap newImage = ((Filters)e.Argument).processImage(image, backgroundWorker1);
            if (backgroundWorker1.CancellationPending != true)
                image = newImage;
        }

        // Изменение цвета полосы прогресса:
        private void backgroundWorker1_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            progressBar1.Value = e.ProgressPercentage;
        }

        // Визуализизация обработанного изображения на форме и обнуление полосы прогресса.
        private void backgroundWorker1_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            if (!e.Cancelled)
            {
                pictureBox1.Image = image;
                pictureBox1.Refresh();
            }
            progressBar1.Value = 0;
        }

        // Отмена выполнения преобразование изображение каким-либо фильтром
        private void button1_Click(object sender, EventArgs e)
        {
            backgroundWorker1.CancelAsync();
        }




        // Точечный Фильтр "Инверсия"
        private void инверсияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new InvertFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        // Точечный Фильтр "Сепия"
        private void сепияToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new SepiaFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        // Точечный Фильтр "Яркость" (Выше // Ниже)
        private void вышеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new BrightnessHigherFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void нижеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filter = new BrightnessBelowFilter();
            backgroundWorker1.RunWorkerAsync(filter);
        }

        // Фильтр "ЧБ" 
        private void черноБелыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new WhiteBlack();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтры Монохрома (Серый, Красный, Зеленый, Голубой)
        private void серыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeGrey();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void красныйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeRed();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void зеленыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeGreen();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void голубойToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeBlue();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void коричневыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeRedGreen();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void фиолетовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeRedBlue();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void бирюзовыйToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MonochromeGreenBlue();
            backgroundWorker1.RunWorkerAsync(filters);
        }





        // Фильтр "Размытие" 
        private void размытиеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new BlurFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Гаусса"  
        private void фильтрГауссаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new GaussianFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Резкость"
        private void резкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new SarpnessFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Ультра Резкость" 
        private void ульраРезкостьToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new SarpnessUltraFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Собеля"
        private void фильтрСобеляToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new SobelFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Тиснение"
        private void тиснениеToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new EbossingFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Щарра" 
        private void операторЩарраToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new CharraFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Прюитта"
        private void операторПрюиттаToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new PryittaFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Медианный Фильтр
        private void медианныйФильтрToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MedianFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }



        // Фильтр "Линейное Растяжение"
        private void линейноеРастяжениеэToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new LinearStretching();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Серый Мир"
        private void серыйМирToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filters = new GreyWorldFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        // Фильтр "Идеальный Отражатель"
        private void идеальныйОтражательToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters perfectReflector = new PerfectReflector();
            backgroundWorker1.RunWorkerAsync(perfectReflector);
        }

        // Фильтр "Светящиеся Края"
        private void светящиесяКраяToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            Filters filters = new LuminousEdze();
            backgroundWorker1.RunWorkerAsync(filters);
        }


        // Эрозия
        private void крестToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErosDiam eros = new ErosDiam(1);
            backgroundWorker1.RunWorkerAsync(eros);
        }
        private void ромбToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErosDiam eros = new ErosDiam(2);
            backgroundWorker1.RunWorkerAsync(eros);
        }
        private void квадратToolStripMenuItem_Click(object sender, EventArgs e)
        {
            ErosDiam eros = new ErosDiam(3);
            backgroundWorker1.RunWorkerAsync(eros);
        }

        // Дилатация
        private void крестToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DilatDiam dilat = new DilatDiam(1);
            backgroundWorker1.RunWorkerAsync(dilat);
        }
        private void ромбToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DilatDiam dilat = new DilatDiam(2);
            backgroundWorker1.RunWorkerAsync(dilat);
        }
        private void квадратToolStripMenuItem1_Click(object sender, EventArgs e)
        {
            DilatDiam dilat = new DilatDiam(3);
            backgroundWorker1.RunWorkerAsync(dilat);
        }

        // Размыкание
        private void крестToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DiskDiam disk = new DiskDiam(1);
            backgroundWorker1.RunWorkerAsync(disk);
        }
        private void ромбToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DiskDiam disk = new DiskDiam(2);
            backgroundWorker1.RunWorkerAsync(disk);
        }
        private void квадратToolStripMenuItem2_Click(object sender, EventArgs e)
        {
            DiskDiam disk = new DiskDiam(3);
            backgroundWorker1.RunWorkerAsync(disk);
        }

        // Замыкание
        private void крестToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ClosDiam clos = new ClosDiam(1);
            backgroundWorker1.RunWorkerAsync(clos);
        }
        private void ромбToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ClosDiam clos = new ClosDiam(1);
            backgroundWorker1.RunWorkerAsync(clos);
        }
        private void квадратToolStripMenuItem3_Click(object sender, EventArgs e)
        {
            ClosDiam clos = new ClosDiam(1);
            backgroundWorker1.RunWorkerAsync(clos);
        }

        // Цилиндр
        private void крестToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            TopHatDiam topHatDiam = new TopHatDiam(1);
            backgroundWorker1.RunWorkerAsync(topHatDiam);
        }
        private void ромбToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            TopHatDiam topHatDiam = new TopHatDiam(2);
            backgroundWorker1.RunWorkerAsync(topHatDiam);
        }
        private void квадратToolStripMenuItem4_Click(object sender, EventArgs e)
        {
            TopHatDiam topHatDiam = new TopHatDiam(3);
            backgroundWorker1.RunWorkerAsync(topHatDiam);
        }

        // Чёрная шляпа
        private void крестToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            BlackHatDiam blackHatDiam = new BlackHatDiam(1);
            backgroundWorker1.RunWorkerAsync(blackHatDiam);
        }
        private void ромбToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            BlackHatDiam blackHatDiam = new BlackHatDiam(2);
            backgroundWorker1.RunWorkerAsync(blackHatDiam);
        }
        private void квадратToolStripMenuItem5_Click(object sender, EventArgs e)
        {
            BlackHatDiam blackHatDiam = new BlackHatDiam(3);
            backgroundWorker1.RunWorkerAsync(blackHatDiam);
        }

        // Градиент
        private void крестToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            GradDiam grad = new GradDiam(1);
            backgroundWorker1.RunWorkerAsync(grad);
        }
        private void ромбToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            GradDiam grad = new GradDiam(2);
            backgroundWorker1.RunWorkerAsync(grad);
        }
        private void квадратToolStripMenuItem6_Click(object sender, EventArgs e)
        {
            GradDiam grad = new GradDiam(3);
            backgroundWorker1.RunWorkerAsync(grad);
        }


        // Геометрические Элементы
        private void влевоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MovingLeft();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void вправоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new MovingRigth();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void волны1ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new Waves1();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void волны2ToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new Waves2();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void поворотToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Filters filters = new Turn();
            backgroundWorker1.RunWorkerAsync(filters);
        }

        private void коррекцияСОпорнымЦветомToolStripMenuItem_Click(object sender, EventArgs e)
        {
            CorrectionFilter filters = new CorrectionFilter();
            backgroundWorker1.RunWorkerAsync(filters);
        }
        private void стеклоToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Glass glass = new Glass();
            backgroundWorker1.RunWorkerAsync(glass);
        }
    }

    
}
