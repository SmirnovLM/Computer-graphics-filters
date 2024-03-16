using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    // Фильтр "Резкость"
    class SarpnessFilter: MatrixFilters
    {
        public SarpnessFilter()
        {
            const int size = 3;
            kernel = new float[size, size] {{0, -1, 0}, {-1, 5, -1}, {0, -1, 0}};
        }
    }
}
