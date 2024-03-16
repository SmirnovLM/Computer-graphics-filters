using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Filters
{
    class BlurFilter: MatrixFilters
    {
        // Фильтр "Размытие":
        public BlurFilter() {
            const int size = 3;
            kernel = new float[size, size]; // Матрица 3х3, составленная из элементов 1/9
            for (int i = 0; i < size; i++)
                for (int j = 0; j < size; j++)
                    kernel[i, j] = 1.0f / (float)(size * size);      
        }
    }
}
