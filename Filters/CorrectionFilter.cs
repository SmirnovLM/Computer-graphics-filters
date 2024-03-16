using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Filters
{
    class CorrectionFilter: Filters
    {
        int Rs;
        int Gs;
        int Bs;
        int Rd;
        int Gd;
        int Bd;
        public CorrectionFilter()
        {
            ColorDialog cd = new ColorDialog();
            cd.Color = Color.White;
            cd.FullOpen = true;
            cd.ShowDialog();
            Color s = cd.Color;
            Rs = s.R;
            Gs = s.G;
            Bs = s.B;
            cd.Color = Color.White;
            cd.ShowDialog();
            Color d = cd.Color;
            Rd = d.R;
            Gd = d.G;
            Bd = d.B;
        }

        protected override Color calculateNewPixelColor(Bitmap sourseImage, int x, int y)
        {
            Color r = sourseImage.GetPixel(x, y);
            return Color.FromArgb(
            Clamp(r.R * Rd / Rs),
            Clamp(r.G * Gd / Gs),
            Clamp(r.B * Bd / Bs));
        }
    }
}
