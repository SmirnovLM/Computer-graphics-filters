using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Ультра Резкость"
    class SarpnessUltraFilter : MatrixFilters
    {
        public SarpnessUltraFilter()
        {
            const int size = 3;
            kernel = new float[size, size] { {-1, -1, -1}, {-1, 9, -1}, {-1, -1, -1} };
        }
    }
}
