using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MPI_Quicksort
{
    // WORKS PERFECTLY
    internal class ParallelQuicksort
    {
        private static void Main(string[] args)
        {
            List<int> toSort1 = new List<int> { 3, 2, 5, 6, 1, 34, 76, 7, 2, 3, 1, 5, 7, 2 };

            SortingAlgorithms.ParallelQuickSort(toSort1, 0, toSort1.Count - 1);
        }
    }
}